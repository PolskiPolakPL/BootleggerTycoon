using UnityEngine;

public class BuildTool : BaseTool
{
    [SerializeField] StructureSO structureSO;
    // Start is called before the first frame update
    void Start()
    {
        InitializeTool();
        BuildSys.CreatePreview(structureSO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        HandleDestroy();
    }
}
