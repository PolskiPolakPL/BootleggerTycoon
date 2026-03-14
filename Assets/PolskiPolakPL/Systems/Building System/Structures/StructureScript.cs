using UnityEngine;

[RequireComponent(typeof(Outline))]
public class StructureScript : MonoBehaviour, IPickable
{
    public StructureSO StructureSO;
    Outline outline;
    void Start()
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

    private void OnDestroy()
    {
        if (GameManager.Instance)
            GameManager.Instance.Player.GainMoney(StructureSO.Cost);
    }
}