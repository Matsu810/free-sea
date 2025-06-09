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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(BossSpawnRoutine());
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
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
    // 必要に応じて他のパワーアップも追加
}