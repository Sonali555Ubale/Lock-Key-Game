using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class LocalPlayerCamera : NetworkBehaviour
{
    [SerializeField]
   private CinemachineVirtualCamera virtualCam;
  
    void Start()
    {
        if (isLocalPlayer)
        {
            virtualCam = CinemachineVirtualCamera.FindObjectOfType<CinemachineVirtualCamera>();
            virtualCam.LookAt = this.gameObject.transform;
            virtualCam.Follow = this.gameObject.transform;
        }
           
    }

 
}
