using UnityEngine;
using TMPro;
/// <summary>
/// Interaction System made with this
/// <seealso href="https://youtu.be/b7Yf6BFx6js">tutorial</seealso>
/// </summary>
public class InteractionUIManager : MonoBehaviour
{
    //Attributes
    [SerializeField] TMP_Text interactionMessage;

    //Singleton statement
    public static InteractionUIManager Instance;
    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void EnableInteractionText(string text)
    {
        interactionMessage.text = text;
        interactionMessage.gameObject.SetActive(true);
    }
    public void DisableInteractionText()
    {
        interactionMessage.gameObject.SetActive(false);
    }

}
