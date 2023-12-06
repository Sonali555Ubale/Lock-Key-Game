using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerColorSetter : NetworkBehaviour
{
    [Header("Color Selection")]
    [SerializeField] private Image colorPreviewImage;
    [SerializeField] public Color selectedColor = Color.black;

    // ...

    public void SetPlayerColor(Color color)
    {
        selectedColor = color;
        colorPreviewImage.color = color;
    }

    public void SavePlayerInfo()
    {
       
        //  selected color to the server
        CmdSetPlayerInfo(selectedColor);
    }

    [Command]
   public void CmdSetPlayerInfo(Color playerColor)
    {
        playerColor = playerColor == Color.clear ? Color.white : playerColor; // Default color if clear
        RpcUpdatePlayerInfo(playerColor);
    }

    [ClientRpc]
    void RpcUpdatePlayerInfo( Color playerColor)
    {
       
        selectedColor = playerColor;
        colorPreviewImage.color = playerColor;
    }
}
