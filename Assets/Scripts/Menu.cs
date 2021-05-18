using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject start;
    public GameObject controls;
    public GameObject credits;
    public GameObject exit;
    public TMP_Text Control;
    public TMP_Text Credit;

    void Start()
    {
        Control.enabled = false;
        Credit.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnMouse()
    {
        Start1();
        Controls();
        Credits();
        Exit();
    }

    public void Start1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Controls()
    {
        Control.enabled = !Control.enabled;
    }

    public void Credits()
    {
        Credit.enabled = !Credit.enabled;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
