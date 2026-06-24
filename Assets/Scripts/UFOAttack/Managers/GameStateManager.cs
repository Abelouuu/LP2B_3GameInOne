using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    // Instance unique du GameStateManager, accessible depuis les autres scripts
    public static GameStateManager Instance;

    [Header("Start")]
    // Interface affichée au début de la partie
    [SerializeField] private GameObject startUI;

    // Durée pendant laquelle le jeu reste bloqué au démarrage
    [SerializeField] private float startDuration = 4.5f;

    [Header("Pause")]
    // Menu affiché quand le jeu est en pause
    [SerializeField] private GameObject pauseMenu;

    [Header("Game Over")]
    // Menu affiché après la mort du joueur
    [SerializeField] private GameObject gameOverMenu;

    // Interface contenant l'animation de game over
    [SerializeField] private GameObject gameOverAnimationUI;

    // Animator utilisé pour lancer l'animation de game over
    [SerializeField] private Animator gameOverAnimator;

    // Temps d'attente avant d'afficher le menu de game over
    [SerializeField] private float gameOverMenuDelay = 1.5f;

    [Header("Player")]
    // Référence vers le joueur
    [SerializeField] private GameObject player;

    [Header("Scene")]
    // Nom de la scène du jeu, utilisée pour recommencer la partie
    [SerializeField] private string gameSceneName = "UFOGameScene";

    [Header("Audio")]
    // Musique principale de la partie
    [SerializeField] private AudioClip gameMusic;

    // Son joué lors de l'ouverture ou fermeture de la pause
    [SerializeField] private AudioClip pauseSound;

    // Musique jouée sur l'écran de game over
    [SerializeField] private AudioClip gameOverMusic;

    // États principaux du jeu
    private bool isStarting = true;
    private bool isPaused = false;
    private bool isGameOver = false;

    private void Awake()
    {
        // Initialisation de l'instance
        Instance = this;

        // Le jeu est mis en pause au début pour afficher l'écran de départ
        Time.timeScale = 0f;
    }

    private void Start()
    {
        // On cache les menus au lancement
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameOverAnimationUI.SetActive(false);

        // Lance la séquence de début de partie
        StartCoroutine(StartGameRoutine());
    }

    private void Update()
    {
        // La touche Échap permet d'activer ou désactiver la pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private IEnumerator StartGameRoutine()
    {
        // Attend en temps réel, même si Time.timeScale vaut 0
        yield return new WaitForSecondsRealtime(startDuration);

        // La partie commence réellement
        isStarting = false;
        Time.timeScale = 1f;

        // Supprime l'interface de départ après un petit délai
        Destroy(startUI, 2f);

        // Lance la musique principale du jeu
        AudioManager.Instance.PlayMusic(gameMusic);
    }

    public void TogglePause()
    {
        // On ne peut pas mettre en pause pendant le début ou après un game over
        if (isStarting || isGameOver)
            return;

        // Alterne entre pause et reprise du jeu
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (isStarting || isGameOver)
            return;

        isPaused = true;
        pauseMenu.SetActive(true);

        // Arrête le temps du jeu
        Time.timeScale = 0f;

        // Joue le son de pause et met la musique en pause
        AudioManager.Instance.PlaySFX(pauseSound);
        AudioManager.Instance.PauseMusic();
    }

    public void ResumeGame()
    {
        if (isGameOver)
            return;

        isPaused = false;
        pauseMenu.SetActive(false);

        // Relance le temps du jeu
        Time.timeScale = 1f;

        // Joue le son puis reprend la musique
        AudioManager.Instance.PlaySFX(pauseSound);
        AudioManager.Instance.ResumeMusic();
    }

    public void TriggerGameOver()
    {
        // Sécurité pour éviter de lancer le game over plusieurs fois
        if (isGameOver)
            return;

        isGameOver = true;
        isPaused = false;

        // Baisse le volume des effets sonores pour la transition de fin
        AudioManager.Instance.SetSFXVolume(0.3f);

        // Désactive la pause et relance le temps si le joueur était en pause
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        // Empêche le joueur de continuer à tirer après sa mort
        DisablePlayerShoot();

        // Active l'interface et l'animation de game over
        gameOverAnimationUI.SetActive(true);
        gameOverAnimator.SetBool("IsGameOver", true);

        // Lance l'affichage du menu de game over après un délai
        StartCoroutine(ShowGameOverMenuAfterDelay());
    }

    private void DisablePlayerShoot()
    {
        // Récupère le script de tir du joueur
        PlayerShoot shoot = player.GetComponent<PlayerShoot>();

        // Désactive le tir si le script existe
        if (shoot != null)
        {
            shoot.enabled = false;
        }
    }

    private IEnumerator ShowGameOverMenuAfterDelay()
    {
        // Baisse progressivement la musique et les sons
        yield return StartCoroutine(AudioManager.Instance.FadeMusicAndSFX(1f));

        // Attend avant d'afficher le menu final
        yield return new WaitForSeconds(gameOverMenuDelay);

        // Affiche le menu de game over et lance sa musique
        gameOverMenu.SetActive(true);
        AudioManager.Instance.PlayGameOverMusic(gameOverMusic);
    }

    public void RestartGame()
    {
        // Réinitialise le temps puis recharge la scène du jeu
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadMainMenu()
    {
        // Réinitialise le temps puis charge le menu principal
        Time.timeScale = 1F;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // Quitte l'application, utile surtout dans le build final
        Application.Quit();
    }
}