using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�A�^�b�`�����e�L�X�g���b�V���v����_�ł�����
        if (GetComponent<TMPro.TextMeshProUGUI>() != null)
        {
            var textMesh = GetComponent<TMPro.TextMeshProUGUI>();
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, Mathf.PingPong(Time.time, 1));
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found on this GameObject.");
        }


    }
}
