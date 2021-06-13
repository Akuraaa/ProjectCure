using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public float rotationSpeed;
    public float _speed;
    public float damage;
    private AudioSource _audio;
    [SerializeField] private GameObject renderAxe;
    [SerializeField] private AudioClip axeSound;

    [Range(5, 100)]
    [Tooltip("After how long time should the bullet prefab be destroyed?")]
    public float destroyAfter;
    [Tooltip("If enabled the bullet destroys on impact")]
    public bool destroyOnImpact = false;
    [Tooltip("Minimum time after impact that the bullet is destroyed")]
    public float minDestroyTime;
    [Tooltip("Maximum time after impact that the bullet is destroyed")]
    public float maxDestroyTime;


    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audio.PlayOneShot(axeSound);
    }
    void Update()
    {
        renderAxe.transform.Rotate(rotationSpeed, 0, 0);
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStats>().SendMessage("TakeDamage", damage);
            Destroy(gameObject);
        }
        else
        {
            if (!destroyOnImpact)
            {
                StartCoroutine(DestroyTimer());
            }
            else
            {
                Destroy(gameObject);
            }

        }
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
    }

    private IEnumerator DestroyTimer()
    {
        //Wait random time based on min and max values
        yield return new WaitForSeconds
            (Random.Range(minDestroyTime, maxDestroyTime));
        //Destroy bullet object
        Destroy(gameObject);
    }
}
