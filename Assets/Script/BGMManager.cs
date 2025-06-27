using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [System.Serializable]
    public class BGMEntry
    {
        public string sceneName;   // �V�[��
        public AudioClip bgmClip;
    }

    [SerializeField] private List<BGMEntry> bgmList = new List<BGMEntry>();
    [SerializeField] private AudioSource audioSource;

    public static BGMManager Instance { get; private set; }

    private void Awake()
    {
        // �V���O���g����
        if (Instance == null)
        {
            Instance = this;
         
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGMForCurrentScene();
    }

    private void PlayBGMForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayBGMForScene(currentScene);
    }

    // �V�[�����i�܂��͔C�ӂ̓o�^���j��BGM��؂�ւ�
    public void PlayBGMForScene(string sceneOrSpecialName)
    {
        foreach (BGMEntry entry in bgmList)
        {
            if (entry.sceneName == sceneOrSpecialName)
            {
                if (entry.bgmClip != null && audioSource != null)
                {
                    audioSource.clip = entry.bgmClip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                break;
            }
        }
    }

    // ����AudioClip�w���BGM��؂�ւ�
    public void PlayBGM(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}