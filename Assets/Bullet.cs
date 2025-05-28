using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 弾の移動方向を設定
        direction = transform.forward; // 例えば、弾が前方に飛ぶ場合
        // ライフタイムを設定
        Destroy(gameObject, lifetime);
    }
    // 弾の速度
    public float speed = 10f;
    // 弾のライフタイム
    public float lifetime = 5f;
    // 弾のダメージ
    public int damage = 1;
    // 弾の移動方向
    private Vector3 direction;
   
    void Update()
    {
        // 弾の移動(Velocityを使用)
        rb.velocity = direction * speed;

    }
}
