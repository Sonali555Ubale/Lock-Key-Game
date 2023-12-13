using Mirror;
using UnityEngine;
using UnityEngine.UI;
public class ServerController : NetworkBehaviour
{
   
    public Button buttonStopHost, buttonStopClient;

	public void Start()
	{
        //Make sure to attach these Buttons in the Inspector
        buttonStopHost.onClick.AddListener(ButtonStopHost);
		buttonStopClient.onClick.AddListener(ButtonStopClient);
				
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
            NetworkManager.singleton.StopHost();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }

    }

    public void SetupCanvas()
    {
        if (isServerOnly || isServer )
        {
            buttonStopHost.gameObject.SetActive(true);           
            buttonStopClient.gameObject.SetActive(true);
        }
        else if (!isServer )
            buttonStopClient.gameObject.SetActive(true);
        else
        {
            buttonStopHost.gameObject.SetActive(false);
            buttonStopClient.gameObject.SetActive(false);
        }
    }
}
