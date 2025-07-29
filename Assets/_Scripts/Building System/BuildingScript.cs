using UnityEngine;

public class BuildingScript : BuildingState
{
    Structure targetStructure;

    public void SelectObject(Structure structureSO)
    {
        targetStructure = structureSO;
        BuilderManager.Instance.CreatePreview(structureSO);
    }
    private void Update()
    {
        BuilderManager.Instance.RenderPreview();


        if (Input.GetKeyDown(KeyCode.R))
        {
            BuilderManager.Instance.RotateObject(90);
        }


        if (Input.GetMouseButton(0))
        {
            BuilderManager.Instance.PlaceObject(targetStructure);
            BuilderManager.Instance.OnStructurePlaced?.Invoke();
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            BuilderManager.Instance.DestroyPreview();
            Deactivate();
        }
    }
}
