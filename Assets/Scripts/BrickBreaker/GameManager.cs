using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Required for Keyboard input

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Statistics")]
    public int score = 0;
    public int viesRestantes = 3;

    [Header("UI (Gameplay Interface)")]
    public TextMeshProUGUI texteScore; 
    public GameObject imageVie1;
    public GameObject imageVie2;
    public GameObject imageVie3;

    [Header("End Screens")]
    public GameObject panneauGameOver;   
    public GameObject ecranVictoire;     
    public GameObject ecranpause;
    

    [Header("End Screen Score Texts")]
    public TextMeshProUGUI texteScoreFinDefaite;  
    public TextMeshProUGUI texteScoreFinVictoire; 

    [Header("Global Audio")]
    public AudioSource audioSourceGlobal; 
    public AudioClip sonVictoire;         
    public AudioClip sonDefaite; 

    private bool estEnPause = false; 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (panneauGameOver != null) panneauGameOver.SetActive(false);
        if (ecranVictoire != null) ecranVictoire.SetActive(false);
        
        Time.timeScale = 1f; 
        MettreAJourUI();
    }

    void Update()
    {
        // 🌟 NEW: Pressing Escape now toggles the Pause Menu instead of quitting!
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            BoutonTogglePause();
        }
    }

    // Action linked to your Pause Button or Escape key
    public void BoutonTogglePause()
    {
        if (panneauGameOver.activeSelf || ecranVictoire.activeSelf) return;

        estEnPause = !estEnPause;

        if (estEnPause)
        {
            Time.timeScale = 0f; // Freeze the game
            ecranpause.SetActive(true);
            Debug.Log("Game Paused via Button/Escape");
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            ecranpause.SetActive(false);
            Debug.Log("Game Resumed via Button/Escape");
        }
    }

    // 🌟 LINKED SCENE FIX: This function now targets your exact "Main Menu" scene
    public void BoutonMenuPrincipal()
    {
        Time.timeScale = 1f; // CRUCIAL: Always reset time scale to 1 before changing scenes!
        SceneManager.LoadScene("MainMenu"); // Loads your scene named "Main Menu"
    }

    public void AjouterPoints(int points)
    {
        score += points;
        MettreAJourUI();
        VerifierConditionVictoire();
    }

    void VerifierConditionVictoire()
    {
        GameObject[] briquesRestantes = GameObject.FindGameObjectsWithTag("Brick");
        if (briquesRestantes.Length <= 1)
        {
            Victoire();
        }
    }

    void Victoire()
    {
        Debug.Log("GameManager: VICTORY!");
        CouperTousLesSons();
        
        if (audioSourceGlobal != null && sonVictoire != null)
        {
            audioSourceGlobal.clip = sonVictoire;
            audioSourceGlobal.loop = false;
            audioSourceGlobal.Play();
        }

        if (texteScoreFinVictoire != null) 
        {
            texteScoreFinVictoire.text = "FINAL SCORE: " + score;
        }

        if (ecranVictoire != null) ecranVictoire.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void PerdreVie()
    {
        viesRestantes--;
        MettreAJourUI();

        if (viesRestantes <= 0)
        {
            GameOver();
        }
        else
        {
            Ball balle = FindAnyObjectByType<Ball>();
            if (balle != null) balle.ResetBalle();
        }
    }

    public void AjouterVie()
    {
        if (viesRestantes < 3)
        {
            viesRestantes++;
            MettreAJourUI(); 
            Debug.Log("GameManager: Life added! Total lives = " + viesRestantes);
        }
    }

    void MettreAJourUI()
    {
        if (texteScore != null) texteScore.text = "SCORE: " + score;
        if (imageVie1 != null) imageVie1.SetActive(viesRestantes >= 1);
        if (imageVie2 != null) imageVie2.SetActive(viesRestantes >= 2);
        if (imageVie3 != null) imageVie3.SetActive(viesRestantes >= 3);
    }

    void GameOver()
    {
        Debug.Log("GameManager: GAME OVER!");
        CouperTousLesSons();

        if (audioSourceGlobal != null && sonDefaite != null)
        {
            audioSourceGlobal.clip = sonDefaite;
            audioSourceGlobal.loop = false;
            audioSourceGlobal.Play();
        }

        if (texteScoreFinDefaite != null) 
        {
            texteScoreFinDefaite.text = "FINAL SCORE: " + score;
        }

        if (panneauGameOver != null) panneauGameOver.SetActive(true);
        Time.timeScale = 0f; 

        Ball balle = FindAnyObjectByType<Ball>();
        if (balle != null) 
        {
            Rigidbody2D rb = balle.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero; 
        }
    }

    void CouperTousLesSons()
    {
        AudioSource[] toutesLesSourcesAudio = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource source in toutesLesSourcesAudio)
        {
            source.Stop();
        }
    }

    public void BoutonRejouer()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}