using TMPro;
using UnityEngine;

namespace Platformer
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        private int score;
        [SerializeField] private TextMeshProUGUI coinText;

        public int CurrentScore => score;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (coinText != null)
            {
                coinText.text = "Coins: " + CurrentScore.ToString();
            }
        }

        public void ChangeScore(int coinValue)
        {
            score += coinValue;
            UpdateUI();
        }
    }
}
