using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BreakableObject _breakable = collision.GetComponent<BreakableObject>();
        if (_breakable != null)
        {
            _breakable.Break();
        }

        EnemyHealth _enemyHealth = collision.GetComponent<EnemyHealth>();
        if (_enemyHealth != null)
        {
            _enemyHealth.TakeDamage(1, transform.position, true);
        }
    }
}
