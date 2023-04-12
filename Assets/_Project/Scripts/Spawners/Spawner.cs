using Mirror;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Spawners
{
    internal class Spawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        private void Awake() {
            foreach (Transform t in spawnPoints) 
                t.AddComponent<NetworkStartPosition>();
        }
    }
}
