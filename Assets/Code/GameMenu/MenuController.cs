namespace RunlingRun.GameMenu
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Photon.Pun;
    using Photon.Realtime;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuController : MonoBehaviourPunCallbacks
    {
        private readonly string VersionName = "0.0.1";

        public TMP_InputField GameCodeInput;
        public TMP_InputField UsernameInput;

        private void Awake()
        {

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = VersionName;
            PhotonNetwork.ConnectUsingSettings();
        }

        //         void ClearConsole()
        //         {
        // #if !UNITY_EDITOR
        //             var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        //             var type = assembly.GetType("UnityEditor.LogEntries");
        //             var method = type.GetMethod("Clear");
        //             method.Invoke(new object(), null);
        // #endif
        //         }

        public override void OnConnectedToMaster()
        {
            // ClearConsole();

            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Connected to Server!");
        }

        public void JoinCreateGame()
        {
            TypedLobby lobby = new TypedLobby("Game", LobbyType.Default);
            PhotonNetwork.LocalPlayer.NickName = UsernameInput.text;
            PhotonNetwork.JoinOrCreateRoom(GameCodeInput.text, new RoomOptions() { MaxPlayers = 8 }, lobby);
        }

        public override void OnJoinedRoom()
        {
            // TODO: Add support for multiple maps
            PhotonNetwork.LoadLevel("TestMap");
        }
    }
}

