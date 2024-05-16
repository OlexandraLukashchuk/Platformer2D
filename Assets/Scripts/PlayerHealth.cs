using Platformer;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCanvas;
    private Animator m_animator;
    [SerializeField] private Slider healthSlider;
    [SerializeField] bool m_noBlood = true;
    [SerializeField] private float health = 100f;
    public float maxHealth;
    [SerializeField] private GameObject Hero;

    public Image HealthBar;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        maxHealth = health;
    }

    private void Update()
    {
        healthSlider.value = health / 100f;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Hurt(); // Trigger Hurt animation when taking damage
        if (health <= 0)
        {
            Die();
        }
    }

    private void Hurt()
    {
        // Player is hurt with animation
        m_animator.SetTrigger("Hurt");
    }

    public void Die()
    {
        // Player dies with animation

        m_animator.SetTrigger("Death");
        GameOverCanvas.SetActive(true);
        Hero.SetActive(false);
        // Optionally destroy the GameObject after the death animation
    }
}
