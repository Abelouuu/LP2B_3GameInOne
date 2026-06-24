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
    public void Replay()
    {
        SceneManager.LoadScene("GameApple");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}