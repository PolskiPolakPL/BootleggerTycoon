using UnityEngine;

[CreateAssetMenu(fileName ="New BuildObject", menuName = "ScriptableObject/BuildObject")]
public class StructureSO : ScriptableObject
{
    public GameObject StructurePrefab;
    public GameObject PreviewPrefab;
    public float Cost;
    public string Name;
}