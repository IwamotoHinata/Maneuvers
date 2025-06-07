using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public float CurrentVolume { get; private set; }
    public AudioSource AudioSource { get; private set; }
    private AudioClip _nowSe;

    [SerializeField] private float overlapInterval = 0.3f;
    [SerializeField] private SerializableKeyPair<SeType, AudioClip>[] ses;
    private Dictionary<SeType, AudioClip> _seDictionary;
    private Dictionary<SeType, AudioClip> SeDictionary => _seDictionary ??= ses.ToDictionary(p => p.Key, p => p.Value);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource = GetComponent<AudioSource>();
            CurrentVolume = AudioSource.volume;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void shotSe(SeType type)
    {
        if (_nowSe == SeDictionary[type]) return;
        _nowSe = SeDictionary[type];

        if (_nowSe != null)
        {
            AudioSource.PlayOneShot(_nowSe);
        }

        StartCoroutine(resetSe());
    }

    private IEnumerator resetSe()
    {
        yield return new WaitForSeconds(overlapInterval);
        _nowSe = null;
    }
}
