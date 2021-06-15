using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public Image healthBar;
    public Image bloodyScreen;
    public Image crowbarImg;
    public Image oilImg;

    public TMP_Text healthText;
    public TMP_Text oilCountText;
    public float maxHealth = 100;
    public float curHealth = 0;
    public int oilCount = 0;

    public float timeToFade = 2;
    public bool hitPlayer = false;
    private Color alphaColor;

    //public float timerToFinishLevel;

    [SerializeField] private TMP_Text situationText;
    [SerializeField] public bool haveCode, openDoor, haveMap, haveElectricity, haveCrowbar, canGenerator;
    public GameObject electricityBox;
    public GameObject[] sparks;
    public GameObject giantBoxCollider;


    //[SerializeField] private TMP_Text timeText;
    //[SerializeField] private Light[] spotLights;
    //[SerializeField] private Color normalColor, warningColor;
    [SerializeField] private AudioClip pickUp;
    [SerializeField] private GameObject map;

    private void Start()
    {
        oilCount = 0;
        oilCountText.text = oilCount + "/3";
        oilImg.gameObject.SetActive(false);
        Cursor.visible = false;
        alphaColor = bloodyScreen.color;
        alphaColor.a = 0;
        bloodyScreen.color = alphaColor;
        curHealth = maxHealth;
        healthText.text = curHealth.ToString();
        SetHealthBar();
        situationText.gameObject.SetActive(false);
        //timeText.gameObject.SetActive(false);
        //for (int i = 0; i < spotLights.Length; i++)
        //{
        //    spotLights[i].color = normalColor;
        //}
    }

    private void Update()
    {
        bloodyScreen.color = alphaColor;
        if (hitPlayer)
        {
            timeToFade -= Time.deltaTime;
            if (timeToFade <= 0)
            {
                bloodyScreen.color = alphaColor;
                alphaColor.a -= Time.deltaTime;
                timeToFade = .25f;
                return;
            }

            if (alphaColor.a <= 0)
            {
                hitPlayer = false;
                timeToFade = 2;
            }
        }

        if (haveMap && Input.GetKey(KeyCode.V))
        {
            map.SetActive(true);
        }
        else
        {
            map.SetActive(false);
        }

        if (haveCrowbar)
        {
            crowbarImg.gameObject.SetActive(true);
        }
        else
        {
            crowbarImg.gameObject.SetActive(false);
        }

        if (haveElectricity)
        {
            electricityBox.SetActive(false);
        }
        else
        {
            electricityBox.SetActive(true);
        }

        oilCountText.text = oilCount + "/3";
    }

    public void SetHealthBar()
    {
        healthText.text = curHealth.ToString();
        healthBar.fillAmount = curHealth / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        alphaColor.a = (maxHealth - curHealth) * 0.01f;
        bloodyScreen.color = alphaColor;
        SetHealthBar();
        if (curHealth < 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Derrota");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mapa"))
        {
            haveMap = true;
            GetComponent<AudioSource>().PlayOneShot(pickUp);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            if (oilCount == 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para abrir";
                if (Input.GetKeyDown(KeyCode.F))
                {            
                    openDoor = true;
                    if (openDoor)
                    {
                        situationText.gameObject.SetActive(false);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        SceneManager.LoadScene("Win");
                    }
                }
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "No puedes abrir porque no hay electricidad";
            }

        }

        if (other.gameObject.CompareTag("Generator"))
        {
            oilImg.gameObject.SetActive(true);
            if (oilCount == 0)
            {
                situationText.gameObject.SetActive(true);
                oilImg.gameObject.SetActive(true);
                situationText.text = "Necesitas combustible";
            }
            else if (oilCount > 0 && oilCount < 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Necesitas mas combustible";
            }
            else if (oilCount == 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para activar el generador";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    other.GetComponent<Outline>().enabled = false;
                }
            }
        }


        if (other.gameObject.CompareTag("Oil"))
        {
            oilImg.gameObject.SetActive(true);
            if (oilCount < 3)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para agarrar";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    oilCount += 1;
                    GetComponent<AudioSource>().PlayOneShot(pickUp);
                    Destroy(other.gameObject);
                    situationText.gameObject.SetActive(false);
                }
            }
        }

        if (other.gameObject.CompareTag("Electricity"))
        {
            if (haveElectricity)
            {
                situationText.gameObject.SetActive(false);         
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Deberia cortar la electricidad";
            }
        }


        if (other.gameObject.CompareTag("Crowbar"))
        {
            if (!haveCrowbar)
            {
                haveCrowbar = true;
                GetComponent<AudioSource>().PlayOneShot(pickUp);
                Destroy(other.gameObject);
            }
        }


        if (other.gameObject.CompareTag("ElectricBox"))
        {
            if (haveCrowbar)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presione F para usar la barreta";

                if (Input.GetKeyDown(KeyCode.F))
                {
                    haveElectricity = true;
                    foreach (var spark in sparks)
                    {
                        spark.SetActive(false);
                    }
                    situationText.gameObject.SetActive(false);
                    giantBoxCollider.SetActive(false);

                }
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Deberia buscar una barreta";
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
       if (other.gameObject.CompareTag("Door"))
       {
           situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Crowbar"))
       {
           situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Electricity"))
       {
           situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("ElectricBox"))
       {
           situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Generator"))
       {
           situationText.gameObject.SetActive(false);
       }
       if (other.gameObject.CompareTag("Oil"))
       {
           situationText.gameObject.SetActive(false);
       }
    }

    //private void SetLightning()
    //{
    //    for (int i = 0; i < spotLights.Length; i++)
    //    {
    //        spotLights[i].GetComponent<LightScript>().changeColor = true;
    //        spotLights[i].color = warningColor;
    //    }
    //}
    //
    //private void SetTimerOn()
    //{
    //    if (timerToFinishLevel > 0)
    //    {
    //        timerToFinishLevel -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        timerToFinishLevel = 0;
    //    }
    //    DisplayTime(timerToFinishLevel);
    //}
    //
    //void DisplayTime(float timeToDisplay)
    //{
    //    if (timeToDisplay < 0)
    //    {
    //        timeToDisplay = 0;
    //    }
    //
    //    float seconds = Mathf.FloorToInt(timeToDisplay % 60);
    //    float miliseconds = timeToDisplay % 1 * 100;
    //
    //    timeText.text = string.Format("{0:00}:{1:00}", seconds, miliseconds);
    //}
}
