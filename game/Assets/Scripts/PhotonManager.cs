using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

namespace com.greghilston {
    /// <summary>
    /// Handles syncing with Photon.
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(LookController))]
    public class PhotonManager : MonoBehaviourPunCallbacks {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject localPlayerInstance;

        void Awake() {
            // Important: used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine) {
                PhotonManager.localPlayerInstance = this.gameObject;
            } else {
                GetComponent<MovementController>().enabled = false;
                GetComponent<LookController>().enabled = false;
            }
            // Critical: we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        void Start() {
            #if UNITY_5_4_OR_NEWER
                // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endif
        }

        void CalledOnLevelWasLoaded(int level) {
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

        #if UNITY_5_4_OR_NEWER
            void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode) {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            }
        #endif
    }
}