using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

/// <summary>
/// Manages the Game Over screen logic, displays the final score, and handles button clicks to restart or return to the menu.
/// </summary>
public class GameOver_Script : MonoBehaviour
{
    // --- PUBLIC ATTRIBUTES ---
    // Reference to the 3D TextMeshPro object used to display the player's performance
    public TextMeshProUGUI finalScoreDisplay; 

    // References to the interactive 2D GameObject buttons in the scene
    public GameObject replayButton;
    public GameObject menuButton;

    /// <summary>
    /// Called before the first frame update.
    /// Retrieves the saved score from memory and updates the screen text.
    /// </summary>
    void Start()
    {
        // Step 1: Retrieve the saved score from Unity's persistent storage (defaults to 0 if not found)
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        
        // Step 2: Output the fetched score to the Unity Console for debugging purposes
        Debug.Log("The retrieved score from PlayerPrefs is: " + finalScore);

        // Step 3: Check if the text component is properly assigned, then update its content
        if (finalScoreDisplay != null)
        {
            finalScoreDisplay.text = "Score Final : " + finalScore;
        }
        else
        {
            // Log an error in the console to alert the developer if the field is empty in the Inspector
            Debug.LogError("Warning! The Final Score Display component is not assigned in the Inspector!");
        }
    }

    /// <summary>
    /// Called once per frame.
    /// Monitors mouse clicks and uses 2D Raycasting to detect physical clicks on the button GameObjects.
    /// </summary>
    void Update()
    {
        // Check if the player has pressed the left mouse button (or tapped the screen)
        if (Input.GetMouseButtonDown(0)) 
        {
            // Convert the screen mouse position into 2D world space coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Cast a ray at the mouse position to check if it hits a 2D Collider
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Verify if the raycast actually hit an object with a collider
            if (hit.collider != null)
            {
                // Trigger action if the clicked object is the Replay button
                if (hit.collider.gameObject == replayButton)
                {
                    // Reload the main gameplay scene
                    SceneManager.LoadScene("Game");
                }
                // Trigger action if the clicked object is the Menu button
                else if (hit.collider.gameObject == menuButton)
                {
                    // Load the main title screen scene
                    SceneManager.LoadScene("MainMenu"); 
                }
            }
        }
    }
}