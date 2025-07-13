using UnityEngine;
using UnityEngine.Events;

public class SellingScript : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] float range = 100;
    Camera cam;
    Ray ray;
    RaycastHit hit;

    public UnityEvent OnSellModeEnter;
    public UnityEvent OnSelldModeExit;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            HandelSelling();
        if(Input.GetKeyDown(KeyCode.X))
            CancelSelling();
    }

    void HandelSelling()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        //if raycast DOESN'T hit ANYTHING
        if (!Physics.Raycast(ray, out hit, range))
            return;

        Transform hitT = hit.transform;
        bool isOnOccupiedList = BuilderManager.Instance.occupiedPositions.Contains(hitT.position);
        if (hitT.TryGetComponent<StructureScript>(out StructureScript structureScript))
        {
            if (isOnOccupiedList)
                BuilderManager.Instance.occupiedPositions.Remove(hitT.position);
            structureScript.Sell();
        }
    }

    void CancelSelling()
    {
        OnSelldModeExit?.Invoke();
    }
}
