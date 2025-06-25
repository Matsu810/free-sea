using Unity.VisualScripting;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // Gem�̎��
    public enum GemType { Shield, SpeedUp, ThreeWay , Cannon }
    public GemType gemType = GemType.Shield;

    // �F�̐ݒ�
    public Color shieldColor = Color.yellow;
    public Color speedUpColor = Color.cyan;
    public Color threeWayColor = new Color(0.6f, 0f, 1f); // ��
    public Color cannonColor = Color.white; // �L���m���C�͔�


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

    // Bullet�����������Ƃ�
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject); // Bullet������

            // GemType���T�C�N��������
            CycleGemType();

            UpdateGemColor();
        }
        else if (other.CompareTag("Player"))
        {
            // �v���C���[��Gem�擾���A���ʂ�K�p
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (gemType)
                {
                    case GemType.Shield:
                        player.AddShield(3); // �o���A��ǉ�(3��)
                        break;
                    case GemType.SpeedUp:
                        player.AddSpeedBuff(); // ��F�X�s�[�h�A�b�v
                        break;
                    case GemType.ThreeWay:
                        player.EnableThreeWayShot(); // ��F3WAY�V���b�g
                        break;
                    case GemType.Cannon:
                        player.SetBulletType(Player.BulletType.Cannon);
                        break;
                }
            }
            Destroy(gameObject); // Gem������
        }
    }

    void CycleGemType()
    {
        // GemType�����̎�ނ�
        gemType = (GemType)(((int)gemType + 1) % System.Enum.GetNames(typeof(GemType)).Length);
    }
}