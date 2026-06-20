using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BossEventManager : MonoBehaviour
{
    [Header("Timing")]
    public float firstBossEventDelay = 60f;
    public float delayBetweenBossEvents = 60f;
    public float warningDuration = 3f;

    [Header("References")]
    public EnemySpawner enemySpawner;
    public GameObject eventBossPrefab;
    public Transform eventBossSpawnPoint;
    public GameObject playerObject;

    [Header("Boss Health Bar")]
    [SerializeField] private BossHealthBarUI bossHealthBarUI;
    [SerializeField] private string bossDisplayName = "BIGBOSS";

    [Header("Boss Music")]
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float bossMusicFadeInDuration = 2f;
    [SerializeField] private float bossMusicFadeOutDuration = 2f;

    private bool bossEventActive = false;

    private void Start()
    {
        StartCoroutine(BossEventLoop());
    }

    private IEnumerator BossEventLoop()
    {
        yield return new WaitForSeconds(firstBossEventDelay);

        while (true)
        {
            yield return StartCoroutine(StartBossEvent());

            yield return new WaitForSeconds(delayBetweenBossEvents);
        }
    }

    private IEnumerator StartBossEvent()
    {
        if (bossEventActive)
            yield break;

        bossEventActive = true;
        AudioManager.Instance.PlayBossMusicWithFade(bossMusic, bossMusicFadeInDuration);

        if (enemySpawner != null)
        {
            enemySpawner.SetSpawning(false);
        }

        EvacuateCurrentEnemies();

        yield return new WaitForSeconds(warningDuration);

        GameObject boss = Instantiate(eventBossPrefab, eventBossSpawnPoint.position, Quaternion.identity);

        EnemyHealth bossHealth = boss.GetComponent<EnemyHealth>();

        if (bossHealth != null && bossHealthBarUI != null)
        {
            bossHealth.SetBossHealthBar(bossHealthBarUI, bossDisplayName);
        }

        EventBossController bossController = boss.GetComponent<EventBossController>();
        if (bossController != null)
        {
            bossController.playerObject = playerObject;
        }

        while (boss != null)
        {
            yield return null;
        }

        if (enemySpawner != null)
        {
            enemySpawner.SetSpawning(true);
        }

        bossEventActive = false;
        AudioManager.Instance.ReturnToNormalMusicWithFade(bossMusicFadeOutDuration);
    }

    private void EvacuateCurrentEnemies()
    {
        List<EnemyHealth> enemiesToEvacuate = new List<EnemyHealth>(EnemyHealth.ActiveEnemies);

        foreach (EnemyHealth enemyHealth in enemiesToEvacuate)
        {
            if (enemyHealth == null)
                continue;

            if (!enemyHealth.canBeEvacuatedByBossEvent)
                continue;

            EnemyEvacuation evacuation = enemyHealth.GetComponent<EnemyEvacuation>();

            if (evacuation == null)
            {
                evacuation = enemyHealth.gameObject.AddComponent<EnemyEvacuation>();
            }

            evacuation.StartEvacuation();
        }
    }
}