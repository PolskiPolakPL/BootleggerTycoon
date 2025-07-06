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

    //[SerializeField] KeyCode Interaction = KeyCode.F;
    [SerializeField] KeyCode BuildKey = KeyCode.B;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(BuildKey))
        {
            BuilderManager.Instance.SwitchBuilderView();
        }
    }
}
