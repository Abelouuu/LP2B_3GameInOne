using UnityEngine;

public class Brick : MonoBehaviour
{
    [Header("Configuration")]
    public int valeurPoints = 100; 

    [Header("Bonus Prefabs (Drop Blue Cubes Here)")]
    public GameObject prefabCoeur;       // Gives a life
    public GameObject prefabEtoile;      // Boosts speed (Fast)

    [Header("Malus Prefabs (Drop Skulls Here)")]
    public GameObject prefabMalusRouge;  // Removes a life
    public GameObject prefabMalusNoir;   // Slows down the paddle (Slow)

    // Essential static safeguards required by the Level Generator
    public static bool etoileDejaTombee = false;
    public static bool coeurDejaTombe = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            DetruireLaBrique();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            DetruireLaBrique();
        }
    }

    void DetruireLaBrique()
    {
        // Instant disabling of the collider to prevent phantom ball bounces
        Collider2D c = GetComponent<Collider2D>();
        if (c != null) c.enabled = false;

        // Send points to the GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.AjouterPoints(valeurPoints);
        }

        // BONUS / MALUS DROP SYSTEM
        // Roll a random number between 0 and 100
        int de = Random.Range(0, 100);

        // 1. Chance to drop the FAST STAR (15% chance, max 1 per level)
        if (!etoileDejaTombee && de < 15 && prefabEtoile != null)
        {
            Instantiate(prefabEtoile, transform.position, Quaternion.identity);
            etoileDejaTombee = true;
        }
        // 2. Chance to drop the LIFE HEART (15% chance, max 1 per level)
        else if (!coeurDejaTombe && de >= 15 && de < 30 && prefabCoeur != null)
        {
            Instantiate(prefabCoeur, transform.position, Quaternion.identity);
            coeurDejaTombe = true;
        }
        // 3. Chance to drop the RED SKULL life damage (10% chance)
        else if (de >= 30 && de < 40 && prefabMalusRouge != null)
        {
            Instantiate(prefabMalusRouge, transform.position, Quaternion.identity);
        }
        // 4. Chance to drop the BLACK SKULL slowness malus (10% chance)
        else if (de >= 40 && de < 50 && prefabMalusNoir != null)
        {
            // 🌟 SAFEGUARD: Check if the paddle is already slowed down
            PaddleMovement paddle = FindAnyObjectByType<PaddleMovement>();

            if (paddle != null && paddle.estRalenti)
            {
                Debug.Log("The paddle is already slowed down, black skull spawn canceled!");
                // Optional: you could replace it with something else here, 
                // but the brick just destroys itself without dropping anything.
            }
            else
            {
                // If the paddle is not slowed down, the black skull can drop normally
                Instantiate(prefabMalusNoir, transform.position, Quaternion.identity);
            }
        }

        // Final destruction of the brick from the scene
        Destroy(gameObject); 
    }
}