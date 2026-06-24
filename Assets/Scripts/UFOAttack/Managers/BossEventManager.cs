using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BossEventManager : MonoBehaviour
{
    [Header("Timing")]
    // Temps avant la première apparition du boss
    public float firstBossEventDelay = 60f;

    // Temps d'attente entre deux événements de boss
    public float delayBetweenBossEvents = 60f;

    // Temps laissé avant l'apparition du boss, après l'évacuation des ennemis
    public float warningDuration = 3f;

    [Header("References")]
    // Spawner principal des ennemis classiques
    public EnemySpawner enemySpawner;

    // Préfab du boss événementiel
    public GameObject eventBossPrefab;

    // Position où le boss apparaît
    public Transform eventBossSpawnPoint;

    // Référence vers le joueur, transmise au boss pour qu'il puisse viser
    public GameObject playerObject;

    [Header("Boss Health Bar")]
    // Barre de vie affichée pendant le combat contre le boss
    [SerializeField] private BossHealthBarUI bossHealthBarUI;

    // Nom affiché sur la barre de vie du boss
    [SerializeField] private string bossDisplayName = "BIGBOSS";

    [Header("Boss Music")]
    // Musique jouée pendant le combat contre le boss
    [SerializeField] private AudioClip bossMusic;

    // Durée du fondu vers la musique du boss
    [SerializeField] private float bossMusicFadeInDuration = 2f;

    // Durée du fondu pour revenir à la musique normale
    [SerializeField] private float bossMusicFadeOutDuration = 2f;

    // Indique si un événement de boss est déjà en cours
    private bool bossEventActive = false;

    private void Start()
    {
        // Lance la boucle qui fera apparaître les boss à intervalles réguliers
        StartCoroutine(BossEventLoop());
    }

    private IEnumerator BossEventLoop()
    {
        // Attend avant le premier événement de boss
        yield return new WaitForSeconds(firstBossEventDelay);

        while (true)
        {
            // Lance un combat de boss
            yield return StartCoroutine(StartBossEvent());

            // Attend avant de relancer un nouvel événement
            yield return new WaitForSeconds(delayBetweenBossEvents);
        }
    }

    private IEnumerator StartBossEvent()
    {
        // Sécurité pour éviter de lancer deux événements en même temps
        if (bossEventActive)
            yield break;

        bossEventActive = true;

        // Lance la musique du boss avec une transition progressive
        AudioManager.Instance.PlayBossMusicWithFade(bossMusic, bossMusicFadeInDuration);

        // Arrête temporairement le spawn des ennemis classiques
        if (enemySpawner != null)
        {
            enemySpawner.SetSpawning(false);
        }

        // Fait quitter les ennemis déjà présents dans la scène
        EvacuateCurrentEnemies();

        // Petite attente avant l'apparition du boss
        yield return new WaitForSeconds(warningDuration);

        // Création du boss à son point d'apparition
        GameObject boss = Instantiate(eventBossPrefab, eventBossSpawnPoint.position, Quaternion.identity);

        // Récupère la vie du boss pour l'associer à la barre de vie
        EnemyHealth bossHealth = boss.GetComponent<EnemyHealth>();

        if (bossHealth != null && bossHealthBarUI != null)
        {
            bossHealth.SetBossHealthBar(bossHealthBarUI, bossDisplayName);
        }

        // Transmet la référence du joueur au script du boss
        EventBossController bossController = boss.GetComponent<EventBossController>();
        if (bossController != null)
        {
            bossController.playerObject = playerObject;
        }

        // Tant que le boss existe, l'événement reste actif
        while (boss != null)
        {
            yield return null;
        }

        // Quand le boss est détruit, on relance le spawn des ennemis classiques
        if (enemySpawner != null)
        {
            enemySpawner.SetSpawning(true);
        }

        bossEventActive = false;

        // Retour à la musique normale avec un fondu
        AudioManager.Instance.ReturnToNormalMusicWithFade(bossMusicFadeOutDuration);
    }

    private void EvacuateCurrentEnemies()
    {
        // Copie la liste des ennemis actifs pour éviter les problèmes si elle change pendant la boucle
        List<EnemyHealth> enemiesToEvacuate = new List<EnemyHealth>(EnemyHealth.ActiveEnemies);

        foreach (EnemyHealth enemyHealth in enemiesToEvacuate)
        {
            // Ignore les ennemis déjà détruits
            if (enemyHealth == null)
                continue;

            // Certains ennemis peuvent être exclus de l'évacuation
            if (!enemyHealth.canBeEvacuatedByBossEvent)
                continue;

            // Récupère le script d'évacuation de l'ennemi
            EnemyEvacuation evacuation = enemyHealth.GetComponent<EnemyEvacuation>();

            // Si l'ennemi n'en a pas, on l'ajoute automatiquement
            if (evacuation == null)
            {
                evacuation = enemyHealth.gameObject.AddComponent<EnemyEvacuation>();
            }

            // Lance l'évacuation de cet ennemi
            evacuation.StartEvacuation();
        }
    }
}