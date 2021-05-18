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

    public TMP_Text healthText;
    public float maxHealth = 100;
    public float curHealth = 0;

    public float timeToFade = 2;
    public bool hitPlayer = false;
    private Color alphaColor;

    public float timerToFinishLevel;

    [SerializeField] private TMP_Text situationText;
    private bool haveCode, openDoor;


    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Light[] spotLights;
    [SerializeField] private Color normalColor, warningColor;
    [SerializeField] private AudioClip pickUp;

    private void Start()
    {
        Cursor.visible = false;
        alphaColor = bloodyScreen.color;
        alphaColor.a = 0;
        bloodyScreen.color = alphaColor;
        curHealth = maxHealth;
        healthText.text = curHealth.ToString();
        SetHealthBar();

        situationText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].color = normalColor;
        }
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
                
            }

            if (alphaColor.a <= 0)
            {
                hitPlayer = false;
                timeToFade = 2;
            }
        }

        if (openDoor)
        {
            SetLightning();
            //SetTimerOn();
            //SETEAR EL TIEMPO
        }
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
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene("Derrota");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PaperCode"))
        {
            haveCode = true;
            GetComponent<AudioSource>().PlayOneShot(pickUp);         
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            if (haveCode)
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Presiona F para abrir";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    openDoor = true;
                    if (openDoor)
                    {
                        situationText.gameObject.SetActive(false);
                        SceneManager.LoadScene("Win");
                    }
                }
            }
            else
            {
                situationText.gameObject.SetActive(true);
                situationText.text = "Necesitas el codigo para abrir";
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            situationText.gameObject.SetActive(false);
        }
    }

    private void SetLightning()
    {
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].GetComponent<LightScript>().changeColor = true;
            spotLights[i].color = warningColor;
        }
    }

    private void SetTimerOn()
    {
        if (timerToFinishLevel > 0)
        {
            timerToFinishLevel -= Time.deltaTime;
        }
        else
        {
            timerToFinishLevel = 0;
        }
        DisplayTime(timerToFinishLevel);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float miliseconds = timeToDisplay % 1 * 100;

        timeText.text = string.Format("{0:00}:{1:00}", seconds, miliseconds);
    }
}
