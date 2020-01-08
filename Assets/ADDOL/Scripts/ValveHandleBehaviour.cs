using System.Collections.Generic;
using UnityEngine;
using static RightControllerManager;
using static LeftControllerManager;
using System.Linq;

public class ValveHandleBehaviour : MonoBehaviour
{
    public GameObject grabbableValve;
    private int numberOfControllersGrabbed = 0;
    private List<GameObject> listControllerGrab = new List<GameObject>();
    private float globalRotation = 0f;

    void Start()
    {
        RightControllerManager.onGrabPressed += new RightControllerManager.OnGrabPressed(OnGrab);
        RightControllerManager.onGrabReleased += new RightControllerManager.OnGrabReleased(OutGrab);
        LeftControllerManager.onGrabPressed += new LeftControllerManager.OnGrabPressed(OnGrab);
        LeftControllerManager.onGrabReleased += new LeftControllerManager.OnGrabReleased(OutGrab);

        grabbableValve.tag = "Grabbable";
        gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(gameObject.transform.rotation.eulerAngles.x - 90);
        if(gameObject.transform.rotation.eulerAngles.x - 90 == -10)
        {
            globalRotation += 1;
            Debug.Log("Rot +1");
            if(globalRotation == 1)
            {*/
                
            /*}
        }*/
    }

    void OnGrab(GameObject controller)
    {
        Debug.Log("ON GRAB : " + controller.name);
        Debug.Log("ATTACH CONTROLLER TO VALVE");
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.connectedBody = controller.GetComponent<Rigidbody>();

        numberOfControllersGrabbed = gameObject.GetComponents<FixedJoint>().Count();

        Debug.Log(numberOfControllersGrabbed);
        if(numberOfControllersGrabbed == 2)
        {
            Debug.Log("TWO CONTROLLERS ARE ON THE VALVE");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OutGrab(GameObject controller)
    {
        Debug.Log("UNATTACH CONTROLLER TO VALVE");
        FixedJoint fxControllerToDelete = gameObject.GetComponents<FixedJoint>().Where(fx => fx.connectedBody.name == controller.name).SingleOrDefault();

        if (fxControllerToDelete != null)
        {
            fxControllerToDelete.connectedBody = null;
            Destroy(fxControllerToDelete);
            Debug.Log("CONTROLLERS LEFT");
            Debug.Log(numberOfControllersGrabbed);
        }

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
