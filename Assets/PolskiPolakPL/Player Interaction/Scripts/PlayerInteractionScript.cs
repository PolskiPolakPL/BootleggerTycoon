using UnityEngine;

/// <summary>
/// Interaction System made with this
/// <seealso href="https://youtu.be/b7Yf6BFx6js">tutorial</seealso>
/// </summary>
public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] Transform cameraT;
    [SerializeField] float playerReach = 2;
    public KeyCode interactionKey = KeyCode.E;

    Interactable currentInteractable;
    Interactable newInteractable;
    RaycastHit hit;
    Ray ray;
    public Transform interactiontHitT {  get; private set; }

    private void Start()
    {
        if (!cameraT)
            cameraT = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetKeyDown(interactionKey) && currentInteractable)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        ray = new Ray(cameraT.position, cameraT.forward);
        Debug.DrawRay(cameraT.position, cameraT.forward * playerReach,Color.blue);
        if (!Physics.Raycast(ray, out hit, playerReach) || !hit.collider.TryGetComponent<Interactable>(out newInteractable))
        {
            DisableCurrentInteractable();
            return;
        }
        if(currentInteractable && newInteractable != currentInteractable)
        {
            DisableCurrentInteractable();
        }
        if (newInteractable.enabled)
        {
            SetNewCurrentInteractable(newInteractable);
        }
        else
        {
            DisableCurrentInteractable();
        }

    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        if (InteractionUIManager.Instance)
            InteractionUIManager.Instance.EnableInteractionText(currentInteractable.message);

    }
    void DisableCurrentInteractable()
    {
        if (InteractionUIManager.Instance)
            InteractionUIManager.Instance.DisableInteractionText();
        if (!currentInteractable)
            return;
        currentInteractable.DisableOutline();
        currentInteractable = null;
    }
}

