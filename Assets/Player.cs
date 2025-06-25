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
    [SerializeField] private GameObject shieldVisual; // �C���X�y�N�^��ShieldVisual��ݒ�
    [SerializeField] private Color[] shieldColors;    // 0:��, 1:��, 2:��

    private Renderer rend;
    private Color originalColor;
    [HideInInspector]
    public PlayerControllerInput movementScript;

    [SerializeField]private int maxShield = 3; // �ő�o���A��
    private int currentShield = 0; // ���݂̃o���A��


    public int maxExp = 10;
    private int currentExp = 0;

    public enum BulletType
    {
        Normal,
        ThreeWay,
        Cannon
    }
    private BulletType currentBulletType = BulletType.Normal;


    public void AddShield(int amount)
    {
        currentShield += amount;
        if (currentShield > maxShield)
            currentShield = maxShield;
        Debug.Log("�o���A�ǉ�: " + currentShield + "/" + maxShield);
        UpdateShieldVisual();
    }

    private void UpdateShieldVisual()
    {
        if (currentShield > 0)
        {
            shieldVisual.SetActive(true);
            // index���z��͈̔͊O�ɂȂ�Ȃ��悤�ɂ���
            int index = Mathf.Clamp(currentShield - 1, 0, shieldColors.Length - 1);
            shieldVisual.GetComponent<Renderer>().material.color = shieldColors[index];
        }
        else
        {
            shieldVisual.SetActive(false);
        }
    }
    public void AddSpeedBuff()
    {
        if (movementScript != null)
        {
            movementScript.moveSpeed += 10f;
        // SpeedUp!!�G�t�F�N�g�\���i�f�o�b�O�p���O���摜�\���̓R�����g�A�E�g�j
        Debug.Log("SpeedUp!!");
        }

        // if (speedUpImage != null)
        // {
        //     StartCoroutine(ShowSpeedUpCR());
        // }
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
        if (currentShield > 0)
        {
            currentShield--;
            Debug.Log("�o���A���_���[�W��h�����I�c��:" + currentShield);
            StartCoroutine(FlashShield());
            UpdateShieldVisual();
            return;
        }
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
    private IEnumerator FlashShield()
    {
        rend.material.color = Color.cyan;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
    IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}
