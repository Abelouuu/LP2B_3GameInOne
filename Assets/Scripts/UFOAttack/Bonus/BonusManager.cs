using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerShoot playerShoot;
    public PlayerMovement playerMovement;
    public PlayerShield playerShield;
    public BombManager bombManager;
    [SerializeField] private AudioClip bonusSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthBonus"))
        {
            playerHealth.Heal(3);
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("BulletBonus"))
        {
            playerShoot.IncreaseFireRate(0.5f, 5f);
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("SpeedBonus"))
        {
            playerMovement.IncreaseSpeed(1.5f, 5f);
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("ShieldBonus"))
        {
            playerShield.ActivateShield();
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("BombBonus"))
        {
            bombManager.AddBomb();
            AudioManager.Instance.PlaySFX(bonusSound);
            Destroy(collision.gameObject);
        }
    }
}
