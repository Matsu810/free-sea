using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Boss Spawn Settings")]
    public GameObject bossPrefab;          // �{�X�̃v���n�u
    public Transform bossSpawnPoint;       // �{�X�̏o���ʒu
    public float bossSpawnDelay = 5f;      // �{�X�o���܂ł̎��ԁi�b�j
    private bool bossSpawned = false;

    [Header("Field & Gem Move Settings")]
    public float fieldMoveSpeed = 1f;      // Field/Gem�̈ړ����x

    [Header("Gem Drop Settings")]
    public GameObject gemPrefab;           // Gem�v���n�u
    public float gemDropChance = 0.2f;     // 5����1=0.2

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
            Debug.LogWarning("BossPrefab �܂��� BossSpawnPoint ���ݒ肳��Ă��܂���B");
        }
    }

    public void OpenPowerUpSelect()
    {
        // ������UI���J���A�����̃p���[�A�b�v����I��������
        // ��: PowerUpUI.Instance.Open();
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
                // ���ɂ��ǉ��\
        }
    }

    /// <summary>
    /// Field�^�O�����I�u�W�F�N�g���������-Z�����ɓ�����
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
    /// Gem�^�O�����I�u�W�F�N�g���������-Z�����ɓ�����
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
    /// Enemy���|���ꂽ���ɌĂԁB5����1��Gem���h���b�v����
    /// </summary>
    /// <param name="position">Enemy�̈ʒu</param>
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
    // �K�v�ɉ����đ��̃p���[�A�b�v���ǉ�
}