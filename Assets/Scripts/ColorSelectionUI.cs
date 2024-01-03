using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;
using System;
using UnityEditor.EditorTools;

public class ColorSelectionUI : NetworkBehaviour
{
    [SerializeField]
    LAK_NetworkRoomPlayer CurrentNetworkPlayer = null;
    LAK_NetworkRoomManager RoomManager = null;
    public static Color DisplayColor { get; private set; }

    [SerializeField]
    private List<Button> colorButtons = new List<Button>();

    [SerializeField]
    private List<Color> Colors = new List<Color>();

    public Image colorPreviewImage;
    public Color selectedColor;
    private bool isButtonInteracting = true;
    private int indexValue;
    private bool flag = false;
    
    [Header("Color Selection")]
    [SerializeField]
    private Button ReadyButton;
    [SerializeField]
    public Button SetButton;
    [SerializeField]
    private GameObject ColorSelectionPanel;
    

    public void ColorSelect(int index)
    {
        var Player = (CurrentNetworkPlayer == null) ? FindNetworkRoomPlayer() : CurrentNetworkPlayer;
        Player.ColorSelected(index, false);
    }

    // false = taken ,  true = free

    LAK_NetworkRoomPlayer FindNetworkRoomPlayer()
    {
        var list = Resources.FindObjectsOfTypeAll(typeof(LAK_NetworkRoomPlayer));

        foreach (var player in list)
        {
            var temp = player as LAK_NetworkRoomPlayer;
            Debug.Log(temp.index);
            if (temp.isLocalPlayer)
            {
                CurrentNetworkPlayer = temp;
                return CurrentNetworkPlayer;
            }
        }

        return null;
    }

    void Start()
    {
        if (RoomManager == null) RoomManager = (LAK_NetworkRoomManager)FindObjectOfType(typeof(LAK_NetworkRoomManager));
          RoomManager.OnPlayerColorSelection.AddListener(UpdateUI);
    }
    private void OnEnable()
    {
        if (CurrentNetworkPlayer == null) CurrentNetworkPlayer = (LAK_NetworkRoomPlayer)FindObjectOfType(typeof(LAK_NetworkRoomPlayer));

        // Initialize color buttons with click events
        foreach (Button button in colorButtons)
        {
            Color buttonColor = button.GetComponent<Image>().color;
            button.onClick.AddListener(() => OnColorButtonClick(buttonColor));
        }

    }
    private void UpdateUI()
    {
        Debug.Log("This is Update UI Fun:::: index Val is now" +RoomManager.indexVal);

        if(isButtonInteracting==false && flag) 
        {
            indexValue = RoomManager.indexVal;
            if (colorPreviewImage.color == colorButtons[indexValue].GetComponent<Image>().color)
            {
              
                colorButtons[indexValue].interactable = false;
                isButtonInteracting = true;
                flag = false;

            }
           
        }
            Debug.Log(" color disabled::" + colorButtons[indexValue]);
           
        
    }

    public void OnColorButtonClick(Color _color)    // this method is called on color button click
    {

        // Set the selected color when a color button is clicked
        if (isButtonInteracting && SetButton.interactable ==true)
        {
            selectedColor = _color;
            colorPreviewImage.color = _color;
            PlayerPrefs.SetFloat("PlayerColorR", selectedColor.r);
            PlayerPrefs.SetFloat("PlayerColorG", selectedColor.g);
            PlayerPrefs.SetFloat("PlayerColorB", selectedColor.b);
            PlayerPrefs.SetFloat("PlayerColorA", selectedColor.a);
            DisplayColor = _color;
            CmdSelectColor(_color);
        }
            // Notify the server about the color selection
          
        
    }

    [Command]
    private void CmdSelectColor(Color color)
    {
        selectedColor = color;
        DisplayColor = color;
       
        //colorAvailability[color] = false;

        RpcUpdateSelectedColor(color);   // Notify all clients about the color selection
    }

    [ClientRpc]
    private void RpcUpdateSelectedColor(Color color)
    {                           // Update the selected color on all clients
        selectedColor = color;
        DisplayColor = color;
        // SetButtonInteractable(color, false);
    }

    public void OnSetColorButtonClick()
    {
        if (isButtonInteracting  && SetButton.interactable==true)
        {
            flag = true;
            isButtonInteracting = false;
       
         //   CmdSelectColor(selectedColor);
         if(flag==false)
            SetButton.interactable = false;
           
           
        }
         

        ReadyButton.gameObject.SetActive(true);
       


    }


    /*
        public Image colorPreviewImage;
        public Button[] colorButtons;
        public Color selectedColor;

        [Header("Color Selection")]
        [SerializeField]
        private Button ReadyButton;
        [SerializeField]
        private GameObject ColorSelectionPanel;
        [SerializeField]
        LAK_NetworkRoomPlayer RoomPlayer = null;

        [SyncVar(hook = nameof(OnChoosingColor))]
        public bool isButtonInteracting = true;
        [SyncVar(hook = nameof(OnSelectionUIColor))]
        public Color ColorToHide;

        public static Color DisplayColor { get; private set; }

        private Dictionary<Color, bool> colorAvailability = new Dictionary<Color, bool>();


        private void Start()
        {
            *//* this.gameObject.SetActive(true);
              Debug.Log(":::"+this.name);
              RoomPlayerPanel.SetActive(false);*//*


        }

        private void OnEnable()
        {
            if (RoomPlayer == null) RoomPlayer = (LAK_NetworkRoomPlayer)FindObjectOfType(typeof(LAK_NetworkRoomPlayer));

              ColorToHide = selectedColor;
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

        public void OnColorButtonClick(Color _color)    // this method is called on color button click
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
                ColorToHide = _color;

                // Reserve the selected color
                colorAvailability[selectedColor] = false;


                // Notify the server about the color selection
                CmdSelectColor(_color);
            }
        }

        [Command]
        private void CmdSelectColor(Color color)
        {
            selectedColor = color;
            DisplayColor = color;
            ColorToHide = color;
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

                isButtonInteracting = false;                                              // Disable the selected color button on all clients
                colorAvailability[selectedColor] = false;
                SetButtonInteractable(selectedColor, false);
                ReadyButton.gameObject.SetActive(true);

            // Activate the player panel and deactivate the color selection panel
            *//* ColorSelectionPanel.SetActive(false);
             RoomPlayerPanel.SetActive(true);*//*

        }

        public void OnSelectionUIColor(Color oldColor, Color newColor)
        {

            ColorToHide = newColor;

        }*/

}