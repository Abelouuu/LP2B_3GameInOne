using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Vitesse de déplacement du joueur
    public float speed = 5.0f;

    // Coroutine utilisée pour gérer le bonus de vitesse
    public Coroutine speedCoroutine;

    private void Start()
    {
        // Place le joueur à sa position de départ au lancement de la partie
        transform.position = new Vector3(-7f, 0f, 0f);
    }

    private void Update()
    {
        // Déplacement vers la gauche, avec une limite pour ne pas sortir de l'écran
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -8.1f)
        {
            transform.Translate(-speed * Time.deltaTime, 0f, 0f);
        }
        // Déplacement vers la droite, avec une limite à droite
        else if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 8.1f)
        {
            transform.Translate(speed * Time.deltaTime, 0f, 0f);
        }

        // Déplacement vers le haut, avec une limite verticale
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < 2.9f)
        {
            transform.Translate(0f, speed * Time.deltaTime, 0f);
        }
        // Déplacement vers le bas, avec une limite verticale
        else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -4.44f)
        {
            transform.Translate(0f, -speed * Time.deltaTime, 0f);
        }
    }

    public void IncreaseSpeed(float multiplier, float duration)
    {
        // Si un bonus de vitesse est déjà actif, on l'arrête avant d'en lancer un nouveau
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }

        // Lance le bonus de vitesse temporaire
        speedCoroutine = StartCoroutine(SpeedBonus(multiplier, duration));
    }

    private IEnumerator SpeedBonus(float speedMultiplier, float duration)
    {
        // Augmente temporairement la vitesse du joueur
        speed *= speedMultiplier;

        // Attend la durée du bonus
        yield return new WaitForSeconds(duration);

        // Remet la vitesse à sa valeur d'origine
        speed /= speedMultiplier;
    }
}