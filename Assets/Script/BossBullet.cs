using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    private float timer = 0f;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
