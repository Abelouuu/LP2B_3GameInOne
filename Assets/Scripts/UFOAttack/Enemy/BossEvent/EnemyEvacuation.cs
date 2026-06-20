using System.Collections;
using UnityEngine;

public class EnemyEvacuation : MonoBehaviour
{
    public float evacuationSpeed = 8f;
    public float destroyDelay = 2.5f;

    private bool isEvacuating = false;
    private Vector3 evacuationDirection;

    public void StartEvacuation()
    {
        if (isEvacuating)
            return;

        isEvacuating = true;

        StopEnemyBehaviours();
        DisableColliders();

        if (transform.position.y >= 0f)
        {
            evacuationDirection = new Vector3(1f, 1f, 0f).normalized;
        }
        else
        {
            evacuationDirection = new Vector3(1f, -1f, 0f).normalized;
        }

        StartCoroutine(Evacuate());
    }

    private void StopEnemyBehaviours()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this && script.GetType() != typeof(EnemyHealth))
            {
                script.StopAllCoroutines();
                script.enabled = false;
            }
        }
    }

    private void DisableColliders()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
    }

    private IEnumerator Evacuate()
    {
        float timer = 0f;

        while (timer < destroyDelay)
        {
            transform.position += evacuationDirection * evacuationSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}