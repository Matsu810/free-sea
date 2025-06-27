using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHP = 300;
    private int currentHP;

    public GameObject bossBulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireInterval = 1f;
    private float fireTimer = 0f;

    public Transform firePoint; // �e���o���ʒu
    public float bulletSpeed = 20f;
    public Transform playerTransform; // �v���C���[��Transform��Inspector�Őݒ�
    private Transform player;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        currentHP = maxHP;

        // Renderer�擾�i�q�I�u�W�F�N�g�܂ށj
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer not found on Boss or its children!");
        }
        // �v���C���[���擾
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()   
    {
        // ���ˊԊu����
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }
        Vector3 direction = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Fire()
    {
        // �e�𐶐�
        GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);

        // �v���C���[�̌��ݒn����������v�Z
        Vector3 direction = (playerTransform.position - firePoint.position).normalized; 

        // �e�̉�]�iZ������O�Ɍ�����j
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        // Rigidbody �Œe���΂�
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Boss HP: " + currentHP);

        StartCoroutine(FlashRed());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        GameManager.Instance.OnBossDefeated();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(1);

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
        else if (other.CompareTag("PlayerBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            else
            {
                TakeDamage(2); // ������Bullet�X�N���v�g�������ꍇ�̕ی�
            }
            Destroy(other.gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        if (rend != null)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = originalColor;
        }
    }
}
