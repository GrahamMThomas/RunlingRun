namespace RunlingRun.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public Transform SpawnPoint;
        public GameObject CurrentPlayer;
        public Vector3 EndofMapPos;

        // Start is called before the first frame update
        void Start()
        {
            if (!PhotonNetwork.InRoom)
            {
                SceneManager.LoadScene("GameMenu");
            }

            // Create Networked Object
            CurrentPlayer = PhotonNetwork.Instantiate("Munch", SpawnPoint.position, Quaternion.identity);

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

