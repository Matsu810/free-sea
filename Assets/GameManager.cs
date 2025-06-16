using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Boss Spawn Settings")]
    public GameObject bossPrefab;          // ボスのプレハブ
    public Transform bossSpawnPoint;       // ボスの出現位置
    public float bossSpawnDelay = 5f;      // ボス出現までの時間（秒）
    private bool bossSpawned = false;

    [Header("Field & Gem Move Settings")]
    public float fieldMoveSpeed = 1f;      // Field/Gemの移動速度

    [Header("Gem Drop Settings")]
    public GameObject gemPrefab;           // Gemプレハブ
    public float gemDropChance = 0.2f;     // 5分の1=0.2

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(BossSpawnRoutine());
    }

    private void Update()
    {
        MoveFields();
        MoveGems();
    }

    IEnumerator BossSpawnRoutine()
    {
        yield return new WaitForSeconds(bossSpawnDelay);

        if (!bossSpawned)
        {
            SpawnBoss();
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
        // ここでUIを開き、複数のパワーアップから選択させる
        // 例: PowerUpUI.Instance.Open();
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
                // 他にも追加可能
        }
    }

    /// <summary>
    /// Fieldタグを持つオブジェクトをゆっくり-Z方向に動かす
    /// </summary>
    void MoveFields()
    {
        GameObject[] fields = GameObject.FindGameObjectsWithTag("Field");
        foreach (GameObject field in fields)
        {
            field.transform.Translate(0, 0, -fieldMoveSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Gemタグを持つオブジェクトをゆっくり-Z方向に動かす
    /// </summary>
    void MoveGems()
    {
        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
        foreach (GameObject gem in gems)
        {
            gem.transform.Translate(0, 0, -fieldMoveSpeed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Enemyが倒された時に呼ぶ。5分の1でGemをドロップする
    /// </summary>
    /// <param name="position">Enemyの位置</param>
    public void TryDropGem(Vector3 position)
    {
        if (gemPrefab != null && Random.value < gemDropChance)
        {
            Instantiate(gemPrefab, position, Quaternion.identity);
        }
    }
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
    // 必要に応じて他のパワーアップも追加
}