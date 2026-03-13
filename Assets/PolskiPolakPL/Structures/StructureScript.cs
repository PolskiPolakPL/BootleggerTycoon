using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class StructureScript : MonoBehaviour, IPickable
{
    public StructureSO StructureSO;
    [SerializeField] Interactable interactable;

    private void OnValidate()
    {
        interactable = GetComponent<Interactable>();
    }

    private void Awake()
    {
        interactable.OnInteraction += PickUp;
    }

    public void PickUp()
    {
        BuildingSystem.Instance.PickUpStructure(this);
    }
}