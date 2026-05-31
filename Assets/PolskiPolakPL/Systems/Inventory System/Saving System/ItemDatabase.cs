using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public record InventorySaveData
{
    public List<SlotData> slots = new List<SlotData>();
}

[System.Serializable]
public record SlotData
{
    public string itemID;
    public int amount;
}


public class ItemDatabase : MonoBehaviour
{
    [SerializeField] List<ItemData> allItems = new List<ItemData>();

    Dictionary<string,ItemData> itemLookup = new Dictionary<string,ItemData>();

    // Singleton Instance
    public static ItemDatabase Instance { get; private set; }
    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;


        foreach (ItemData item in allItems)
        {
            itemLookup[item.ID] = item;
        }
    }

    public ItemData GetItem(string id)
    {
        return itemLookup.ContainsKey(id) ? itemLookup[id] : null;
    }

}
