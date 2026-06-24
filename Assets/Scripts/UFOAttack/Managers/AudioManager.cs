using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Instance unique de l'AudioManager, accessible depuis les autres scripts
    public static AudioManager Instance;

    [Header("Audio Sources")]
    // Source utilisée pour la musique principale
    [SerializeField] private AudioSource musicSource;

    // Deuxième source utilisée pour faire des transitions entre deux musiques
    [SerializeField] private AudioSource secondaryMusicSource;

    // Source utilisée pour les effets sonores
    [SerializeField] private AudioSource sfxSource;

    // Source utilisée pour les sons ou musiques de game over
    [SerializeField] private AudioSource gameOverSource;

    // Source qui joue actuellement la musique
    private AudioSource currentMusicSource;

    // Coroutine utilisée pour gérer le fade entre deux musiques
    private Coroutine musicFadeCoroutine;

    // Musique normale du jeu, gardée en mémoire pour pouvoir y revenir après le boss
    private AudioClip normalMusicClip;

    // Temps auquel la musique normale s'est arrêtée
    private float normalMusicTime = 0f;

    // Volumes de base des différentes sources audio
    private float baseMusicVolume;
    private float baseSFXVolume;
    private float baseGameOverVolume;

    private void Awake()
    {
        // On initialise l'instance unique
        Instance = this;

        // Au départ, la musique courante est celle de la source principale
        currentMusicSource = musicSource;

        // On sauvegarde les volumes de départ configurés dans l'inspecteur
        if (musicSource != null)
            baseMusicVolume = musicSource.volume;

        if (sfxSource != null)
            baseSFXVolume = sfxSource.volume;

        if (gameOverSource != null)
            baseGameOverVolume = gameOverSource.volume;

        // La deuxième source musicale est cachée au début
        if (secondaryMusicSource != null)
        {
            secondaryMusicSource.volume = 0f;
            secondaryMusicSource.Stop();
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        // Vérifie que la source et le clip existent
        if (musicSource == null || clip == null)
            return;

        // On garde cette musique comme musique normale du jeu
        normalMusicClip = clip;
        normalMusicTime = 0f;

        // Si une transition musicale est en cours, on l'arrête
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = null;
        }

        // On arrête la source secondaire si elle était utilisée
        if (secondaryMusicSource != null)
        {
            secondaryMusicSource.Stop();
            secondaryMusicSource.volume = 0f;
        }

        // On configure puis on lance la musique principale
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

        // Si la musique normale est en cours, on sauvegarde sa position
        if (currentMusicSource != null && currentMusicSource.clip == normalMusicClip)
        {
            normalMusicTime = currentMusicSource.time;
        }

        // Lance une transition vers la musique du boss
        StartMusicCrossfade(bossMusicClip, fadeDuration, true, 0f);
    }

    public void ReturnToNormalMusicWithFade(float fadeDuration)
    {
        if (normalMusicClip == null)
            return;

        // Revient à la musique normale à l'endroit où elle s'était arrêtée
        StartMusicCrossfade(normalMusicClip, fadeDuration, true, normalMusicTime);
    }

    private void StartMusicCrossfade(AudioClip newClip, float fadeDuration, bool loop, float startTime)
    {
        // Évite d'avoir deux transitions audio en même temps
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
        // Vérifie que les sources nécessaires existent
        if (musicSource == null || secondaryMusicSource == null || newClip == null)
            yield break;

        // On choisit l'ancienne source et la nouvelle source à utiliser
        AudioSource oldSource = currentMusicSource;
        AudioSource newSource = currentMusicSource == musicSource ? secondaryMusicSource : musicSource;

        // Préparation de la nouvelle musique
        newSource.clip = newClip;
        newSource.loop = loop;
        newSource.volume = 0f;

        // Permet de reprendre une musique à un temps précis
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

        // Si la durée du fade est nulle, on change directement de musique
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

        // Transition progressive : l'ancienne musique baisse et la nouvelle augmente
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

        // Fin de la transition : on arrête complètement l'ancienne source
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

        // On arrête la musique normale avant de lancer celle du game over
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

        // Joue un effet sonore sans couper les autres sons déjà en cours
        sfxSource.volume = baseSFXVolume;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Modifie le volume de base des effets sonores
        baseSFXVolume = Mathf.Clamp01(volume);

        if (sfxSource != null)
        {
            sfxSource.volume = baseSFXVolume;
        }
    }

    public void PlayGameOverSFX(AudioClip clip, float volume = 1f)
    {
        if (gameOverSource == null || clip == null)
            return;

        // Joue un effet sonore sur la source du game over
        gameOverSource.PlayOneShot(clip, volume);
    }

    public void PauseMusic()
    {
        // Met en pause les deux sources musicales au cas où une transition est en cours
        if (musicSource != null)
            musicSource.Pause();

        if (secondaryMusicSource != null)
            secondaryMusicSource.Pause();
    }

    public void ResumeMusic()
    {
        // Reprend uniquement la musique actuellement utilisée
        if (currentMusicSource != null)
            currentMusicSource.UnPause();
    }

    public void StopGameOverAudio()
    {
        // Arrête les sons ou musiques liés au game over
        if (gameOverSource != null)
            gameOverSource.Stop();
    }

    public IEnumerator FadeMusicAndSFX(float duration)
    {
        // Sauvegarde les volumes actuels avant de les diminuer
        float musicStart = musicSource != null ? musicSource.volume : 0f;
        float secondaryMusicStart = secondaryMusicSource != null ? secondaryMusicSource.volume : 0f;
        float sfxStart = sfxSource != null ? sfxSource.volume : 0f;

        float timer = 0f;

        // Baisse progressivement la musique et les effets sonores
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

        // Force les volumes à 0 à la fin du fade
        if (musicSource != null)
            musicSource.volume = 0f;

        if (secondaryMusicSource != null)
            secondaryMusicSource.volume = 0f;

        if (sfxSource != null)
            sfxSource.volume = 0f;
    }

    private void StopCurrentMusic()
    {
        // Arrête une éventuelle transition en cours
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = null;
        }

        // Arrête la source musicale principale
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.volume = 0f;
        }

        // Arrête aussi la source secondaire
        if (secondaryMusicSource != null)
        {
            secondaryMusicSource.Stop();
            secondaryMusicSource.volume = 0f;
        }
    }
}