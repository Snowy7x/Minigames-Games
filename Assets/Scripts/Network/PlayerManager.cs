using System;
using System.IO;
using Game;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    public class PlayerManager : NetworkClass
    {
        public Player player;
        public bool IsDead;
        public int actorNumber;
        public string playerName;

        private void Start()
        {
            playerName = photonView.Owner.NickName;
            actorNumber = photonView.Owner.ActorNumber;            
            if (!GameManager.Instance.players.Contains(this))
            {
                GameManager.Instance.players.Add(this);
            }
            
            if (IsMine)
            {
                GameManager.Instance.localPlayer = this;
            }
        }
        
      

        public void SpawnPlayer(Vector3 pos = new Vector3())
        {
            Debug.Log("Spawning player");
            if (player != null && player.gameObject.activeSelf)
            {
                Debug.Log("Player already spawned");
                return;
            }
            if (player != null)
            {
                Debug.Log("Player already exists");
                player.gameObject.SetActive(true);
                Respawn(pos);
                return;
            }
            GameObject obj = PhotonNetwork.Instantiate(Path.Join("PhotonPrefabs", "Player"),
                pos, Quaternion.identity, 0, new object[] {photonView.ViewID});
            player = obj.GetComponent<Player>();
        }
        
        public void Die()
        {
            player.Die();
        }
        
        public void Respawn(Vector3 pos = new Vector3())
        {
            player.transform.position = pos;
            player.Respawn();
        }
    }
}