using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage;
    public float range, fireRate;
    private float nextTimeToFire = 0;

    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1;
    private bool isReloading;
    public bool changeWeapon = true;

    public Camera fpsCam;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    private void FixedUpdate()
    {
        if (isReloading)
            return;

        if (currentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            changeWeapon = false;
            return;
        }
    }
    void Shoot()
    {
        currentAmmo--;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        _anim.SetInteger("Bullets", currentAmmo);
        yield return new WaitForSeconds(reloadTime - .25f);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
        changeWeapon = true;
    }
}
