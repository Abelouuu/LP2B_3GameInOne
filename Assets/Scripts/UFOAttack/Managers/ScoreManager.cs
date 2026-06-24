using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    // Instance unique du ScoreManager, accessible depuis les autres scripts
    public static ScoreManager instance;

    // Textes affichés dans l'interface
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreGameOverText;
    public TextMeshProUGUI ramText;
    public TextMeshProUGUI purpleText;
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI bossText;
    public TextMeshProUGUI trackerText;
    public TextMeshProUGUI laserText;

    // Score total du joueur
    public int score = 0;

    // Compteurs du nombre d'ennemis tués par type
    public int ramScore = 0;
    public int purpleScore = 0;
    public int blueScore = 0;
    public int bossScore = 0;
    public int trackerScore = 0;
    public int laserScore = 0;

    // Score actuellement affiché, utilisé pour l'animation du score
    private int displayedScore = 0;

    // Durée de l'animation quand le score augmente
    public float scoreAnimationDuration = 0.3f;

    // Coroutine utilisée pour gérer l'animation du score
    private Coroutine scoreCoroutine;

    void Awake()
    {
        // Mise en place d'une instance unique
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // S'il existe déjà un ScoreManager, on détruit celui-ci
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount, string enemyType)
    {
        // Ajoute les points au score total
        score += amount;

        // Met à jour le compteur correspondant au type d'ennemi détruit
        switch (enemyType)
        {
            case "Ram":
                ramScore++;
                ramText.text = ": " + ramScore + "Killed";
                break;

            case "Purple":
                purpleScore++;
                purpleText.text = ": " + purpleScore + "Killed";
                break;

            case "Blue":
                blueScore++;
                blueText.text = ": " + blueScore + "Killed";
                break;

            case "Boss":
                bossScore++;
                bossText.text = ": " + bossScore + "Killed";
                break;

            case "Tracker":
                trackerScore++;
                trackerText.text = ": " + trackerScore + "Killed";
                break;

            case "Laser":
                laserScore++;
                laserText.text = ": " + laserScore + "Killed";
                break;
        }

        // Met à jour le score affiché dans le menu de game over
        scoreGameOverText.text = score.ToString();

        // Si une animation du score est déjà en cours, on l'arrête
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }

        // Lance l'animation du score
        scoreCoroutine = StartCoroutine(AnimateScore());
    }

    IEnumerator AnimateScore()
    {
        // Score de départ affiché avant l'animation
        int startScore = displayedScore;

        // Score final à atteindre
        int targetScore = score;

        float t = 0f;

        // Fait augmenter progressivement le score affiché
        while (t < scoreAnimationDuration)
        {
            t += Time.deltaTime;
            float ratio = t / scoreAnimationDuration;

            displayedScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, ratio));

            scoreText.text = displayedScore.ToString();

            yield return null;
        }

        // On force la valeur finale pour éviter un petit décalage
        displayedScore = targetScore;
        scoreText.text = displayedScore.ToString();
    }
}