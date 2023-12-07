using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ColorSelectionUI : NetworkBehaviour
{

    public Image colorPreviewImage;
    public Button[] colorButtons;

    public Color selectedColor = Color.white;
   

    [Header("Color Selection")]
    [SerializeField] private ColorSelectionUI colorSelectionUI = null;
    [SerializeField]
    private GameObject ColorPanel = null;

    private void Start()
    {
        selectedColor = colorPreviewImage.color;
        // Initialize color buttons with click events
        foreach (Button button in colorButtons)
        {
            button.onClick.AddListener(() => OnColorButtonClick(button.GetComponent<Image>().color));
        }

    }

    public void OnColorButtonClick(Color _color)
    {
        // Set the selected color when a color button is clicked
        selectedColor = _color;
        colorPreviewImage.color = _color;
    }

    public void OnSetColorButtonClick()
    {
        selectedColor = colorPreviewImage.color;
        Debug.Log("colorPreviewImage Colllooor:::" +selectedColor);
        GetSelectedColor();
    }

    public Color GetSelectedColor()
    {
       
        Debug.Log("colorPreviewImage Colllooor!!!!!" + selectedColor);
        return selectedColor;
    }
}
    