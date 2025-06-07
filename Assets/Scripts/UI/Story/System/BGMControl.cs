using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMControl : MonoBehaviour
{
    [SerializeField] private List<AudioClip> bgms;
    [SerializeField] private AudioSource audioSource;
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    private float initVolume;
    private Coroutine stopRoutine = null;
    
    private void Awake()
    {
        initVolume = audioSource.volume;
        
        foreach (var b in bgms)
        {
            audioDic.Add(b.name, b);
        }
    }

    public void ChangeBGM(string audioName)
    {
        if (stopRoutine == null)
        {
            stopRoutine = StartCoroutine(ChangeRoutine(1.0f, audioName));
        }
    }
    
    public void PlayBGM(string audioName)
    {
        audioSource.clip = audioDic[audioName];
        audioSource.volume = initVolume;
        audioSource.Play();
    }

    public void StopBGM()
    {
        if (stopRoutine == null)
        {
            stopRoutine = StartCoroutine(StopRoutine(1.0f));
        }
    }

    private IEnumerator StopRoutine(float time)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= initVolume * (Time.deltaTime / time);
            yield return null;
        }
        audioSource.volume = 0.0f;
        stopRoutine = null;
    }
    
    private IEnumerator ChangeRoutine(float time, string audioName)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= initVolume * (Time.deltaTime / time);
            yield return null;
        }
        audioSource.volume = 0.0f;
        PlayBGM(audioName);
        stopRoutine = null;
    }
}
