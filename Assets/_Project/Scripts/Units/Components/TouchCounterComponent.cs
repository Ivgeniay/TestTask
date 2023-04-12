using Engine;
using Mirror;
using UnityEngine;

namespace Units.Components
{
    internal class TouchCounterComponent : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnTouchCountChanged))] public int count = 0;
        public int Count { get => count; }

        private string nickName;

        public void IncreaseCount() {
            CmdIncreaseCount(count = count + 1);
        }

        [Command]
        private void CmdIncreaseCount(int count) {
            if (isLocalPlayer)
            {
                this.count = count;
                RpcSetCounter(count);
            }
        }

        [ClientRpc]
        private void RpcSetCounter(int count) {
            this.count = count;
        }

        [ClientRpc]
        private void RpcResetCounter() {
            this.count = 0;
        }

        private void OnTouchCountChanged(int oldTouchCount, int newTouchCount) {
            if (string.IsNullOrEmpty(nickName)) 
                nickName = GetComponent<UnitNameComponent>().NickName;
            
            GameEngine.Instance.PlayerTouch(nickName, newTouchCount);
            Debug.Log($"Old:{oldTouchCount} New:{newTouchCount} in {nickName}");

            if (newTouchCount >= GameEngine.Instance.WinCondition) {
                GameEngine.Instance.EndGame(nickName);
                //GameEngine.Instance.EndGame(gameObject);
            }
        }



    }
}
