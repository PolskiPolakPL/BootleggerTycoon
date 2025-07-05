using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }

    [SerializeField] KeyCode Interaction = KeyCode.F;
    [SerializeField] KeyCode BuildModeKey = KeyCode.B;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(BuildModeKey))
        {
            bool isBuildModeActive = BuildingSystem.Instance.transform.GetChild(0).gameObject.activeInHierarchy;
            if (isBuildModeActive)
            {
                BuildingSystem.Instance.ExitBuildMode();
            }
            else
            {
                BuildingSystem.Instance.EnterBuildMode();
            }
        }
    }
}
