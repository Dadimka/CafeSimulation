using System;
using System.Collections.Generic;

namespace CafeSimulation.Models.Classes;

public class Chef
{
    private OrderItem? _workingItem;
    private int _workingTime = 0;
    private int _id;

    public Chef(int id)
    {
        _id = id;
    }

    public OrderItem? DoWork(ref Queue<OrderItem> workingLine)
    {
        if (_workingTime == 0)
        {
            workingLine.TryDequeue(out _workingItem);
            if (_workingItem is not null)
            {
                _workingTime = _workingItem.FoodObject.cookingTime;
                _workingTime -= 1;
                Console.WriteLine(
                    $"CHEF №{_id}: начал варить {_workingItem.FoodObject.name} для стола №{_workingItem.TableId}. Осталось : {_workingTime} мин.");
            }
            else
            {
                Console.WriteLine($"CHEF №{_id}: нет работы на линии");
            }
        }
        else
        {
            _workingTime -= 1;
            Console.WriteLine(
                $"CHEF №{_id}: варит {_workingItem.FoodObject.name} для стола №{_workingItem.TableId}. Осталось : {_workingTime} мин.");
        }

        if (_workingTime == 0 & _workingItem is not null)
        {
            Console.WriteLine(
                $"CHEF №{_id}: закончил варить {_workingItem.FoodObject.name} для стола №{_workingItem.TableId}.");

            return _workingItem;
        }
        else
        {
            return null;
        }
    }

    public bool IsBusy
    {
        get { return _workingTime > 0; }
    }

    public int Id
    {
        get { return _id; }
    }
}