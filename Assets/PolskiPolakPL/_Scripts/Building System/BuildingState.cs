using UnityEngine;
using UnityEngine.Events;

public class BuildingState : MonoBehaviour
{

    public UnityEvent OnStateModeEnter;
    public UnityEvent OnStateModeExit;

    public void Activate()
    {
        gameObject.SetActive(true);
        OnStateModeEnter?.Invoke();
    }
    public void Deactivate()
    {
        OnStateModeExit?.Invoke();
        gameObject.SetActive(false);
    }
}
