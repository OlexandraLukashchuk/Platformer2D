using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathFalling : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
           Die();
        }
       
    }
    public void Die()
    {
        
        Destroy(gameObject);
    }


}
