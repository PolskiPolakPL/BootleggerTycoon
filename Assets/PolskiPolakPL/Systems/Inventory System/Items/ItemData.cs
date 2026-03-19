using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string DisplayName;
    public GameObject WorldPrefab;
    public GameObject HandPrefab;
    public Rect UVRect = new Rect(0,0,0.1f,0.1f);
    public bool isThrowable;
}
