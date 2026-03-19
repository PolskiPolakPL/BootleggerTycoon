using UnityEngine;

public class BuildTool : MonoBehaviour
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
        if(!buildingSystem.HasPreview())
            buildingSystem.CreatePreview(structureSO);
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Build();
    }

    void Build()
    {
        if(!buildingSystem.PlaceStructure(structureSO))
            return;
        InventorySystem.Instance.ClearSelectedSlot();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
