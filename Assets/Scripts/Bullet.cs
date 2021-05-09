﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;

	[Header("Impact Effect Prefabs")]
	public Transform[] bloodImpactPrefabs;
	public Transform[] metalImpactPrefabs;
	public Transform[] dirtImpactPrefabs;
	public Transform[] concreteImpactPrefabs;

	public int damage;

	private void Start()
	{
		//Start destroy timer
		StartCoroutine(DestroyAfter());
	}

    private void OnCollisionEnter(Collision collision)
    {
		if (!destroyOnImpact)
		{
			StartCoroutine(DestroyTimer());
		}
		else
		{
			Destroy(gameObject);
		}

		if (collision.gameObject.tag == "Player")
		{
			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
		}

		if (collision.gameObject.tag == "Enemy")
        {
			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }

		if (collision.transform.tag == "Metal")
		{
			//Instantiate random impact prefab from array
			Instantiate(metalImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		if (collision.transform.tag == "Dirt")
		{
			//Instantiate random impact prefab from array
			Instantiate(dirtImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		if (collision.transform.tag == "Concrete")
		{
			//Instantiate random impact prefab from array
			Instantiate(concreteImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		if (collision.transform.tag == "ExplosiveBarrel")
		{
			//Toggle "explode" on explosive barrel object
			//collision.transform.gameObject.GetComponent
			//	<ExplosiveBarrelScript>().explode = true;
			//Destroy bullet object
			Destroy(gameObject);
		}

	}

    private void OnTriggerEnter(Collider other)
	{
		if (!destroyOnImpact)
		{
			StartCoroutine(DestroyTimer());
		}
		else
		{
			Destroy(gameObject);
		}

		if (other.gameObject.transform.tag == "Player")
		{
			Physics.IgnoreCollision(other, GetComponent<Collider>());
		}

		if (other.transform.gameObject.GetComponent<Target>())
		{
			Instantiate(bloodImpactPrefabs[Random.Range(0, bloodImpactPrefabs.Length)], transform.position, Quaternion.identity);
			other.transform.GetComponent<Target>().TakeDamage(damage);
		}	
	}


	private IEnumerator DestroyTimer()
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter()
	{
		//Wait for set amount of time
		yield return new WaitForSeconds(destroyAfter);
		//Destroy bullet object
		Destroy(gameObject);
	}
}