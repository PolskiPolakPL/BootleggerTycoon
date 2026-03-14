using UnityEngine;

public class BaseTool : MonoBehaviour
{
    //Building Ssytem
    protected BuildingSystem BuildSys { get; private set; }
    protected StructureScript SelectedStructure {  get; private set; }
    StructureScript newStructure;

    //Raycast
    Transform playerCamT;
    float range;
    Ray buildRay;

    protected void InitializeTool()
    {
        BuildSys = BuildingSystem.Instance;
        playerCamT = Camera.main.transform;
        range = BuildSys.buildingRange;
    }

    protected void CheckStructureRaycast()
    {
        buildRay = new Ray(playerCamT.position, playerCamT.forward);
        Debug.DrawRay(playerCamT.position, playerCamT.forward * range, Color.yellow);
        // NO HIT AT ALL OR NO HIT STRUCTURE
        if (!Physics.Raycast(buildRay, out RaycastHit hit, range) || !hit.collider.TryGetComponent<StructureScript>(out newStructure))
        {
            DisableCurrentStructure();
            return;
        }
        if (SelectedStructure && SelectedStructure != newStructure)
            DisableCurrentStructure();
        if (newStructure.enabled)
            SetNewCurrentStructure();
        else
            DisableCurrentStructure();
    }

    void SetNewCurrentStructure()
    {
        SelectedStructure = newStructure;
        SelectedStructure.EnableOutline();
    }

    void DisableCurrentStructure()
    {
        if (!SelectedStructure)
            return;
        SelectedStructure.DisableOutline();
        SelectedStructure = null;
    }

    protected void HandleDestroy()
    {
        if (!BuildSys)
            return;
        BuildSys.CancelMovement();
        DisableCurrentStructure();
    }

}
