using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public GameObject explosionAnim;
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Bomb"))
        {
            explosionAnim = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            Destroy(explosionAnim, 1f);
            Destroy(gameObject);
        }
    }
}
