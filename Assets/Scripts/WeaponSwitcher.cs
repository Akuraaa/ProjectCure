using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeapon = 0;
    public Transform[] guns;
    [HideInInspector]
    public bool m4, pistol;

    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        foreach (var gun in guns)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                pistol = true;
                m4 = false;
                guns[0].gameObject.SetActive(true);
                StartCoroutine(changeWeapon());
                guns[1].gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                m4 = true;
                pistol = false;
                guns[1].gameObject.SetActive(true);
                StartCoroutine(changeWeapon());
                guns[0].gameObject.SetActive(false);
            }
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    IEnumerator changeWeapon()
    {
        yield return new WaitForSeconds(.5f);
    }
}
