using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

namespace Com.MyCompany.MyGame {
    /// <summary>
    /// Player manager.
    /// Handles fire Input.
    /// </summary>
    public class MyPlayerManager : MonoBehaviourPunCallbacks, IPunObservable {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject localPlayerInstance;
        [Tooltip("The current Health of our player")]
        public float Health = 1f;
        [Tooltip("How fast this player moves")]
        public float movementSpeed = 5f;
        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        public GameObject PlayerUiPrefab;
        [Tooltip("The camera this player is using")]
        [SerializeField]
        private Camera camera;
        //True, when the user is firing
        bool IsFiring;
        // Movement player desires to apply
        Vector3 movement;
        // Position of our player's mouse
        Vector2 mousePosition;


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake() {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine) {
                MyPlayerManager.localPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start() {
            if (PlayerUiPrefab != null) {
                GameObject _uiGo =  Instantiate(PlayerUiPrefab);
                _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            #if UNITY_5_4_OR_NEWER
                // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endif
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update() {
            if (Health <= 0f) {
                GameManager.Instance.LeaveRoom();
            }

            // so our test scene will handle input just like when we're online
            if (PhotonNetwork.IsConnected == false || photonView.IsMine) {
                ProcessInputs();
            }
        }

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs() {
            // movement
            Rigidbody rb = GetComponent<Rigidbody>();
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");

            // aiming
            // Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
            // Physics.Raycast(ray, Vector3.down, 100f, var out hit, yourLayerMaskToDetectGround);

            //Need to get vector of mouse pos
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            // mousePos.y = 0;
            transform.LookAt(mousePos);

            Debug.Log("mousePos: " + mousePos);

            // firing
            if (Input.GetButtonDown("Fire1")) {
                if (!IsFiring) {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1")) {
                if (IsFiring) {
                    IsFiring = false;
                }
            }
        }

        void FixedUpdate() {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.MovePosition(rb.position + movement * this.movementSpeed * Time.fixedDeltaTime);

            Vector2 lookDirection = mousePosition - (Vector2)rb.position;
            float angleToTurnPlayerToLookDirection = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            Debug.Log("angleToTurnPlayerToLookDirection: " + angleToTurnPlayerToLookDirection);
            //rb.rotation = Quaternion.Euler(0, angleToTurnPlayerToLookDirection, 0);
        }

        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// </summary>
        void OnTriggerEnter(Collider other) {
            if (!photonView.IsMine) {
                return;
            }
        }

        /// <summary>
        /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        /// </summary>
        /// <param name="other">Other.</param>
        void OnTriggerStay(Collider other) {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine) {
                return;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                // We own this player: send the others our data
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else {
                // Network player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }

        #if !UNITY_5_4_OR_NEWER
            /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
            void OnLevelWasLoaded(int level) {
                this.CalledOnLevelWasLoaded(level);
            }
        #endif

        #if UNITY_5_4_OR_NEWER
            public override void OnDisable() {
                // Always call the base to remove callbacks
                base.OnDisable ();
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        #endif


        void CalledOnLevelWasLoaded(int level) {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        #if UNITY_5_4_OR_NEWER
            void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode) {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            }
        #endif
    }
}