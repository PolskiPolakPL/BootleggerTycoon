using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] int selectedItemSlotIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        SwitchItemSlot(selectedItemSlotIndex);
    }

    // Update is called once per frame
    void Update()
    {

        int previousSelectedWeapon = selectedItemSlotIndex;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedItemSlotIndex++;
            selectedItemSlotIndex = selectedItemSlotIndex % transform.childCount;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedItemSlotIndex <= 0)
                selectedItemSlotIndex = transform.childCount - 1;
            else
                selectedItemSlotIndex--;
        }

        if (selectedItemSlotIndex != previousSelectedWeapon)
            SwitchItemSlot(selectedItemSlotIndex);
    }

    void SwitchItemSlot(int selectedWeapon)
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (selectedWeapon == i)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
