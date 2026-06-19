using UnityEngine;

public class DestroyPlayerProj : MonoBehaviour
{
    public GameObject explosionAnim;
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Missile")
        {
            explosionAnim = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            Destroy(explosionAnim, 1f);
            Destroy(gameObject);
        }
    }
}
