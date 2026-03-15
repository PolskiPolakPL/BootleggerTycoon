using UnityEngine;

public class BuildTool : BaseTool
{
    [SerializeField] StructureSO structureSO;
    // Start is called before the first frame update
    void Start()
    {
        InitializeTool();
    }

    private void Update()
    {
        if(!BuildSys.HasPreview())
            BuildSys.CreatePreview(structureSO);
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Build();
    }

    void Build()
    {
        if(!BuildSys.PlaceStructure(structureSO))
            return;
        InventorySystem.Instance.ClearSelectedSlot();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        HandleDestroy();
    }
}
