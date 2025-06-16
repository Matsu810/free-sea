using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 20;
    private int currentHP;
    public bool isDead = false;

    [Header("Bullet Prefabs")]
    public GameObject normalBulletPrefab;
    public GameObject threeWayBulletPrefab;
    public GameObject cannonBulletPrefab;

    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float spreadAngle = 15f;

    private Renderer rend;
    private Color originalColor;
    [HideInInspector]
    public PlayerControllerInput movementScript;

    public int maxExp = 10;
    private int currentExp = 0;

    public enum BulletType
    {
        Normal,
        ThreeWay,
        Cannon
    }
    private BulletType currentBulletType = BulletType.Normal;

    public void AddScore(int amount)
    {
        // スコア加算処理
    }

    public void AddSpeedBuff()
    {
        if (movementScript != null)
        {
            movementScript.moveSpeed += 10f;
        }
    }

    public void EnableThreeWayShot()
    {
        SetBulletType(BulletType.ThreeWay);
    }

    public void EnableNormalShot()
    {
        SetBulletType(BulletType.Normal);
    }

    public void EnableCannonShot()
    {
        SetBulletType(BulletType.Cannon);
    }

    public void SetBulletType(BulletType type)
    {
        currentBulletType = type;
    }

    public void Fire()
    {
        switch (currentBulletType)
        {
            case BulletType.Normal:
                FireNormal();
                break;
            case BulletType.ThreeWay:
                FireThreeWay();
                break;
            case BulletType.Cannon:
                FireCannon();
                break;
        }
    }

    void FireNormal()
    {
        GameObject bullet = Instantiate(normalBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
    }

    void FireThreeWay()
    {
        float[] angles = { 0f, -spreadAngle, spreadAngle };
        foreach (float angle in angles)
        {
            Quaternion rot = bulletSpawnPoint.rotation * Quaternion.Euler(0, angle, 0);
            GameObject bullet = Instantiate(threeWayBulletPrefab, bulletSpawnPoint.position, rot);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = rot * Vector3.forward * bulletSpeed;
        }
    }

    void FireCannon()
    {
        GameObject bullet = Instantiate(cannonBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
    }

    public void AddExp(int amount)
    {
        if (isDead) return;
        currentExp += amount;
        Debug.Log("EXP: " + currentExp + "/" + maxExp);
        if (currentExp >= maxExp)
        {
            currentExp = 0;
            GameManager.Instance.OpenPowerUpSelect();
        }
    }

    void Start()
    {
        currentHP = maxHP;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        movementScript = GetComponent<PlayerControllerInput>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

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

//using System.Collections;
//using UnityEngine;

//public class Player : MonoBehaviour
//{
//    public int maxHP = 20;
//    private int currentHP;
//    public bool isDead = false; // プレイヤーが死んでいるかどうか

//    [Header("Bullet Prefabs")]
//    public GameObject normalBulletPrefab;    // ノーマル弾プレハブ
//    public GameObject threeWayBulletPrefab;  // 3WAY弾プレハブ
//    public GameObject cannonBulletPrefab;    // キャノン弾プレハブ

//    public Transform bulletSpawnPoint;       // 弾の発射位置
//    public float bulletSpeed = 10f;          // 弾の速度
//    public float spreadAngle = 15f;          // 左右の開き角度

//    private Renderer rend;
//    private Color originalColor;
//    private PlayerControllerInput movementScript; // ← ここはあなたの移動スクリプト名

//    public int maxExp = 10;
//    private int currentExp = 0;

//    // 弾タイプ
//    public enum BulletType
//    {
//        Normal,
//        ThreeWay,
//        Cannon
//    }
//    private BulletType currentBulletType = BulletType.Normal;

//    public void AddScore(int amount)
//    {
//        // スコア加算処理
//    }

//    public void AddSpeedBuff()
//    {
//        // スピードアップ処理  
//        if (movementScript != null)
//        {
//            movementScript.moveSpeed += 10f;
//        }
//    }

//    public void EnableThreeWayShot()
//    {
//        SetBulletType(BulletType.ThreeWay);
//        // 必要なら一定時間で切り替え解除する処理も追加可能
//    }

//    public void EnableNormalShot()
//    {
//        SetBulletType(BulletType.Normal);
//    }

//    public void EnableCannonShot()
//    {
//        SetBulletType(BulletType.Cannon);
//        // 必要なら一定時間で切り替え解除する処理も追加可能
//    }

//    public void SetBulletType(BulletType type)
//    {
//        currentBulletType = type;
//    }

//    // 通常の攻撃処理（例：Updateから呼ぶ、またはFireメソッドなど）
//    void Fire()
//    {
//        switch (currentBulletType)
//        {
//            case BulletType.Normal:
//                FireNormal();
//                break;
//            case BulletType.ThreeWay:
//                FireThreeWay();
//                break;
//            case BulletType.Cannon:
//                FireCannon();
//                break;
//        }
//    }

//    void FireNormal()
//    {
//        GameObject bullet = Instantiate(normalBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
//        Rigidbody rb = bullet.GetComponent<Rigidbody>();
//        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
//    }

//    void FireThreeWay()
//    {
//        float[] angles = { 0f, -spreadAngle, spreadAngle };
//        foreach (float angle in angles)
//        {
//            Quaternion rot = bulletSpawnPoint.rotation * Quaternion.Euler(0, angle, 0);
//            GameObject bullet = Instantiate(threeWayBulletPrefab, bulletSpawnPoint.position, rot);
//            Rigidbody rb = bullet.GetComponent<Rigidbody>();
//            rb.velocity = rot * Vector3.forward * bulletSpeed;
//        }
//    }

//    void FireCannon()
//    {
//        GameObject bullet = Instantiate(cannonBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
//        Rigidbody rb = bullet.GetComponent<Rigidbody>();
//        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
//    }

//    // 経験値を加算するメソッド
//    public void AddExp(int amount)
//    {
//        if (isDead) return;
//        currentExp += amount;
//        Debug.Log("EXP: " + currentExp + "/" + maxExp);
//        if (currentExp >= maxExp)
//        {
//            currentExp = 0;
//            // パワーアップ選択画面を表示
//            GameManager.Instance.OpenPowerUpSelect();
//        }
//    }

//    void Start()
//    {
//        currentHP = maxHP;
//        rend = GetComponent<Renderer>();
//        originalColor = rend.material.color;
//        movementScript = GetComponent<PlayerControllerInput>(); // ← ここもスクリプト名に合わせて！
//    }

//    public void TakeDamage(int damage)
//    {
//        if (isDead) return;

//        currentHP -= damage;
//        Debug.Log("Player HP: " + currentHP);

//        StartCoroutine(FlashRed());

//        if (currentHP <= 0)
//        {
//            Die();
//        }
//    }

//    void Die()
//    {
//        Debug.Log("Player died");
//        isDead = true;

//        // プレイヤーの操作を無効化
//        if (movementScript != null)
//            movementScript.enabled = false;

//        rend.enabled = false;
//        GetComponent<Collider>().enabled = false;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("BossBullet"))
//        {
//            TakeDamage(2);
//            Destroy(other.gameObject);
//        }
//    }

//    IEnumerator FlashRed()
//    {
//        rend.material.color = Color.red;
//        yield return new WaitForSeconds(0.1f);
//        rend.material.color = originalColor;
//    }
//}