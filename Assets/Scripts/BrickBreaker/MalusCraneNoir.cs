using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MalusCraneNoir : MonoBehaviour
{
    public float vitesseDescente = 3.5f;

    void Update()
    {
        // 1. Constant downward movement
        transform.Translate(Vector2.down * vitesseDescente * Time.deltaTime);

        // 2. TP REQUIREMENT: Return to the main menu when Escape is pressed
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("MenuPrincipal");
        }

        // Safety destroy if the black skull falls out of bounds
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    // 🌟 PHYSICAL DETECTION: Triggers instantly when the skull touches the Paddle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the Paddle
        if (collision.gameObject.GetComponent<PaddleMovement>() != null || collision.gameObject.CompareTag("Paddle"))
        {
            PaddleMovement paddleCible = collision.gameObject.GetComponent<PaddleMovement>();
            
            if (paddleCible != null)
            {
                Debug.Log("Black Skull: Collected! Activating slowness malus.");
                
                // Trigger the slowness effect on the paddle
                paddleCible.ActiverMalusLenteur();
            }

            // Instantly destroy the skull from the scene
            Destroy(gameObject);
        }
    }
}