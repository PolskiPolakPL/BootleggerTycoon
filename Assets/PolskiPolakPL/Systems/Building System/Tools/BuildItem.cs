using UnityEngine;

public class BuildItem : MonoBehaviour
{
    [SerializeField] StructureSO structureSO;
    BuildingSystem buildingSystem;
    // Start is called before the first frame update
    void Start()
    {
        buildingSystem = BuildingSystem.Instance;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
            HandleLeftCick();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            HandleRightClick();
    }
    void HandleLeftCick()
    {
        if (!buildingSystem.HasPreview())
        {
            buildingSystem.CreatePreview(structureSO);
            return;
        }
        Build();
    }

    void HandleRightClick()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
    }

    void Build()
    {
        if(!buildingSystem.PlaceStructure(structureSO))
            return;
        InventorySystem.Instance.ConsumeSelectedItem();
    }

    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
