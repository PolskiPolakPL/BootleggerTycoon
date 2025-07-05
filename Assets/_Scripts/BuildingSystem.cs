using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }

    public static List<Vector3> OccupiedPositions = new List<Vector3>();


    [Header("Raycast")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] float range=100;

    [Header("Grid Building System")]
    [SerializeField] Transform buildParent;
    [SerializeField] float gridSize = 1;
    [SerializeField] BuildObject machine;
    [SerializeField][Range(0, 1)] float previewOpacity = 0.5f;


    Ray ray;
    RaycastHit hit;
    GameObject preview;

    private void Start()
    {
        if(!cam)
            cam = Camera.main;
        preview = CreatePreview(machine.BuildModel);
        foreach (Transform child in buildParent)
        {
            OccupiedPositions.Add(child.position);
        }
    }
    private void Update()
    {
        RenderPreview();

        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.transform.Rotate(new Vector3(0,45,0));
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!OccupiedPositions.Contains(preview.transform.position))
            {
                Instantiate(machine.BuildPrefab, preview.transform.position,preview.transform.rotation,buildParent);
                OccupiedPositions.Add(preview.transform.position);
            }
        }
    }

    void RenderPreview()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range, raycastLayerMask))
        {
            Vector3 hitPoint = hit.point;
            Vector3 newPos = new Vector3(
                Mathf.Round(hitPoint.x / gridSize) * gridSize,
                Mathf.Round(hitPoint.y / gridSize) * gridSize,
                Mathf.Round(hitPoint.z / gridSize) * gridSize
                );
            preview.transform.position = newPos;
            if (!preview.activeInHierarchy)
            {
                preview.SetActive(true);
            }
        }
        else
        {
            preview.SetActive(false);
        }
    }

    GameObject CreatePreview(GameObject target)
    {
        GameObject obj = Instantiate(target);
        obj.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Material mat;
        Color color;
        foreach (Renderer renderer in renderers)
        {
            mat = renderer.material;
            color = mat.color;
            color.a = previewOpacity;
            mat.color = color;

            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_Zwrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
        return obj;
    }
}
