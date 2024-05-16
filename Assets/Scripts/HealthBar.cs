using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float minHealth;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

}
