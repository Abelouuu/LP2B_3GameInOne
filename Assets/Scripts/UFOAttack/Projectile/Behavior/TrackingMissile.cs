using UnityEngine;
using System.Collections;

public class TrackingMissile : MonoBehaviour
{
    // Vitesse de déplacement du missile
    public float speed = 8f;

    // Référence vers le joueur à suivre
    public GameObject playerObject;

    [Header("Tracking")]
    // Vitesse à laquelle le missile tourne vers le joueur
    public float rotationSpeed = 2f;

    // Durée pendant laquelle le missile suit le joueur
    public float trackingDuration = 1.5f;

    // Petite pause avant que le missile parte tout droit
    public float pauseDuration = 0.5f;

    // Transform du joueur, récupéré depuis playerObject
    private Transform player;

    // Direction actuelle du missile
    private Vector2 direction;

    private enum MissileState
    {
        Tracking, // Le missile suit le joueur
        Pause,   // Le missile s'arrête temporairement de suivre
        Straight // Le missile part tout droit
    }

    // État actuel du missile
    private MissileState state = MissileState.Tracking;

    private void Start()
    {
        // Si le joueur est assigné, on récupère son Transform
        if (playerObject != null)
            player = playerObject.transform;

        // Lance la routine qui change les états du missile
        StartCoroutine(MissileRoutine());

        // Sécurité : détruit le missile après 8 secondes
        Destroy(gameObject, 8f);
    }

    private void Update()
    {
        // Pendant la phase de suivi, le missile ajuste sa direction puis avance
        if (state == MissileState.Tracking)
        {
            TrackPlayer();
            Move();
        }
        // Pendant la phase finale, le missile avance simplement tout droit
        else if (state == MissileState.Straight)
        {
            Move();
        }
    }

    private IEnumerator MissileRoutine()
    {
        // Première phase : le missile suit le joueur
        state = MissileState.Tracking;

        yield return new WaitForSeconds(trackingDuration);

        // Deuxième phase : pause courte
        state = MissileState.Pause;

        yield return new WaitForSeconds(pauseDuration);

        // Troisième phase : le missile part tout droit plus rapidement
        state = MissileState.Straight;
        speed *= 2f;
    }

    private void Move()
    {
        // Déplace le missile dans sa direction actuelle
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        // Définit la direction de départ du missile
        direction = newDirection.normalized;

        // Oriente visuellement le missile dans sa direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void TrackPlayer()
    {
        // Si le joueur n'existe pas, le missile garde sa direction actuelle
        if (player == null)
            return;

        // Calcule la direction entre le missile et le joueur
        Vector2 targetDirection =
            ((Vector2)player.position - (Vector2)transform.position).normalized;

        // Limite la rotation pour que le missile ne tourne pas instantanément
        float maxRadiansDelta =
            rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;

        // Fait tourner progressivement la direction vers le joueur
        direction = Vector3.RotateTowards(
            direction,
            targetDirection,
            maxRadiansDelta,
            0f
        ).normalized;

        // Met à jour la rotation visuelle du missile
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }
}