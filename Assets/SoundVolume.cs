using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(ListenerMethod);
    }
    
 
    public void ListenerMethod(float value)
    {
        Debug.Log(value);
    }
}
