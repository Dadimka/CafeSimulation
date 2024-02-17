using CafeSimulation.AbstractClasses;

namespace CafeSimulation.Models.Classes.Food;

public class AdditionalDish : IFoodObject
{
    public string name => "Additional Dish";
    public int cookingTime => 7;
    public FoodType type => FoodType.AdditionalDish;

    public override string ToString()
    {
        return $"{name} {cookingTime}";
    }
}