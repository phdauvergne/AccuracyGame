using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slider : MonoBehaviour 
{
    Text textComponent;
    Slider sliderComponent;

    void Start()
    {
        textComponent = GetComponent<Text>();
    }

    public void SetSliderValue(float sliderValue)
    {
        textComponent.text = sliderValue.ToString();
    }
    
}