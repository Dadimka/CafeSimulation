using System;
using System.Collections.Generic;

namespace CafeSimulation.Models.Classes;

public class Waiter
{
    private bool _isBusy = false;

    private int _time = 0;

    public Dictionary<string, List<int>> DoWork(ref Kitchen kitchen, ref List<Table> tables)
    {
        Dictionary<String, List<int>> statistic = new Dictionary<string, List<int>>();
        statistic["waiting_time"] = new List<int>();
        _time++;
        foreach (Table table in tables)
        {
            if (table.IsWaitingOrder())
            {
                List<OrderItem> orderItems = table.DoOrder();
                foreach (OrderItem item in orderItems)
                {
                    item.OrderTime = _time;
                }

                kitchen.DistributeOrders(orderItems);
                Console.Write($"WAITER: взял заказ у стола №{table.TableId}\n");
                return statistic;
            }
        }

        Console.Write("WAITER: новых заказов нет\n");
        if (kitchen.HasReadyFood())
        {
            foreach (OrderItem item in kitchen.PassReadyFood(3))
            {
                foreach (Table table in tables)
                {
                    if (table.TableId == item.TableId)
                    {
                        table.TakeFood(item);
                        Console.Write(
                            $"WAITER: выдал {item.FoodObject.name} клиенту №{item.GuestId} за столом №{table.TableId}\n");
                        int waitingTime = _time - item.OrderTime;
                        Console.Write($"заказ выполнен за {waitingTime} мин.\n");
                        statistic["waiting_time"].Add(waitingTime);
                    }
                }
            }

            return statistic;
        }

        Console.Write("WAITER: нет готовой еды\n");
        return statistic;
    }

    public int Time
    {
        get { return _time; }
    }

    public bool IsBusy
    {
        get { return _isBusy; }
    }
}