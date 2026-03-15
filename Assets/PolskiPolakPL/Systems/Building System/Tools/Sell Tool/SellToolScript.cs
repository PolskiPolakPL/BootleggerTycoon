using UnityEngine;

public class SellToolScript : BaseTool
{
    // Start is called before the first frame update
    private void Awake()
    {
        InitializeTool();
    }

    // Update is called once per frame
    void Update()
    {
        CheckStructureRaycast();
        if (Input.GetKeyDown(KeyCode.Mouse0))
            SellStructure();
    }

    void SellStructure()
    {
        if (SelectedStructure)
            Destroy(SelectedStructure.gameObject);
    }

    private void OnDestroy()
    {
        HandleDestroy();
    }
}
