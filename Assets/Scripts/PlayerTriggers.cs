using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    public bool haveMap;
    public bool haveCode;
    [SerializeField] private GameObject noCodeText;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mapa")
        {
            haveMap = true;
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Code")
        {
            haveCode = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "FinalDoor")
        {
            if (!haveCode)
            {
                noCodeText.SetActive(true);
            }
        }
    }
}
