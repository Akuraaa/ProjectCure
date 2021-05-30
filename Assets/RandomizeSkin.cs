using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSkin : MonoBehaviour
{
    public List<GameObject> zombieSkins = new List<GameObject>();
    private int n;
    private void Awake()
    {
        n = Random.Range(0, zombieSkins.Count - 1);    
    }

    private void Start()
    {
        foreach (var skin in zombieSkins)
        {
            skin.gameObject.SetActive(false);
        }

        zombieSkins[n].SetActive(true);
    }
}
