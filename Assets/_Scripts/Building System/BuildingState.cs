using UnityEngine;
using UnityEngine.Events;

public class BuildingState : MonoBehaviour
{

    public UnityEvent OnStateModeEnter;
    public UnityEvent OnStateModeExit;

    public void Activate()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        OnStateModeEnter?.Invoke();
    }
    public void Deactivate()
    {
        Cursor.lockState = CursorLockMode.Confined;
        OnStateModeExit?.Invoke();
        gameObject.SetActive(false);
    }
}
