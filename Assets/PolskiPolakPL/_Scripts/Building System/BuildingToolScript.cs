using UnityEngine;

public class BuildingToolScript : MonoBehaviour
{
    [SerializeField] Transform playerCamT;
    BuildingSystem buildingSystem;
    Ray buildRay;
    StructureScript currentStructure;
    StructureScript newStructure;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
        buildingSystem = BuildingSystem.Instance;
        if (!playerCamT)
            playerCamT = Camera.main.transform;
        range = buildingSystem.buildingRange;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStructureRaycast();
        HandleLeftClick();
    }

    void HandleLeftClick()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
        // NO PREVIEW - Pick up
        if (!buildingSystem.HasPreview() && currentStructure)
            currentStructure.PickUp();

        // PREVIEW & CAN PLACE - Place
        else if(buildingSystem.canPlace)
            buildingSystem.MoveStructure();
    }

    void CheckStructureRaycast()
    {
        buildRay = new Ray(playerCamT.position, playerCamT.forward);
        Debug.DrawRay(playerCamT.position, playerCamT.forward * range, Color.yellow);
        // NO HIT AT ALL OR NO HIT STRUCTURE
        if (!Physics.Raycast(buildRay, out RaycastHit hit, range) || !hit.collider.TryGetComponent<StructureScript>(out newStructure))
        {
            DisableCurrentStructure();
            return;
        }
        if(currentStructure && currentStructure != newStructure)
            DisableCurrentStructure();
        if (newStructure.enabled)
            SetNewCurrentStructure();
        else
            DisableCurrentStructure();
    }

    void SetNewCurrentStructure()
    {
        currentStructure = newStructure;
        currentStructure.EnableOutline();
    }

    void DisableCurrentStructure()
    {
        if (!currentStructure)
            return;
        currentStructure.DisableOutline();
        currentStructure = null;
    }

    private void OnDestroy()
    {
        if(!buildingSystem)
            return;
        DisableCurrentStructure();
    }
}
