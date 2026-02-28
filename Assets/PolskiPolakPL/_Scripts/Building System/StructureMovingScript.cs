using UnityEngine;

public class StructureMovingScript : MonoBehaviour
{
    //player camera
    [SerializeField] Camera playerCam;
    [SerializeField] float buildingRange = 3;
    [SerializeField] LayerMask includeLayer;

    // preview
    [SerializeField] float previewOpacity;
    [SerializeField] float rotateAngleStep = 15;

    GameObject previewGO;
    Transform previousT;
    StructureSO targetStructure;

    private void OnValidate()
    {
        if(!playerCam)
            playerCam = Camera.main;
    }

    void Update()
    {
        RenderPreview();

        if (Input.GetKey(KeyCode.E))
            RotateStructure(rotateAngleStep * Time.deltaTime);
        if(Input.GetKey(KeyCode.Q))
            RotateStructure(-rotateAngleStep * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.X))
        {
            Rollback();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (targetStructure)
                PlaceStructure();
            else
                PickUpStructure();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Rollback();
        }
    }

    void PickUpStructure()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

        //if raycast DOESN'T hit ANYTHING
        if (!Physics.Raycast(ray, out RaycastHit hit, buildingRange))
            return;

        Transform hitT = hit.transform;
        if (hitT.TryGetComponent<StructureScript>(out StructureScript structureScript))
        {
            previousT = hitT;
            targetStructure = structureScript.StructureSO;
            CreatePreview(targetStructure, previousT);

            // Temporarily hides prevoius object
            previousT.gameObject.SetActive(false);
        }
    }

    public void RotateStructure(float angle)
    {
        if (previewGO)
            previewGO.transform.Rotate(new Vector3(0, angle, 0));
    }

    void PlaceStructure()
    {
        // Places new object and removes preview
        if (previewGO)
        {
            Transform preview = previewGO.transform;
            previousT.position = preview.position;
            previousT.rotation = preview.rotation;
            previousT.gameObject.SetActive(true);
            previousT = null;
            if (previewGO)
                Destroy(previewGO);
            targetStructure = null;
        }
    }

    void Rollback()
    {
        if (targetStructure) // rollbacks changes
        {
            previousT.gameObject.SetActive(true);
            targetStructure = null;
            if (previewGO)
                Destroy(previewGO);
        }
    }

    public void RenderPreview()
    {
        if (!previewGO)
            return;

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, buildingRange, includeLayer))
        {
            previewGO.SetActive(false);
            return;
        }

        Vector3 newPos = hit.point;
        previewGO.transform.position = newPos;
        if (!previewGO.activeInHierarchy)
            previewGO.SetActive(true);
    }

    public void CreatePreview(StructureSO target, Transform targetT)
    {
        if (previewGO)
            Destroy(previewGO);
        previewGO = Instantiate(target.StructureModel, targetT.position, targetT.rotation, transform);
        previewGO.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = previewGO.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = MaterialMaker.MakePreviewMaterial(renderer.material, previewOpacity);
        }
    }
}
