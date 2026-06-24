using UnityEngine;

public class BonusCoeur : MonoBehaviour
{
    public float vitesseDescente = 3.5f;

    void Update()
    {
        // Constant downward movement
        transform.Translate(Vector2.down * vitesseDescente * Time.deltaTime);

        // Safety destroy if the player misses the heart and it goes out of bounds
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    // 🌟 PHYSICAL DETECTION: Triggers instantly when the heart touches the Paddle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the Paddle
        if (collision.gameObject.GetComponent<PaddleMovement>() != null || collision.gameObject.CompareTag("Paddle"))
        {
            if (GameManager.instance != null)
            {
                // Call the GameManager function to add a life and update the UI
                GameManager.instance.AjouterVie(); 
            }
            
            // Destroy the falling bonus item from the scene
            Destroy(gameObject); 
        }
    }
}