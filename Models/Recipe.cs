using bojan_recipe.Data.Migrations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bojan_recipe.Models
{
    public enum RecipeType
    {
        Food,
        Drink
    }

    public enum FoodType
    {
        Meatarian,
        Vegeterian,
        Vegan
    }

    public class Recipe
    {
        public int Id { get; set; } // Unique identifier for the recipe
        public string Name { get; set; } // Name of the recipe
        public string Description { get; set; } // Description of the recipe
        public string Preparation { get; set; } // Description of the preparation procedure
        public string Ingredients { get; set; } // List of ingredients
        public string ImageUrl { get; set; } // URL to the recipe's image
        public int CaloryCount { get; set; } // Calorie count for the recipe
        public RecipeType Type { get; set; } // Food or Drink

        // If the recipe is for Food:
        public FoodType FoodType { get; set; } // Breakfast, Lunch, Dinner, Dessert, Snack

        // If the recipe is for Drink:
        public bool IsAlcoholic { get; set; } // True if the drink contains alcohol
        public string CreatedBy { get; set; }
    }
}
