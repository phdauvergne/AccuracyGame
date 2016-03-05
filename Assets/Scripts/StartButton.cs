using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour
{
    private GameController gameController;
    public int check = 1;   //1 = checked, 0 = unchecked
    private float sliderValue = 0.3f;

    public void StartLevel()
    {
        PlayerPrefs.SetFloat("spawnDelay", sliderValue);
        PlayerPrefs.SetInt("checked", check);
        Application.LoadLevel("_scene");
    }

    // changes the value of sliderValue before sending it to the other level
    public void setSliderValue(float value)
    {
        sliderValue = value;
    }

    public void setChecked()
    {
        if (check == 1)
            check = 0;
        else
            check = 1;
        
        Debug.Log("checked : " + check);
    }
}
