using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBase : Target
{
    public StateMachine _stateMachine;

    public List<Transform> waypoints = new List<Transform>();
    public float speed;

    public float minIdleTime = 3, maxIdleTime = 5;
    public float fireRate = 1.5f, viewAngle = 45;
    public float radius = 8;
    public float zombieRangeDist, zombieMeleeDist;

    public Transform axeSpawnPoint;
    public GameObject axePrefab;

    public FPSController player;
    public Transform playerPosDetection;

    public AudioClip rangeAttack, meleeAttack;
    public int currentWaypoint = 0;

    public bool playerInSight = false, playerInRange = false;
    public bool isRange;

    private void Awake()
    {
        player = FindObjectOfType<FPSController>();
        playerPosDetection = player.transform;
        _anim = GetComponent<Animator>();
        _stateMachine = new StateMachine();
        _stateMachine.AddState(new PatrolState(_stateMachine, this));
        _stateMachine.AddState(new IdleState(_stateMachine, this));
        _stateMachine.AddState(new ChaseState(_stateMachine, this));
        _stateMachine.AddState(new RangeAttackState(_stateMachine, this));
        _stateMachine.AddState(new MeleeAttackState(_stateMachine, this));
    }

    private void Start()
    {
        _stateMachine.SetState<PatrolState>();
    }

    private void Update()
    {
        _stateMachine.Update();
        if (Vector3.Distance(player.transform.position, transform.position) > radius)
            playerInRange = true;
        else
            playerInRange = false;

        if (playerInRange)
        {
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, radius, 1 << 8))
                {
                   if (isRange)
                       _stateMachine.SetState<RangeAttackState>();
                   else
                       _stateMachine.SetState<MeleeAttackState>();
                }
            }
        }
    }

    public void OnRangeAttack()
    {
        if (!isRange)
        {
            return;
        }
        else
        {
            _audio.PlayOneShot(rangeAttack);
            GameObject axe = Object.Instantiate(axePrefab);
            axe.transform.position = axeSpawnPoint.position;
            axe.transform.up = axeSpawnPoint.forward;
        }
    }

    public void OnMeleeAttack()
    {
        if (isRange)
        {
            return;
        }
        else
        {
            _audio.PlayOneShot(meleeAttack);
            player.GetComponent<FPSController>().SendMessage("TakeDamage", damage);
        }
    }
}
