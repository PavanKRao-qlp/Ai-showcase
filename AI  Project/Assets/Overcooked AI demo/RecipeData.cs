using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable/cookai/RecipeData", order = 1)]
public class RecipeData : ScriptableObject
{
    [System.Serializable]
    public struct Ingredient {
        [System.Flags]
        public enum STATE
        {
            NONE = 0,
            RAW = 1,
            SLICED = 2,
            BOILDED = 4,
            FRIED = 8
        }

        public string ID;
        public string Description;
    }

    

    [System.Serializable]
    public struct Recipe {
        [System.Serializable]
        public struct IngredientState
        {
            public string IngredientID;
            public Ingredient.STATE State;
        }

        public List<IngredientState> RecipeInfo;
    }

    public List<Ingredient> Ingredients;
    public List<Recipe> Recipes;
    public Recipe GetRandomRecipe()
    {
        var recipeID = UnityEngine.Random.Range(0, Recipes.Count);
        return Recipes[recipeID];
    }
}
