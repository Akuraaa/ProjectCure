using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Target
{
    public Target parentScript;

    public float speed = 4f;

    private bool aggro;
    public float timeToAggro;

    private Vector3 Direction;
    public Transform player;
    public Transform[] waypoints;
    private int waypointIndex = 0;

    private bool followTarget;
    private float followRange;
    private float _followRange;

    public float attackRange;
    public float visionRange;
    private bool getRotation;

    int playerMask = 1 << 8;
    int wallMask = 1 << 11;

    private bool stun;
    private float stunTime = 0.5f;

    private bool cooldown;
    public float cooldowntime = 1;
    private float _cooldowntime = 1;

    private Vector3 rotation;

    private Rigidbody rigid;

    private float invulnerabilityTime;
    private Vector3 direction;
    public Transform orbSpawnpoint;
    public GameObject axePrefab;
    [SerializeField] AudioClip attackSound;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        parentScript = GetComponent<Target>();
        _cooldowntime = cooldowntime;
        rigid = GetComponent<Rigidbody>();
        timeToDie = 5f;
    }


    private void Update()
    {
        //target = new Vector3 (player.position.x,transform.position.y, player.position.z);
        if (!parentScript._isDead)
        {
            invulnerabilityTime -= Time.deltaTime;
            followRange = Vector3.Distance(transform.position, player.position);

            if (followRange < attackRange && !cooldown)
            {
                _anim.SetBool("IsAttacking", true);
                Cooldown();
                //Attack();
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
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyDeath");
        }
    }

    void Move()
    {
        if (stun)
        {
            stunTime -= Time.deltaTime;

            if (stunTime <= 0)
            {

                stunTime = 0.5f;
                stun = false;
            }
        }

        if (aggro && !stun)
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
            if (followRange < visionRange && !stun)
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
        _cooldowntime -= Time.deltaTime;
        if (_cooldowntime <= 0)
        {

            _anim.SetBool("IsAttacking", false);
            cooldown = false;
            _cooldowntime = cooldowntime;
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
                stun = true;
                invulnerabilityTime = 1f;
            }

            if (health <= 0)
            {
                _isDead = true;
                rigid.useGravity = true;
            }

        }
    }

    public void OnAnimatorRangeAttack()
    {
        _audio.PlayOneShot(attackSound);
        AxeScript axe = Object.Instantiate(axePrefab, orbSpawnpoint.position, orbSpawnpoint.rotation).GetComponent<AxeScript>();
        axePrefab.GetComponent<AxeScript>().VectorToPlayer(orbSpawnpoint.transform.position, player.transform.position);
    }
}
