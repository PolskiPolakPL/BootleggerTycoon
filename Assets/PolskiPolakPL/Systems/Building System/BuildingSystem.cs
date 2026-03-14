using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    //player camera
    [SerializeField] Transform playerCamT;
    public float buildingRange = 3;
    [field:SerializeField] public LayerMask buildOnLayer { get; private set; }

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
        CreatePreview(structureScr.StructureSO);
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
        DestroyPreview();
    }

    void UpdatePreviewPosition()
    {
        ray = new Ray(playerCamT.position, playerCamT.forward);
        //Doesn't hit correct layer
        if (!Physics.Raycast(ray, out RaycastHit hit, buildingRange, buildOnLayer))
        {
            DenyPlacement();
            previewGO.SetActive(false);
            return;
        }
        // else
        CheckValidPlacement();
        previewGO.transform.position = hit.point;
        if (!previewGO.activeInHierarchy)
            previewGO.SetActive(true);
    }

    void CheckValidPlacement()
    {
        PrewiewScript previewScr;
        if(!previewGO.TryGetComponent<PrewiewScript>(out previewScr))
        {
            Debug.LogWarning("Preview has no PreviewScript!");
            return;
        }
        if (previewScr.IsValid())
            AllowPlacement();
        else
            DenyPlacement();
    }

    void DenyPlacement()
    {
        if (!canPlace)
            return;
        canPlace = false;
        SetPreviewMaterial(false);
    }
    void AllowPlacement()
    {
        if (canPlace)
            return;
        canPlace = true;
        SetPreviewMaterial(true);
    }

    public void CreatePreview(StructureSO structureData)
    {
        previewGO = Instantiate(structureData.PreviewPrefab, transform);
        SetPreviewMaterial(canPlace);
    }

    void SetPreviewMaterial(bool isValid)
    {
        Material material;
        if (isValid)
            material = validMaterial;
        else
            material = invalidMaterial;
        Renderer[] renderers = previewGO.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    private void OnDisable()
    {
        if (!HasPreview())
            return;

        CancelMovement();
        DestroyPreview();
    }

    public void CancelMovement()
    {
        if (previousT)
        {
            previousT.gameObject.SetActive(true);
            previousT = null;
        }
        DestroyPreview();
    }

    void DestroyPreview()
    {
        if (previewGO)
            Destroy(previewGO);
    }

    public bool HasPreview()
    {
        return previewGO;
    }
}
