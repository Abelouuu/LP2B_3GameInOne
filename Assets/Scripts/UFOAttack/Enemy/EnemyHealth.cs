using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public static readonly List<EnemyHealth> ActiveEnemies = new List<EnemyHealth>();

    public int health = 5;

    public EnemyScore enemyScore;
    public string enemyType;

    public GameObject explosionAnim;
    public float scale = 1f;

    [Header("Boss Event")]
    public bool canBeEvacuatedByBossEvent = true;
    [SerializeField] private BossHealthBarUI bossHealthBarUI;
    private int maxHealth;

    [Header("Bomb Settings")]
    public bool instantKilledByBomb = true;
    public int bombDamage = 20;

    private bool isDead = false;

    private void Awake()
    {
        maxHealth = health;
    }

    public void SetBossHealthBar(BossHealthBarUI newBossHealthBarUI, string bossName)
    {
        bossHealthBarUI = newBossHealthBarUI;

        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.Show(bossName, maxHealth);
            bossHealthBarUI.SetHealth(health);
        }
    }

    private void OnEnable()
    {
        if (!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }
    }

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
        }

        if (collision.CompareTag("Bomb"))
        {
            if (instantKilledByBomb)
            {
                Die();
            }
            else
            {
                TakeDamage(bombDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;

        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.SetHealth(health);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        
        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.SetHealth(0);
            bossHealthBarUI.Hide();
        }
        
        if (ScoreManager.instance != null && enemyScore != null)
        {
            ScoreManager.instance.AddScore(enemyScore.points, enemyType);
        }

        if (explosionAnim != null)
        {
            GameObject explosion = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(scale, scale, scale);
            Destroy(explosion, 1f);
        }

        Destroy(gameObject);
    }
}