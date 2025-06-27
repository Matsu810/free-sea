using UnityEngine;

public class LockOnBullet : MonoBehaviour
{
    public float speed = 10f;
    public float homingRadius = 2f;
    public float lifeTime = 3f;

    private Transform target;
    private float timer = 0f;

    void Start()
    {
        // 起動時に一番近い UnderEnemy を探す
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("UnderEnemy");
        float closestDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= homingRadius && dist < closestDist)
            {
                closestDist = dist;
                target = enemy.transform;
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 追尾処理（範囲内の敵がいた場合のみ）
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.forward = direction;
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        // 時間経過で自動消滅
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UnderEnemy"))
        {
            // エフェクトやスコア加算などもここに追加可能
            Destroy(gameObject); // 弾を消す
            // Destroy(other.gameObject); // 敵も消す場合はこちらを有効に
        }
    }
}
