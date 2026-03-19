using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    //player camera
    [SerializeField] Transform playerCamT;
    public float buildRange = 3;
    [field:SerializeField] public LayerMask buildOnLayer { get; private set; }

    // preview
    [SerializeField] Material validMaterial;
    [SerializeField] Material invalidMaterial;
    [SerializeField][Tooltip("Angular speed of preview object when rotated. [deg/s]")] float rotateSpeed = 90;

    public StructureScript SelectedStructure{ get; private set; }
    StructureScript newStructure;

    private GameObject previewGO;
    private Transform previousT;
    private Ray buildRay;
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
        CheckBuildingRaycast();
        if (!HasPreview())
            return;

        if (Input.GetKey(KeyCode.E))
            RotatePreview(rotateSpeed * Time.deltaTime);

        if(Input.GetKey(KeyCode.Q))
            RotatePreview(-rotateSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(1) && previousT)
            CancelPlacement();
    }

    public void PickUpStructure(StructureScript structureScr)
    {
        previousT = structureScr.transform;
        CreatePreview(structureScr.StructureSO);
        previewGO.transform.rotation = previousT.rotation;
        // Temporarily hides prevoius object
        previousT.gameObject.SetActive(false);
    }

    void RotatePreview(float angle)
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

    public bool PlaceStructure(StructureSO structureData)
    {
        if (!previewGO || !canPlace)
            return false;
        Transform previewT = previewGO.transform;
        Instantiate(structureData.StructurePrefab, previewT.position, previewT.rotation);
        DestroyPreview();
        return true;
    }


    void CheckValidPlacement()
    {
        if (IsPlaceSpotValid())
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

    public void CancelPlacement()
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
        previewGO = null;
    }

    public bool HasPreview()
    {
        return previewGO;
    }

    bool IsPlaceSpotValid()
    {
        return !IsPreviewColliding();
    }

    // ChatGPT's Method (probably needs fixing/optimizing)
    bool IsPreviewColliding()
    {
        Collider[] ownColliders = previewGO.GetComponentsInChildren<Collider>();
        foreach (var own in ownColliders)
        {
            Collider[] hits = Physics.OverlapBox(
                own.bounds.center,
                own.bounds.extents,
                own.transform.rotation,
                ~BuildingSystem.Instance.buildOnLayer
            );

            foreach (var hit in hits)
            {
                if (!hit.GetComponent<StructureScript>())
                    continue;
                if (Physics.ComputePenetration(
                    own, own.transform.position, own.transform.rotation,
                    hit, hit.transform.position, hit.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    return true;
                }
            }
        }
        return false;
    }


    // EXPERIMANTEAL

    void CheckBuildingRaycast()
    {
        buildRay = new Ray(playerCamT.position, playerCamT.forward);
        if (HasPreview())
        {
            UpdatePreviewPosition();
        }
        else
        {
            HandleStructureSelection();
        }

    }
    void UpdatePreviewPosition()
    {
        if (!Physics.Raycast(buildRay, out RaycastHit hit, buildRange, buildOnLayer))
        {
            DenyPlacement();
            previewGO.SetActive(false);
            return;
        }
        // else
        previewGO.transform.position = hit.point;
        CheckValidPlacement();
        if (!previewGO.activeInHierarchy)
            previewGO.SetActive(true);
    }

    void HandleStructureSelection()
    {
        if (!Physics.Raycast(buildRay, out RaycastHit hit, buildRange) || !hit.collider.TryGetComponent<StructureScript>(out newStructure))
        {
            DisableCurrentStructure();
            return;
        }
        if (SelectedStructure && SelectedStructure != newStructure)
            DisableCurrentStructure();
        if (newStructure.enabled)
            SetNewCurrentStructure();
        else
            DisableCurrentStructure();
    }

    void SetNewCurrentStructure()
    {
        SelectedStructure = newStructure;
        SelectedStructure.EnableOutline();
    }

    public void DisableCurrentStructure()
    {
        if (!SelectedStructure)
            return;
        SelectedStructure.DisableOutline();
        SelectedStructure = null;
    }
}
