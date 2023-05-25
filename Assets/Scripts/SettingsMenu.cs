using System;
using UnityEngine.Audio;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public TextMeshPro volumeText, volumeUp, volumeDown, low, medium, high;

    private float currentVolume = 0.5f;

    private void Start()
    {
        SetVolume(currentVolume);
        mainMixer.SetFloat("Volume", -40f);
        SetQuality(1);
    }

    public void SetQuality(int qualityIndex)
    {
        low.color = new Color32(255, 255, 255, 255);
        medium.color = new Color32(255, 255, 255, 255);
        high.color = new Color32(255, 255, 255, 255);

        if (qualityIndex == 0)
        {
            low.color = new Color32(203, 128, 30, 255);
        }
        else if (qualityIndex == 1)
        {
            medium.color = new Color32(203, 128, 30, 255);
        }
        else if (qualityIndex == 2)
        {
            high.color = new Color32(203, 128, 30, 255);
        }

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void upVolume()
    {
        currentVolume += 0.1f;
        if (currentVolume >= 1f)
            currentVolume = 1f;
        
        SetVolume(currentVolume);
    }

    public void downVolume()
    {
        currentVolume -= 0.1f;
        if (currentVolume <= 0f)
            currentVolume = 0f;

        SetVolume(currentVolume);
    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("Volume", (volume*80f)-80f);
        volumeText.text = Convert.ToInt32(volume*10f).ToString();
    }

    public void SelectColor(TextMeshPro tm)
    {
        tm.color = new Color32(203, 128, 30, 255);
    }

    public void UnSelectColor(TextMeshPro tm)
    {
        tm.color = new Color32(255, 255, 255, 255);
    }
    public void SelectGraphics(TextMeshPro tm)
    {
        if (tm.color != new Color32(203, 128, 30, 255))
            tm.color = new Color32(32, 32, 32, 255);
    }

    public void UnSelectGraphics(TextMeshPro tm)
    {
        if (tm.color != new Color32(203, 128, 30, 255))
            tm.color = new Color32(255, 255, 255, 255);
    }

}
