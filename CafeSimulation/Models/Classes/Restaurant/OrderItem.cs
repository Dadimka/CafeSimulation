using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes;

public class OrderItem
{
    private int _guestId;
    private int _tableId;
    private int _orderTime;
    private IFoodObject _foodObject;

    public OrderItem(int idH, int idT, IFoodObject item)
    {
        _guestId = idH;
        _tableId = idT;
        _foodObject = item;
    }

    public int GuestId
    {
        get { return _guestId; }
    }

    public int TableId
    {
        get { return _tableId; }
    }

    public int OrderTime
    {
        get { return _orderTime; }

        set { _orderTime = value; }
    }

    public IFoodObject FoodObject
    {
        get { return _foodObject; }
    }
}