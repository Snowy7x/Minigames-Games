using System;
using Game;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Network
{
    public class Player : NetworkClass
    {
        PlayerManager playerManager;
        [SerializeField] TMP_Text playerNameText;
        public string id;
        string playerName;
        bool isAlive = true;

        void Start()
        {
            int viewId = (int) photonView.InstantiationData[0];
            playerManager = PhotonView.Find(viewId).GetComponent<PlayerManager>();
            playerName = photonView.Owner.NickName;
            id = photonView.Owner.UserId;
            playerNameText.text = playerName;
            
            if (playerManager.player == null)
            {
                playerManager.player = this;
            }
        }
        
        public void Die()
        {
            if (!isAlive) return;
            isAlive = false;
            gameObject.SetActive(false);
            photonView.RPC(nameof(DieRPC), RpcTarget.Others);
        }
        
        [PunRPC]
        public void DieRPC()
        {
            isAlive = false;
            gameObject.SetActive(false);
        }

        [PunRPC]
        public void Respawn()
        {
            isAlive = true;
            GameManager.Instance.SpawnPlayerManager();
        }
    }
}