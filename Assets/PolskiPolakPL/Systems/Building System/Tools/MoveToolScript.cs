using UnityEngine;

public class MoveToolScript : BaseTool
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeTool();
    }

    // Update is called once per frame
    void Update()
    {
        CheckStructureRaycast();
        HandleLeftClick();
    }

    void HandleLeftClick()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;
        // NO PREVIEW - Pick up
        if (!BuildSys.HasPreview() && SelectedStructure)
        {
            SelectedStructure.PickUp();
            return;
        }
        // PREVIEW & CAN PLACE - Place
        if(BuildSys.HasPreview() && BuildSys.canPlace)
        {
            BuildSys.MoveStructure();
            return;
        }
    }

    private void OnDestroy()
    {
        HandleDestroy();
    }
}
