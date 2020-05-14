using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace com.greghilston {
    // Responsible for instantiating our Player prefab with Photon and loading the correctlevel
    public class GameManager : MonoBehaviourPunCallbacks {
        public static GameManager instance;
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        void Start() {
            GameManager.instance = this;

            if (PhotonManager.localPlayerInstance == null) {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom() {
            SceneManager.LoadScene(0);
        }

        public void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }

        void LoadArena() {
            Debug.LogFormat("PhotonNetwork : Loading Level : player_test");
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            PhotonNetwork.LoadLevel("player_test");
        }

        public override void OnPlayerEnteredRoom(Player other) {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other) {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }
    }
}