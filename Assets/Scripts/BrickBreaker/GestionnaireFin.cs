using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireFin : MonoBehaviour
{
    public void RejouerLeJeu()
    {
        // VERY IMPORTANT: Reset the time scale to 1 in case it was frozen!
        Time.timeScale = 1f; 
        
        // ⚠️ REPLACE with the exact name of your gameplay level scene
        SceneManager.LoadScene("MonNiveau"); 
    }

    public void RetourAuMenu()
    {
        // Reset the time scale before loading the main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}