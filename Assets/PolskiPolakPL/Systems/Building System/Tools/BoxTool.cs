using UnityEngine;

public class BoxTool : MonoBehaviour
{
    BuildingSystem buildingSystem;
    [SerializeField] GameObject boxPrefab;
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
        GameObject boxGO = Instantiate(boxPrefab, selectedT.position + Vector3.up, selectedT.rotation);
        //boxGO.GetComponent<ItemScript>().itemData = selectedStructure.StructureSO.itemData;
        Destroy(selectedStructure.gameObject);
    }
    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
