using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ColorSelectionUI : NetworkBehaviour
{

    public Image colorPreviewImage;
    public Button[] colorButtons;

    private Color selectedColor = Color.white;

    [Header("Color Selection")]
    [SerializeField] private ColorSelectionUI colorSelectionUI = null;
    [SerializeField]
    private GameObject ColorPanel = null;

    private void Start()
    {
        // Initialize color buttons with click events
        foreach (Button button in colorButtons)
        {
            button.onClick.AddListener(() => OnColorButtonClick(button.GetComponent<Image>().color));
        }
    }

    private void OnColorButtonClick(Color color)
    {
        // Set the selected color when a color button is clicked
        selectedColor = color;
        colorPreviewImage.color = color;
    }

    public Color GetSelectedColor()
    {
        return selectedColor;
    }
}
    