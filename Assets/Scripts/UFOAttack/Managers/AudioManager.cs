using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource gameOverSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayGameOverMusic(AudioClip clip, bool loop = true)
    {
        gameOverSource.clip = clip;
        gameOverSource.loop = loop;
        gameOverSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayGameOverSFX(AudioClip clip, float volume = 1f)
    {
        gameOverSource.PlayOneShot(clip, volume);
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void StopGameOverAudio()
    {
        gameOverSource.Stop();
    }

    public IEnumerator FadeMusicAndSFX(float duration)
    {
        float musicStart = musicSource.volume;
        float sfxStart = sfxSource.volume;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            musicSource.volume = Mathf.Lerp(musicStart, 0f, t);
            sfxSource.volume = Mathf.Lerp(sfxStart, 0f, t);

            yield return null;
        }
    }
}