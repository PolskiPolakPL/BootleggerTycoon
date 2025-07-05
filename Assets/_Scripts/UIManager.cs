using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;



    [SerializeField] TMP_Text moneyLabel;

    PlayerScript playerScript;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }
    private void Start()
    {
        playerScript = GameManager.Instance.Player;
        playerScript.OnPlayerGainMoney += UpdateMoneyLabel;
        playerScript.OnPlayerLooseMoney += UpdateMoneyLabel;
        UpdateMoneyLabel();
    }

    void UpdateMoneyLabel()
    {
        moneyLabel.text = playerScript.GetMoney() + "$";
    }

    private void OnDestroy()
    {
        playerScript.OnPlayerLooseMoney -= UpdateMoneyLabel;
        playerScript.OnPlayerGainMoney -= UpdateMoneyLabel;
    }
}
