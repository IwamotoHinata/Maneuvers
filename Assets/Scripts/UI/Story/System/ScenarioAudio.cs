using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ScenarioAudio : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audios;
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        foreach (var a in audios)
        {
            audioDic.Add(a.name,a);
        }
    }

    public void ShotSE(string audioName)
    {
        audioSource.PlayOneShot(audioDic[audioName]);
    }

    public void StopSE()
    {
        audioSource.Stop();
    }
}
