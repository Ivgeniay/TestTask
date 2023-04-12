using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UI;
using Units.Components;
using UnityEngine;

namespace Engine
{
    internal class GameEngine : NetworkBehaviour
    {
        public int WinCondition { get => winCondition; }

        [SerializeField] private int winCondition = 3;
        [SerializeField] private float restartSceneDelay = 5;
        [SerializeField] private UIMediator uiMediator;
        private Dictionary<string, int> touchCounts = new Dictionary<string, int>();

        #region Singleton
        private static GameEngine instance;
        public static GameEngine Instance { get => instance; private set => instance = value; }
        #endregion

        private void Awake() {
            Instance = this;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void EndGame(string NickName) {
            if (!isServer) return;

            RpcShowWinner(NickName + " is winner");
        }

        public void PlayerTouch(string nickName, int newTouchCount) {
            if (!isServer) return;

            if (!touchCounts.ContainsKey(nickName))
                touchCounts.Add(nickName, 0);
            
            touchCounts[nickName] = newTouchCount;

            var str = FromDictionaryToString(touchCounts);
            RpcShowLeaderboard(str);
        }

        private string FromDictionaryToString(Dictionary<string, int> touchCounts) {
            StringBuilder sb = new StringBuilder();
            foreach(var el in touchCounts) {
                sb
                    .Append(el.Key)
                    .Append(" :")
                    .Append(el.Value)
                    .Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        [ClientRpc]
        private void RpcShowLeaderboard(string winnerText)
        {
            uiMediator.HideWinner();
            uiMediator.ShowLeaderboard(winnerText);
        }

        [ClientRpc]
        private void RpcShowWinner(string winnerText) {
            uiMediator.HideLeaderboard();
            uiMediator.ShowWinner(winnerText);
            StartCoroutine(RestartSceneRoutine(restartSceneDelay));
        }

        private IEnumerator RestartSceneRoutine(float secondDelay) {
            yield return new WaitForSeconds(secondDelay);
            RestartScene();
        }
        private void RestartScene() {
            if (isServer) {
                RandomNameGenerator.CleanUsedName();
                NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
            }
        }
    }
}
