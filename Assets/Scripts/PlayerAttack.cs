using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0;
    public int damage = 10;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
            }
        }
    }

    private void Attack()
    {
        // Losuje typ ataku spośród dwóch możliwości.
        int attackType = Random.Range(1, 3);

// Ustawia animator postaci tak, aby odtworzył animację ataku, której nazwa zależy od wylosowanego typu ataku.
        GetComponent<Animator>().SetTrigger("Attack" + attackType);

// Ustawia flagę attacking na true, co może być używane do blokowania pewnych akcji gracza w trakcie ataku.
        attacking = true;

// Przeszukuje przestrzeń wokół obiektu gracza w poszukiwaniu koliderów warstwy "enemyLayer" w odległości 1 jednostki.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("enemyLayer"));

// Iteruje przez znalezione kolizje.
        foreach (Collider2D hitCollider in hitColliders)
        {
            // Sprawdza, czy kolizja nie należy do obiektu gracza, aby nie zadawać obrażeń samemu sobie.
            if (hitCollider.gameObject != gameObject)
            {
                // Pobiera komponent EnemyHealth z obiektu, który został trafiony.
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();

                // Sprawdza, czy obiekt ma komponent EnemyHealth. Jeśli tak, to znaczy, że jest to przeciwnik, który może przyjąć obrażenia.
                if (enemyHealth != null)
                {
                    // Wywołuje funkcję TakeDamage na komponencie EnemyHealth, zadając przeciwnikowi określoną ilość obrażeń.
                    enemyHealth.TakeDamage(damage);
                }
                else
                {
                    // Jeśli obiekt nie ma komponentu EnemyHealth, wypisuje informację diagnostyczną w konsoli.
                    Debug.Log("Enemy does not have EnemyHealth component.");
                }
            }
        }

// Sprawdza, czy była chociaż jedna kolizja z przeciwnikiem.
        if (hitColliders.Length == 1)
        {
            // Jeśli nie było trafień, wypisuje informację diagnostyczną w konsoli.
            Debug.Log("No enemies hit.");
        }

    }
    
}