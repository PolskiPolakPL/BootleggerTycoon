using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;
    public GameObject WorldPrefab;
    public GameObject HandPrefab;
    public bool isThrowable;
}
