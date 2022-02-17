using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider slider;
    public AudioSource audioSource;
    public float currentVolume;

    private void Start()
    {
        currentVolume = 1f;
    }

    private void Update()
    {
        audioSource.volume = currentVolume;

        slider.value = currentVolume;
    }

    public void SetVolume()
    {
        currentVolume = slider.value;
    }
}
