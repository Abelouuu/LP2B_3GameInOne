using UnityEngine;

public class BonusMovement : MonoBehaviour
{
    // Vitesse de déplacement du bonus vers la gauche
    [SerializeField] private float speed = 3f;

    // Position en X à partir de laquelle le bonus est détruit
    [SerializeField] private float destroyX = -12f;

    public void SetSpeed(float newSpeed)
    {
        // Permet de modifier la vitesse du bonus depuis un autre script
        speed = newSpeed;
    }

    private void Update()
    {
        // Le bonus se déplace constamment vers la gauche
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Si le bonus sort trop loin de l'écran, il est supprimé
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}