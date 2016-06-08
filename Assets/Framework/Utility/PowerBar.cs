using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerBar : MonoBehaviour {

    public float sliderSpeed = 2f;
    Slider slider;

    public bool hasLaunched = false;

    float startTime;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () 
    {
       if (!LevelController.instance.isPlaying)
        {
            return;
        }
        if (!hasLaunched)
        {
            slider.value = Time.time - startTime;
            if (slider.value >= 1f)
            {
                startTime = Time.time;
            }
        }
	}
}
