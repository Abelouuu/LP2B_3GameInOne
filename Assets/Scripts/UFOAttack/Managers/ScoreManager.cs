using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreGameOverText;
    public TextMeshProUGUI ramText;
    public TextMeshProUGUI purpleText;
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI bossText;
    public TextMeshProUGUI trackerText;
    public TextMeshProUGUI laserText;

    public int score = 0;

    public int ramScore = 0;
    public int purpleScore = 0;
    public int blueScore = 0;
    public int bossScore = 0;
    public int trackerScore = 0;
    public int laserScore = 0;

    private int displayedScore = 0;

    public float scoreAnimationDuration = 0.3f;

    private Coroutine scoreCoroutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount, string enemyType)
    {
        score += amount;

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
        scoreGameOverText.text = score.ToString();
        if (scoreCoroutine != null){
            StopCoroutine(scoreCoroutine);
        }
        scoreCoroutine = StartCoroutine(AnimateScore());
    }

    IEnumerator AnimateScore()
    {
        int startScore = displayedScore;
        int targetScore = score;

        float t = 0f;

        while (t < scoreAnimationDuration)
        {
            t += Time.deltaTime;
            float ratio = t / scoreAnimationDuration;

            displayedScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, ratio));

            scoreText.text = displayedScore.ToString();

            yield return null;
        }

        displayedScore = targetScore;
        scoreText.text = displayedScore.ToString();
    }
}