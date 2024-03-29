﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;


/// <summary>
/// Make the Steam VR available in multiplayer by deactivating script for UserOther
/// Support version # SteamVR Unity Plugin - v2.0.1
/// Support version # SteamVR Unity Plugin - v2.4.5 (sdk 1.7.15)
/// </summary>
public class VR_CameraRigMultiuser : MonoBehaviourPunCallbacks
{

    // reference to SteamController
    public GameObject SteamVRLeft, SteamVRRight, SteamVRCamera;
    public GameObject UserOtherLeftHandModel, UserOtherRightHandModel;
    private GameObject goFreeLookCameraRig;
    public GameObject WeatherMenu;
    public static GameObject UserMeInstance;
    private GameObject UserMePanel;

    public GameObject OperatorCanvasPrefab;
    public GameObject TutorCanvasPrefab;

    // Use this for initialization
    void Start()
    {
        updateGoFreeLookCameraRig();
        steamVRactivation();
    }

    void Awake()
    {
        if (photonView.IsMine)
        {
            Debug.LogFormat("Avatar UserMe created for userId {0}", photonView.ViewID);
            UserMeInstance = gameObject;
            if (UserMeInstance.tag == "Operator")
            {
                Debug.Log("IS OPERATOR");
                UserMePanel = Instantiate(OperatorCanvasPrefab, SteamVRCamera.transform);
            }
            else
            {
                Debug.Log("Is TUTOR");
                UserMePanel = Instantiate(TutorCanvasPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                UserMePanel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// deactivate the FreeLookCameraRig since we are using the HTC version
    /// Execute only in client side
    /// </summary>
    protected void updateGoFreeLookCameraRig()
    {
        // Client execution ONLY LOCAL
        if (!photonView.IsMine) return;

        goFreeLookCameraRig = null;

        try
        {
            // Get the Camera to set as the follow camera
            goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            // Deactivate the FreeLookCameraRig since we are using the SteamVR camera
            goFreeLookCameraRig.SetActive(false);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }


    /// <summary>
    /// If we are the client who is using the HTC, activate component of SteamVR in the client using it
    /// If we are not the client using this specific HTC, deactivate some scripts.
    /// </summary>
    protected void steamVRactivation()
    {
        // client execution for ALL

        // Left activation if UserMe, deactivation if UserOther
        SteamVRLeft.GetComponent<SteamVR_Behaviour_Pose>().enabled = photonView.IsMine;

        // Left SteamVR_RenderModel activation if UserMe, deactivation if UserOther
        SteamVRLeft.GetComponentInChildren<SteamVR_RenderModel>().enabled = photonView.IsMine;
        SteamVRLeft.transform.Find("Model").gameObject.SetActive(photonView.IsMine);

        // Right activation if UserMe, deactivation if UserOther
        SteamVRRight.GetComponent<SteamVR_Behaviour_Pose>().enabled = photonView.IsMine;

        // Left SteamVR_RenderModel activation if UserMe, deactivation if UserOther
        SteamVRRight.GetComponentInChildren<SteamVR_RenderModel>().enabled = photonView.IsMine;
        SteamVRRight.transform.Find("Model").gameObject.SetActive(photonView.IsMine);

        // Camera activation if UserMe, deactivation if UserOther
        SteamVRCamera.GetComponent<Camera>().enabled = photonView.IsMine;

        if (!photonView.IsMine)
        {
            // ONLY for player OTHER

            // Create the model of the LEFT Hand for the UserOther, use a SteamVR model  Assets/SteamVR/Models/vr_glove_left_model_slim.fbx
            var modelLeft = Instantiate(UserOtherLeftHandModel);
            // Put it as a child of the SteamVRLeft Game Object
            modelLeft.transform.parent = SteamVRLeft.transform;

            // Create the model of the RIGHT Hand for the UserOther Assets/SteamVR/Models/vr_glove_right_model_slim.fbx
            var modelRight = Instantiate(UserOtherRightHandModel);
            // Put it as a child of the SteamVRRight Game Object
            modelRight.transform.parent = SteamVRRight.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Don't do anything if we are not the UserMe isLocalPlayer
        if (!photonView.IsMine) return;
    }

    
    [PunRPC]
    void ThrowBall(Vector3 position, Vector3 directionAndSpeed, PhotonMessageInfo info)
    {
        
    }
}