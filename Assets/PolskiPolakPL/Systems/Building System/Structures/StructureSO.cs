using UnityEngine;

[CreateAssetMenu(fileName ="New BuildObject", menuName = "ScriptableObject/BuildObject")]
public class StructureSO : ScriptableObject
{
    public string Name;
    public float Cost;
    public GameObject StructurePrefab;
    public GameObject PreviewPrefab;
}