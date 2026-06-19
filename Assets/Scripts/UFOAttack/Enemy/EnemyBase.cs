using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Audio")]
    [Range(0f, 1f)]
    public float shootVolume = 0.5f;

    protected virtual void Start()
    {
        StartCoroutine(Behavior());
    }

    protected abstract IEnumerator Behavior();

    protected void PlaySound(AudioClip clip)
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlaySFX(clip, shootVolume);
        }
    }

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }
}