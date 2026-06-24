using UnityEngine;

public class GenerateurNiveau : MonoBehaviour
{
    [Header("Available Bricks")]
    public GameObject[] prefabsBriquesNormales; 
    public GameObject prefabBriqueDoree;
    
    [Header("Level Parameters")]
    public int colonnes = 12; 
    public int lignes = 6;
    public float espacementX = 1.3f; // Slightly reduced to fit nicely on the screen
    public float espacementY = 0.8f;
    public Vector2 positionDepart = new Vector2(-8.5f, 4.5f); 

    [Header("Random Settings")]
    [Range(0, 100)]
    public int chanceDeSpawn = 85; 
    [Range(0, 100)]
    public int chanceBriqueDoree = 15; 

    void Start()
    {
        // Reset the unique item limits before spawning the grid
        Brick.etoileDejaTombee = false;
        Brick.coeurDejaTombe = false;

        GenererNiveauBasique(); 
    }

    public void GenererNiveauBasique()
    {
        // 🛑 ABSOLUTE SAFEGUARD: If the list is empty, stop execution to prevent a crash!
        if (prefabsBriquesNormales == null || prefabsBriquesNormales.Length == 0)
        {
            Debug.LogError("WARNING: The list 'Prefabs Briques Normales' is EMPTY on object: " + gameObject.name + ". Drag your brick prefabs into it!");
            return; // Stops the function right here to avoid an IndexOutOfRangeException
        }

        // Loop through columns and rows to generate the level grid
        for (int x = 0; x < colonnes; x++)
        {
            for (int y = 0; y < lignes; y++)
            {
                // Decide whether a brick should spawn at this coordinate based on percentage
                if (Random.Range(0, 100) < chanceDeSpawn)
                {
                    Vector2 positionBrique = new Vector2(
                        positionDepart.x + (x * espacementX),
                        positionDepart.y - (y * espacementY)
                    );

                    // Roll a separate chance to decide if it spawns a golden brick
                    if (Random.Range(0, 100) < chanceBriqueDoree)
                    {
                        if (prefabBriqueDoree != null)
                        {
                            Instantiate(prefabBriqueDoree, positionBrique, Quaternion.identity);
                        }
                    }
                    else
                    {
                        // Safely select a random brick from the regular array list
                        int indexAleatoire = Random.Range(0, prefabsBriquesNormales.Length);
                        Instantiate(prefabsBriquesNormales[indexAleatoire], positionBrique, Quaternion.identity);
                    }
                }
            }
        }
    }
}