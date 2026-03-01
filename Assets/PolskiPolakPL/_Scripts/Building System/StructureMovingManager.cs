using Unity.VisualScripting;
using UnityEngine;

public class StructureMovingManager : MonoBehaviour
{
    //player camera
    [SerializeField] Transform playerCamT;
    [SerializeField] float buildingRange = 3;
    [SerializeField] LayerMask includeLayer;

    // preview
    [SerializeField] float previewOpacity;
    [SerializeField] float rotateAngleStep = 15;

    GameObject previewGO;
    Transform previousT;
    Ray ray;
    bool canPlace = false;

    public static StructureMovingManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance && Instance!=this)
            Destroy(this.gameObject);
        else
            Instance = this;
        if (!playerCamT)
            playerCamT = Camera.main.transform;
    }

    void Update()
    {
        if (!previewGO)
            return;
        UpdatePreviewPosition();

        if (Input.GetKey(KeyCode.E))
            RotateStructure(rotateAngleStep * Time.deltaTime);

        if(Input.GetKey(KeyCode.Q))
            RotateStructure(-rotateAngleStep * Time.deltaTime);
        
        if (Input.GetMouseButtonDown(0) && canPlace)
            PlaceStructure();

        if (Input.GetMouseButtonDown(1))
            UndoMovement();
    }

    public void PickUpStructure(StructureScript structureScr)
    {
        if(previewGO)
            return;
            previousT = structureScr.transform;
            previewGO = CreatePreview(structureScr.StructureSO);

            // Temporarily hides prevoius object
            previousT.gameObject.SetActive(false);
    }

    void RotateStructure(float angle)
    {
        previewGO.transform.Rotate(new Vector3(0, angle, 0));
    }

    void PlaceStructure()
    {
        // Places new object and removes preview
        Transform preview = previewGO.transform;
        previousT.position = preview.position;
        previousT.rotation = preview.rotation;
        previousT.gameObject.SetActive(true);
        previousT = null;
        Destroy(previewGO);
    }

    void UndoMovement()
    {
        previousT.gameObject.SetActive(true);
        previousT = null;
        Destroy(previewGO);
    }

    void UpdatePreviewPosition()
    {
        ray = new Ray(playerCamT.position, playerCamT.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, buildingRange, includeLayer))
        {
            previewGO.SetActive(false);
            canPlace = false;
            return;
        }
        canPlace = true;
        previewGO.transform.position = hit.point;
        if (!previewGO.activeInHierarchy)
            previewGO.SetActive(true);
    }

    public GameObject CreatePreview(StructureSO structureData)
    {
        GameObject newGO = Instantiate(structureData.StructureModel, transform);
        newGO.name = $"[Preview] {structureData.Name}";
        newGO.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = newGO.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = MaterialMaker.MakePreviewMaterial(renderer.material, previewOpacity);
        }
        return newGO;
    }
}
