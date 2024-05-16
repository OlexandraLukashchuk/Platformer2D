using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public PlayerHealth pHealth;
    public float damage;
   
    // Funkcja wywoływana, gdy obiekt z tym skryptem koliduje z innym obiektem.
    private void OnCollisionEnter2D(Collision2D collision2D) 
    {
        // Sprawdza, czy obiekt, z którym wystąpiła kolizja, ma tag "Player".
        if (collision2D.gameObject.CompareTag("Player"))
        {
            // Pobiera komponent PlayerHealth z obiektu gracza, z którym wystąpiła kolizja.
            pHealth = collision2D.gameObject.GetComponent<PlayerHealth>();

            // Sprawdza, czy obiekt gracza ma komponent PlayerHealth. Jeśli tak, to znaczy, że może przyjąć obrażenia.
            if (pHealth != null)
            {
                // Wywołuje funkcję TakeDamage na komponencie PlayerHealth, zadając graczowi określoną ilość obrażeń.
                pHealth.TakeDamage(damage);
            }
        }
    }
}
