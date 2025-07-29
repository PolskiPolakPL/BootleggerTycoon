using UnityEngine;

public class SellingScript : BuildingState
{
    float range;

    // Start is called before the first frame update
    void Start()
    {
        range = BuilderManager.Instance.BuildRange;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
            HandelSelling();

        if(Input.GetKeyDown(KeyCode.X))
            CancelSelling();
    }

    void HandelSelling()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if raycast DOESN'T hit ANYTHING
        if (!Physics.Raycast(ray, out RaycastHit hit, range))
            return;

        Transform hitT = hit.transform;
        bool isOnOccupiedList = BuilderManager.Instance.occupiedPositions.Contains(hitT.position);
        if (hitT.TryGetComponent<StructureScript>(out StructureScript structureScript))
        {
            if (isOnOccupiedList)
                BuilderManager.Instance.occupiedPositions.Remove(hitT.position);
            structureScript.Sell();
            BuilderManager.Instance.OnStructureSold?.Invoke();
        }
    }

    void CancelSelling()
    {
        Deactivate();
    }
}
