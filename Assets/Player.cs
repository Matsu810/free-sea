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
        // �X�R�A���Z����
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
//    public bool isDead = false; // �v���C���[������ł��邩�ǂ���

//    [Header("Bullet Prefabs")]
//    public GameObject normalBulletPrefab;    // �m�[�}���e�v���n�u
//    public GameObject threeWayBulletPrefab;  // 3WAY�e�v���n�u
//    public GameObject cannonBulletPrefab;    // �L���m���e�v���n�u

//    public Transform bulletSpawnPoint;       // �e�̔��ˈʒu
//    public float bulletSpeed = 10f;          // �e�̑��x
//    public float spreadAngle = 15f;          // ���E�̊J���p�x

//    private Renderer rend;
//    private Color originalColor;
//    private PlayerControllerInput movementScript; // �� �����͂��Ȃ��̈ړ��X�N���v�g��

//    public int maxExp = 10;
//    private int currentExp = 0;

//    // �e�^�C�v
//    public enum BulletType
//    {
//        Normal,
//        ThreeWay,
//        Cannon
//    }
//    private BulletType currentBulletType = BulletType.Normal;

//    public void AddScore(int amount)
//    {
//        // �X�R�A���Z����
//    }

//    public void AddSpeedBuff()
//    {
//        // �X�s�[�h�A�b�v����  
//        if (movementScript != null)
//        {
//            movementScript.moveSpeed += 10f;
//        }
//    }

//    public void EnableThreeWayShot()
//    {
//        SetBulletType(BulletType.ThreeWay);
//        // �K�v�Ȃ��莞�ԂŐ؂�ւ��������鏈�����ǉ��\
//    }

//    public void EnableNormalShot()
//    {
//        SetBulletType(BulletType.Normal);
//    }

//    public void EnableCannonShot()
//    {
//        SetBulletType(BulletType.Cannon);
//        // �K�v�Ȃ��莞�ԂŐ؂�ւ��������鏈�����ǉ��\
//    }

//    public void SetBulletType(BulletType type)
//    {
//        currentBulletType = type;
//    }

//    // �ʏ�̍U�������i��FUpdate����ĂԁA�܂���Fire���\�b�h�Ȃǁj
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

//    // �o���l�����Z���郁�\�b�h
//    public void AddExp(int amount)
//    {
//        if (isDead) return;
//        currentExp += amount;
//        Debug.Log("EXP: " + currentExp + "/" + maxExp);
//        if (currentExp >= maxExp)
//        {
//            currentExp = 0;
//            // �p���[�A�b�v�I����ʂ�\��
//            GameManager.Instance.OpenPowerUpSelect();
//        }
//    }

//    void Start()
//    {
//        currentHP = maxHP;
//        rend = GetComponent<Renderer>();
//        originalColor = rend.material.color;
//        movementScript = GetComponent<PlayerControllerInput>(); // �� �������X�N���v�g���ɍ��킹�āI
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

//        // �v���C���[�̑���𖳌���
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