using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;
    public void SliderValue(float v)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(v) * multiplier);
    }


    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
        {
            slider.value = _value;
            SliderValue(_value); 
        }
    }







}
