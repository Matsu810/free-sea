using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastBoss : MonoBehaviour
{
    public GameObject explosionEffectPrefab; // Inspector�Ŕ����G�t�F�N�gPrefab���Z�b�g
    private Rigidbody rb;

    void Start()
    {
        BGMManager.Instance.PlayBGMForScene("LastBoss"); // ������BGM���{�X�p�ɐ؂�ւ�
        StartCoroutine(GravityAndExplosionSequence());
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // ������Ԃ͏d�̓I�t
        }
        StartCoroutine(GravityAndExplosionSequence());
    }

    // �e���͂����i���˕Ԃ��j����
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Rigidbody bulletRb = other.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = -bulletRb.velocity;
            }
            // �_���[�W�����͂��Ȃ�
        }
    }

    IEnumerator GravityAndExplosionSequence()
    {
        // 5�b��ɏd��ON
        yield return new WaitForSeconds(5f);
        if (rb != null)
        {
            rb.useGravity = true;
        }

        // �����10�b��ɔ������{�X����
        yield return new WaitForSeconds(10f);
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        // �����ڂƓ����蔻�������
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        // �����5�b��ɃN���A�V�[����
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ClearScene");
    }
 //       Destroy(gameObject);

}