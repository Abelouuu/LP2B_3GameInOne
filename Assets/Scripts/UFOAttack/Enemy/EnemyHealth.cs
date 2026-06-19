using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 5;

    public EnemyScore enemyScore;
    public string enemyType;

    public GameObject explosionAnim;

    public float scale;

    private bool isDead = false;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("PlayerBullet"))
        {
            health--;
            if (health <= 0)
            {
                ScoreManager.instance.AddScore(enemyScore.points, enemyType);

                explosionAnim = Instantiate(explosionAnim, transform.position, Quaternion.identity);
                explosionAnim.transform.localScale = new Vector3(scale,scale,scale);
                Destroy(explosionAnim, 1f);
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Bomb"))
        {
            ScoreManager.instance.AddScore(enemyScore.points, enemyType);

            explosionAnim = Instantiate(explosionAnim, transform.position, Quaternion.identity);
            explosionAnim.transform.localScale = new Vector3(scale,scale,scale);
            Destroy(explosionAnim, 1f);
            Destroy(gameObject);
        }
    }

    
}
