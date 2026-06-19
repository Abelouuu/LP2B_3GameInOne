using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private SpriteRenderer shieldRenderer;
    [SerializeField] private Sprite[] shieldStates;

    private int shieldHealth = 0;

    public bool IsActive => shieldHealth > 0;

    public void ActivateShield()
    {
        shieldHealth = shieldStates.Length;

        shieldObject.SetActive(true);
        UpdateShieldVisual();
    }

    public void TakeShieldDamage(int damage)
    {
        shieldHealth -= damage;

        if (shieldHealth <= 0)
        {
            shieldHealth = 0;
            shieldObject.SetActive(false);
        }
        else
        {
            UpdateShieldVisual();
        }
    }

    private void UpdateShieldVisual()
    {
        int index = shieldHealth - 1;
        shieldRenderer.sprite = shieldStates[index];
    }
}