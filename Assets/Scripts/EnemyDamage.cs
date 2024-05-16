using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public EnemyHealth eHealth;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Enemy"))
        {
            eHealth = collision2D.gameObject.GetComponent<EnemyHealth>();
            if (eHealth != null)
            {
                eHealth.TakeDamage(damage);
            }
        }
    }
}
