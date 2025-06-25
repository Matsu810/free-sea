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
    public float bossSpawnDelay = 30f; // 30�b�Ń{�X�o��
    private bool bossSpawned = false;

    [Header("Field & Gem Move Settings")]
    public float fieldMoveSpeed = 1f;

    [Header("Gem Drop Settings")]
    public GameObject gemPrefab;
    public float gemDropChance = 0.2f;

    [Header("Game Phase")]
    public float phase1Duration = 30f; // 0-30�b���t�F�[�Y1
    public float gameDuration = 60f;   // 1�Q�[���̒���(��)
    private float elapsedTime = 0f;

    private enum GamePhase
    {
        Phase1, // 0-30�b �����_���G�X�|�[�����{�X�o��
        Phase2, // 30�b�ȍ~
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
        // 1. �S�Ă�Enemy��j��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // 2. EnemySpawner�̐������~�߂�
        if (enemySpawner != null)
        {
            enemySpawner.canSpawn = false;
        }
        // 3. 10�b���ClearScene�ɑJ��
        StartCoroutine(LoadClearSceneAfterDelay(10f));
    }
    private IEnumerator LoadClearSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ClearScene");
    }
    private void Start()
    {
        // �G�����_���X�|�[���͊������������p
        // ��FStartCoroutine(RandomEnemySpawnRoutine());
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
            // �K�v�Ȃ�G�X�|�[���I���Ȃ�
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
            Debug.LogWarning("BossPrefab �܂��� BossSpawnPoint ���ݒ肳��Ă��܂���B");
        }
    }

    public void OpenPowerUpSelect()
    {
        Debug.Log("�p���[�A�b�v�I����ʂ�\�����܂�");
    }

    public void ApplyPowerUp(PowerUpType type, Player player)
    {
        switch (type)
        {
            case PowerUpType.IncreaseMaxHP:
                player.maxHP += 5;
                break;
            case PowerUpType.IncreaseSpeed:
                // �v���C���[�̃X�s�[�h���グ��
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
        Debug.Log("�Q�[���I���I");
        // �Q�[���I������
    }
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
}

//boss�̎��S���m�F������ClearScene�ɑJ�ڂ���悤�ɂ���
