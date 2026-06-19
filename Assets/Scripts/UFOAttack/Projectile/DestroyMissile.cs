using UnityEngine;

public class DestroyMissile : MonoBehaviour
{
    public GameObject explosionAnim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bomb"))
        {
            explosionAnim = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            Destroy(explosionAnim, 1f);
            Destroy(gameObject);
        }
    }
}