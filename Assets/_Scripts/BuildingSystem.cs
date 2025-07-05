using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance;


    [Header("Raycast")]
    [SerializeField] Camera cam;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] float range=100;

    [Header("Grid Building System")]
    [SerializeField] Transform buildParent;
    [SerializeField] float gridSize = 1;
    [SerializeField][Range(0, 1)] float previewOpacity = 0.5f;
    List<Vector3> occupiedPositions = new List<Vector3>();

    Ray ray;
    RaycastHit hit;

    private void Awake()
    {

        //Singleton
        if (Instance && Instance != this)
            Destroy(gameObject);
        else Instance = this;

        //Camera
        if(!cam)
            cam=Camera.main;

        //Add existing positions to list
        foreach (Transform child in BuildingSystem.Instance.buildParent)
        {
            BuildingSystem.Instance.occupiedPositions.Add(child.position);
        }
    }

    public void RenderPreview(GameObject previewGO)
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

    public void PlaceObject(BuildObject target, GameObject previewGO)
    {
        if (!occupiedPositions.Contains(previewGO.transform.position))
        {
            Transform previewT = previewGO.transform;
            Instantiate(target.BuildPrefab, previewT.position, previewT.rotation, BuildingSystem.Instance.buildParent);
            occupiedPositions.Add(previewT.position);
        }
    }

    public GameObject CreatePreview(BuildObject target)
    {
        GameObject previewGO = Instantiate(target.BuildModel);
        previewGO.layer = LayerMask.NameToLayer("Preview");
        Renderer[] renderers = previewGO.GetComponentsInChildren<Renderer>();
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
        return previewGO;
    }


}
