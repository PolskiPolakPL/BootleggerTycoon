using UnityEngine;

[CreateAssetMenu(fileName ="New Machine", menuName = "ScriptableObject/Machine")]
public class Machines : ScriptableObject
{
    public GameObject Machine;
    public GameObject Preview;
    public float BuildingCost;
}
