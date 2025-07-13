using UnityEngine;
using UnityEngine.Events;

public class MovingScript : MonoBehaviour
{

    Structure targetStructure;
    Transform previousT;

    [Header("Raycast")]
    [SerializeField] float range = 100;
    Camera cam;
    Ray ray;
    RaycastHit hit;

    public UnityEvent OnMoveModeEnter;
    public UnityEvent OnMoveModeExit;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
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
            OnMoveModeExit?.Invoke();
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

    void PickUpStructure()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        //if raycast DOESN'T hit ANYTHING
        if (!Physics.Raycast(ray, out hit, range))
            return;

        Transform hitT = hit.transform;
        if (hitT.TryGetComponent<StructureScript>(out StructureScript structureScript))
        {
            previousT = hitT;
            targetStructure = structureScript.StructureSO;
            BuilderManager.Instance.CreatePreview(targetStructure);

            // Temporarily hides prevoius object
            hitT.gameObject.SetActive(false);
        }
    }

    void HandlePickUpPlacement()
    {
        if (targetStructure)
        {
            // Removes prevoius object from scene and occupied list
            if (BuilderManager.Instance.occupiedPositions.Contains(previousT.position))
            {
                BuilderManager.Instance.occupiedPositions.Remove(previousT.position);
            }
            Destroy(previousT.gameObject);

            // Places new object and removes preview
            BuilderManager.Instance.PlaceObject(targetStructure);
            BuilderManager.Instance.DestroyPreview();
            targetStructure = null;
        }
        else
        {
            PickUpStructure();
        }
    }

    void Rollback()
    {
        if (targetStructure) // rollbacks changes
        {
            hit.transform.gameObject.SetActive(true);
            targetStructure = null;
            BuilderManager.Instance.DestroyPreview();
        }
    }
}
