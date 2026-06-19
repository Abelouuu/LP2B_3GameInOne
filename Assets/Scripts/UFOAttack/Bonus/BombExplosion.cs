using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombExplosion : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.CompareTag("Projectile") && collision.CompareTag("laser") && collision.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }
    }
}