using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //�ǂ̃L�[��{�^���������Ă��Q�[���X�^�[�g
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // �V�[����؂�ւ��鏈��
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            Debug.Log("Space�L�[��������܂����B�Q�[���V�[���Ɉڍs���܂��B");
        }
    }
}
        
