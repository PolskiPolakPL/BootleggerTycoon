using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string DisplayName;
    public GameObject WorldPrefab;
    public GameObject HandPrefab;
    public bool isThrowable;
}
