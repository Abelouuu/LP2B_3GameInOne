using UnityEngine;

public class ZoneMort : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that fell into the bottom zone is the ball
        if (collision.CompareTag("Ball"))
        {
            // Call the dedicated GameManager function to handle life reduction and UI updates
            if (GameManager.instance != null)
            {
                GameManager.instance.PerdreVie(); 
            }
        }
    }
}