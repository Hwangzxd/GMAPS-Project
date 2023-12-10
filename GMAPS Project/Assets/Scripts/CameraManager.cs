using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera topDownCamera;
    public CinemachineVirtualCamera lookBackCamera;

    public CinemachineVirtualCamera startCamera;
    private CinemachineVirtualCamera currentCamera;

    private void Start()
    {
        currentCamera = startCamera;

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == currentCamera)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }
    }

    public void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        currentCamera = newCamera;

        currentCamera.Priority = 20;

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != currentCamera)
            {
                cameras[i].Priority = 10;
            }
        }
    }
}
