using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EnemySpawner enemySpawner;

    [Header("Boss Prefabs")]
    public GameObject bossPrefab;         // 中ボス
    public GameObject lastBossPrefab;     // ラストボス
    public Transform bossSpawnPoint;

    [Header("SE & UI")]
    public AudioClip warningSE;
    public TextMeshProUGUI warningText; // ←型をTextMeshProUGUIに変更
    public float warningFlashInterval = 0.3f; // 点滅速度
    public int warningFlashCount = 6;         // 何回点滅するか

    [Header("Field & Gem Move Settings")]
    public float fieldMoveSpeed = 1f;

    [Header("Gem Drop Settings")]
    public GameObject gemPrefab;
    public float gemDropChance = 0.2f;


    private enum BossPhase { None, Boss, LastBoss, Cleared }
    private BossPhase bossPhase = BossPhase.None;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SpawnBoss();
    }
    private void Update()
    {
        MoveFields();
        MoveGems();

        // 必要に応じて他のフェーズ制御...
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

    // 中ボス出現
    void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossPhase = BossPhase.Boss;
        }
    }
    public void TryDropGem(Vector3 position)
    {
        if (gemPrefab != null && Random.value < gemDropChance)
        {
            Instantiate(gemPrefab, position, Quaternion.identity);
        }
    }
    // ラストボス出現
    void SpawnLastBoss()
    {
        if (lastBossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(lastBossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossPhase = BossPhase.LastBoss;
        }
    }

    // 中ボス撃破時に呼ばれる
    public void OnBossDefeated()
    {
        if (bossPhase != BossPhase.Boss) return;

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
        // 3. SE再生＆WARNING表示→3秒後にラストボス出現
        StartCoroutine(WarnAndSpawnLastBoss());
    }

    // ラストボス撃破時に呼ばれる
    public void OnLastBossDefeated()
    {
        if (bossPhase == BossPhase.LastBoss)
        {
            bossPhase = BossPhase.Cleared;
            StartCoroutine(LoadClearSceneAfterDelay(1.5f)); // 1.5秒後に画面遷移
        }
    }

    private IEnumerator WarnAndSpawnLastBoss()
    {
        // SE 再生
        if (warningSE != null)
        {
            AudioSource.PlayClipAtPoint(warningSE, Camera.main.transform.position);
        }
        // WARNING!!点滅
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true); // 表示
            for (int i = 0; i < warningFlashCount; i++)
            {
                warningText.alpha = (warningText.alpha == 1f) ? 0f : 1f; // 点滅
                yield return new WaitForSeconds(warningFlashInterval);
            }
            warningText.alpha = 1f; // 最後は表示
        }
        yield return new WaitForSeconds(3f - warningFlashInterval * warningFlashCount);
        if (warningText != null)
            warningText.gameObject.SetActive(false); // 非表示

        SpawnLastBoss();
    }

    private IEnumerator LoadClearSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ClearScene");
    }


}
//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;
//    public EnemySpawner enemySpawner;

//    [Header("Boss Prefabs")]
//    public GameObject bossPrefab;         // 既存：中ボス
//    public GameObject lastBossPrefab;     // 追加：ラストボス
//    public Transform bossSpawnPoint;

//    [Header("SE & UI")]
//    public AudioClip warningSE;
//    public Text warningText; // Canvas上のText。Inspectorで割り当て
//    public float warningFlashInterval = 0.3f; // 点滅速度
//    public int warningFlashCount = 6;         // 何回点滅するか

//    [Header("Field & Gem Move Settings")]
//    public float fieldMoveSpeed = 1f;

//    [Header("Gem Drop Settings")]
//    public GameObject gemPrefab;
//    public float gemDropChance = 0.2f;

//    [Header("Game Phase")]
//    public float phase1Duration = 30f; // 0-30秒がフェーズ1
//    public float gameDuration = 60f;   // 1ゲームの長さ(例)
//    private float elapsedTime = 0f;

