using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("Start")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private float startDuration = 4.5f;

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameOverAnimationUI;
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private float gameOverMenuDelay = 1.5f;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "UFOGameScene";

    [Header("Audio")]
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip gameOverMusic;

    private bool isStarting = true;
    private bool isPaused = false;
    private bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 0f;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameOverAnimationUI.SetActive(false);

        StartCoroutine(StartGameRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private IEnumerator StartGameRoutine()
    {
        yield return new WaitForSecondsRealtime(startDuration);

        isStarting = false;
        Time.timeScale = 1f;

        Destroy(startUI, 2f);

        AudioManager.Instance.PlayMusic(gameMusic);
    }

    public void TogglePause()
    {
        if (isStarting || isGameOver)
            return;

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

        Time.timeScale = 0f;

        AudioManager.Instance.PlaySFX(pauseSound);
        AudioManager.Instance.PauseMusic();
    }

    public void ResumeGame()
    {
        if (isGameOver)
            return;

        isPaused = false;
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        AudioManager.Instance.PlaySFX(pauseSound);
        AudioManager.Instance.ResumeMusic();
    }

    public void TriggerGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        isPaused = false;

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        DisablePlayerShoot();

        gameOverAnimationUI.SetActive(true);
        gameOverAnimator.SetBool("IsGameOver", true);

        StartCoroutine(ShowGameOverMenuAfterDelay());
    }

    private void DisablePlayerShoot()
    {
        PlayerShoot shoot = player.GetComponent<PlayerShoot>();

        if (shoot != null)
        {
            shoot.enabled = false;
        }
    }

    private IEnumerator ShowGameOverMenuAfterDelay()
    {
        yield return StartCoroutine(AudioManager.Instance.FadeMusicAndSFX(1f));

        yield return new WaitForSeconds(gameOverMenuDelay);

        gameOverMenu.SetActive(true);
        AudioManager.Instance.PlayGameOverMusic(gameOverMusic);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1F;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}