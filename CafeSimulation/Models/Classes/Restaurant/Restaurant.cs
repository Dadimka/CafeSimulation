using System;
using System.Collections.Generic;
using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes;

public class Restaurant
{
    private List<Dictionary<String, List<int>>> globalStat =
        new List<Dictionary<string, List<int>>>(new Dictionary<string, List<int>>[1441]);

    private int _tableQuantity;
    private int _waiterQuantity;
    private int _chefQuantity;
    private int _beginWork;
    private int _endWork;
    private int _maxpeople;
    private int _capacitytable;

    private Kitchen _kitchen;
    private Queue<int> _entranceQueue = new Queue<int>();
    // private int _maxCapacity = 0;

    public List<Table> tableList = [];
    private List<Waiter> _waiterList = [];

    private int _time;


    Random random = new Random();

    private Dictionary<int, int> futureQueue = new Dictionary<int, int>();

    public Restaurant(int tables, int waiters, int chefs, int beginWork, int endWork, int peakTime, int stdDev,
        int groupQuantity, int maxpeople, int capacitytable)
    {
        _tableQuantity = tables;
        _waiterQuantity = waiters;
        _chefQuantity = chefs;
        _beginWork = beginWork;
        _endWork = endWork;
        _maxpeople = maxpeople;
        _capacitytable = capacitytable;
        _time = beginWork * 60;
        _kitchen = new Kitchen(_chefQuantity);


        for (int i = 0; i < _tableQuantity; i++)
        {
            // _maxCapacity += capacity;
            tableList.Add(new Table(i, _capacitytable));
        }

        tableList.Sort();

        for (int i = 0; i < _waiterQuantity; i++)
        {
            _waiterList.Add(new Waiter());
        }

        generateFutureQueue(peak: peakTime, stdDev: stdDev, kolichestvo: groupQuantity, maxpeople: _maxpeople);
    }

    public void EntranceEnqeue(int n)
    {
        _entranceQueue.Enqueue(n);
    }

    public void ProcessQueue()
    {
        int groupCounter;
        if (_entranceQueue.TryDequeue(out groupCounter))
        {
            foreach (Table table in tableList)
            {
                if (!table.IsBusy && groupCounter <= table.MaxCapacity)
                {
                    Console.Write($"REST: посадили {groupCounter} гостей за стол №{table.TableId}\n");
                    table.PlaceHuman(groupCounter);
                    return;
                }
            }

            Console.Write($"REST: не хватает столов для {groupCounter} гостей\n");
            return;
        }
        else
        {
            Console.Write($"REST: на входе никого нет\n");
            return;
        }
    }

    public void RefreshWaiters()
    {
        foreach (Waiter waiter in _waiterList)
        {
            if (!waiter.IsBusy)
            {
                Dictionary<String, List<int>> result = waiter.DoWork(ref _kitchen, ref tableList);
                if (result.ContainsKey("waiting_time"))
                {
                    if (globalStat[_time] is null)
                    {
                        globalStat[_time] = new Dictionary<string, List<int>>();
                        globalStat[_time]["waiting_time"] = new List<int>();
                    }

                    globalStat[_time]["waiting_time"].AddRange(result["waiting_time"]);
                }
            }
        }
    }

    public void RefreshTables()
    {
        foreach (Table table in tableList)
        {
            if (table.IsBusy)
            {
                table.Refresh();
            }
        }
    }

    private void Tick()
    {
        Console.Write($"================tick№{_time}================\n");
        _time += 1;
        ProcessFutureQueue();
        ProcessQueue();
        RefreshWaiters();
        _kitchen.Refresh();
        RefreshTables();
    }

    private void ProcessFutureQueue()
    {
        int groupOfPeople;
        if (futureQueue.TryGetValue(_time, out groupOfPeople))
        {
            EntranceEnqeue(groupOfPeople);
        }
    }

    private void generateFutureQueue(int peak = 4, int stdDev = 18, int kolichestvo = 20, int minPeople = 1,
        int maxpeople = 6)
    {
        for (int i = 0; i < kolichestvo; i++)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);
            double randNormal = peak * 60 + stdDev * 60 * randStdNormal;
            int entranceTime = Convert.ToInt32(randNormal);
            futureQueue[entranceTime] = random.Next(minPeople, maxpeople + 1);
        }
    }

    public List<Dictionary<string, List<int>>> Simulate()
    {
        for (int i = _beginWork * 60; i < _endWork * 60; i++)
        {
            Console.Write($"{i} {_time}");
            Tick();
        }

        return globalStat;
    }
}