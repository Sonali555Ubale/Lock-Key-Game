using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ServerController : NetworkBehaviour
{
   
    public Button buttonStopHost, buttonStopClient, buttonReturnToRoom;

	public void Start()
	{
        //Make sure to attach these Buttons in the Inspector
        buttonStopHost.onClick.AddListener(ButtonStopHost);
		buttonStopClient.onClick.AddListener(ButtonStopClient);
        buttonReturnToRoom.onClick.AddListener(ButtonReturntoRoom);
        SetupCanvas();
	}
    public void ButtonStopHost()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

        SetupCanvas();
    }

    public void ButtonStopClient()
    {
        // stop client if client-only
       if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
       SetupCanvas();
    }

    public void ButtonReturntoRoom()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            if (NetworkServer.active && Utils.IsSceneActive("GameScene"))
                   NetworkRoomManager.singleton.ServerChangeScene("RoomScene");
        }
    }

    public void SetupCanvas()
    {
        if (isServerOnly || isServer)
        {
            buttonStopHost.gameObject.SetActive(true);
            buttonStopClient.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name != "RoomScene")
                buttonReturnToRoom.gameObject.SetActive(true);
        }
        else if (!isServer)
        {
            buttonStopClient.gameObject.SetActive(true);
            buttonReturnToRoom.gameObject.SetActive(false);
        }
        else
        {
            buttonStopHost.gameObject.SetActive(false);
            buttonStopClient.gameObject.SetActive(false);
            if (SceneManager.GetActiveScene().name != "RoomScene")
                buttonReturnToRoom.gameObject.SetActive(true);
        }
    }
}
