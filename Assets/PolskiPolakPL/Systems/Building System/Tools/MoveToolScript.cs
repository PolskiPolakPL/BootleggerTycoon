using UnityEngine;

public class MoveToolScript : MonoBehaviour
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
        HandleLeftClick();
        HandleRightClick();
    }

    void HandleLeftClick()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
        // NO PREVIEW - Pick up
        if (!buildingSystem.HasPreview() && buildingSystem.SelectedStructure)
        {
            buildingSystem.SelectedStructure.PickUp();
            return;
        }
        // PREVIEW & CAN PLACE - Place
        if(buildingSystem.HasPreview() && buildingSystem.canPlace)
        {
            buildingSystem.MoveStructure();
            return;
        }
    }
    void HandleRightClick()
    {
        if(!Input.GetKeyDown(KeyCode.Mouse1))
            return;
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
    }

    private void OnDestroy()
    {
        if (buildingSystem.HasPreview())
            buildingSystem.CancelPlacement();
        buildingSystem.DisableCurrentStructure();
    }
}
