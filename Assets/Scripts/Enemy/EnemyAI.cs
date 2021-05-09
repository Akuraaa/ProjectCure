using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float minIdleTime = 3;
    public float maxIdleTime = 5;
    public float fireRate = 1.5f;
    public float viewAngle = 45;
    public float distanceToShoot;
    public LayerMask _lm;
    public bool view;
    public float radius;

    public float speed;

    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public FPSController player;

    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypointTarget = 0;

    public bool playerInRange = false;
    public bool playerInSight = false;

    public Animator animator;

    public StateMachine sm;
    public EnemyDecisionTree enemyTree;
    public AudioSource audioSource;
    public AudioClip attackSound;

    void Awake()
    {
        sm = new StateMachine();
        sm.AddState(new EnemyPatrolState(sm, this));
        sm.AddState(new EnemyIdleState(sm, this));
        sm.AddState(new EnemyShootState(sm, this));
        sm.AddState(new EnemySeekState(sm, this));
        animator = GetComponent<Animator>();
        player = FindObjectOfType<FPSController>();
        enemyTree = new EnemyDecisionTree(this);
        enemyTree.SetNodes();
    }

    private void Start()
    {
        enemyTree._init.Execute();
    }

    void Update()
    {
        sm.Update();
        if (view)
        {
                radius *= 2;
                viewAngle *= 1.5f;
        }
        else
        {
                radius /= 2;
                viewAngle /= 1.5f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<FPSController>())
        {
            playerInRange = true;
            player = other.GetComponent<FPSController>();
            LineOfSight();
            enemyTree._init.Execute();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FPSController>())
        {
            playerInRange = false;
            playerInSight = false;
            enemyTree._init.Execute();
        }
    }

    public void OnAnimatorRangeAttack()
    {
        audioSource.PlayOneShot(attackSound);
        BulletScript bullet = Object.Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation).GetComponent<BulletScript>();
        bullet.transform.up = bulletSpawn.forward;
    }

    bool LineOfSight()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, dirToPlayer, out hit, radius, _lm))
            {
                playerInSight = hit.transform.gameObject.layer == 8;
            }
            else
            {
                view = false;
                return false;
            }

        }
        return playerInSight;
    }

    public void ActionShoot()
    {
        sm.SetState<EnemyShootState>();
    }

    public void ActionSeek()
    {
        sm.SetState<EnemySeekState>();
    }

    public void ActionPatrol()
    {
        sm.SetState<EnemyPatrolState>();
    }

    public bool QuestionDistanceShoot()
    {
        view = true;
        return Vector3.Distance(transform.position, player.transform.position) < distanceToShoot;
    }

    public bool QuestionIsPlayerOnSight()
    {
        return LineOfSight();
    }
}
