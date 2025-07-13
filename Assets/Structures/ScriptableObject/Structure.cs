using UnityEngine;

[CreateAssetMenu(fileName ="New BuildObject", menuName = "ScriptableObject/BuildObject")]
public class Structure : ScriptableObject
{
    public GameObject StructurePrefab;
    public GameObject StructureModel;
    public string Name;
    public float[] BuildingCost; //building cost per level
    public float SellValueMultiplier; //part of cost value per level
}