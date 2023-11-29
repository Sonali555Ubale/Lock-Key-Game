using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using System.Linq;
using System.Collections.Generic;

public class NetworkManagerLobby : NetworkManager
{
    [Scene]
    [SerializeField] private string menuScene = string.Empty;

    [SerializeField] private int minPlayers = 3;

    [Header("Room")]
    [SerializeField] NetworkRoomPlayerLobby roomPlayerPrefab;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> RoomPlayer { get; } = new List<NetworkRoomPlayerLobby>(); //stores name of the diff. room player

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
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
        if (SceneManager.GetActiveScene().name!= menuScene)
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
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isDenner = RoomPlayer.Count == 0;               // 0th member of the list is the host or the leader
                                                                                        //when in the menu scene it spawns the player prefabs for connection
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsDenner = isDenner;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnStopServer()
    {
        RoomPlayer.Clear();
    }

    public void NotifyReadyState()
    {
        foreach (var player in RoomPlayer)
            player.handleReadyToStart(IsReadyToStart());

        Debug.Log("NotifyReadyState() is called...");
    }
     
    public bool IsReadyToStart()
    {
        if (numPlayers < minPlayers) { return false; }
        foreach (var player in RoomPlayer)
        {
            if(!player.isReady) { return false; }
        }
        Debug.Log("IsReadyToStart() is called...");
        return true;
       
    }
}
