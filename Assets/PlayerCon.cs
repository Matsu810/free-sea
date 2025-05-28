using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float moveSpeed = 5f;
    public float fireInterval = 0.2f; // 発射間隔（秒）

    private float fireTimer = 0f;

    private void Update()
    {
        // 入力取得（WASD or 矢印キー）
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 入力ベクトルを正規化
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 移動処理（向きは変えない）
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // タイマー更新
        fireTimer += Time.deltaTime;

        // スペースキーで弾発射（0.2秒に1回だけ）
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireInterval)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            fireTimer = 0f; // タイマーリセット
        }
    }
}
