using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes.Food;

public class Drink : IFoodObject
{
    public string name => "Drink";
    public int cookingTime => 2;
    public FoodType type => FoodType.Drink;

    public override string ToString()
    {
        return $"{name} {cookingTime}";
    }
}