using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using System.Linq;
using System.Collections.Generic;

public class NetworkManagerLobby : NetworkManager
{
    [Scene]
    [SerializeField] private string TestMenuScene = string.Empty;

    [SerializeField] private int minPlayers = 3;

    [Header("Room")]
    [SerializeField] NetworkRoomPlayerLobby roomPlayerPrefab;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab;
    
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public List<NetworkRoomPlayerLobby> RoomPlayer { get; } = new List<NetworkRoomPlayerLobby>(); //stores name of the diff. room player
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);      //ClientScene is now merged with NetworkClient
            
        }
    }

    public override void OnClientConnect() //is called on the client whwn connected to the server
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if(numPlayers >= maxConnections)                 //called on server when client connects
        {
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().name != TestMenuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(conn.identity != null)
        {                                                                       //access the list in the NetworkRoomPlayerLobby and remove that player form the active players
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            RoomPlayer.Remove(player);
            NotifyReadyState();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {                                                               //called on the server when a client adds player with ClientScene.AddPlayer
        if (SceneManager.GetActiveScene().name == TestMenuScene)
        {
            bool isDenner = RoomPlayer.Count == 0;               // 0th member of the list is the host or the leader
                                                                                        //when in the menu scene it spawns the player prefabs for connection
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsDenner = isDenner;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);   //giving server side authority to client
           // NetworkServer.Spawn(roomPlayerInstance.gameObject, conn);        //giving authority to client
           
        }
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
        RoomPlayer.Clear();
        GamePlayers.Clear();
    }

    public void NotifyReadyState()
    {
        foreach (var player in RoomPlayer)
        {
            player.handleReadyToStart(IsReadyToStart());
        }
    }

    public bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayer)
        {
            if (!player.isReady) { return false; }
        }
        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == TestMenuScene)
        {
           
            if (!IsReadyToStart()) { return; }

            ServerChangeScene("GameScene");
        }
    }

    public override void ServerChangeScene(string newSceneName)  //Start game scene
    {
        if (SceneManager.GetActiveScene().name == TestMenuScene)
        {
            for (int i = RoomPlayer.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayer[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayer[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    
}
