using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;

public class ColorSelectionUI : NetworkBehaviour
{
    public Image colorPreviewImage;
    public Button[] colorButtons;

    public Color selectedColor = Color.red;

    [Header("Color Selection")]
    [SerializeField]
    private GameObject RoomPlayerPanel;
    [SerializeField]
    private GameObject ColorSelectionPanel;

    [SyncVar(hook = nameof(OnChoosingColor))]
    public bool isButtonInteracting = true;

    public static Color DisplayColor { get; private set; }

    private Dictionary<Color, bool> colorAvailability = new Dictionary<Color, bool>();

    private void Start()
    {
       
        ColorSelectionPanel.SetActive(true);
        RoomPlayerPanel.SetActive(false);

        // Initialize color buttons with click events
        foreach (Button button in colorButtons)
        {
            Color buttonColor = button.GetComponent<Image>().color;
            button.onClick.AddListener(() => OnColorButtonClick(buttonColor));
        }

        InitializeColorAvailability();
    }

    private void InitializeColorAvailability()
    {
        // Initialize color availability dictionary
        foreach (Button button in colorButtons)
        {
            Color buttonColor = button.GetComponent<Image>().color;
            colorAvailability[buttonColor] = true;
        }
    }

    public void OnChoosingColor(bool _, bool newval)
    {
         isButtonInteracting = newval;
    }

    public void OnColorButtonClick(Color _color)
    {
        if (isButtonInteracting && colorAvailability.ContainsKey(_color))
        {
            // Set the selected color when a color button is clicked
            selectedColor = _color;
            colorPreviewImage.color = _color;
            PlayerPrefs.SetFloat("PlayerColorR", selectedColor.r);
            PlayerPrefs.SetFloat("PlayerColorG", selectedColor.g);
            PlayerPrefs.SetFloat("PlayerColorB", selectedColor.b);
            PlayerPrefs.SetFloat("PlayerColorA", selectedColor.a);
            DisplayColor = _color;

            // Reserve the selected color
            colorAvailability[_color] = false;

            // Notify the server about the color selection
            CmdSelectColor(_color);
        }
    }

    [Command]
    private void CmdSelectColor(Color color)
    {
        selectedColor = color;
        DisplayColor = color;

        // colorAvailability[color] = false;
        
       RpcUpdateSelectedColor(color);   // Notify all clients about the color selection
    }

    [ClientRpc]
    private void RpcUpdateSelectedColor(Color color)
    {                           // Update the selected color on all clients
        selectedColor = color;
        DisplayColor = color;
       // SetButtonInteractable(color, false);
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
        if (!isOwned || !isLocalPlayer || isClient)
        {
            isButtonInteracting = false;                                              // Disable the selected color button on all clients
            colorAvailability[selectedColor] = false;
            SetButtonInteractable(selectedColor, false);
        }
        // Activate the player panel and deactivate the color selection panel
        // ColorSelectionPanel.SetActive(false);
        // RoomPlayerPanel.SetActive(true);
    }
}
