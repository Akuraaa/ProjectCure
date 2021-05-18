using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private int _currentHealth;
    [SerializeField] private int health;
    [SerializeField] public float speed;
    [SerializeField] private float damage;

    private Animator _anim;
    protected AudioSource _audio;

    [SerializeField] private AudioClip _zombieHitSound;
    protected bool _isDead = false;
    protected bool _isReceivingDamage = false;
    private bool isDead;
    protected float timeToDie;
    [SerializeField] protected float timeToIdle = 5;
    protected float currentIdleDuration = 0;

    [SerializeField] protected List<Transform> waypoints = new List<Transform>();
    protected float fireRate = 1.5f;
    public float viewAngle, radius;

    protected bool playerInSight;
    protected bool playerInRange;
    protected FPSController player;
    protected int currentWaypoint = 0;


    private void Start()
    {
        
    }

    private void Update()
    {
        if (playerInRange)
        {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) <viewAngle)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, radius, 1 << 8))
                {
                    Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        playerInSight = true;
        speed = 0;

        if (Vector3.Distance(player.transform.position, transform.position) < 9.9f)
        {
            playerInSight = false;
            Chase();
        }
        else
        {
            transform.LookAt(player.transform);
        }
    }

    private void Chase()
    {
        if(currentWaypoint > waypoints.Count -1)
        {
            Idle();
        }
        else
        {
            transform.LookAt(player.transform.position);
        }
    }

    private void Patrol()
    {
        currentWaypoint = 0;
        Transform target = waypoints[currentWaypoint].GetComponent<Transform>();
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < .5f)
        {
            if (currentWaypoint < waypoints.Count - 1)
            {
                currentWaypoint++;
            }
            else
            {
                waypoints.Reverse();
                currentWaypoint = 0;
            }
        }
        
    }

    private void Idle()
    {
        timeToIdle = Random.Range(0, timeToIdle);
        currentIdleDuration = 0;
    }

    public virtual void TakeDamage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
                _audio.PlayOneShot(_zombieHitSound);
            _anim.Play("ZombieHit", 0, 0);
        }

        if (health <= 0)
        {
            _isDead = true;
            _anim.Play("ZombieDeath");
            Destroy(gameObject, _anim.GetCurrentAnimatorClipInfo(0).LongLength);
        }
    }
}
