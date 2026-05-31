using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelScript : MonoBehaviour
{
    [Header("Item Description Panel")]
    [SerializeField] GameObject panelGO;
    [SerializeField] RawImage itemImage;
    [SerializeField] TMP_Text itemNameTextField;
    [SerializeField] TMP_Text descriptionTextField;

    public void HandleDescriptionPanel(ItemSlot hoveredSlot)
    {
        if (!hoveredSlot)
        {
            HidePanel();
            return;
        }

        ItemData item = hoveredSlot.GetItem();
        if (!item)
        {
            HidePanel();
            return;
        }
        DisplyPanel(item);
    }

    public void DisplyPanel(ItemData item)
    {
        // set image
        itemImage.texture = item.ImageTexture;
        itemImage.uvRect = item.UVRect;
        // set name
        itemNameTextField.text = item.DisplayName;
        // set description
        descriptionTextField.text = item.Description;

        panelGO.SetActive(true);
    }

    public void HidePanel()
    {
        panelGO.SetActive(false);
    }

}
