using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PaddleMovement : MonoBehaviour
{
    [Header("Paddle Movement Settings")]
    public float vitesseBase = 10f;
    public float vitesseBoostee = 18f;
    public float vitesseLente = 5f; 
    public float limiteX = 7.5f;
    public bool estRalenti = false; // 🌟 Tells the brick whether the paddle is currently slowed down

    [Header("Paddle Sprites")]
    public Sprite spriteNormal;
    public Sprite spriteBoost; // "Fast"
    public Sprite spriteLent;   // "Slow"

    private SpriteRenderer spriteRenderer;
    private float vitesseActuelle;
    private Coroutine coroutineEffet; // To handle and override active modifier effects

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        vitesseActuelle = vitesseBase;
        
        if (spriteRenderer != null && spriteNormal != null)
        {
            spriteRenderer.sprite = spriteNormal;
        }
    }

    void Update()
    {
        // Gather keyboard horizontal input
        float mouvement = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed) mouvement = -1f;
            if (Keyboard.current.rightArrowKey.isPressed) mouvement = 1f;
        }

        // Translate the paddle position horizontally
        transform.Translate(Vector2.right * mouvement * vitesseActuelle * Time.deltaTime);

        // Clamp the position to prevent the paddle from moving off-screen
        float positionX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
    }

    public void ActiverBoostVitesse()
    {
        // 🌟 If a speed boost is activated, it is definitely not a slowness malus anymore
        estRalenti = false; 
        DemarrerEffet(vitesseBoostee, spriteBoost, true);
    }

    public void ActiverMalusLenteur()
    {
        // 🌟 Set the safeguard flag to true so bricks stop dropping black skulls
        estRalenti = true; 
        DemarrerEffet(vitesseLente, spriteLent, false);
    }

    private void DemarrerEffet(float nouvelleVitesse, Sprite nouveauSprite, bool flip)
    {
        // Cancel the running coroutine effect before applying a new one
        if (coroutineEffet != null)
        {
            StopCoroutine(coroutineEffet);
        }
        coroutineEffet = StartCoroutine(ChronoEffet(nouvelleVitesse, nouveauSprite, flip));
    }

    private IEnumerator ChronoEffet(float nouvelleVitesse, Sprite nouveauSprite, bool flip)
    {
        vitesseActuelle = nouvelleVitesse;

        if (spriteRenderer != null && nouveauSprite != null)
        {
            spriteRenderer.sprite = nouveauSprite;
            spriteRenderer.flipY = flip;
        }

        // Wait for the modifier effect duration (10 seconds)
        yield return new WaitForSeconds(10f);

        // Reset to default movement configurations
        vitesseActuelle = vitesseBase;
        estRalenti = false; // 🌟 Safely revert state back to false once the duration finishes

        if (spriteRenderer != null && spriteNormal != null)
        {
            spriteRenderer.sprite = spriteNormal;
            spriteRenderer.flipY = false;
        }

        coroutineEffet = null;
    }
}