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
    public GameObject bossPrefab;         // ���{�X
    public GameObject lastBossPrefab;     // ���X�g�{�X
    public Transform bossSpawnPoint;

    [Header("SE & UI")]
    public AudioClip warningSE;
    public TextMeshProUGUI warningText; // ���^��TextMeshProUGUI�ɕύX
    public float warningFlashInterval = 0.3f; // �_�ő��x
    public int warningFlashCount = 6;         // ����_�ł��邩

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

        // �K�v�ɉ����đ��̃t�F�[�Y����...
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

    // ���{�X�o��
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
    // ���X�g�{�X�o��
    void SpawnLastBoss()
    {
        if (lastBossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(lastBossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossPhase = BossPhase.LastBoss;
        }
    }

    // ���{�X���j���ɌĂ΂��
    public void OnBossDefeated()
    {
        if (bossPhase != BossPhase.Boss) return;

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
        // 3. SE�Đ���WARNING�\����3�b��Ƀ��X�g�{�X�o��
        StartCoroutine(WarnAndSpawnLastBoss());
    }

    // ���X�g�{�X���j���ɌĂ΂��
    public void OnLastBossDefeated()
    {
        if (bossPhase == BossPhase.LastBoss)
        {
            bossPhase = BossPhase.Cleared;
            StartCoroutine(LoadClearSceneAfterDelay(1.5f)); // 1.5�b��ɉ�ʑJ��
        }
    }

    private IEnumerator WarnAndSpawnLastBoss()
    {
        // SE �Đ�
        if (warningSE != null)
        {
            AudioSource.PlayClipAtPoint(warningSE, Camera.main.transform.position);
        }
        // WARNING!!�_��
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true); // �\��
            for (int i = 0; i < warningFlashCount; i++)
            {
                warningText.alpha = (warningText.alpha == 1f) ? 0f : 1f; // �_��
                yield return new WaitForSeconds(warningFlashInterval);
            }
            warningText.alpha = 1f; // �Ō�͕\��
        }
        yield return new WaitForSeconds(3f - warningFlashInterval * warningFlashCount);
        if (warningText != null)
            warningText.gameObject.SetActive(false); // ��\��

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
//    public GameObject bossPrefab;         // �����F���{�X
//    public GameObject lastBossPrefab;     // �ǉ��F���X�g�{�X
//    public Transform bossSpawnPoint;

//    [Header("SE & UI")]
//    public AudioClip warningSE;
//    public Text warningText; // Canvas���Text�BInspector�Ŋ��蓖��
//    public float warningFlashInterval = 0.3f; // �_�ő��x
//    public int warningFlashCount = 6;         // ����_�ł��邩

//    [Header("Field & Gem Move Settings")]
//    public float fieldMoveSpeed = 1f;

//    [Header("Gem Drop Settings")]
//    public GameObject gemPrefab;
//    public float gemDropChance = 0.2f;

//    [Header("Game Phase")]
//    public float phase1Duration = 30f; // 0-30�b���t�F�[�Y1
//    public float gameDuration = 60f;   // 1�Q�[���̒���(��)
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

//        // �K�v�ɉ����đ��̃t�F�[�Y����...
//    }

//    // ���{�X�o��
//    void SpawnBoss()
//    {
//        if (bossPrefab != null && bossSpawnPoint != null)
//        {
//            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
//            bossPhase = BossPhase.Boss;
//        }
//    }

//    // ���X�g�{�X�o��
//    void SpawnLastBoss()
//    {
//        if (lastBossPrefab != null && bossSpawnPoint != null)
//        {
//            Instantiate(lastBossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
//            bossPhase = BossPhase.LastBoss;
//        }
//    }

//    // ���{�X���j���ɌĂ΂��
//    public void OnBossDefeated()
//    {
//        if (bossPhase != BossPhase.Boss) return;

//        // 1. �S�Ă�Enemy��j��
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
//        foreach (GameObject enemy in enemies)
//        {
//            Destroy(enemy);
//        }
//        // 2. EnemySpawner�̐������~�߂�
//        if (enemySpawner != null)
//        {
//            enemySpawner.canSpawn = false;
//        }
//        // 3. SE�Đ���WARNING�\����3�b��Ƀ��X�g�{�X�o��
//        StartCoroutine(WarnAndSpawnLastBoss());
//    }

//    // ���X�g�{�X���j���ɌĂ΂��
//    public void OnLastBossDefeated()
//    {
//        if (bossPhase == BossPhase.LastBoss)
//        {
//            bossPhase = BossPhase.Cleared;
//            StartCoroutine(LoadClearSceneAfterDelay(1.5f)); // 1.5�b��ɉ�ʑJ��
//        }
//    }

//    private IEnumerator WarnAndSpawnLastBoss()
//    {
//        // SE �Đ�
//        if (warningSE != null)
//        {
//            AudioSource.PlayClipAtPoint(warningSE, Camera.main.transform.position);
//        }
//        // WARNING!!�_��
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

//    // --- �����@�\�͂��̂܂܎c�� ---

//    public void OpenPowerUpSelect()
//    {
//        Debug.Log("�p���[�A�b�v�I����ʂ�\�����܂�");
//    }

//    public void ApplyPowerUp(PowerUpType type, Player player)
//    {
//        switch (type)
//        {
//            case PowerUpType.IncreaseMaxHP:
//                player.maxHP += 5;
//                break;
//            case PowerUpType.IncreaseSpeed:
//                // �v���C���[�̃X�s�[�h���グ��
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