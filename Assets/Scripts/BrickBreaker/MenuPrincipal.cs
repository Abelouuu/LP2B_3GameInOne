using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void LancerBrickBreaker()
    {
        // Make sure time flows normally when starting the game
        Time.timeScale = 1f; 
        
        // Load the actual gameplay level scene
        SceneManager.LoadScene("scene1"); 
    }
}