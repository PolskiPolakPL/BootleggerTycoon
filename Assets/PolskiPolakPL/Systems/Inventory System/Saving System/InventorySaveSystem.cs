using System.IO;
using UnityEngine;

public class InventorySaveSystem : MonoBehaviour
{
    string path;

    private void Awake()
    {
        path = Application.persistentDataPath + "/inventory.json";
    }

    public void Save()
    {
        InventorySaveData data = new InventorySaveData();

        SlotData slotData;
        foreach(ItemSlot slot in InventorySystem.Instance.playerInventorySlots)
        {
            slotData = new SlotData();

            if (slot.HasItem())
            {
                slotData.itemID = slot.GetItem().ID;
                slotData.amount = slot.GetAmount();
            }
            else
            {
                slotData.itemID = "";
                slotData.amount = 0;
            }

            data.slots.Add(slotData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log($"Saved to {path}!");
    }

    public void Load()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(path);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
        var slots = InventorySystem.Instance.playerInventorySlots;

        SlotData slotData;
        for(int i = 0; i < slots.Count; i++)
        {
            if (i >= data.slots.Count) break;

            slotData = data.slots[i];

            if (!string.IsNullOrEmpty(slotData.itemID))
            {
                ItemData item = ItemDatabase.Instance.GetItem(slotData.itemID);
                slots[i].SetItem(item,slotData.amount);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        Debug.Log("Inventory loaded!");
    }
}
