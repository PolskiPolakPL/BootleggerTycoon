using UnityEngine;

public class BuildingToolScript : MonoBehaviour
{
    [SerializeField] Transform playerCamT;
    BuildingSystem buildingSystem;
    Ray buildRay;
    StructureScript structure;
    // Start is called before the first frame update
    void Start()
    {
        buildingSystem = BuildingSystem.Instance;
        buildingSystem.gameObject.SetActive(true);
        if (!playerCamT)
            playerCamT = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLeftClick();
    }

    void HandleLeftClick()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
        // NO PREVIEW - Pick up
        if (!buildingSystem.HasPreview())
            TryPickUpStructure();

        // PREVIEW & CAN PLACE - Place
        else if(buildingSystem.canPlace)
            buildingSystem.MoveStructure();
    }

    void TryPickUpStructure()
    {
        buildRay = new Ray(playerCamT.position, playerCamT.forward);

        // NO HIT AT ALL
        if (!Physics.Raycast(buildRay, out RaycastHit hit, buildingSystem.buildingRange))
            return;

        // NO HIT STRUCTURE
        if (!hit.collider.TryGetComponent<StructureScript>(out structure))
            return;

        //HIT STRUCTURE
        structure.PickUp();
    }

    private void OnDestroy()
    {
        if(!buildingSystem)
            return;
        buildingSystem.gameObject.SetActive(false);
    }
}
