using UnityEngine;

public class GhostFade : MonoBehaviour
{
    // Vitesse à laquelle le fantôme devient transparent
    [SerializeField] private float fadeSpeed = 2f;

    // Vitesse à laquelle le fantôme rétrécit
    [SerializeField] private float shrinkSpeed = 1f;

    // Vitesse de déplacement du fantôme
    [SerializeField] private float movementSpeed = -8f;

    // SpriteRenderer utilisé pour modifier la transparence
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // On récupère le SpriteRenderer attaché à l'objet
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Diminue progressivement l'opacité du sprite
        Color color = spriteRenderer.color;
        color.a -= fadeSpeed * Time.deltaTime;
        spriteRenderer.color = color;

        // Réduit progressivement la taille de l'objet
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        // Déplace le fantôme horizontalement
        transform.position += Vector3.right * movementSpeed * Time.deltaTime;

        // Sécurité pour éviter que la taille devienne négative
        if (transform.localScale.x < 0)
        {
            transform.localScale = Vector3.zero;
        }

        // Quand le sprite est totalement transparent, on détruit l'objet
        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}