using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ColorSelectionUI : NetworkBehaviour
{

    public Image colorPreviewImage;
    public Button[] colorButtons;

    public Color selectedColor = Color.red;
   

    [Header("Color Selection")]
    [SerializeField] private ColorSelectionUI colorSelectionUI = null;
    [SerializeField]
    private GameObject ColorPanel = null;

    public static Color DisplayColor { get; private set; }
    private  string[] PlayerPrefsNameKey = new string[]{"PlayerColorR", "PlayerColorG", "PlayerColorB", "PlayerColorA"};

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
        PlayerPrefs.SetFloat("PlayerColorR", selectedColor.r);
        PlayerPrefs.SetFloat("PlayerColorG", selectedColor.g);
        PlayerPrefs.SetFloat("PlayerColorB", selectedColor.b);
        PlayerPrefs.SetFloat("PlayerColorA", selectedColor.a);
        DisplayColor = _color;
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
    