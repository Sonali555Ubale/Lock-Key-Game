using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameOverManager : MonoBehaviour
{
   public void OnGameQuit()
    {
        Application.Quit();
    }

    public void OnGameRestart()
    {
        //SceneManager.LoadScene(1);

        NetworkRoomManager.singleton.ServerChangeScene("RoomScene");
    }
}
