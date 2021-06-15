using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneratorScript : MonoBehaviour
{
    public GameObject text;
    public Outline outlineScript;

    private void Update()
    {
        if (text == null && outlineScript == null)
        {
            return;
        }
    }
}
