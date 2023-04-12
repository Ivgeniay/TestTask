using Mirror;
using System;
using TMPro;
using UnityEngine;

namespace Units.Components
{
    internal class UnitNameComponent : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI nicknameTMP;
        [SyncVar(hook = nameof(OnNameChanged))] private string nickName;
        public string NickName { get => nickName; }

        private void Start() {
            if (isLocalPlayer) {
                CmdSetPlayerName();
            }
        }

        [Command]
        private void CmdSetPlayerName() {
            var name = RandomNameGenerator.GetRandomName();
            nickName = name;
            RpcSetPlayerName(name);
        }

        [ClientRpc]
        private void RpcSetPlayerName(string name) =>
            nickName = name;
        

        private void OnNameChanged(string oldValue, string newValue) =>
            nicknameTMP.text = newValue;
        
    }
}
