using UnityEngine;

public class MovingScript : BuildingState
{
    Structure targetStructure;
    Transform previousT;
        float range;
    // Start is called before the first frame update
    void Start()
    {
        range = BuilderManager.Instance.BuildRange;
    }

    // Update is called once per frame
    void Update()
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


        if (Input.GetKeyDown(KeyCode.X))
        {
            Rollback();
            Deactivate();
        }


        if (Input.GetMouseButtonDown(0))
        {
            HandlePickUpPlacement();
        }


        if (Input.GetMouseButtonDown(1))
        {
            Rollback();
        }
    }

    void HandlePickUpPlacement()
    {
        if (targetStructure)
        {
            PlaceStructure();
        }
        else
        {
            PickUpStructure();
        }
    }

    void PlaceStructure()
    {
        // Places new object and removes preview
        if (BuilderManager.Instance.PlaceObject(targetStructure))
        {
            BuilderManager.Instance.DestroyPreview();
            targetStructure = null;
            //removes previous object
            Destroy(previousT.gameObject);
        }
    }

    void PickUpStructure()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if raycast DOESN'T hit ANYTHING
        if (!Physics.Raycast(ray, out RaycastHit hit, range))
            return;

        Transform hitT = hit.transform;
        if (hitT.TryGetComponent<StructureScript>(out StructureScript structureScript))
        {
            previousT = hitT;
            targetStructure = structureScript.StructureSO;
            BuilderManager.Instance.CreatePreview(targetStructure);
            if (BuilderManager.Instance.occupiedPositions.Contains(previousT.position))
                BuilderManager.Instance.occupiedPositions.Remove(previousT.position);

                // Temporarily hides prevoius object
                hitT.gameObject.SetActive(false);
        }
    }

    void Rollback()
    {
        if (targetStructure) // rollbacks changes
        {
            if (!BuilderManager.Instance.occupiedPositions.Contains(previousT.position))
                BuilderManager.Instance.occupiedPositions.Add(previousT.position);
            previousT.gameObject.SetActive(true);
            targetStructure = null;
            BuilderManager.Instance.DestroyPreview();
        }
    }
}
