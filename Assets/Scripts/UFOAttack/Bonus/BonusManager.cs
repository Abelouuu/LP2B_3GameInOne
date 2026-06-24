using UnityEngine;

public class BonusManager : MonoBehaviour
{
    // Références vers les scripts du joueur qui vont être modifiés par les bonus
    public PlayerHealth playerHealth;
    public PlayerShoot playerShoot;
    public PlayerMovement playerMovement;
    public PlayerShield playerShield;
    public BombManager bombManager;

    // Son joué lorsqu'un bonus est récupéré
    [SerializeField] private AudioClip bonusSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bonus de vie : soigne le joueur
        if (collision.CompareTag("HealthBonus"))
        {
            playerHealth.Heal(3);
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }

        // Bonus de tir : augmente temporairement la cadence de tir
        else if (collision.CompareTag("BulletBonus"))
        {
            playerShoot.IncreaseFireRate(0.5f, 5f);
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }

        // Bonus de vitesse : augmente temporairement la vitesse du joueur
        else if (collision.CompareTag("SpeedBonus"))
        {
            playerMovement.IncreaseSpeed(1.5f, 5f);
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }

        // Bonus de bouclier : active la protection du joueur
        else if (collision.CompareTag("ShieldBonus"))
        {
            playerShield.ActivateShield();
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }

        // Bonus de bombe : ajoute une bombe au joueur
        else if (collision.CompareTag("BombBonus"))
        {
            bombManager.AddBomb();
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }
    }
}