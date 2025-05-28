using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // �e�̈ړ�������ݒ�
        direction = transform.forward; // �Ⴆ�΁A�e���O���ɔ�ԏꍇ
        // ���C�t�^�C����ݒ�
        Destroy(gameObject, lifetime);
    }
    // �e�̑��x
    public float speed = 10f;
    // �e�̃��C�t�^�C��
    public float lifetime = 5f;
    // �e�̃_���[�W
    public int damage = 1;
    // �e�̈ړ�����
    private Vector3 direction;
   
    void Update()
    {
        // �e�̈ړ�(Velocity���g�p)
        rb.velocity = direction * speed;

    }
}
