using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventSystemManager : MonoBehaviour
{
    public GameObject TutorTextContainer;
    public GameObject OperatorTextContainer;
    public bool isFireRunning = false;
    public GameObject PathToSafeZone;

    static EventSystemManager inst;

    public static EventSystemManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject();
                inst = go.AddComponent<EventSystemManager>();
            }
            return inst;
        }
    }

    void Awake()
    {
        inst = this;
        Inst.OperatorTextContainer.SetActive(false);
        Inst.PathToSafeZone.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateDeactivateFire()
    {
        Inst.isFireRunning = !Inst.isFireRunning;

        if(Inst.isFireRunning)
        {
            //Activate animation
            Inst.PathToSafeZone.SetActive(true);

            foreach (GameObject can in GameObject.FindGameObjectsWithTag("TutorCanvas"))
            {
                can.GetComponentInChildren<Text>().text = "Follow the path to safe zone.";
            }

            foreach (GameObject can in GameObject.FindGameObjectsWithTag("OperatorCanvas"))
            {
                can.GetComponentInChildren<Text>().text = "Follow the path to safe zone.";
                can.SetActive(true);
            }
        }
        else
        {
            //Deactivate animation
            Inst.PathToSafeZone.SetActive(false);

            foreach (GameObject can in GameObject.FindGameObjectsWithTag("TutorCanvas"))
            {
                can.GetComponentInChildren<Text>().text = "You survived the fire.";
            }

            foreach (GameObject can in GameObject.FindGameObjectsWithTag("OperatorCanvas"))
            {
                can.GetComponentInChildren<Text>().text = "You survived the fire.";
            }
            StartCoroutine(Inst.DisableText());
        }
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(5f);
        foreach (GameObject can in GameObject.FindGameObjectsWithTag("OperatorCanvas"))
        {
            can.SetActive(false);
        }

        foreach (GameObject can in GameObject.FindGameObjectsWithTag("TutorCanvas"))
        {
            can.GetComponentInChildren<Text>().text = "Press Action Touch to trigger FIRE.";
        }
    }
}
