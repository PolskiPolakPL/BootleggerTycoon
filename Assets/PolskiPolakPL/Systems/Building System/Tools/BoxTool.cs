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

    void StoreStructure(StructureScript selectedStructure)
    {
        if (!selectedStructure)
            return;
        Transform selectedT = selectedStructure.transform;
        Instantiate(selectedStructure.StructureSO.boxPrefab, selectedT.position + Vector3.up, selectedT.rotation);
        Destroy(selectedStructure.gameObject);
    }
    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
