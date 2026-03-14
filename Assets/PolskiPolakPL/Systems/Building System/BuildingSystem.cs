using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    //player camera
    [SerializeField] Transform playerCamT;
    public float buildingRange = 3;
    [SerializeField] LayerMask includeLayer;

    // preview
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField][Tooltip("Angular speed of preview object when rotated. [deg/s]")] float rotateSpeed = 90;

    private GameObject previewGO;
    private Transform previousT;
    private Ray ray;
    public bool canPlace { get; private set; } = false;

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
        if (!HasPreview())
            return;
        UpdatePreviewPosition();

        if (Input.GetKey(KeyCode.E))
            RotateStructure(rotateSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.Q))
            RotateStructure(-rotateSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
            CancelMovement();
    }

    public void PickUpStructure(StructureScript structureScr)
    {
        previousT = structureScr.transform;
        previewGO = CreatePreview(structureScr.StructureSO);
        previewGO.transform.rotation = previousT.rotation;
        // Temporarily hides prevoius object
        previousT.gameObject.SetActive(false);
    }

    void RotateStructure(float angle)
    {
        previewGO.transform.Rotate(new Vector3(0, angle, 0));
    }

    public void MoveStructure()
    {
        // Places new object and removes preview
        Transform preview = previewGO.transform;
        previousT.position = preview.position;
        previousT.rotation = preview.rotation;
        previousT.gameObject.SetActive(true);
        previousT = null;
        Destroy(previewGO);
    }

    public void CancelMovement()
    {
        previousT.gameObject.SetActive(true);
        previousT = null;
        Destroy(previewGO);
    }

    void UpdatePreviewPosition()
    {
        ray = new Ray(playerCamT.position, playerCamT.forward);
        //Doesn't hit correct layer
        if (!Physics.Raycast(ray, out RaycastHit hit, buildingRange, includeLayer))
        {
            canPlace = false;
            SetPreviewMaterial(previewGO);
            previewGO.SetActive(false);
            return;
        }
        // else
        canPlace = true;
        SetPreviewMaterial(previewGO);
        previewGO.transform.position = hit.point;
        if (!previewGO.activeInHierarchy)
            previewGO.SetActive(true);
    }

    public GameObject CreatePreview(StructureSO structureData)
    {
        GameObject newGO = Instantiate(structureData.StructureModel, transform);
        newGO.name = $"[Preview] {structureData.Name}";
        newGO.layer = LayerMask.NameToLayer("Preview");
        SetPreviewMaterial(newGO);
        return newGO;
    }

    void SetPreviewMaterial(GameObject targetGO)
    {
        Material material;
        if (canPlace)
            material = validMaterial;
        else
            material = invalidMaterial;
        Renderer[] renderers = targetGO.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.material = material;
        }
    }

    private void OnDisable()
    {
        if (!HasPreview())
            return;

        if (previousT)
            CancelMovement();
        else
        {
            Destroy(previewGO);
        }
    }

    public bool HasPreview()
    {
        return previewGO;
    }
}
