using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public Image EnemyHealthBar;

    void Start()
    {
        maxHealth = health;
        UpdateEnemyHealthBar();
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        UpdateEnemyHealthBar();

        // Check if the enemy is defeated
        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateEnemyHealthBar()
    {
        // Update the UI health bar based on the current health
        if (EnemyHealthBar != null)
        {
            EnemyHealthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        // Implement any death-related logic here
        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
