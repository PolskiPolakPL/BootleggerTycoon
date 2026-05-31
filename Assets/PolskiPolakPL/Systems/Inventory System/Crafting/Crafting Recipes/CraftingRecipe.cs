using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public record CraftingIngredient
{
    public ItemData item;
    public int amount = 1;
}

[CreateAssetMenu(fileName ="New Recipe", menuName = "ScriptableObject/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftingIngredient> ingredients;
    public ItemData result;
    public int resultAmount = 1;
}
