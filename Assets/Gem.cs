using UnityEngine;

public class Gem : MonoBehaviour
{
    // Gem�̎��
    public enum GemType { Score, SpeedUp, ThreeWay , Cannon }
    public GemType gemType = GemType.Score;

    // �F�̐ݒ�
    public Color scoreColor = Color.yellow;
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
            case GemType.Score: rend.material.color = scoreColor; break;
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
                    case GemType.Score:
                        player.AddScore(10); // ��F�X�R�A�����Z
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