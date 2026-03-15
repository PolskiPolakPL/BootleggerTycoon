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
            SelectedStructure.PickUp();

        // PREVIEW & CAN PLACE - Place
        else if(BuildSys.canPlace)
            BuildSys.MoveStructure();
    }

    private void OnDestroy()
    {
        HandleDestroy();
    }
}
