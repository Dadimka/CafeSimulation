using System;
using System.Collections.Generic;
using System.Data.Common;
using CafeSimulation.AbstractClasses;
using CafeSimulation.Models.Classes.Food;

namespace CafeSimulation.Models.Classes;

public class Human
{
    private int _id;
    private List<IFoodObject> _order = new List<IFoodObject>();
    private List<IFoodObject> _food = new List<IFoodObject>();
    private bool _isWaitingFood = false;
    private bool _isWaitingOrder = false;
    private bool _isEating = false;
    private bool _isFinished = false;

    private int _eatingTime = -1;


    public Human(int id)
    {
        _id = id;
    }

    public void MakeOrder()
    {
        Console.Write($"HUMAN №{_id}: придумал заказ\n");
        _isWaitingOrder = true;
        Drink napitok = new Drink();
        MainDish eda = new MainDish();
        AdditionalDish salad = new AdditionalDish();
        _order = [napitok, eda, salad];
    }

    public List<IFoodObject> Order()
    {
        return _order;
    }

    public List<IFoodObject> TakeOrder()
    {
        Console.Write($"HUMAN №{_id}: мой заказ взяли\n");
        _isWaitingOrder = false;
        _isWaitingFood = true;
        return _order;
    }

    public void TakeFood(IFoodObject foodObject)
    {
        _order.Remove(foodObject);
        _food.Add(foodObject);
        if (_order.Count == 0)
        {
            _eatingTime = 15;
            _isEating = true;
            _isWaitingFood = false;
        }
    }


    public void Refresh()
    {
        if (_eatingTime == 0)
        {
            _isFinished = true;
            _isEating = false;
        }

        if (_isEating)
        {
            _eatingTime--;
        }
    }

    public bool IsWaitingFood
    {
        get { return _isWaitingFood; }
    }

    public bool IsWaitingOrder
    {
        get { return _isWaitingOrder; }
    }

    public bool IsEating
    {
        get { return _isEating; }
    }

    public bool IsFinished
    {
        get { return _isFinished; }
    }

    public int Id
    {
        get { return _id; }
    }

    // public override string ToString()
    // {
    //     return string.Format("===\n" +
    //                          "human\n" +
    //                          "isWaitingFood: {0}\n" +
    //                          "isWaitingOrder: {1}\n" +
    //                          "order: {2}\n", _isWaitingFood, _isWaitingOrder, _order);
    // }
}