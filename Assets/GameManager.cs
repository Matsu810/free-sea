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
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
    // �K�v�ɉ����đ��̃p���[�A�b�v���ǉ�
}