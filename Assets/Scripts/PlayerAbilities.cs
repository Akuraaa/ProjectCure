using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerController player;

    [Header("Updraft")]
    public bool isUpdrafting;
    public float updraftForce;
    public float updraftCooldown;
    [SerializeField] private AudioClip updraftSound;
    [SerializeField] private Image updraftUI;

    [Header("Dash")]
    public bool isDash;
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    private Vector3 dashVector;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private Image dashUI;


    [SerializeField] private ParticleSystem forwardDash, backwardDash, leftDash, rightDash, updraftParticle;

    [Header("Invisibility")]
    public bool isInvisibility;
    public float invisibilityTime;
    public float invisibilityCooldown;
    private float _invisibilityTime;
    [SerializeField] private AudioClip invisibilitySound;
    [SerializeField] private Image invisiblityUI;
    [SerializeField] private Image cloakFeedback;
    private Color alphaColor;
    private Color abilitieNotReadyColor;
    private Color abilitieReadyColor;
    private AudioSource _audio;


    private void Awake()
    {
        alphaColor = cloakFeedback.color;
        alphaColor.a = 0;
        cloakFeedback.color = alphaColor;

        _invisibilityTime = invisibilityTime;

        _audio = GetComponent<AudioSource>();
        player = GetComponent<PlayerController>();
        abilitieNotReadyColor = new Color(.3f, .009f, .15f, .5f);
        abilitieReadyColor = new Color(1, 1, 1, 1);
    }

    void Update()
    {
        Updraft();
        Dash();
        Invisibility();
    }

    void Updraft()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isUpdrafting && player._isGrounded)
        {
            _audio.PlayOneShot(updraftSound);
            updraftParticle.Play();
            player._velocity.y = Mathf.Sqrt(updraftForce * -2f * player._gravity);
            isUpdrafting = true;
            updraftUI.fillAmount = 0;
            updraftUI.color = abilitieNotReadyColor;
        }
        if (isUpdrafting)
        {
            updraftUI.fillAmount += 1 / updraftCooldown * Time.deltaTime;
           
            if(updraftUI.fillAmount >= 1)
            {
                updraftUI.fillAmount = 1;
                updraftUI.color = abilitieReadyColor;
                isUpdrafting = false;
            }
        }
    }

    void Invisibility()
    {
        cloakFeedback.color = alphaColor;
        if (Input.GetKeyDown(KeyCode.C) && !isInvisibility)
        {
            alphaColor.a = .75f;
            cloakFeedback.color = alphaColor;
            transform.gameObject.layer = 10;
            _audio.PlayOneShot(invisibilitySound);
            isInvisibility = true;
            invisiblityUI.fillAmount = 0;
            invisiblityUI.color = abilitieNotReadyColor;
        }
        if (transform.gameObject.layer == 10)
        {
            invisibilityTime -= Time.deltaTime;
            if (invisibilityTime <= 0)
            {
                alphaColor.a = 0;
                cloakFeedback.color = alphaColor;
                transform.gameObject.layer = 8;
                invisibilityTime = _invisibilityTime;
            }
        }
        if (isInvisibility)
        {
            invisiblityUI.fillAmount += 1 / invisibilityCooldown * Time.deltaTime;
            if (invisiblityUI.fillAmount >= 1)
            {
                invisiblityUI.fillAmount = 1;
                invisiblityUI.color = abilitieReadyColor;
                isInvisibility = false;
            }
        }

    }

    void Dash()
    {
        if (Input.GetKey(KeyCode.W))
        {
            dashVector = transform.forward * dashForce;
            dashVector.y = 0;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dashVector = -transform.forward * dashForce;
            dashVector.y = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            dashVector = -transform.right * dashForce;
            dashVector.y = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dashVector = transform.right * dashForce;
            dashVector.y = 0;
        }
        else
        {
            dashVector = player.transform.forward * dashForce;
            dashVector.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isDash)
        {
            if (dashVector.z > 0 && Mathf.Abs(dashVector.x) <= dashVector.z)
            {
                forwardDash.Play();
            }

            if (dashVector.z < 0 && Mathf.Abs(dashVector.x) <= Mathf.Abs(dashVector.z))
            {
                backwardDash.Play();
            }

            if (dashVector.x > 0 && Mathf.Abs(dashVector.z) <= dashVector.x)
            {
                rightDash.Play();
            }

            if (dashVector.x < 0 && Mathf.Abs(dashVector.z) <= Mathf.Abs(dashVector.x))
            {
                leftDash.Play();
            }

            _audio.PlayOneShot(dashSound);
            StartCoroutine(DashCoroutine());
            isDash = true;
            dashUI.fillAmount = 0;
            dashUI.color = abilitieNotReadyColor;
        }
        if (isDash)
        {
            dashUI.fillAmount += 1 / dashCooldown * Time.deltaTime;
            if (dashUI.fillAmount >= 1)
            {
                dashUI.fillAmount = 1;
                dashUI.color = abilitieReadyColor;
                isDash = false;
            }
        }
    }

    IEnumerator DashCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            player._controller.Move(dashVector * dashForce * Time.deltaTime);
            yield return null;
        }
    }

}
