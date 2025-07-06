using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingScript : MonoBehaviour
{
    [SerializeField] List<BuildObject> buildingObjects = new List<BuildObject>();
    [SerializeField] int targetIndex;
    public UnityEvent OnBuildModeEnter;
    public UnityEvent OnBuildModeExit;

    public void SelectObject(int index)
    {
        BuilderManager.Instance.CreatePreview(buildingObjects[index]);
        Cursor.lockState = CursorLockMode.Locked;
        OnBuildModeEnter?.Invoke();
    }
    private void Update()
    {
        BuilderManager.Instance.RenderPreview();

        if (Input.GetKeyDown(KeyCode.E))
        {
            BuilderManager.Instance.RotateObject(-45);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BuilderManager.Instance.RotateObject(45);
        }
        if (Input.GetMouseButton(0))
        {
            BuilderManager.Instance.PlaceObject(buildingObjects[targetIndex]);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            BuilderManager.Instance.DestroyPreview();
            Cursor.lockState = CursorLockMode.Confined;
            OnBuildModeExit?.Invoke();
        }
    }
}
