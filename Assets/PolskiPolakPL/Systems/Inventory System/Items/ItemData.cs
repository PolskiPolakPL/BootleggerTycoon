using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public string ID;
    public string DisplayName;
    [TextArea] public string Description;
    public GameObject WorldPrefab;
    public GameObject HandPrefab;
    [Header("Miscellaneous Stats")]
    [Min(1)] public int StackSize = 1;
    public int Durability;
    [Header("UI")]
    public Texture ImageTexture;
    public Rect UVRect = new Rect(0,0,0.1f,0.1f);
}
