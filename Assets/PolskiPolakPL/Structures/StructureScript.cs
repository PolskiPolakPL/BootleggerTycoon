using UnityEngine;
public class StructureScript : MonoBehaviour, IPickable
{
    public StructureSO StructureSO;

    public void PickUp()
    {
        BuildingSystem.Instance.PickUpStructure(this);
    }
}