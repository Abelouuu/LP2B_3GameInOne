using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the player's movement, game timer, score tracking, and interaction with falling fruits.
/// </summary>
public class Player_Script : MonoBehaviour
{
    // --- EDITABLE ATTRIBUTES ---
    [SerializeField] protected float speed = 10.0f;         // Speed modifier for horizontal movement
    [SerializeField] protected float timeRemaining = 120f;  // Total game duration (2 minutes by default)
    [SerializeField] private AudioClip rottenSound;         // Specific audio clip for bad fruit collisions

    // --- INTERNAL REFERENCES (3D TEXTMESHPRO COMPATIBLE) ---
    public TextMeshPro displayed_text;                      // Screen text reference to update the score display
    public TextMeshPro timer_text;                          // Screen text reference to update the countdown
    public AudioClip ref_audioClip;                         // Audio clip played when catching regular or golden fruits

    // --- PRIVATE / PROTECTED VARIABLES ---
    protected int score = 0;                                // The player's active score during the game session
    protected AudioSource ref_audioSource;                  // Runtime component used to play back sound effects

    /// <summary>
    /// Called before the first frame update.
    /// Initializes components, resets data registries, and prepares initial HUD states.
    /// </summary>
    void Start()
    {
        // Add and configure the runtime AudioSource component for standard feedback
        ref_audioSource = gameObject.AddComponent<AudioSource>();
        ref_audioSource.clip = ref_audioClip;
        ref_audioSource.loop = false;

        // Reset the score record in Unity memory at the start of a new game session
        PlayerPrefs.SetInt("FinalScore", 0);
        PlayerPrefs.Save();

        // Display the initial score setup on screen if the reference is valid
        if (displayed_text != null)
        {
            displayed_text.text = "Score : " + score;
        }
    }

    /// <summary>
    /// Called once per frame.
    /// Handles user input translation, horizontal boundary clamping, and real-time countdown progression.
    /// </summary>
    void Update()
    {
        // 1. CHARACTER MOVEMENT PROCESSING
        // Retrieve horizontal axis input values (Left/Right or A/D inputs)
        float moveInput = Input.GetAxis("Horizontal");
        // Apply directional movement based on frame timing data and the predefined speed modifier
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);

        // Enforce boundary constraints so CatchBoy cannot navigate off-screen
        float clampedX = Mathf.Clamp(transform.position.x, -8.5f, 8.5f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("MainMenu");
        }

        // 2. TIMING AND SESSION END DETECTION
        if (timeRemaining > 0)
        {
            // Reduce the countdown timer by the elapsed time since the previous frame
            timeRemaining -= Time.deltaTime;
            // Format and update the visual timer representation
            DisplayTime(timeRemaining);
        }
        else
        {
            // Ensure values do not drop below absolute zero
            timeRemaining = 0;
            // Transition directly to the Game Over scene when time expires
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverApple");
        }
    }

    /// <summary>
    /// Formats raw time variables into standard digital stopwatch syntax (MM:SS) and updates the HUD.
    /// </summary>
    /// <param name="timeToDisplay">Raw float parameter representing the total remaining seconds.</param>
    void DisplayTime(float timeToDisplay)
    {
        // Separate total time into absolute minute blocks
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        // Extract the trailing seconds remainder from the mathematical block
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        
        // Push the compiled text data structure directly to the 3D text renderer
        if (timer_text != null)
        {
            timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    /// <summary>
    /// Automatically triggered when a collision occurs. Evaluates fruit types via Tags to compute points and feedback.
    /// </summary>
    /// <param name="col">Object container hosting structural data regarding the physical interaction.</param>
    void OnCollisionEnter2D(Collision2D col)
    {
        // Check if the collided object is a rare Golden Fruit bonus
        if (col.gameObject.CompareTag("Golden"))
        {
            score += 2; // Golden apple reward = +2 points
            if (ref_audioSource != null) ref_audioSource.Play(); // Play standard collection sound
        }
        // Check if the collided object is an unfavorable Rotten Fruit malus
        else if (col.gameObject.CompareTag("Rotten"))
        {
            score -= 2; // Rotten apple penalty = -2 points
            if (score < 0) score = 0; // Boundary safety mechanism to prevent negative point tallies

            // Play the malus audio cue on a quieter configuration profile (attenuated volume level)
            if (ref_audioSource != null && rottenSound != null)
            {
                ref_audioSource.PlayOneShot(rottenSound, 0.1f);
            }
        }
        // Default condition handles interactions with baseline Red Fruits
        else
        {
            score++; // Standard apple reward = +1 point
            if (ref_audioSource != null) ref_audioSource.Play(); // Play standard collection sound
        }
        
        // Update the 3D TextMeshPro object with the refreshed score data
        if (displayed_text != null)
        {
            displayed_text.text = "Score  " + score;
        }

        // Save the updated score record into system registries for cross-scene transition access
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();
    }
}