using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

namespace WS3
{
    public class UserManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject UserMeInstance;

        public Material PlayerLocalMat;
        /// <summary>
        /// Represents the GameObject on which to change the color for the local player
        /// </summary>
        public GameObject GameObjectLocalPlayerColor;

        /// <summary>
        /// The FreeLookCameraRig GameObject to configure for the UserMe
        /// </summary>
        GameObject goFreeLookCameraRig = null;

        #region Snwoball Spawn
        /// <summary>
        /// The Transform from which the snow ball is spawned
        /// </summary>
        [SerializeField] Transform snowballSpawner;
        /// <summary>
        /// The prefab to create when spawning
        /// </summary>
        [SerializeField] GameObject SnowballPrefab;



        // Use to configure the throw ball feature
        [Range(0.2f, 100.0f)] public float MinSpeed;
        [Range(0.2f, 100.0f)] public float MaxSpeed;
        [Range(0.2f, 100.0f)] public float MaxSpeedForPressDuration;
        private float pressDuration = 0;

        #endregion

        void Awake()
        {
            if (photonView.IsMine)
            {
                Debug.LogFormat("Avatar UserMe created for userId {0}", photonView.ViewID);
                UserMeInstance = gameObject;

            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("isLocalPlayer:" + photonView.IsMine);
            Health = 3;
            updateGoFreeLookCameraRig();
            followLocalPlayer();
            activateLocalPlayer();
            UpdateHealthMaterial();
        }

                /// <summary>
        /// Get the GameObject of the CameraRig
        /// </summary>
        protected void updateGoFreeLookCameraRig()
        {
            if (!photonView.IsMine) return;
            try
            {
                // Get the Camera to set as the followed camera
                goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
            }
        }

        /// <summary>
        /// Make the CameraRig following the LocalPlayer only.
        /// </summary>
        protected void followLocalPlayer()
        {
            if (photonView.IsMine)
            {
                if (goFreeLookCameraRig != null)
                {
                    // find Avatar EthanHips
                    Transform transformFollow = transform.Find("EthanSkeleton/EthanHips") != null ? transform.Find("EthanSkeleton/EthanHips") : transform;
                    // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
                    goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
                    Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
                }
            }
        }

        protected void activateLocalPlayer()
        {
            // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
            // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
            GetComponent<ThirdPersonUserControl>().enabled = photonView.IsMine;
            GetComponent<Rigidbody>().isKinematic = !photonView.IsMine;
            if (photonView.IsMine)
            {
                try
                {
                    // Change the material of the Ethan Glasses
                    GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
                }
                catch (System.Exception)
                {

                }
            }
        }


        #region Snwoball Spawn
        // Update is called once per frame
        void Update()
        {
            // Don't do anything if we are not the UserMe isLocalPlayer
            if (!photonView.IsMine) return;

            if (Input.GetButtonDown("Fire1"))
            {
                // Start Loading time when fire is pressed
                pressDuration = 0.0f;
            }
            else if (Input.GetButton("Fire1"))
            {
                // count the time the Fire1 is pressed
                //pressDuration += ???; 
                pressDuration += Time.deltaTime;
            }

            else if (Input.GetButtonUp("Fire1"))
            {
                // When releasing Fire1, spawn the ball
                // Define the initial speed of the Snowball between MinSpeed and MaxSpeed according to the duration the button is pressed
                var speed = Mathf.Clamp(MinSpeed + pressDuration / MaxSpeedForPressDuration * (MaxSpeed - MinSpeed), MinSpeed, MaxSpeed); // update with the right value
                Debug.Log(string.Format("time {0:F2} <  {1} => speed {2} < {3} < {4}", pressDuration, MaxSpeedForPressDuration, MinSpeed, speed, MaxSpeed));
                photonView.RPC("ThrowBall", RpcTarget.AllViaServer, snowballSpawner.position, speed * snowballSpawner.forward);
            }
        }

        [PunRPC]
        void ThrowBall(Vector3 position, Vector3 directionAndSpeed, PhotonMessageInfo info)
        {
            // Tips for Photon lag compensation. Il faut compenser le temps de lag pour l'envoi du message.
            // donc décaler la position de départ de la balle dans la direction
            float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
            Debug.LogFormat("PunRPC: ThrowBall {0} -> {1} lag:{2}", position, directionAndSpeed, lag);

            // Create the Snowball from the Snowball Prefab
            GameObject snowball = Instantiate(
                SnowballPrefab,
                position + directionAndSpeed * Mathf.Clamp(lag, 0, 1.0f),
                Quaternion.identity);


            // Add velocity to the Snowball
            snowball.GetComponent<Rigidbody>().velocity = directionAndSpeed;

            // Destroy the Snowball after 5 seconds
            Destroy(snowball, 5.0f);
        }
        #endregion

        #region Health Management
        [SerializeField] private Material Health3;
        [SerializeField] private Material Health2;
        [SerializeField] private Material Health1;

        private int previousHealth;
        public int Health { get; private set; }
        /// <summary>
        /// The Transform from which the snow ball is spawned
        /// </summary>
        [SerializeField] float ForceHit;

        public void HitBySnowball()
        {
            if (!photonView.IsMine) return;
            Debug.Log("Got me");
            var rb = GetComponent<Rigidbody>();
            rb.AddForce((-transform.forward + (transform.up * 0.1f) ) * ForceHit, ForceMode.Impulse);


            // Manage to leave room as UserMe
            if (--Health <= 0)
            {
                PhotonNetwork.LeaveRoom();
            }
        }

        public void UpdateHealthMaterial()
        {
            try
            {
                var rend = transform.Find("EthanBody").gameObject.GetComponent<Renderer>();
                switch (Health)
                {
                    case 1:
                        rend.material = Health1;
                        break;

                    case 2:
                        rend.material = Health2;
                        break;

                    case 3:
                    default:
                        rend.material = Health3;
                        break;
                }
            }
            catch (System.Exception)
            {

            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(Health);
            }
            else
            {
                Health = (int)stream.ReceiveNext();
            }

            if(previousHealth != Health) UpdateHealthMaterial();
            previousHealth = Health;
        }

        #endregion
    }
}
