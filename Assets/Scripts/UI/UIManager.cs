using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject loadingMenu;

    private void Awake()
    {
        Instance = this;
        ShowLoadingMenu();
    }

    private void Start()
    {
        Launcher.Instance.OnJoinedLobbyEvent += ShowMainMenu;
        Launcher.Instance.OnTryJoin += ShowLoadingMenu;
        Launcher.Instance.OnJoinedRoomEvent += ShowGameMenu;
        Launcher.Instance.OnCreatedRoomEvent += ShowGameMenu;
    }

    private void OnDisable()
    {
        ReleaseEvents();
    }

    private void OnDestroy()
    {
        ReleaseEvents();
    }
    
    void ReleaseEvents()
    {
        Launcher.Instance.OnJoinedLobbyEvent -= ShowGameMenu;
        Launcher.Instance.OnTryJoin -= ShowLoadingMenu;
        Launcher.Instance.OnJoinedRoomEvent -= ShowGameMenu;
        Launcher.Instance.OnCreatedRoomEvent -= ShowGameMenu;
    }

    #region Menu

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        loadingMenu.SetActive(false);
    }

    public void ShowGameMenu()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        loadingMenu.SetActive(false);
    }

    public void ShowLoadingMenu()
    {
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);
        loadingMenu.SetActive(true);
    }
    
    #endregion
}
