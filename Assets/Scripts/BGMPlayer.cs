using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMType
{
    Title,
    Battle,
    Win,
    Lose,
    Draw,
    Tutorial
}

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }
    public float CurrentVolume { get; private set; }
    public AudioSource AudioSource { get; private set; }
    private AudioClip _nowSe;

    [SerializeField] private SerializableKeyPair<BGMType, AudioClip>[] bgMs;
    private Dictionary<BGMType, AudioClip> _bgmDictionary;
    private Dictionary<BGMType, AudioClip> BgmDictionary => _bgmDictionary ??= bgMs.ToDictionary(p => p.Key, p => p.Value);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource = GetComponent<AudioSource>();
            CurrentVolume = AudioSource.volume;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += sceneMusic;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void playBGM(BGMType type)
    {
        //���O�ɗ����Ă���BGM���~�߂�
        AudioSource.Stop();

        //������BGM���Z�b�g�E�Đ�
        _nowSe = BgmDictionary[type];

        if (_nowSe != null)
        {
            AudioSource.clip = _nowSe;
            AudioSource.Play();
        }
    }
    public void sceneMusic(Scene nextScene, LoadSceneMode mode)
    {
        //���U���g�V�[���ł̓��[�v���O��
        if (nextScene.name == "ResultScene")
        { 
            AudioSource.loop = false;
        }

        //���U���g���烁�C���ɖ߂����Ƃ��Ƀ��[�v��ݒ肷��B
        if (nextScene.name == "MatchingTest")
        {
            AudioSource.loop = true;
        }

        //�`���[�g���A���V�[���ɑJ�ڂ����Ƃ���BGM��ύX
        if (nextScene.name == "Tutorial1")
        {
            playBGM(BGMType.Tutorial);
        }

        //�X�g�[���[���n�܂�Ƃ���BGM������
        if (nextScene.name == "Sec1")
        {
            AudioSource.Stop();
        }
    }
}
