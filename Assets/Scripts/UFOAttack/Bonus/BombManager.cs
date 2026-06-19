using UnityEngine;
using UnityEngine.UI;

public class BombManager : MonoBehaviour
{
    [SerializeField] private Image[] bombIcons;

    [SerializeField] private Sprite bombAvailableSprite;
    [SerializeField] private Sprite bombEmptySprite;
    [SerializeField] private GameObject bombExplosionPrefab;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip bombSound;


    private int currentBombs = 0;
    private int maxBombs = 4;

    private void Start()
    {
        UpdateBombUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseBomb();
        }
    }

    public void AddBomb()
    {
        if (currentBombs < maxBombs)
        {
            currentBombs++;
            UpdateBombUI();
        }
    }

    private void UseBomb()
    {
        if (currentBombs > 0)
        {
            AudioManager.Instance.PlaySFX(bombSound);
            currentBombs--;
            UpdateBombUI();

            Instantiate(bombExplosionPrefab, transform.position, Quaternion.identity);
        } else
        {
            AudioManager.Instance.PlaySFX(errorSound);   
        }
    }

    private void UpdateBombUI()
    {
        for (int i = 0; i < bombIcons.Length; i++)
        {
            if (i < currentBombs)
            {
                bombIcons[i].sprite = bombAvailableSprite;
            }
            else
            {
                bombIcons[i].sprite = bombEmptySprite;
            }
        }
    }
}