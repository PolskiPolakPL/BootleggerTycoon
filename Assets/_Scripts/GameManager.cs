using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerScript Player;
    private void Awake()
    {
        if(Instance && Instance!= this)
            Destroy(gameObject);
        else Instance = this;
    }
}
