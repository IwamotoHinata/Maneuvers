using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionPresenter : MonoBehaviour
{
    [SerializeField] private OptionManager optionManager;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource seAudioSource;
    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(ChangeBGMValue);
        seSlider.onValueChanged.AddListener(ChangeSEValue);
    }

    void ChangeBGMValue(float value)
    {
        bgmAudioSource.volume = value;
    }

    void ChangeSEValue(float value)
    {
        seAudioSource.volume = value;
    }
}
