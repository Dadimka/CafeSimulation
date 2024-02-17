using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes.Food;

public class MainDish : IFoodObject
{
    public string name => "Main Dish";
    public int cookingTime => 15;
    public FoodType type => FoodType.MainDish;

    public override string ToString()
    {
        return $"{name} {cookingTime}";
    }
}