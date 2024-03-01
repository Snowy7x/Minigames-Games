using System;
using Photon.Pun;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    
    public event Action OnConnectedToMasterEvent;
    public event Action OnJoinedLobbyEvent;
    public event Action OnTryJoin;
    public event Action OnJoinedRoomEvent;
    public event Action OnCreatedRoomEvent; 

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        UIManager.Instance.ShowLoadingMenu();
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void JoinRandomRoom()
    {
        OnTryJoin?.Invoke();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void Leave()
    {
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        OnConnectedToMasterEvent?.Invoke();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 1000);
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        OnJoinedLobbyEvent?.Invoke();
        base.OnJoinedLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        OnJoinedRoomEvent?.Invoke();
        base.OnJoinedRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room");
        OnCreatedRoomEvent?.Invoke();
        base.OnCreatedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join random failed {message}");
        base.OnJoinRandomFailed(returnCode, message);
    }
    
    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
        base.OnLeftRoom();
    }

    #endregion
}