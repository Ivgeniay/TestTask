﻿using Mirror;
using UnityEngine;

namespace UI
{
    internal class DisconnectButtonUI : MonoBehaviour
    {
        public void Disconnect()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
                NetworkManager.singleton.StopHost();
            else if (NetworkClient.active)
                NetworkClient.Disconnect();
            else if (NetworkServer.active)
                NetworkManager.singleton.StopServer();
        }
    }
}
