using UnityEngine;

public class DestroyRam : MonoBehaviour
{
    public GameObject explosionAnim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            explosionAnim = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            Destroy(explosionAnim, 1f);
            Destroy(gameObject);
        }
    }
}