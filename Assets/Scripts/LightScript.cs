using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    Light lightSettings;
    bool change = false;
    public bool changeColor = false;
    public float timeToChange;
    public float timeScale;

    private void Awake()
    {
        lightSettings = GetComponent<Light>();
    }
    private void Start()
    {
        lightSettings.color = Color.white;
    }

    private void Update()
    {
        if (changeColor)
        {
            lightSettings.range = timeToChange;

            if (change)
                timeToChange -= Time.deltaTime * timeScale;
            else
                timeToChange += Time.deltaTime * timeScale;

            if (lightSettings.range <= 10)
                change = false;
            else if (lightSettings.range >= 13)
                change = true;
        }
    }

}
