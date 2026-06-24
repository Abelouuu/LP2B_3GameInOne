using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombExplosion : MonoBehaviour
{
    private void Start()
    {
        // L'explosion est détruite automatiquement après 2 secondes
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si un objet touché correspond à une cible destructible, on le détruit
        if (collision.CompareTag("Enemy") && collision.CompareTag("Projectile") && collision.CompareTag("laser") && collision.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }
    }
}