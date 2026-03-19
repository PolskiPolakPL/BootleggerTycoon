using UnityEngine;

public class SellToolScript : MonoBehaviour
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
            SellStructure(buildingSystem.SelectedStructure);
    }

    void SellStructure(StructureScript selectedStructure)
    {
        if (selectedStructure)
            Destroy(selectedStructure.gameObject);
    }
    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
