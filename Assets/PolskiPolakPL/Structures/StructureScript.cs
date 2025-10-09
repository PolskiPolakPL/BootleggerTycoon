using UnityEngine;

public class StructureScript : MonoBehaviour
{
    public Structure StructureSO;
    float currentCost;
    float returnVal;
    float[] levelCosts;
    bool isUpgrateable;
    int currentLevel = 0;
    int maxLevel;
    // Start is called before the first frame update
    void Start()
    {
        levelCosts = StructureSO.BuildingCost;
        maxLevel = levelCosts.Length-1;
        returnVal = GetCurrentSellVal();
        CheckUpgrades();
    }

    void CheckUpgrades()
    {
        if (currentLevel<maxLevel)
            isUpgrateable = true;
        else
            isUpgrateable = false;
    }

    public float GetCurrentCost()
    {
        return levelCosts[currentLevel];
    }

    public float GetCurrentSellVal()
    {
        return currentCost * StructureSO.SellValueMultiplier;
    }

    public void Sell()
    {
        Debug.Log($"Sold {StructureSO.Name} for {GetCurrentSellVal()}");
        Destroy(gameObject);
    }
}
