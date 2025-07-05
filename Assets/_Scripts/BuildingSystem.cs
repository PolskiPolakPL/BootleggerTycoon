using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask raycastLayerMask;
    [SerializeField] float range=100;
    [SerializeField] Machines targetMachine;
    Ray ray;
    RaycastHit hit;
    GameObject preview;

    private void Start()
    {
        if(!cam)
            cam = Camera.main;
        preview = Instantiate(targetMachine.Preview,transform);
    }
    private void Update()
    {
        RenderPreview();
        if (Input.GetKeyDown(KeyCode.E))
        {
            preview.transform.Rotate(new Vector3(0,45,0));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            preview.transform.Rotate(new Vector3(0, -45, 0));
        }
    }

    void RenderPreview()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range, raycastLayerMask))
        {
            Vector3 newPos = hit.point;
            newPos.x = Mathf.Round(newPos.x);
            newPos.z = Mathf.Round(newPos.z);
            preview.transform.position = newPos;
            if (!preview.activeInHierarchy)
            {
                preview.SetActive(true);
            }
        }
        else
        {
            preview.SetActive(false);
        }
    }
}
