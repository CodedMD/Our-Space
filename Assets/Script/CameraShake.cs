using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraShake : MonoBehaviour
{
    public static CameraShake _instance;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;


    //On The Awake of my Script
    public void Awake()
    {
        _instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    //Shake Camera Parameters
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }



    //Shake Controls
    private void Update()
    {
        if(shakeTimer >0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }


}
