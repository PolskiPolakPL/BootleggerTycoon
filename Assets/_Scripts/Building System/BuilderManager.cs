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
    [SerializeField] SellingScript selling;
    [SerializeField] MovingScript moving;


    [Header("Raycast")]
    [SerializeField] LayerMask raycastLayerMask;
    public float BuildRange=100;
    Camera cam;
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

        //Camera
        cam=Camera.main;

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
        CreatePreviewMaterial(renderers);
    }

    public void CreatePreview(Structure target, Transform targetT)
    {
        if (previewGO)
            Destroy(previewGO);
        previewGO = Instantiate(target.StructureModel, targetT.position, targetT.rotation, transform);
        previewGO.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = previewGO.GetComponentsInChildren<Renderer>();
        CreatePreviewMaterial(renderers);
    }

    void CreatePreviewMaterial(Renderer[] renderers)
    {
        Material mat;
        UnityEngine.Color color;
        foreach (Renderer renderer in renderers)
        {
            mat = renderer.material;
            color = mat.color;
            color.a = previewOpacity;
            mat.color = color;

            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_Zwrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
        }
    }


}
