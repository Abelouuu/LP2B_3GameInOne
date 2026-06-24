using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the lifecycle and destruction behavior of falling fruits in the game.
/// </summary>
public class AppleScript : MonoBehaviour
{
    /// <summary>
    /// Called before the first frame update.
    /// Used for initial configurations when the fruit object is instantiated.
    /// </summary>
    void Start()
    {
        // No specific initialization needed for the fruit at spawn time
    }

    /// <summary>
    /// Called once per frame.
    /// Monitors the fruit's position to trigger clean-up if it falls out of bounds.
    /// </summary>
    void Update()
    {
        // Check if the fruit has fallen below the visible screen limit (-10.0 units on the Y axis)
        if (transform.position.y < -10.0f)
        {
            // Destroy the fruit object to prevent memory leaks and keep the hierarchy clean
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Automatically triggered by Unity when this 2D collider touches another 2D collider.
    /// Handles the disappearance of the fruit upon physical contact.
    /// </summary>
    /// <param name="col">Information about the specific collision event and the object touched.</param>
    void OnCollisionEnter2D(Collision2D col)
    {
        // Destroy the fruit instantly because it hit an object (the player or a bounding box)
        Destroy(gameObject);
    }
}