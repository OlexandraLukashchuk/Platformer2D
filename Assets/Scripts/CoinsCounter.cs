using TMPro;
using UnityEngine;

namespace Platformer
{
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinCounterText;

        void Update()
        {
            // Update the UI text with the current coin count
            coinCounterText.text = "Coins: " + ScoreManager.instance.CurrentScore.ToString();
        }
    }
}
