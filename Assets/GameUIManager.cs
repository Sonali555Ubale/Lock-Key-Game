using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    public Transform CanvasTransform { get => this.gameObject.transform; }
    private GameUIManager() { }
    private static GameUIManager instance = null;
    public static GameUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameUIManager();
            }
            return instance;
        }
    }

}
