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
        if(Input.GetKeyDown(KeyCode.Mouse0))
            CastBuildingRay();
    }

    void CastBuildingRay()
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
        buildingSystem.gameObject.SetActive(false);
    }
}
