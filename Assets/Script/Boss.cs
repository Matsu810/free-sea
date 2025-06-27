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

    public Transform firePoint; // 弾を出す位置
    public float bulletSpeed = 20f;
    public Transform playerTransform; // プレイヤーのTransformをInspectorで設定
    private Transform player;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        currentHP = maxHP;

        // Renderer取得（子オブジェクト含む）
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer not found on Boss or its children!");
        }
        // プレイヤーを取得
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()   
    {
        // 発射間隔制御
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
        // 弾を生成
        GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);

        // プレイヤーの現在地から方向を計算
        Vector3 direction = (playerTransform.position - firePoint.position).normalized; 

        // 弾の回転（Z方向を前に向ける）
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        // Rigidbody で弾を飛ばす
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
                TakeDamage(2); // 万が一Bulletスクリプトが無い場合の保険
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
