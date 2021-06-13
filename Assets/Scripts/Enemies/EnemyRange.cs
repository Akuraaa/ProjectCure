using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Target
{
    private bool aggro;
    public float timeToAggro;

    private Vector3 Direction;
    public Transform player;
    public Transform[] waypoints;
    private int waypointIndex = 0;

    private bool followTarget;
    private float followRange;

    public float attackRange;
    public float visionRange;
    private bool getRotation;

    int playerMask = 1 << 8;
    int wallMask = 1 << 11;

    private bool cooldown;
    public float cooldownTime = 1;
    private float _cooldownTime = 1;

    private Vector3 rotation;

    public Transform axeSP;
    public GameObject axePrefab;
    [SerializeField] AudioClip attackSound;

    private float invulnerabilityTime = 1;
    private void Awake()
    {
        cooldown = false;
        this.enabled = true;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _cooldownTime = cooldownTime;
    }


    private void Update()
    {
        Debug.Log("La vida del zombie es " + health);
        if (!_isDead)
        {
            invulnerabilityTime -= Time.deltaTime;
            followRange = Vector3.Distance(transform.position, player.position);

            if (followRange < attackRange && !cooldown)
            {
                speed = 0;
                _anim.SetTrigger("Attack");
                cooldown = true;
            }
            else
            {
                speed = .5f;
            }

            if (cooldown)
            {
                Cooldown();
            }

            if (!cooldown)
            {
                Move();
            }
        }
    }

    void Move()
    {

        if (aggro)
        {
            followTarget = true;
            _anim.SetBool("IsWalking", false);
            transform.rotation = Quaternion.LookRotation(player.position - transform.position);
            transform.position += transform.forward * speed * Time.deltaTime;
            getRotation = false;
            timeToAggro -= Time.deltaTime;
            if (timeToAggro <= 0)
            {
                timeToAggro = 10f;
                aggro = false;
            }
        }

        else
        {
            if (followRange < visionRange)
            {
                Direction = player.position - transform.position;
                Direction = Direction.normalized;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, Direction, out hit, followRange, wallMask))
                {
                    followTarget = false;
                    _anim.SetBool("IsWalking", true);
                }

                else if (Physics.Raycast(transform.position, Direction, out hit, followRange, playerMask))
                {
                    followTarget = true;
                    _anim.SetBool("IsWalking", false);
                    transform.rotation = Quaternion.LookRotation(player.position - transform.position);
                    transform.position += transform.forward * speed * Time.deltaTime;
                    getRotation = false;
                }

                else
                {
                    followTarget = false;
                    _anim.SetBool("IsWalking", true);
                }

            }

            else

            {
                followTarget = false;
                _anim.SetBool("IsWalking", true);

            }

            if (!followTarget)
            {
                Waypoints();
                _anim.SetBool("IsWalking", true);

                rotation = new Vector3(waypoints[waypointIndex].position.x, transform.position.y, waypoints[waypointIndex].position.z);

                transform.rotation = Quaternion.LookRotation(rotation - transform.position);

                if (!getRotation)
                {
                    transform.rotation = Quaternion.LookRotation(rotation - transform.position);
                    getRotation = true;
                }

                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }

    }

    void Cooldown()
    {
        _cooldownTime -= Time.deltaTime;
        if (_cooldownTime <= 0)
        {

            _anim.SetBool("IsAttacking", false);
            cooldown = false;
            _cooldownTime = cooldownTime;
            //speed = 0.5f;
        }
    }

    public void Waypoints()
    {
        if (Vector3.Distance(transform.position, rotation) <= 0.1f)
        {
            waypointIndex++;

            if (getRotation)
            {
                getRotation = false;
            }

            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
            rotation = new Vector3(waypoints[waypointIndex].position.x, transform.position.y, waypoints[waypointIndex].position.z);
            transform.rotation = Quaternion.LookRotation(rotation - transform.position);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (invulnerabilityTime <= 0)
        {
            if (health > 0)
            {
                health -= damage;
                aggro = true;
                timeToAggro = 10f;
                _audio.PlayOneShot(_receiveDamage);
                _anim.SetTrigger("ZombieHit");
                invulnerabilityTime = 1;
            }

            if (health <= 0)
            {
                _anim.SetBool("IsDead", true);
                _isDead = true;
            }
        }
    }

    public void OnAnimatorRangeAttack()
    {
        _audio.PlayOneShot(attackSound);
        AxeScript axe = Instantiate(axePrefab, axeSP.position, axeSP.rotation).GetComponent<AxeScript>();
        axe.transform.LookAt(player);
    }
}
