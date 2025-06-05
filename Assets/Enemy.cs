using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHP = 1;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float moveSpeed = 2f; // Zマイナス方向への移動速度

    private float fireTimer = 0f;
    private int currentHP;
    private Transform player;
    private Rigidbody rb;

    private void Start()
    {
        currentHP = maxHP;

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.back * moveSpeed; // ← Rigidbody で移動

        // プレイヤーを取得
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        HandleShooting();
        Vector3 direction = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
       
    }

    private void HandleShooting()
    {
        if (player == null) return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            Fire();
        }
    }

    private void Fire()
    {
        if (player == null) return;

        Vector3 direction = (player.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.velocity = direction * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
            }

            TakeDamage(1);
        }

        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
