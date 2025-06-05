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
        // ������UI���J���A�����̃p���[�A�b�v����I��������
        // ��: PowerUpUI.Instance.Open();
        Debug.Log("�p���[�A�b�v�I����ʂ�\�����܂�");
    }

    public void ApplyPowerUp(PowerUpType type, Player player)
    {
        switch (type)
        {
            case PowerUpType.IncreaseMaxHP:
                player.maxHP += 5;
                break;
            case PowerUpType.IncreaseSpeed:
                // �v���C���[�̃X�s�[�h���グ��
                break;
                // ���ɂ��ǉ��\
        }
    }
}

public enum PowerUpType
{
    IncreaseMaxHP,
    IncreaseSpeed,
    // �K�v�ɉ����đ��̃p���[�A�b�v���ǉ�
}