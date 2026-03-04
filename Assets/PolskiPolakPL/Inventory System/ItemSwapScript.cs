using UnityEngine;

public class ItemSwapScript : MonoBehaviour
{
    [SerializeField] int selectedIndex = 0;
    public Transform activeHand;
    // Start is called before the first frame update
    void Awake()
    {
        SetActiveHand(selectedIndex);
    }

    // Update is called once per frame
    void Update()
    {

        int previousSelectedIndex = selectedIndex;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedIndex++;
            selectedIndex %= transform.childCount;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex <= 0)
                selectedIndex = transform.childCount - 1;
            else
                selectedIndex--;
        }

        if (selectedIndex != previousSelectedIndex)
        {
            SetActiveHand(selectedIndex);
        }
            
    }

    void SetActiveHand(int selectedIndex)
    {
        int i = 0;
        foreach (Transform handSlot in transform)
        {
            if (selectedIndex == i)
            {
                handSlot.gameObject.SetActive(true);
                activeHand = handSlot;
            }
            else
                handSlot.gameObject.SetActive(false);
            i++;
        }
    }
}
