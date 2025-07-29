using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class BuilderManager : MonoBehaviour
{
    public static BuilderManager Instance;
    [Header("Script References")]
    [SerializeField] BuildingScript building;
    [SerializeField] MovingScript moving;
    public UnityEvent OnStructurePlaced;
    [SerializeField] SellingScript selling;
    public UnityEvent OnStructureSold;


    [Header("Raycast")]
    [SerializeField] LayerMask raycastLayerMask;
    public float BuildRange=100;
    public Camera cam;
    Ray ray;
    RaycastHit hit;
    GameObject previewGO;

    [Header("Grid System")]
    [SerializeField] Transform buildParent;
    [SerializeField] float gridSize = 1;
    public List<Vector3> occupiedPositions = new List<Vector3>();

    [Header("Building System")]
    [SerializeField] GameObject buildView;
    [SerializeField][Range(0, 1)] float previewOpacity = 0.5f;
    public UnityEvent OnBuilderViewEnter;
    public UnityEvent OnBuilderViewEnd;


    private void Awake()
    {

        //Singleton
        if (Instance && Instance != this)
            Destroy(gameObject);
        else Instance = this;

        //Add existing positions to list
        foreach (Transform child in BuilderManager.Instance.buildParent)
        {
            BuilderManager.Instance.occupiedPositions.Add(child.position);
        }
    }

    public void SwitchBuilderView()
    {
        if (buildView.activeInHierarchy)// BuilderView OFF
        {
            building.gameObject.SetActive(false);
            moving.gameObject.SetActive(false);
            selling.gameObject.SetActive(false);
            DestroyPreview();
            OnBuilderViewEnd?.Invoke();
        }
        else // BuilderView ON
        {
            OnBuilderViewEnter?.Invoke();
        }
    }

    public void DestroyPreview()
    {
        if(previewGO)
            Destroy(previewGO);
    }

    public void RotateObject(float angle)
    {
        if(previewGO)
            previewGO.transform.Rotate(new Vector3(0, angle, 0));
    }

    public void RenderPreview()
    {
        if (!previewGO)
            return;
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, BuildRange, raycastLayerMask))
        {
            Vector3 hitPoint = hit.point;
            Vector3 newPos = new Vector3(
                Mathf.Round(hitPoint.x / gridSize) * gridSize,
                Mathf.Round(hitPoint.y / gridSize) * gridSize,
                Mathf.Round(hitPoint.z / gridSize) * gridSize
                );
            previewGO.transform.position = newPos;
            if (!previewGO.activeInHierarchy)
            {
                previewGO.SetActive(true);
            }
        }
        else
        {
            previewGO.SetActive(false);
        }
    }

    public bool PlaceObject(Structure target)
    {
        if (CanPlace())
        {
            Transform previewT = previewGO.transform;
            Instantiate(target.StructurePrefab, previewT.position, previewT.rotation, buildParent);
            occupiedPositions.Add(previewT.position);
            return true;
        }
        return false;
    }

    public bool CanPlace()
    {
        return previewGO && !occupiedPositions.Contains(previewGO.transform.position);
    }

    public Transform GetPreviewTransform()
    {
        return previewGO.transform;
    }

    public void CreatePreview(Structure target)
    {
        if(previewGO)
            Destroy(previewGO);
        previewGO = Instantiate(target.StructureModel, transform);
        previewGO.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = previewGO.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = GetPreviewMaterial(renderer.material);
        }
    }

    public void CreatePreview(Structure target, Transform targetT)
    {
        if (previewGO)
            Destroy(previewGO);
        previewGO = Instantiate(target.StructureModel, targetT.position, targetT.rotation, transform);
        previewGO.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = previewGO.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = GetPreviewMaterial(renderer.material);
        }
    }

    Material GetPreviewMaterial(Material original)
    {
        Material mat = new Material(original);

        mat.shader = Shader.Find("Standard");    // Set "Standard" Shader
        mat.name = "prev-"+original.name;        // Material name
        mat.SetFloat("_Mode", 3f);               // Rendering Mode: Transparent
        mat.renderQueue = 3000;                  // Render Queue
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        UnityEngine.Color color = mat.color;
        color.a = previewOpacity;
        mat.color = color;

        return mat;
    }


}
