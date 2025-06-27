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
        // �N�����Ɉ�ԋ߂� UnderEnemy ��T��
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

        // �ǔ������i�͈͓��̓G�������ꍇ�̂݁j
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.forward = direction;
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        // ���Ԍo�߂Ŏ�������
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UnderEnemy"))
        {
            // �G�t�F�N�g��X�R�A���Z�Ȃǂ������ɒǉ��\
            Destroy(gameObject); // �e������
            // Destroy(other.gameObject); // �G�������ꍇ�͂������L����
        }
    }
}
