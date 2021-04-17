namespace RunlingRun.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public Transform SpawnPoint;
        public Vector3 EndofMapPos;

        // --- Singleton Pattern
        private static GameManager _instance = null;
        public static GameManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern

        void Start()
        {
            if (!PhotonNetwork.InRoom)
            {
                SceneManager.LoadScene("GameMenu");
            }

            GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint/Dummy");
            // Create Room Objects
            foreach (GameObject spawn in enemySpawnPoints)
            {
                PhotonNetwork.InstantiateRoomObject("Elon", spawn.transform.position, Quaternion.identity);
            }

            EndofMapPos = GameObject.Find("EndofMap").transform.position;
        }
    }
}

