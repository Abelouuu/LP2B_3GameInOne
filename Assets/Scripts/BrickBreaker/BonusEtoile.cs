using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BonusEtoile : MonoBehaviour
{
    public float vitesseDescente = 3f;

    void Update()
    {
        // 1. Constant downward movement
        transform.Translate(Vector2.down * vitesseDescente * Time.deltaTime);

        // 2. TP REQUIREMENT: Return to the main menu when Escape is pressed
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("MenuPrincipal");
        }

        // Safety destroy if the star falls out of bounds
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    // 🌟 PHYSICAL DETECTION: Triggers instantly when the star touches the Paddle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the Paddle
        if (collision.gameObject.GetComponent<PaddleMovement>() != null || collision.gameObject.CompareTag("Paddle"))
        {
            PaddleMovement paddleCible = collision.gameObject.GetComponent<PaddleMovement>();
            
            if (paddleCible != null)
            {
                Debug.Log("Star Bonus: Collected! Activating speed boost.");
                
                // Activate the speed boost on the paddle
                paddleCible.ActiverBoostVitesse();
            }

            // Instantly destroy the star collected from the scene
            Destroy(gameObject);
        }
    }
}