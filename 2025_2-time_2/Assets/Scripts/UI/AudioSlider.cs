using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Slider slider;
    [SerializeField] private string subgroupVolume;

    private void Start()
    {
        SyncSlider();
    }

    private void SyncSlider()
    {
        //if (Mixer.GetFloat(subgroupVolume, out float dB))
        //{
        //    float linear = Mathf.Pow(10f, dB / 20f);
        //    slider.value = linear;

        //    if (slider.targetGraphic != null)
        //        UpdateDisplayText(slider.value);
        //}
        if (AudioManager.Instance.GetSubgroupVolume(subgroupVolume, out float db)) 
        {
            slider.value = db;

            if (slider.targetGraphic != null)
                UpdateDisplayText(slider.value);
        }
        else
        {
            Debug.LogWarning($"Could not get {subgroupVolume} value from AudioMixer");
        }
    }

    private void UpdateDisplayText(float value)
    {
        if (displayText != null)
        {
            displayText.text = value.ToString("N2");
        }
    }

    public void OnChangeSlider(float Value)
    {
        displayText.text = Value.ToString("N2");
        AudioManager.Instance.SetSubgroupVolume(subgroupVolume, Value);
    }
}