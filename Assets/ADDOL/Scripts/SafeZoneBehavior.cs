using UnityEngine;

public class SafeZoneBehavior : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(EventSystemManager.Inst.isFireRunning)
        {
            if(other.gameObject.tag == "Operator")
            {
                EventSystemManager.Inst.ActivateDeactivateFire();
            }
        }
    }
}
