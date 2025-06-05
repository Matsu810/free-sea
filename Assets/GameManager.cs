using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenPowerUpSelect()
    {
        // ここでUIを開き、複数のパワーアップから選択させる
        // 例: PowerUpUI.Instance.Open();
        Debug.Log("パワーアップ選択画面を表示します");
    }

    public void ApplyPowerUp(PowerUpType type, Player player)
    {
        switch (type)
        {
            case PowerUpType.IncreaseMaxHP:
                player.maxHP += 5;
                break;
            case PowerUpType.IncreaseSpeed:
                // プレイヤーのスピードを上げる
                break;
                // 他にも追加可能
        }
    }
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
    // 必要に応じて他のパワーアップも追加
}