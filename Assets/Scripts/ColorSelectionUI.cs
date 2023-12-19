using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ColorSelectionUI : NetworkBehaviour
{
    public Image colorPreviewImage;
    public Button[] colorButtons;

    public Color selectedColor = Color.red;

    [Header("Color Selection")]
    [SerializeField]
    public ColorSelectionUI colorSelectionUI = null;
    [SerializeField]
    private GameObject RoomPlayerPanel;
    [SerializeField]
    private GameObject ColorSelectionPanel;

    [SyncVar(hook = nameof(OnChoosingColor))]
    public bool isButtonInteracting = true;

    public static Color DisplayColor { get; private set; }

    private List<Color> selectedColors = new List<Color>(); // List to store selected colors

    private void Start()
    {
        selectedColor = colorPreviewImage.color;
        ColorSelectionPanel.SetActive(true);
        RoomPlayerPanel.SetActive(false);

        // Initialize color buttons with click events
        foreach (Button button in colorButtons)
        {
            Color buttonColor = button.GetComponent<Image>().color;
            button.onClick.AddListener(() => OnColorButtonClick(buttonColor));
        }
    }

    public void OnChoosingColor(bool _, bool newval)
    {
        isButtonInteracting = newval;
    }

    public void OnColorButtonClick(Color _color)
    {
        if (!selectedColors.Contains(_color))
        {
            // Set the selected color when a color button is clicked
            selectedColor = _color;
            colorPreviewImage.color = _color;
            PlayerPrefs.SetFloat("PlayerColorR", selectedColor.r);
            PlayerPrefs.SetFloat("PlayerColorG", selectedColor.g);
            PlayerPrefs.SetFloat("PlayerColorB", selectedColor.b);
            PlayerPrefs.SetFloat("PlayerColorA", selectedColor.a);
            DisplayColor = _color;

            // Add the selected color to the list and disable the button
            selectedColors.Add(_color);
            SetButtonInteractable(_color, false);
        }
    }

    private void SetButtonInteractable(Color color, bool interactable)
    {
        foreach (Button button in colorButtons)
        {
            if (button.GetComponent<Image>().color == color)
            {
                button.interactable = interactable;
                break;
            }
        }
    }

    public void OnSetColorButtonClick()
    {
        // Activate the player panel and deactivate the color selection panel
       // ColorSelectionPanel.SetActive(false);
    //    RoomPlayerPanel.SetActive(true);
        GetSelectedColor();
    }

    public Color GetSelectedColor()
    {
        return selectedColor;
    }
}
