using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }
    private void Awake()
    {
        //Singleton
        if (Instance && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }

    public void LockCursor(bool locked)
    {
        if (locked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.Confined;
    }
}
