using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public GameObject PropPrafab;
    public GameObject ItemPrefab;
    public GameObject EquippedPrefab;
}
