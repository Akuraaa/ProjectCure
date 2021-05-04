using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbilities : MonoBehaviour
{
    public FpsControllerLPFP player;

    [Header("Super Jump")]
    public bool isSuperJump;
    public float superJumpForce;
    public float superJumpCooldown;
    public Image superJumpImg;
    public AudioClip superJumpClip;

    [Header("Dash")]
    public Vector3 dashVector;
    public bool isDash;
    public float dashForce;
    public float dashCooldown;
    public Image dashImg;
    public AudioClip dashClip;

    [Header("Cloak")]
    public bool isCloak;
    public float cloakTime;
    private float _cloakTime;
    public float cloakCooldown;
    public LayerMask cloakMask, playerMask;
    public Material[] armsMaterial;
    public Image cloakImg;

    private void Awake()
    {
        player = GetComponent<FpsControllerLPFP>();
    }

    private void Start()
    {
        _cloakTime = cloakTime;
        foreach (var mat in armsMaterial)
        {
            mat.SetFloat("_Transparency", 0);
        }
    }

    private void Update()
    {
        MegaJump();
        Dash();
        Cloak();
    }

    void MegaJump()
    {
        if (Input.GetKey(KeyCode.Q) && !isSuperJump && player._isGrounded)
        {
            player._audioSource.PlayOneShot(superJumpClip);
            player._rigidbody.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);
            player._isGrounded = false;
            isSuperJump = true;
            superJumpImg.fillAmount = 0;
        }
        if (isSuperJump)
        {
            superJumpImg.fillAmount += 1 / superJumpCooldown * Time.deltaTime;

            if (superJumpImg.fillAmount >= 1)
            {
                superJumpImg.fillAmount = 1;
                isSuperJump = false;
            }
        }
    }

    void Dash()
    {
        if (Input.GetKey(KeyCode.W) || !Input.anyKey)
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
            dashVector = new Vector3(0, 0, 1 * dashForce);
            dashVector = dashVector - transform.localRotation.eulerAngles;
            dashVector.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isDash)
        {
            player._audioSource.PlayOneShot(dashClip);
            player._rigidbody.AddForce(dashVector * dashForce * Time.deltaTime);
            isDash = true;
            dashImg.fillAmount = 0;
        }   
        if (isDash)
        {
            dashImg.fillAmount += 1 / dashCooldown * Time.deltaTime;

            if (dashImg.fillAmount >= 1)
            {
                dashImg.fillAmount = 1;
                isDash = false;
            }
        }
    }

    void Cloak()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isCloak)
        {
            isCloak = true;
            cloakImg.fillAmount = 0;
        }

        if (isCloak)
        {
            cloakTime -= Time.time;
            gameObject.layer = cloakMask;
            foreach (var mat in armsMaterial)
            {
                mat.SetFloat("_Transparency", .5f);
            }
            if (cloakTime <= 0)
            {
                gameObject.layer = playerMask;
                foreach (var mat in armsMaterial)
                {
                    mat.SetFloat("_Transparency", 0);
                }
                cloakTime = _cloakTime;
            }
            cloakImg.fillAmount += 1 / cloakCooldown * Time.deltaTime;
            if (cloakImg.fillAmount >= 1)
            {
                cloakImg.fillAmount = 1;
                isCloak = false;
            }
        }
    }

}
