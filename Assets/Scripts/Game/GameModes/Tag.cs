using System;
using Network;
using Photon.Pun;
using UnityEngine;

namespace Game.GameModes
{
    public class Tag : GameMode
    {
        public float timeLimit = 60f;
        public float currentTimeLimit;
        public float timeLimitDecrement = 15f;
        public float timeLeft;
        public float distanceToTag = 5f;
        public int round = 1;
        public PlayerManager taggedPlayer;
        public PlayerManager[] alivePlayers => Array.FindAll(players, player => !player.IsDead);
        public PlayerManager[] deadPlayers => Array.FindAll(players, player => player.IsDead);

        public override void PlayerInteraction(PlayerManager player)
        {
            if (IsGameEnded) return;
            if (player != taggedPlayer) return;
            // Check if another player is close enough to tag
            foreach (var otherPlayer in players)
            {
                if (otherPlayer == player) continue;
                if (Vector3.Distance(player.transform.position, otherPlayer.transform.position) <= distanceToTag)
                {
                    photonView.RPC(nameof(TagPlayerRPC), RpcTarget.All, otherPlayer.actorNumber);
                }
            }

        }

        public override void PlayerDeath(PlayerManager player)
        {
            // TODO: Handle player death
        }

        public override void StartGame()
        {
            // TODO: Handle game start
            photonView.RPC(nameof(StartGameRPC), RpcTarget.All);
        }

        public override void EndGame()
        {
            // TODO: Handle game end
        }

        public override void CheckWin()
        {
            if (GameManager.Instance.players.Count == 1)
            {
                IsGameEnded = true;
                EndGame();
            }
        }

        protected override void Update()
        {
            base.Update();
            // Update time left
            if (IsGameStarted && !IsGameEnded && !IsGamePaused)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0)
                {
                    EndRound();
                }
            }
        }

        public void StartRound()
        {
            currentTimeLimit = timeLimit - timeLimitDecrement * (round - 1);
            timeLeft = currentTimeLimit;
            
            // Tag random player
            taggedPlayer = alivePlayers[UnityEngine.Random.Range(0, alivePlayers.Length)];
            
            photonView.RPC(nameof(StartRoundRPC), RpcTarget.All, currentTimeLimit, timeLeft, taggedPlayer.actorNumber);
        }
        
        public void EndRound()
        {
            photonView.RPC(nameof(EndRoundRPC), RpcTarget.All);
        }

        [PunRPC]
        public void StartGameRPC()
        {
            IsGameStarted = true;
            players = GameManager.Instance.players.ToArray();
            Spawn();
            
            if (PhotonNetwork.IsMasterClient)
            {
                StartRound();
            }
        }
        
        [PunRPC]
        public void StartRoundRPC(float currentTimeLimitL, float timeLeftL, int taggedPlayerNum)
        {
            currentTimeLimit = currentTimeLimitL;
            timeLeft = timeLeftL;
            foreach (var player in players)
            {
                if (player.actorNumber == taggedPlayerNum)
                {
                    taggedPlayer = player;
                    break;
                }
            }
        }
        
        [PunRPC]
        public void EndRoundRPC()
        {
            IsGamePaused = true;
            taggedPlayer.Die();
            taggedPlayer = null;
            // Kill tagged player
            round++;
        }

        [PunRPC]
        public void Spawn()
        {
            GameManager.Instance.SpawnPlayer();
        }
        
        [PunRPC]
        public void TagPlayerRPC(int id)
        {
            if (IsGameEnded) return;
            if (taggedPlayer.actorNumber == id) return;
            // Remove tagged player
            UntagPlayer();
            
            foreach (var player in players)
            {
                if (player.actorNumber == id)
                {
                    taggedPlayer = player;
                    break;
                }
            }
        }

        private void UntagPlayer()
        {
            taggedPlayer = null;
            // TODO: Untag player
        }
    }
}