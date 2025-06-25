using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EnemySpawner enemySpawner;
    [Header("Boss Spawn Settings")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public float bossSpawnDelay = 30f; // 30秒でボス出現
    private bool bossSpawned = false;

    [Header("Field & Gem Move Settings")]
    public float fieldMoveSpeed = 1f;

    [Header("Gem Drop Settings")]
    public GameObject gemPrefab;
    public float gemDropChance = 0.2f;

    [Header("Game Phase")]
    public float phase1Duration = 30f; // 0-30秒がフェーズ1
    public float gameDuration = 60f;   // 1ゲームの長さ(例)
    private float elapsedTime = 0f;

    private enum GamePhase
    {
        Phase1, // 0-30秒 ランダム敵スポーン＆ボス出現
        Phase2, // 30秒以降
        Ended
    }
    private GamePhase currentPhase = GamePhase.Phase1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void OnBossDefeated()
    {
        // 1. 全てのEnemyを破壊
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // 2. EnemySpawnerの生成を止める
        if (enemySpawner != null)
        {
            enemySpawner.canSpawn = false;
        }
        // 3. 10秒後にClearSceneに遷移
        StartCoroutine(LoadClearSceneAfterDelay(10f));
    }
    private IEnumerator LoadClearSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ClearScene");
    }
    private void Start()
    {
        // 敵ランダムスポーンは既存実装を活用
        // 例：StartCoroutine(RandomEnemySpawnRoutine());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        MoveFields();
        MoveGems();

        PhaseControl();
    }

    private void PhaseControl()
    {
        if (currentPhase == GamePhase.Phase1 && elapsedTime >= phase1Duration)
        {
            currentPhase = GamePhase.Phase2;
            // 必要なら敵スポーン終了など
        }

        if (!bossSpawned && elapsedTime >= bossSpawnDelay)
        {
            SpawnBoss();
        }

        if (elapsedTime >= gameDuration && currentPhase != GamePhase.Ended)
        {
            currentPhase = GamePhase.Ended;
            EndGame();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossSpawned = true;
            Debug.Log("Boss Spawned!");
        }
        else
        {
            Debug.LogWarning("BossPrefab または BossSpawnPoint が設定されていません。");
        }
    }

    public void OpenPowerUpSelect()
    {
        Debug.Log("パワーアップ選択画面を表示します");
    }

    public void ApplyPowerUp(PowerUpType type, Player player)
    {
        switch (type)
        {
            case PowerUpType.IncreaseMaxHP:
                player.maxHP += 5;
                break;
            case PowerUpType.IncreaseSpeed:
                // プレイヤーのスピードを上げる
                break;
        }
    }

    void MoveFields()
    {
        GameObject[] fields = GameObject.FindGameObjectsWithTag("Field");
        foreach (GameObject field in fields)
        {
            field.transform.Translate(0, 0, -fieldMoveSpeed * Time.deltaTime, Space.World);
        }
    }

    void MoveGems()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
        foreach (GameObject gem in gems)
        {
            gem.transform.Translate(0, 0, -fieldMoveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void TryDropGem(Vector3 position)
    {
        if (gemPrefab != null && Random.value < gemDropChance)
        {
            Instantiate(gemPrefab, position, Quaternion.identity);
        }
    }

    private void EndGame()
    {
        Debug.Log("ゲーム終了！");
        // ゲーム終了処理
    }
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
}

//bossの死亡を確認したらClearSceneに遷移するようにする
