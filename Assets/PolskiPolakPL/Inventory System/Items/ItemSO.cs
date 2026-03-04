using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public GameObject ItemPrefab;
    public GameObject HandItemPrefab;
    public GameObject UIPrefab;
    public bool isThrowable;
}
