using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHP = 20;
    private int currentHP;
    public bool isDead = false; // プレイヤーが死んでいるかどうか

    private Renderer rend;
    private Color originalColor;
    private PlayerControllerInput movementScript; // ← ここはあなたの移動スクリプト名
    public int maxExp = 10;
    private int currentExp = 0;

    // 経験値を加算するメソッド
    public void AddExp(int amount)
    {
        if (isDead) return;
        currentExp += amount;
        Debug.Log("EXP: " + currentExp + "/" + maxExp);
        if (currentExp >= maxExp)
        {
            currentExp = 0;
            // パワーアップ選択画面を表示
            GameManager.Instance.OpenPowerUpSelect();
        }
    }
    void Start()
    {
        currentHP = maxHP;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        movementScript = GetComponent<PlayerControllerInput>(); // ← ここもスクリプト名に合わせて！
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 死亡後はダメージ無視

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
        isDead = true;

        // プレイヤーの操作を無効化
        if (movementScript != null)
            movementScript.enabled = false;

        rend.enabled = false;
        GetComponent<Collider>().enabled = false;
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