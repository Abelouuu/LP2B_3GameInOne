using System.Collections;
using UnityEngine;

public class EnemyEvacuation : MonoBehaviour
{
    // Vitesse à laquelle l'ennemi quitte l'écran
    public float evacuationSpeed = 8f;

    // Temps avant de détruire l'ennemi après le début de l'évacuation
    public float destroyDelay = 2.5f;

    // Permet d'éviter de lancer l'évacuation plusieurs fois
    private bool isEvacuating = false;

    // Direction dans laquelle l'ennemi va partir
    private Vector3 evacuationDirection;

    public void StartEvacuation()
    {
        // Si l'ennemi est déjà en train d'évacuer, on ne relance pas le processus
        if (isEvacuating)
            return;

        isEvacuating = true;

        // On arrête le comportement normal de l'ennemi
        StopEnemyBehaviours();

        // On désactive les collisions pour éviter qu'il interagisse encore avec le joueur
        DisableColliders();

        // Si l'ennemi est en haut de l'écran, il part vers le haut-droite
        if (transform.position.y >= 0f)
        {
            evacuationDirection = new Vector3(1f, 1f, 0f).normalized;
        }
        // Sinon, il part vers le bas-droite
        else
        {
            evacuationDirection = new Vector3(1f, -1f, 0f).normalized;
        }

        // Lancement du déplacement d'évacuation
        StartCoroutine(Evacuate());
    }

    private void StopEnemyBehaviours()
    {
        // On récupère tous les scripts présents sur l'ennemi
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            // On désactive tous les scripts sauf celui-ci et EnemyHealth
            // EnemyHealth est gardé pour conserver une gestion propre de l'ennemi
            if (script != this && script.GetType() != typeof(EnemyHealth))
            {
                script.StopAllCoroutines();
                script.enabled = false;
            }
        }
    }

    private void DisableColliders()
    {
        // On récupère tous les colliders de l'ennemi
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            // On désactive les collisions pendant l'évacuation
            col.enabled = false;
        }
    }

    private IEnumerator Evacuate()
    {
        float timer = 0f;

        // Pendant un certain temps, l'ennemi se déplace vers sa direction d'évacuation
        while (timer < destroyDelay)
        {
            transform.position += evacuationDirection * evacuationSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }

        // Une fois l'évacuation terminée, l'ennemi est détruit
        Destroy(gameObject);
    }
}