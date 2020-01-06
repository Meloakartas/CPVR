using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS3
{
    public class SnowballBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            var hit = collision.gameObject;
            Debug.Log("Snowball hit something:" + hit);
            

            UserManager um = hit.GetComponent<UserManager>();
            if(um != null)
            {
                Debug.Log("  It is a player !!");
                um.HitBySnowball();
            }
            Destroy(gameObject);
        }
    }
}