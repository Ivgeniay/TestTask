using TMPro;
using UnityEngine;

namespace UI
{
    internal class UIMediator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI leaderboardTMP;
        [SerializeField] private TextMeshProUGUI winnerTMP;

        private void Awake()
        {
            HideLeaderboard();
            HideWinner();
        }

        public void ShowLeaderboard(string innerText) {
            leaderboardTMP.gameObject.SetActive(true);
            leaderboardTMP.text = innerText;
        }

        public void HideLeaderboard() {
            leaderboardTMP.text = string.Empty;
            leaderboardTMP.gameObject.SetActive(false);
        }

        public void ShowWinner(string innetTest) {
            winnerTMP.gameObject.SetActive(true); 
            winnerTMP.text = innetTest;
        }

        public void HideWinner() {
            winnerTMP.gameObject.SetActive(false); 
            winnerTMP.text = string.Empty;
        }
    }
}
