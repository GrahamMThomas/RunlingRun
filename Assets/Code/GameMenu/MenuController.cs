namespace RunlingRun.GameMenu
{
    using System.Collections;
    using System.Collections.Generic;
    using Photon.Pun;
    using Photon.Realtime;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuController : MonoBehaviourPunCallbacks
    {
        private readonly string VersionName = "0.0.1";

        public TMP_InputField GameCodeInput;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = VersionName;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Connected to Server!");
        }

        public void CreateGame()
        {
            PhotonNetwork.CreateRoom(GameCodeInput.text, new RoomOptions() { MaxPlayers = 8 });
        }

        public void JoinGame()
        {
            PhotonNetwork.JoinRoom(GameCodeInput.text);
        }

        public override void OnJoinedRoom()
        {
            // TODO: Add support for multiple maps
            PhotonNetwork.LoadLevel("TestMap");
        }
    }
}

