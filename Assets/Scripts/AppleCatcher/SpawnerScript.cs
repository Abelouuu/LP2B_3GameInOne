using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the randomized, balanced spawning of various fruit items using a bag system, 
/// and controls background music initialization alongside a visual camera fade-out effect.
/// </summary>
public class SpawnerScript : MonoBehaviour
{
    // --- PUBLIC ATTRIBUTES ---
    public AudioClip ref_audioClip;                         // Background music track assigned via Inspector
    public SpriteRenderer fader_renderer;                   // Full-screen overlay sprite utilized for the intro transition fade

    // --- PROTECTED PREFAB REFERENCES ---
    protected GameObject apple_prefab;                      // Holds the default red apple asset data
    protected GameObject rotten_prefab;                     // Holds the rotten apple hazard asset data
    protected GameObject golden_prefab;                     // Holds the rare golden apple bonus asset data

    // --- PROTECTED GAMEPLAY VARIABLES ---
    protected float timer = 3f;                             // Internal countdown tracking the delay until the next fruit spawn
    protected AudioSource ref_audioSource;                  // Audio playback pipeline dedicated to looping background music
    protected float current_alpha = 1;                      // Alpha transparency rating used to manipulate the overlay fade

    // --- PRIVATE BALANCED RANDOMIZATION MECHANICS ---
    private List<GameObject> fruitBag = new List<GameObject>(); // Dynamic array container behaving as a virtual "fruit bag"

    /// <summary>
    /// Called before the first frame update. 
    /// Asynchronously streams file assets, establishes ambient audio states, and loads the initial bag sequence.
    /// </summary>
    void Start()
    {
        // Dynamically locate and extract core game prefabs out of the designated Assets/Resources folder directories
        apple_prefab = Resources.Load<GameObject>("Apple_prefab");
        rotten_prefab = Resources.Load<GameObject>("Rotten_prefab"); 
        golden_prefab = Resources.Load<GameObject>("Golden_prefab"); 

        // Set up the persistent background loop mechanism via runtime component allocation
        ref_audioSource = gameObject.AddComponent<AudioSource>();
        ref_audioSource.loop = true;
        ref_audioSource.volume = 0.5f;
        ref_audioSource.clip = ref_audioClip;

        // Construct, load, and shuffle the baseline layout ratios inside the empty list structure
        RefillBag();

        // Initiate the coroutine process handling the ambient scene opening fade behavior
        StartCoroutine( FadeOutFromWhite() );
    }

    /// <summary>
    /// Restores the virtual bag content to enforce absolute spawn distributions (70% Red, 20% Rotten, 10% Golden),
    /// and applies a randomized Fisher-Yates array shuffle.
    /// </summary>
    void RefillBag()
    {
        // Flush out any remaining residues from the list data structure to guarantee fresh population loops
        fruitBag.Clear();

        // Inject precisely 70% baseline Red Apple units (7 items per bag loop)
        for (int i = 0; i < 7; i++) { fruitBag.Add(apple_prefab); }

        // Inject precisely 20% hazardous Rotten Apple units (2 items per bag loop)
        for (int i = 0; i < 2; i++) { fruitBag.Add(rotten_prefab); }

        // Inject precisely 10% highly valuable Golden Apple units (1 item per bag loop)
        for (int i = 0; i < 1; i++) { fruitBag.Add(golden_prefab); }

        // FISHER-YATES SHUFFLE ALGORITHM:
        // Iterate backward through the populated list matrix to rearrange elements into randomized index arrays
        for (int i = 0; i < fruitBag.Count; i++)
        {
            // Back up the element reference data situated at the current index location pointer
            GameObject temp = fruitBag[i];
            // Identify a randomized index match situated between the progressive pointer and list boundary length
            int randomIndex = Random.Range(i, fruitBag.Count);
            // Swap out data values between the active index position and selected random array point
            fruitBag[i] = fruitBag[randomIndex];
            fruitBag[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Called once per frame. 
    /// Manages the spawn timer interval updates, draws upcoming fruit items out of the bag, and instances them onto the scene canvas.
    /// </summary>
    void Update()
    {
        // Subtract elapsed frame time ticks directly from the primary cooldown value
        timer -= Time.deltaTime;

        // Process conditional asset generation the moment the delay timer hits zero or drops below
        if (timer <= 0)
        {
            // Compute a horizontal viewport offset constraint to frame item generation coordinates (-8.5 to 8.5)
            float randomX = Random.value * 17f - 8.5f;

            // Instantly restock and re-shuffle item populations if the current layout index list hits zero empty states
            if (fruitBag.Count == 0)
            {
                RefillBag();
            }

            // Retrieve the first prefab data structure resting on top of the list stack index allocation
            GameObject prefabToSpawn = fruitBag[0];
            // Pull the selected active element index out of memory lists to block duplicate spawns
            fruitBag.RemoveAt(0);

            // Execute game object instancing safely after validating that prefab asset file packages aren't missing
            if (prefabToSpawn != null)
            {
                // Duplicate the designated game prefab blueprint into the scene layout canvas
                GameObject newItem = Instantiate(prefabToSpawn);
                // Snap the duplicate coordinates onto the calculated random X offset point at a structural drop height of 6.0 units
                newItem.transform.position = new Vector3(randomX, 6.0f, 0);
            }

            // Compute a randomized delay range before opening up permissions for the subsequent spawn loop cycle
            timer = 0.5f + Random.value * 1f;
        }
    }

    /// <summary>
    /// Coroutine handling the fading animation transition out of a blank white screen overlay state.
    /// </summary>
    /// <returns>An IEnumerator workflow object managing timing dependencies over frame durations.</returns>
    IEnumerator FadeOutFromWhite()
    {
        // Induce a brief operational execution freeze to allow core graphics hardware environments to catch up
        yield return new WaitForSeconds(0.5f);
        // Safely switch on background loop playback tracking after confirming pipeline components exist
        if (ref_audioSource != null) ref_audioSource.Play();

        // Progressively scale alpha transparency rating down until full target visibility goals are met
        while (current_alpha > 0)
        {
            // Systematically decrease transparency values across progressive engine frame refresh cycles
            current_alpha -= Time.deltaTime / 2;
            // Apply the updated transparency configuration settings over the structural canvas renderer color arrays
            if (fader_renderer != null) fader_renderer.color = new Color(1, 1, 1, current_alpha);
            // Yield execution workflow focus back over to general systems until the consecutive frame refresh pass
            yield return null;
        }

        // Cleanly wipe the screen canvas fade-out geometry out of active engine hierarchies once operational tasks end
        if (fader_renderer != null) Destroy(fader_renderer.gameObject);
    }
}