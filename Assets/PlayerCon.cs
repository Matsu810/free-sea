using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float moveSpeed = 5f;
    public float fireInterval = 0.2f; // ���ˊԊu�i�b�j

    private float fireTimer = 0f;

    private void Update()
    {
        // ���͎擾�iWASD or ���L�[�j
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // ���̓x�N�g���𐳋K��
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // �ړ������i�����͕ς��Ȃ��j
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // �^�C�}�[�X�V
        fireTimer += Time.deltaTime;

        // �X�y�[�X�L�[�Œe���ˁi0.2�b��1�񂾂��j
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireInterval)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            fireTimer = 0f; // �^�C�}�[���Z�b�g
        }
    }
}
