using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    [SerializeField] BuildObject targetObject;
    GameObject previewGO;

    private void Start()
    {
        previewGO = BuildingSystem.Instance.CreatePreview(targetObject);
    }
    private void Update()
    {
        BuildingSystem.Instance.RenderPreview(previewGO);

        if (Input.GetKeyDown(KeyCode.R))
        {
            previewGO.transform.Rotate(new Vector3(0, 45, 0));
        }
        if (Input.GetMouseButtonDown(0))
        {
            BuildingSystem.Instance.PlaceObject(targetObject, previewGO);
        }
    }
}
