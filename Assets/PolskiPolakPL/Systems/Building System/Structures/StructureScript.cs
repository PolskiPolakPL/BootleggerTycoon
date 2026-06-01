using UnityEngine;

[RequireComponent(typeof(Outline))]
public class StructureScript : MonoBehaviour, IPickable
{
    public StructureSO StructureSO;
    [field: SerializeField] public ItemData itemData {  get; private set; }
    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void PickUp()
    {
        BuildingSystem.Instance.PickUpStructure(this);
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }
}