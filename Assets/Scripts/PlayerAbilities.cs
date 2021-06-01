using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerAbilities : MonoBehaviour
{
    private FPSController player;

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

    private bool fward, bward, left, right;
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
        player = GetComponent<FPSController>();
        abilitieNotReadyColor = new Color(.3f, .009f, .15f, .5f);
        abilitieReadyColor = new Color(1, 1, 1, 1);
    }

    void Update()
    {
        //Updraft();
        Dash();
        //Invisibility();
    }

    void Updraft()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isUpdrafting && player._isGrounded)
        {
            _audio.PlayOneShot(updraftSound);
            updraftParticle.Play();
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * updraftForce, ForceMode.Impulse);
            //player._velocity.y = Mathf.Sqrt(updraftForce * -2f * player._gravity);
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
            fward = true;
            bward = false;
            left = false;
            right = false;
            dashVector = transform.forward * dashForce;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            fward = false;
            bward = true;
            left = false;
            right = false;
            dashVector = -transform.forward * dashForce;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            fward = false;
            bward = false;
            left = true;
            right = false;
            dashVector = -transform.right * dashForce;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            fward = false;
            bward = false;
            left = false;
            right = true;
            dashVector = transform.right * dashForce;
        }
        else
        {
            fward = true;
            bward = false;
            left = false;
            right = false;
            dashVector = player.transform.forward * dashForce;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isDash)
        {
            if (fward)
            {
                forwardDash.Play();
            }
            if (bward)
            {
                backwardDash.Play();
            }

            if (right)
            {
                rightDash.Play();
            }

            if (left)
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
            player.GetComponent<Rigidbody>().AddForce(dashVector * dashForce, ForceMode.Impulse);  
            //player._controller.Move(dashVector * dashForce * Time.deltaTime);
            yield return null;
        }
    }

}
