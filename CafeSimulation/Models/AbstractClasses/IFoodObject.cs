namespace CafeSimulation.AbstractClasses;

public enum FoodType
{
    Drink,
    AdditionalDish,
    MainDish
}

public interface IFoodObject // интерфейс для объекта Food
{
    public string name { get; }
    public int cookingTime { get; }
    public FoodType type { get; }
}