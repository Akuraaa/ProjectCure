﻿using System.Collections;
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
    public Transform target;
    public AudioClip rangeAttack, meleeAttack;
    public int currentWaypoint = 0;

    public Rigidbody _rb;
    public bool playerInSight = false;
    public bool isRange;
    public float turnSpeed = .1f;
    public Vector3 direction;
    public Quaternion rotGoal;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
        transform.LookAt(target);
        transform.position += transform.forward * speed * Time.deltaTime;
        if (LineOfSight() == true)
        {
            if (isRange)
            {
                if (Vector3.Distance(transform.position, player.transform.position) > zombieRangeDist)
                {
                    _stateMachine.SetState<RangeAttackState>();
                }
                else
                    _stateMachine.SetState<ChaseState>();
            }
            else
            {
                if (Vector3.Distance(transform.position, player.transform.position) > zombieMeleeDist)
                {
                    _stateMachine.SetState<MeleeAttackState>();
                }
                else
                    _stateMachine.SetState<ChaseState>();
            }
        }
        _stateMachine.Update();
    }

    public bool LineOfSight()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, dirToPlayer, out hit, radius, 1 << 8))
            {
                playerInSight = hit.transform.gameObject.layer == 8;
            }
            else
            {
                return false;
            }
        }
        return playerInSight;
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
