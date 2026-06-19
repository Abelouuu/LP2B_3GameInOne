using UnityEngine;

public class GhostFade : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float shrinkSpeed = 1f;

    [SerializeField] private float movementSpeed = -8f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Fade
        Color color = spriteRenderer.color;
        color.a -= fadeSpeed * Time.deltaTime;
        spriteRenderer.color = color;

        // Shrink
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
        transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        // Sécurité pour éviter les tailles négatives
        if (transform.localScale.x < 0)
        {
            transform.localScale = Vector3.zero;
        }

        // Destruction
        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}