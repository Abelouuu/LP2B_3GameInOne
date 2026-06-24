using UnityEngine;
using UnityEngine.InputSystem; 

public class Ball : MonoBehaviour
{
    [Header("Ball Physics")]
    public float vitesseInitiale = 5f;
    
    private float vitesse;
    private Rigidbody2D rb;

    [Header("Paddle Attachment")]
    public GameObject paddle;         
    public float decalageHauteur = 0.4f; 
    private bool estColleeAuPaddle = true;

    [Header("Ball Audio")]
    public AudioSource audioSourceBalle; 
    public AudioClip sonRebond;          

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (audioSourceBalle == null) audioSourceBalle = GetComponent<AudioSource>();
        if (paddle == null) paddle = GameObject.Find("Paddle"); 

        ResetBalle();
    }

    void Update()
    {
        // Handle ball position and input when it is attached to the paddle.
        if (estColleeAuPaddle && paddle != null)
        {
            Vector3 nouvellePosition = paddle.transform.position;
            nouvellePosition.y += decalageHauteur; 
            transform.position = nouvellePosition;

            bool toucheEspacePressee = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
            bool clicGauchePresse = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

            if (toucheEspacePressee || clicGauchePresse)
            {
                LancerBalle();
            }
        }
    }

    public void ResetBalle()
    {
        estColleeAuPaddle = true;
        vitesse = vitesseInitiale; 

        if (rb != null)
        {
            rb.simulated = false; 
            rb.linearVelocity = Vector2.zero;
        }
    }

    void LancerBalle()
    {
        if (rb != null && estColleeAuPaddle)
        {
            estColleeAuPaddle = false; 
            rb.simulated = true;         
            
            Vector2 directionInitiale = Vector2.up;
            rb.linearVelocity = directionInitiale * vitesse;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        JouerSonRebond();

        // Calculate a directional angle offset depending on where the ball hits the paddle
        if (collision.gameObject.name == "Paddle")
        {
            float xBall = transform.position.x;
            float xPadlle = collision.transform.position.x;
            float xgap = xBall - xPadlle;
            Vector2 gapvect = new Vector2(xgap * 3, 0);
            rb.linearVelocity += gapvect;
            rb.linearVelocity = rb.linearVelocity.normalized * vitesse;
        }

        // Detect the death zone underneath the paddle by its exact Hierarchy name
        if (collision.gameObject.name == "ZoneGameOver")
        {
            if (GameManager.instance != null) 
            {
                GameManager.instance.PerdreVie(); // Remove a life 
            }
            
            ResetBalle(); // Reset and stick the ball back onto the paddle properly!
        }
    }

    public void JouerSonRebond()
    {
        if (audioSourceBalle != null && sonRebond != null)
        {
            audioSourceBalle.PlayOneShot(sonRebond);
        }
    }
}