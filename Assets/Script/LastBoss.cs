using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastBoss : MonoBehaviour
{
    public GameObject explosionEffectPrefab; // Inspectorで爆発エフェクトPrefabをセット
    private Rigidbody rb;

    void Start()
    {
        BGMManager.Instance.PlayBGMForScene("LastBoss"); // ここでBGMをボス用に切り替え
        StartCoroutine(GravityAndExplosionSequence());
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // 初期状態は重力オフ
        }
        StartCoroutine(GravityAndExplosionSequence());
    }

    // 弾をはじく（跳ね返す）処理
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Rigidbody bulletRb = other.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = -bulletRb.velocity;
            }
            // ダメージ処理はしない
        }
    }

    IEnumerator GravityAndExplosionSequence()
    {
        // 5秒後に重力ON
        yield return new WaitForSeconds(5f);
        if (rb != null)
        {
            rb.useGravity = true;
        }

        // さらに10秒後に爆発＆ボス消滅
        yield return new WaitForSeconds(10f);
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        // 見た目と当たり判定を消す
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        // さらに5秒後にクリアシーンへ
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ClearScene");
    }
 //       Destroy(gameObject);

}