using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 20;
    private int currentHP;
    public bool isDead = false; // �v���C���[������ł��邩�ǂ���

    private Renderer rend;
    private Color originalColor;
    private PlayerControllerInput movementScript; // �� �����͂��Ȃ��̈ړ��X�N���v�g��
    public int maxExp = 10;
    private int currentExp = 0;

    // �o���l�����Z���郁�\�b�h
    public void AddExp(int amount)
    {
        if (isDead) return;
        currentExp += amount;
        Debug.Log("EXP: " + currentExp + "/" + maxExp);
        if (currentExp >= maxExp)
        {
            currentExp = 0;
            // �p���[�A�b�v�I����ʂ�\��
            GameManager.Instance.OpenPowerUpSelect();
        }
    }
    void Start()
    {
        currentHP = maxHP;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        movementScript = GetComponent<PlayerControllerInput>(); // �� �������X�N���v�g���ɍ��킹�āI
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // ���S��̓_���[�W����

        currentHP -= damage;
        Debug.Log("Player HP: " + currentHP);

        StartCoroutine(FlashRed());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        isDead = true;

        // �v���C���[�̑���𖳌���
        if (movementScript != null)
            movementScript.enabled = false;

        rend.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossBullet"))
        {
            TakeDamage(2);
            Destroy(other.gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}