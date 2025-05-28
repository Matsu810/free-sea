using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 20;
    private int currentHP;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        currentHP = maxHP;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Player HP: " + currentHP);

        StartCoroutine(FlashRed());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossBullet"))
        {
            TakeDamage(2);
            Destroy(other.gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}
