using System;
using System.Collections.Generic;
using System.IO;
using Game.GameModes;
using Network;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager Instance;
        public GameMode gameMode;
        public GameObject playerPrefab;
        public List<PlayerManager> players = new List<PlayerManager>();
        public PlayerManager localPlayer;
        
        private void Awake()
        {
            Instance = this;
            gameMode = GetComponent<GameMode>();
            if (gameMode == null)
            {
                Debug.LogError("GameMode is null");
            }
        }
        
        void Start()
        {
            SpawnPlayerManager();
        }

        private void Update()
        {
            // gameMode.StartGame();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!gameMode.IsGameStarted && !gameMode.IsGameEnded)
                {
                    StartGame();
                }
            }
        }

        public void SpawnPlayerManager()
        {
            PhotonNetwork.Instantiate(Path.Join("PhotonPrefabs", "PlayerManager"),
                new Vector3(0, 10, 0), Quaternion.identity);
        }

        public void SpawnPlayer(Vector3 pos = new Vector3())
        {
            localPlayer.SpawnPlayer(pos);
        }

        public void StartGame()
        {
            gameMode.StartGame();
        }
    }
}
