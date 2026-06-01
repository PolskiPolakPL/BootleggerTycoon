using UnityEngine;

public class BoxTool : MonoBehaviour
{
    BuildingSystem buildingSystem;
    // Start is called before the first frame update
    void Start()
    {
        buildingSystem = BuildingSystem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StoreStructure(buildingSystem.SelectedStructure);
    }

    void StoreStructure(StructureScript selectedStruct)
    {
        if (!selectedStruct)
            return;
        Transform selectedT = selectedStruct.transform;
        GameObject boxGO = Instantiate(selectedStruct.itemData.WorldPrefab, selectedT.position + Vector3.up, selectedT.rotation);
        Destroy(selectedStruct.gameObject);
    }
    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
