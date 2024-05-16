using UnityEngine;

namespace Platformer
{
    public class Coin : MonoBehaviour
    {
        public int coinValue;
        private bool hasTriggerred;

        private ScoreManager scoreManager;

        private void Start()
        {
            scoreManager = ScoreManager.instance;
        }

        private void OnTriggerEnter2D(Collider2D colision)
        {
            if (colision.CompareTag("Player") && !hasTriggerred)
            {

                hasTriggerred = true;
                scoreManager.ChangeScore(coinValue);
                Destroy(gameObject);
            }
        }
    }
}