//    private enum BossPhase { None, Boss, LastBoss, Cleared }
//    private BossPhase bossPhase = BossPhase.None;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    private void Start()
//    {
//        SpawnBoss();
//    }

//    private void Update()
//    {
//        elapsedTime += Time.deltaTime;

//        MoveFields();
//        MoveGems();

//        // 必要に応じて他のフェーズ制御...
//    }

//    // 中ボス出現
//    void SpawnBoss()
//    {
//        if (bossPrefab != null && bossSpawnPoint != null)
//        {
//            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
//            bossPhase = BossPhase.Boss;
//        }
//    }

//    // ラストボス出現
//    void SpawnLastBoss()
//    {
//        if (lastBossPrefab != null && bossSpawnPoint != null)
//        {
//            Instantiate(lastBossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
//            bossPhase = BossPhase.LastBoss;
//        }
//    }

//    // 中ボス撃破時に呼ばれる
//    public void OnBossDefeated()
//    {
//        if (bossPhase != BossPhase.Boss) return;

//        // 1. 全てのEnemyを破壊
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
//        foreach (GameObject enemy in enemies)
//        {
//            Destroy(enemy);
//        }
//        // 2. EnemySpawnerの生成を止める
//        if (enemySpawner != null)
//        {
//            enemySpawner.canSpawn = false;
//        }
//        // 3. SE再生＆WARNING表示→3秒後にラストボス出現
//        StartCoroutine(WarnAndSpawnLastBoss());
//    }

//    // ラストボス撃破時に呼ばれる
//    public void OnLastBossDefeated()
//    {
//        if (bossPhase == BossPhase.LastBoss)
//        {
//            bossPhase = BossPhase.Cleared;
//            StartCoroutine(LoadClearSceneAfterDelay(1.5f)); // 1.5秒後に画面遷移
//        }
//    }

//    private IEnumerator WarnAndSpawnLastBoss()
//    {
//        // SE 再生
//        if (warningSE != null)
//        {
//            AudioSource.PlayClipAtPoint(warningSE, Camera.main.transform.position);
//        }
//        // WARNING!!点滅
//        if (warningText != null)
//        {
//            warningText.gameObject.SetActive(true);
//            for (int i = 0; i < warningFlashCount; i++)
//            {
//                warningText.enabled = !warningText.enabled;
//                yield return new WaitForSeconds(warningFlashInterval);
//            }
//            warningText.enabled = true;
//        }
//        yield return new WaitForSeconds(3f - warningFlashInterval * warningFlashCount);
//        if (warningText != null)
//            warningText.gameObject.SetActive(false);

//        SpawnLastBoss();
//    }

//    private IEnumerator LoadClearSceneAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        SceneManager.LoadScene("ClearScene");
//    }

//    // --- 既存機能はそのまま残す ---

//    public void OpenPowerUpSelect()
//    {
//        Debug.Log("パワーアップ選択画面を表示します");
//    }

//    public void ApplyPowerUp(PowerUpType type, Player player)
//    {
//        switch (type)
//        {
//            case PowerUpType.IncreaseMaxHP:
//                player.maxHP += 5;
//                break;
//            case PowerUpType.IncreaseSpeed:
//                // プレイヤーのスピードを上げる
//                break;
//        }
//    }

//    void MoveFields()
//    {
//        GameObject[] fields = GameObject.FindGameObjectsWithTag("Field");
//        foreach (GameObject field in fields)
//        {
//            field.transform.Translate(0, 0, -fieldMoveSpeed * Time.deltaTime, Space.World);
//        }
//    }

//    void MoveGems()
//    {
//        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
//        foreach (GameObject gem in gems)
//        {
//            gem.transform.Translate(0, 0, -fieldMoveSpeed * Time.deltaTime, Space.World);
//        }
//    }

//    public void TryDropGem(Vector3 position)
//    {
//        if (gemPrefab != null && Random.value < gemDropChance)
//        {
//            Instantiate(gemPrefab, position, Quaternion.identity);
//        }
//    }
//}

//public enum PowerUpType
//{
//    IncreaseMaxHP,
//    IncreaseSpeed,
//}