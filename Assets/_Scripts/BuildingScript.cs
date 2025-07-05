using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    [SerializeField] BuildObject targetObject;

    private void Start()
    {
        BuildingSystem.Instance.CreatePreview(targetObject);
    }
    private void Update()
    {
        BuildingSystem.Instance.RenderPreview();

        if (Input.GetKeyDown(KeyCode.R))
        {
            BuildingSystem.Instance.RotateObject(45);
        }
        if (Input.GetMouseButtonDown(0))
        {
            BuildingSystem.Instance.PlaceObject(targetObject);
        }
    }
}
