using UnityEngine;

[CreateAssetMenu(fileName ="New BuildObject", menuName = "ScriptableObject/BuildObject")]
public class BuildObject : ScriptableObject
{
    public GameObject BuildPrefab;
    public GameObject BuildModel;
    public float BuildingCost;
    public float SellValue;
}
