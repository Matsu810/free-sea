using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int maxHP = 20;
    private int currentHP;
    public bool isDead = false;

    [Header("Bullet Prefabs")]
    public GameObject normalBulletPrefab;
    public GameObject threeWayBulletPrefab;
    public GameObject cannonBulletPrefab;

    private AudioSource audioSource;

    [Header("SE Clips")]
    public AudioClip shieldBreakSE;
    public AudioClip damageSE;
    public AudioClip deathSE;
    public AudioClip lowHPSE;

    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float spreadAngle = 15f;
    [SerializeField] private GameObject shieldVisual; // インスペクタでShieldVisualを設定
    [SerializeField] private Color[] shieldColors;    // 0:黄, 1:緑, 2:青

    private Renderer rend;
    private Color originalColor;
    [HideInInspector]
    public PlayerControllerInput movementScript;

    [SerializeField]private int maxShield = 3; // 最大バリア数
    private int currentShield = 0; // 現在のバリア数


    public int maxExp = 10;


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
        Debug.Log("バリア追加: " + currentShield + "/" + maxShield);
        UpdateShieldVisual();
    }

    private void UpdateShieldVisual()
    {
        if (currentShield > 0)
        {
            shieldVisual.SetActive(true);
            // indexが配列の範囲外にならないようにする
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
        // SpeedUp!!エフェクト表示（デバッグ用ログ＆画像表示はコメントアウト）
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

    
    void Start()
    {
        currentHP = maxHP;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        movementScript = GetComponent<PlayerControllerInput>();
        audioSource = GetComponent<AudioSource>();
        // AudioSourceが無ければ自動追加
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        if (currentShield > 0)
        {
            currentShield--;
            Debug.Log("バリアがダメージを防いだ！残り:" + currentShield);
            PlaySE(shieldBreakSE);
            StartCoroutine(FlashShield());
            UpdateShieldVisual();
            return;
        }
        if (isDead) return;

        currentHP -= damage;
        Debug.Log("Player HP: " + currentHP);
        // HPが1になった瞬間だけSEを再生
        if (currentHP <= 2)
        {
            PlaySE(lowHPSE);
        }
        PlaySE(damageSE);
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
        PlaySE(deathSE);
        if (movementScript != null)
            movementScript.enabled = false;

        rend.enabled = false;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(GoToTitleAfterDelay());
    }
    private IEnumerator GoToTitleAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("TitleScene");
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
    // 効果音再生用メソッド
    private void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
