using UnityEngine;

public class ControlsHint : MonoBehaviour
{
    // Durée pendant laquelle l'indication des contrôles reste affichée
    [SerializeField] private float displayDuration = 15f;

    private void Start()
    {
        // Détruit automatiquement l'objet après le délai choisi
        Destroy(gameObject, displayDuration);
    }
}