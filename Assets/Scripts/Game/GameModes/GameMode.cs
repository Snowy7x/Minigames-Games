using Network;
using UnityEngine;

namespace Game.GameModes
{
    public abstract class GameMode : NetworkClass
    {
        public string GameModeName;
        public string GameModeDescription;
        public bool IsGameStarted;
        public bool IsGameEnded;
        public bool IsGamePaused;
        public PlayerManager[] players;

        public abstract void PlayerInteraction(PlayerManager player);
        public abstract void PlayerDeath(PlayerManager player);
        public abstract void StartGame();
        public abstract void EndGame();

        public abstract void CheckWin();
        
        protected virtual void Update()
        {
            if (IsGameStarted && !IsGameEnded)
            {
                CheckWin();
            }
        }
    }
}