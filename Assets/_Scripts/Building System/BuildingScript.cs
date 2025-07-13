using UnityEngine;
using UnityEngine.Events;

public class BuildingScript : MonoBehaviour
{
    [SerializeField] Structure targetStructure;
    public UnityEvent OnBuildModeEnter;
    public UnityEvent OnBuildModeExit;

    public void SelectObject(Structure structureSO)
    {
        targetStructure = structureSO;
        BuilderManager.Instance.CreatePreview(structureSO);
        OnBuildModeEnter?.Invoke();
    }
    private void Update()
    {
        BuilderManager.Instance.RenderPreview();

        if (Input.GetKeyDown(KeyCode.E))
        {
            BuilderManager.Instance.RotateObject(45);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BuilderManager.Instance.RotateObject(-45);
        }
        if (Input.GetMouseButton(0))
        {
            BuilderManager.Instance.PlaceObject(targetStructure);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            BuilderManager.Instance.DestroyPreview();
            OnBuildModeExit?.Invoke();
        }
    }
}
