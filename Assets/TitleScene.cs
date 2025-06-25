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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // シーンを切り替える処理
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            Debug.Log("Spaceキーが押されました。ゲームシーンに移行します。");
        }
    }
}
