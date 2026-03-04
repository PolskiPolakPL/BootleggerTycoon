using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{
    public GameObject ItemPrefab;
    public GameObject EquippedPrefab;
    public RawImage UIRawImage;
}
