using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource secondaryMusicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource gameOverSource;

    private AudioSource currentMusicSource;
    private Coroutine musicFadeCoroutine;

    private AudioClip normalMusicClip;
    private float normalMusicTime = 0f;

    private float baseMusicVolume;
    private float baseSFXVolume;
    private float baseGameOverVolume;

    private void Awake()
    {
        Instance = this;

        currentMusicSource = musicSource;

        if (musicSource != null)
            baseMusicVolume = musicSource.volume;

        if (sfxSource != null)
            baseSFXVolume = sfxSource.volume;

        if (gameOverSource != null)
            baseGameOverVolume = gameOverSource.volume;

        if (secondaryMusicSource != null)
        {
            secondaryMusicSource.volume = 0f;
            secondaryMusicSource.Stop();
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null)
            return;

        normalMusicClip = clip;
        normalMusicTime = 0f;

        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = null;
        }

        if (secondaryMusicSource != null)
        {
            secondaryMusicSource.Stop();
            secondaryMusicSource.volume = 0f;
        }

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = baseMusicVolume;
        musicSource.time = 0f;
        musicSource.Play();

        currentMusicSource = musicSource;
    }

    public void PlayBossMusicWithFade(AudioClip bossMusicClip, float fadeDuration)
    {
        if (bossMusicClip == null)
            return;

        if (currentMusicSource != null && currentMusicSource.clip == normalMusicClip)
        {
            normalMusicTime = currentMusicSource.time;
        }

        StartMusicCrossfade(bossMusicClip, fadeDuration, true, 0f);
    }

    public void ReturnToNormalMusicWithFade(float fadeDuration)
    {
        if (normalMusicClip == null)
            return;

        StartMusicCrossfade(normalMusicClip, fadeDuration, true, normalMusicTime);
    }

    private void StartMusicCrossfade(AudioClip newClip, float fadeDuration, bool loop, float startTime)
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }

        musicFadeCoroutine = StartCoroutine(
            CrossfadeMusicRoutine(newClip, fadeDuration, loop, startTime)
        );
    }

    private IEnumerator CrossfadeMusicRoutine(AudioClip newClip, float fadeDuration, bool loop, float startTime)
    {
        if (musicSource == null || secondaryMusicSource == null || newClip == null)
            yield break;

        AudioSource oldSource = currentMusicSource;
        AudioSource newSource = currentMusicSource == musicSource ? secondaryMusicSource : musicSource;

        newSource.clip = newClip;
        newSource.loop = loop;
        newSource.volume = 0f;

        if (startTime > 0f && startTime < newClip.length)
        {
            newSource.time = startTime;
        }
        else
        {
            newSource.time = 0f;
        }

        newSource.Play();

        float oldStartVolume = oldSource != null ? oldSource.volume : 0f;
        float timer = 0f;

        if (fadeDuration <= 0f)
        {
            if (oldSource != null)
            {
                oldSource.Stop();
                oldSource.volume = 0f;
            }

            newSource.volume = baseMusicVolume;
            currentMusicSource = newSource;
            yield break;
        }

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeDuration;

            if (oldSource != null)
            {
                oldSource.volume = Mathf.Lerp(oldStartVolume, 0f, t);
            }

            newSource.volume = Mathf.Lerp(0f, baseMusicVolume, t);

            yield return null;
        }

        if (oldSource != null)
        {
            oldSource.Stop();
            oldSource.volume = 0f;
        }

        newSource.volume = baseMusicVolume;
        currentMusicSource = newSource;
        musicFadeCoroutine = null;
    }

    public void PlayGameOverMusic(AudioClip clip, bool loop = true)
    {
        if (gameOverSource == null || clip == null)
            return;

        StopCurrentMusic();

        gameOverSource.clip = clip;
        gameOverSource.loop = loop;
        gameOverSource.volume = baseGameOverVolume;
        gameOverSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.volume = baseSFXVolume;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayGameOverSFX(AudioClip clip, float volume = 1f)
    {
        if (gameOverSource == null || clip == null)
            return;

        gameOverSource.PlayOneShot(clip, volume);
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();

        if (secondaryMusicSource != null)
            secondaryMusicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (currentMusicSource != null)
            currentMusicSource.UnPause();
    }

    public void StopGameOverAudio()
    {
        if (gameOverSource != null)
            gameOverSource.Stop();
    }

    public IEnumerator FadeMusicAndSFX(float duration)
    {
        float musicStart = musicSource != null ? musicSource.volume : 0f;
        float secondaryMusicStart = secondaryMusicSource != null ? secondaryMusicSource.volume : 0f;
        float sfxStart = sfxSource != null ? sfxSource.volume : 0f;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;

            if (musicSource != null)
                musicSource.volume = Mathf.Lerp(musicStart, 0f, t);

            if (secondaryMusicSource != null)
                secondaryMusicSource.volume = Mathf.Lerp(secondaryMusicStart, 0f, t);

            if (sfxSource != null)
                sfxSource.volume = Mathf.Lerp(sfxStart, 0f, t);

            yield return null;
        }

        if (musicSource != null)
            musicSource.volume = 0f;

        if (secondaryMusicSource != null)
            secondaryMusicSource.volume = 0f;

        if (sfxSource != null)
            sfxSource.volume = 0f;
    }

    private void StopCurrentMusic()
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = null;
        }

        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.volume = 0f;
        }

        if (secondaryMusicSource != null)
        {
            secondaryMusicSource.Stop();
            secondaryMusicSource.volume = 0f;
        }
    }
}