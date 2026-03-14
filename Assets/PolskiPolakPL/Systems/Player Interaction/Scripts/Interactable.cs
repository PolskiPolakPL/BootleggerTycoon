using System;
using UnityEngine;
/// <summary>
/// Interaction System made with this
/// <seealso href="https://youtu.be/b7Yf6BFx6js">tutorial</seealso>
/// </summary>
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{

    public event Action OnInteraction;

    Outline outline;
    public string message;


    public void Interact()
    {
        OnInteraction?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }
}
