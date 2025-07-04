using Unity.VisualScripting;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // Gemの種類
    public enum GemType { Shield, SpeedUp, ThreeWay , Cannon }
    public GemType gemType = GemType.Shield;

    // 色の設定
    public Color shieldColor = Color.yellow;
    public Color speedUpColor = Color.cyan;
    public Color threeWayColor = new Color(0.6f, 0f, 1f); // 紫
    public Color cannonColor = Color.white; // キャノン砲は白


    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateGemColor();
    }

    void UpdateGemColor()
    {
        switch (gemType)
        {
            case GemType.Shield: rend.material.color = shieldColor; break;
            case GemType.SpeedUp: rend.material.color = speedUpColor; break;
            case GemType.ThreeWay: rend.material.color = threeWayColor; break;
            case GemType.Cannon: rend.material.color = cannonColor; break;
        }
    }

    // Bulletが当たったとき
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject); // Bulletを消す

            // GemTypeをサイクルさせる
            CycleGemType();

            UpdateGemColor();
        }
        else if (other.CompareTag("Player"))
        {
            // プレイヤーがGem取得時、効果を適用
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (gemType)
                {
                    case GemType.Shield:
                        player.AddShield(3); // バリアを追加(3回分)
                        break;
                    case GemType.SpeedUp:
                        player.AddSpeedBuff(); // 例：スピードアップ
                        break;
                    case GemType.ThreeWay:
                        player.EnableThreeWayShot(); // 例：3WAYショット
                        break;
                    case GemType.Cannon:
                        player.SetBulletType(Player.BulletType.Cannon);
                        break;
                }
            }
            Destroy(gameObject); // Gemを消す
        }
    }

    void CycleGemType()
    {
        // GemTypeを次の種類に
        gemType = (GemType)(((int)gemType + 1) % System.Enum.GetNames(typeof(GemType)).Length);
    }
}