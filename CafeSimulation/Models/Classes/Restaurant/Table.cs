using System;
using System.Collections.Generic;
using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes;

public class Table : IComparable<Table>
{
    private int _tableId;
    private int _maxCapacity;
    private bool _isBusy = false;
    private List<Human> _guestsList = [];


    public Table(int id)
    {
        _tableId = id;
        _maxCapacity = 2;
    } // 1 конструктор

    public Table(int id, int n)
    {
        _tableId = id;
        _maxCapacity = n;
    } // 2 конструктор


    public bool PlaceHuman(int n)
    {
        if (n > _maxCapacity || _isBusy)
        {
            return false;
        }

        for (int i = 0; i < n; i++)
        {
            Human chel = new Human(i);
            chel.MakeOrder();
            _guestsList.Add(chel);
        }

        _isBusy = true;
        return true;
    }

    public List<OrderItem> DoOrder()
    {
        List<OrderItem> listOrderItems = new List<OrderItem>();
        foreach (Human guest in _guestsList)
        {
            if (guest.IsWaitingOrder)
            {
                foreach (IFoodObject item in guest.TakeOrder())
                {
                    listOrderItems.Add(new OrderItem(guest.Id, _tableId, item));
                }

                return listOrderItems;
            }
        }

        return listOrderItems;
    }

    public void TakeFood(OrderItem item)
    {
        // foreach (OrderItem item in orderItems)
        // {
        foreach (Human guest in _guestsList)
        {
            if (guest.Id == item.GuestId)
            {
                guest.TakeFood(item.FoodObject);
            }
        }
        // }
    }


    public void Refresh()
    {
        // for (int i = _guestsList.Count - 1; i > 0; i--)
        // {
        //     _guestsList[i].Refresh();
        //     if (_guestsList[i].IsFinished)
        //     {
        //         Console.Write($"выгнали гостя {_guestsList[i].Id} из-за стола №{_tableId}\n");
        //         _guestsList.RemoveAt(i);
        //     }
        // }
        int counter = 0;
        foreach (Human guest in _guestsList)
        {
            guest.Refresh();
            if (guest.IsFinished)
            {
                counter++;
            }
        }

        if (counter == _guestsList.Count)
        {
            _guestsList.Clear();
            _guestsList = new List<Human>();
        }

        if (_guestsList.Count == 0)
        {
            _isBusy = false;
            Console.Write($"TABLE №{_tableId}: СВОБОДЕН\n");
        }
    }

    public bool IsWaitingOrder()
    {
        foreach (Human guest in _guestsList)
        {
            if (guest.IsWaitingOrder)
            {
                return true;
            }
        }

        return false;
    }

    public int TableId
    {
        get { return _tableId; }
    }

    public int MaxCapacity
    {
        get { return _maxCapacity; }
    }

    public bool IsBusy
    {
        get { return _isBusy; }
    }

    public int CompareTo(Table compareTable)
    {
        // A null value means that this object is greater.
        if (compareTable == null)
            return 1;
        else
            return this._maxCapacity.CompareTo(compareTable._maxCapacity);
    }

    public bool Equals(Table other)
    {
        if (other == null) return false;
        return (this._maxCapacity.Equals(other._maxCapacity));
    }


//     public override string ToString()
//     {
//         return string.Format("===\n" +
//                              "maxCapacity:{0}\n" +
//                              "isBusy:{1}\n", _maxCapacity, _isBusy);
//     }
}