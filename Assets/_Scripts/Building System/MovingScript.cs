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

    public UnityEvent OnBuildModeEnter;
    public UnityEvent OnBuildModeExit;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PickUpStructure();
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
            hitT.gameObject.SetActive(false);
        }
    }
}
