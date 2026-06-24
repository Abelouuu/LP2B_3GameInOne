using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    // Liste statique qui garde une référence vers tous les ennemis actifs dans la scène
    public static readonly List<EnemyHealth> ActiveEnemies = new List<EnemyHealth>();

    // Points de vie de l'ennemi
    public int health = 5;

    // Informations utilisées pour ajouter le score quand l'ennemi meurt
    public EnemyScore enemyScore;
    public string enemyType;

    // Animation jouée à la mort de l'ennemi
    public GameObject explosionAnim;
    public float scale = 1f;

    [Header("Boss Event")]
    // Permet de savoir si cet ennemi peut être supprimé lors d'un événement de boss
    public bool canBeEvacuatedByBossEvent = true;

    [SerializeField] private BossHealthBarUI bossHealthBarUI;
    private int maxHealth;

    [Header("Bomb Settings")]
    // Détermine si la bombe tue directement l'ennemi ou lui inflige seulement des dégâts
    public bool instantKilledByBomb = true;
    public int bombDamage = 20;

    // Évite que l'ennemi puisse mourir plusieurs fois
    private bool isDead = false;

    private void Awake()
    {
        // On sauvegarde la vie maximale au début, utile pour la barre de vie du boss
        maxHealth = health;
    }

    public void SetBossHealthBar(BossHealthBarUI newBossHealthBarUI, string bossName)
    {
        // On associe une barre de vie à cet ennemi, principalement pour les boss
        bossHealthBarUI = newBossHealthBarUI;

        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.Show(bossName, maxHealth);
            bossHealthBarUI.SetHealth(health);
        }
    }

    private void OnEnable()
    {
        // Quand l'ennemi est activé, on l'ajoute dans la liste des ennemis actifs
        if (!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }
    }

    private void OnDisable()
    {
        // Quand l'ennemi est désactivé ou détruit, on le retire de la liste
        ActiveEnemies.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si l'ennemi est déjà mort, on ignore les collisions
        if (isDead)
            return;

        // Si l'ennemi touche une balle du joueur, il perd 1 point de vie
        if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
        }

        // Si l'ennemi touche une bombe, soit il meurt directement,
        // soit il prend une quantité de dégâts définie
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
        // Empêche de reprendre des dégâts après la mort
        if (isDead)
            return;

        // On retire les dégâts aux points de vie
        health -= damage;

        // Si une barre de vie est liée, on la met à jour
        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.SetHealth(health);
        }

        // Si l'ennemi n'a plus de vie, il meurt
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Sécurité pour éviter d'exécuter la mort plusieurs fois
        if (isDead)
            return;

        isDead = true;

        // Si c'est un boss avec une barre de vie, on la vide puis on la cache
        if (bossHealthBarUI != null)
        {
            bossHealthBarUI.SetHealth(0);
            bossHealthBarUI.Hide();
        }

        // Ajout du score correspondant à l'ennemi détruit
        if (ScoreManager.instance != null && enemyScore != null)
        {
            ScoreManager.instance.AddScore(enemyScore.points, enemyType);
        }

        // Création de l'animation d'explosion à l'endroit de l'ennemi
        if (explosionAnim != null)
        {
            GameObject explosion = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(scale, scale, scale);
            Destroy(explosion, 1f);
        }

        // Destruction de l'objet ennemi
        Destroy(gameObject);
    }
}