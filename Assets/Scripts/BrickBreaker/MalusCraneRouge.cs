using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MalusCraneRouge : MonoBehaviour
{
    public float vitesseDescente = 3.5f;

    void Update()
    {
        // 1. Constant downward movement
        transform.Translate(Vector2.down * vitesseDescente * Time.deltaTime);

        // 2. Return to the main menu when Escape is pressed
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("MenuPrincipal");
        }

        // Safety destroy if the player dodges the skull and it goes out of bounds
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    // 🌟 RELYING ON UNITY PHYSICS: Guaranteed impact detection!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the red skull hit the Paddle
        // (Make sure your Paddle has either the "Paddle" tag or the PaddleMovement script attached)
        if (collision.gameObject.GetComponent<PaddleMovement>() != null || collision.gameObject.CompareTag("Paddle"))
        {
            Debug.Log("Red Skull: Hit! Removing one life.");

            if (GameManager.instance != null)
            {
                GameManager.instance.PerdreVie(); 
            }

            // Destroy the skull immediately from the scene
            Destroy(gameObject);
        }
    }
}