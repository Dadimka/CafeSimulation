using System;
using System.Collections.Generic;
using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes;

public class Kitchen
{
    private int CRITICAL_TIME = 20;

    private int _chefQuantity;
    private int _time = 0;

    private Queue<OrderItem> _mainDishWorkingLine = new Queue<OrderItem>();
    private Queue<OrderItem> _additionalDishWorkingLine = new Queue<OrderItem>();
    private Queue<OrderItem> _drinksWorkingLine = new Queue<OrderItem>();
    private Queue<OrderItem> _readyFood = new Queue<OrderItem>();

    private List<Chef> chefList = new List<Chef>();


    public Kitchen(int chefs)
    {
        _chefQuantity = chefs;
        for (int i = 0; i < _chefQuantity; i++)
        {
            chefList.Add(new Chef(i));
        }
    }

    public void DistributeOrders(List<OrderItem> listOrder)
    {
        foreach (OrderItem item in listOrder)
        {
            if (item.FoodObject.type == FoodType.MainDish)
            {
                _mainDishWorkingLine.Enqueue(item);
            }

            if (item.FoodObject.type == FoodType.AdditionalDish)
            {
                _additionalDishWorkingLine.Enqueue(item);
            }

            if (item.FoodObject.type == FoodType.Drink)
            {
                _drinksWorkingLine.Enqueue(item);
            }
        }
    }

    public List<OrderItem> PassReadyFood(int quantity)
    {
        List<OrderItem> orderList = new List<OrderItem>();
        for (int i = 0; i < quantity; i++)
        {
            OrderItem item;
            _readyFood.TryDequeue(out item);
            if (item is not null)
            {
                orderList.Add(item);
            }
        }

        return orderList;
    }


    public void Refresh()
    {
        _time++;
        foreach (Chef chef in chefList)
        {
            OrderItem peekResult;
            OrderItem? workResult;
            if (chef.IsBusy)
            {
                Queue<OrderItem> workingLine = null;
                workResult = chef.DoWork(ref workingLine);
                if (workResult is not null)
                {
                    _readyFood.Enqueue(workResult);
                }
            }
            else
            {
                if (_drinksWorkingLine.TryPeek(out peekResult))
                {
                    if (_time - peekResult.OrderTime > CRITICAL_TIME)
                    {
                        Console.Write($"CHEF №{chef.Id}: распределен на линию НАПИТКОВ ЭКСТРЕННО\n");

                        workResult = chef.DoWork(ref _drinksWorkingLine);
                        if (workResult is not null)
                        {
                            _readyFood.Enqueue(workResult);
                        }

                        continue;
                    }
                }

                if (_additionalDishWorkingLine.TryPeek(out peekResult))
                {
                    if (_time - peekResult.OrderTime > CRITICAL_TIME)
                    {
                        Console.Write($"CHEF №{chef.Id}: распределен на линию САЛАТОВ ЭКСТРЕННО\n");

                        workResult = chef.DoWork(ref _additionalDishWorkingLine);
                        if (workResult is not null)
                        {
                            _readyFood.Enqueue(workResult);
                        }

                        continue;
                    }
                }

                if (_mainDishWorkingLine.TryPeek(out peekResult))
                {
                    if (_time - peekResult.OrderTime > CRITICAL_TIME)
                    {
                        Console.Write($"CHEF №{chef.Id}: распределен на линию БЛЮД ЭКСТРЕННО\n");

                        workResult = chef.DoWork(ref _mainDishWorkingLine);
                        if (workResult is not null)
                        {
                            _readyFood.Enqueue(workResult);
                        }

                        continue;
                    }
                }

                if (_drinksWorkingLine.TryPeek(out peekResult))
                {
                    Console.Write($"CHEF №{chef.Id}: распределен на линию НАПИТКОВ\n");

                    workResult = chef.DoWork(ref _drinksWorkingLine);
                    if (workResult is not null)
                    {
                        _readyFood.Enqueue(workResult);
                    }

                    continue;
                }

                if (_additionalDishWorkingLine.TryPeek(out peekResult))
                {
                    Console.Write($"CHEF №{chef.Id}: распределен на линию САЛАТОВ\n");

                    workResult = chef.DoWork(ref _additionalDishWorkingLine);
                    if (workResult is not null)
                    {
                        _readyFood.Enqueue(workResult);
                    }

                    continue;
                }

                if (_mainDishWorkingLine.TryPeek(out peekResult))
                {
                    Console.Write($"CHEF №{chef.Id}: распределен на линию БЛЮД\n");
                    workResult = chef.DoWork(ref _mainDishWorkingLine);
                    if (workResult is not null)
                    {
                        _readyFood.Enqueue(workResult);
                    }

                    continue;
                }

                Console.Write($"KITCHEN: нечего готовить\n");
            }
        }

        Console.Write($"KITCHEN: Линия напитков: {_drinksWorkingLine.Count} ед. ожидают приготовления\n");
        Console.Write($"KITCHEN: Линия салатов: {_additionalDishWorkingLine.Count} ед. ожидают приготовления\n");
        Console.Write($"KITCHEN: Линия блюд: {_mainDishWorkingLine.Count} ед. ожидают приготовления\n");
    }


    public int Time
    {
        get { return _time; }
    }

    public bool HasReadyFood()
    {
        return _readyFood.Count > 0;
    }
}