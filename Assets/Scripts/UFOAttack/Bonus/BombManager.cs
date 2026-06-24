using UnityEngine;
using UnityEngine.UI;

public class BombManager : MonoBehaviour
{
    // Images affichées dans l'interface pour représenter les bombes disponibles
    [SerializeField] private Image[] bombIcons;

    // Sprite affiché quand une bombe est disponible
    [SerializeField] private Sprite bombAvailableSprite;

    // Sprite affiché quand un emplacement de bombe est vide
    [SerializeField] private Sprite bombEmptySprite;

    // Préfab de l'explosion créée quand le joueur utilise une bombe
    [SerializeField] private GameObject bombExplosionPrefab;

    // Son joué quand le joueur essaye d'utiliser une bombe sans en avoir
    [SerializeField] private AudioClip errorSound;

    // Son joué quand une bombe est utilisée
    [SerializeField] private AudioClip bombSound;

    // Nombre actuel de bombes possédées par le joueur
    private int currentBombs = 0;

    // Nombre maximum de bombes que le joueur peut stocker
    private int maxBombs = 4;

    private void Start()
    {
        // Initialise l'affichage des bombes au lancement
        UpdateBombUI();
    }

    private void Update()
    {
        // Si le joueur appuie sur E, il utilise une bombe
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseBomb();
        }
    }

    public void AddBomb()
    {
        // Ajoute une bombe seulement si le maximum n'est pas atteint
        if (currentBombs < maxBombs)
        {
            currentBombs++;
            UpdateBombUI();
        }
    }

    private void UseBomb()
    {
        if (currentBombs > 0)
        {
            // Joue le son de bombe
            AudioManager.Instance.PlaySFX(bombSound);

            // Consomme une bombe
            currentBombs--;

            // Met à jour l'interface
            UpdateBombUI();

            // Crée l'explosion à la position du joueur
            Instantiate(bombExplosionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Joue un son d'erreur si aucune bombe n'est disponible
            AudioManager.Instance.PlaySFX(errorSound);   
        }
    }

    private void UpdateBombUI()
    {
        // Parcourt tous les emplacements de bombe dans l'interface
        for (int i = 0; i < bombIcons.Length; i++)
        {
            // Si l'indice est inférieur au nombre de bombes, l'icône est pleine
            if (i < currentBombs)
            {
                bombIcons[i].sprite = bombAvailableSprite;
            }
            // Sinon, l'icône est vide
            else
            {
                bombIcons[i].sprite = bombEmptySprite;
            }
        }
    }
}