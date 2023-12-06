using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerFloatingName : NetworkBehaviour
{
    public TextMeshPro playerNameText;
    public GameObject floatingInfo;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    void OnNameChanged(string _Old, string _New)
    {
        playerNameText.text = _New;
        // playerNameText.text = playerName;
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

       // string name = "Player" + Random.Range(100, 999);
        string name = PlayerNameInput.DisplayName;
        CmdSetupPlayer(name);
    }

    [Command]
    public void CmdSetupPlayer(string _name)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerName = _name;
        playerNameText.text = _name;
        RpcUpdatePlayerName(_name);
    }

    [ClientRpc]
    void RpcUpdatePlayerName(string _name)
    {
        playerNameText.text = _name; // Update the 3D text UI with the player's name for all clients
    }

    private void Start()
    {
        OnStartLocalPlayer();
        floatingInfo.gameObject.transform.SetPositionAndRotation(floatingInfo.transform.position, Quaternion.identity);
    }
    void Update()
    {
     
        if (!isLocalPlayer)
        {
            // make non-local players run this
            floatingInfo.transform.LookAt(floatingInfo.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            return;
        }
    }
}
