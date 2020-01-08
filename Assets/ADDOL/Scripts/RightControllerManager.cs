using Photon.Pun;
using UnityEngine;
using Valve.VR;

public class RightControllerManager : MonoBehaviourPunCallbacks
{
    private GameObject grabbedObject;

    private bool isControllerInside = false;
    private GameObject controller;

    public delegate void OnGrabPressed(GameObject controller);
    public static event OnGrabPressed onGrabPressed;

    public delegate void OnGrabReleased(GameObject controller);
    public static event OnGrabReleased onGrabReleased;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        controller = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.transform.parent.GetComponent<PhotonView>().IsMine) return;

        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand) && grabbedObject)
        {
            if(grabbedObject.tag == "ValveHandle")
            {
                onGrabPressed?.Invoke(controller);
            } else
            {
                GrabSelectedObject(controller);
            }
        }
        if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand) && grabbedObject)
        {
            if(grabbedObject.tag == "ValveHandle")
            {
                onGrabReleased?.Invoke(controller);
            } else
            {
                UngrabSelectedObject(controller);
            }
        }
    }

    void GrabSelectedObject(GameObject controller)
    {
        Debug.Log("GRAB OBJECT");
        FixedJoint fx = controller.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.connectedBody = grabbedObject.GetComponent<Rigidbody>();
    }

    void UngrabSelectedObject(GameObject controller)
    {
        Debug.Log("UNGRAB OBJECT");
        FixedJoint fx = controller.GetComponent<FixedJoint>();
        if (fx)
        {
            fx.connectedBody.GetComponent<Rigidbody>().velocity = controller.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity()*2;
            fx.connectedBody.GetComponent<Rigidbody>().angularVelocity = controller.GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity()*2;
            fx.connectedBody = null;
            Destroy(controller.GetComponent<FixedJoint>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Grabbable")
        {
            Debug.Log("IN A GRABBABLE OBJECT : " + controller.name);
            grabbedObject = other.gameObject;
        }
        if(other.tag == "ValveHandle")
        {
            Debug.Log("IN THE VALVE : " + gameObject.name);
            grabbedObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            Debug.Log("OUT A GRABBABLE OBJECT : " + controller.name);
            grabbedObject = null;
        }
        if(other.tag == "ValveHandle")
        {
            Debug.Log("OUT THE VALVE");
            onGrabReleased?.Invoke(controller);
            grabbedObject = null;
        }
    }
}
