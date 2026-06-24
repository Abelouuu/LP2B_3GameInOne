using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    // Objet visuel du bouclier autour du joueur
    [SerializeField] private GameObject shieldObject;

    // SpriteRenderer utilisé pour changer l'apparence du bouclier
    [SerializeField] private SpriteRenderer shieldRenderer;

    // Différents sprites du bouclier selon son état de vie
    [SerializeField] private Sprite[] shieldStates;

    // Points de vie actuels du bouclier
    private int shieldHealth = 0;

    // Permet de savoir facilement si le bouclier est actif
    public bool IsActive => shieldHealth > 0;

    public void ActivateShield()
    {
        // La vie du bouclier correspond au nombre d'états disponibles
        shieldHealth = shieldStates.Length;

        // Active l'objet du bouclier
        shieldObject.SetActive(true);

        // Met à jour son apparence
        UpdateShieldVisual();
    }

    public void TakeShieldDamage(int damage)
    {
        // Retire les dégâts à la vie du bouclier
        shieldHealth -= damage;

        // Si le bouclier n'a plus de vie, on le désactive
        if (shieldHealth <= 0)
        {
            shieldHealth = 0;
            shieldObject.SetActive(false);
        }
        else
        {
            // Sinon, on change son sprite selon sa vie restante
            UpdateShieldVisual();
        }
    }

    private void UpdateShieldVisual()
    {
        // On utilise la vie restante pour choisir le sprite correspondant
        int index = shieldHealth - 1;
        shieldRenderer.sprite = shieldStates[index];
    }
}