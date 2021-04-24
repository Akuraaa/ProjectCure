using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIA : MonoBehaviour
{
    public float speed;
    public float radius;
    public int hp;

    public float minIdleTime = 3;
    public float maxIdleTime = 5;
    public float fireRate = 1.5f;
    public float viewAngle = 45;
    public float distanceToAttack;
    public LayerMask _lm;

    public FpsControllerLPFP player;

    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypointTarget = 0;

    public bool playerInRange = false;
    public bool playerInSight = false;
    public bool view;
    public bool die;

    public Animator _anim;
    public SphereCollider visionRange;
    public AudioSource audioSource;
    public AudioClip receiveDamage, attackSound;

    public StateMachine sm;
    public EnemyDecisionTree zombieTree;

    void Awake()
    {
        sm = new StateMachine();
        zombieTree = new EnemyDecisionTree(this);
        visionRange = GetComponent<SphereCollider>();
        _anim = GetComponent<Animator>();
        player = FindObjectOfType<FpsControllerLPFP>();
        audioSource = GetComponent<AudioSource>();
        radius = GetComponent<SphereCollider>().radius;
        zombieTree.SetNodes();
    }

    private void Start()
    {
        zombieTree._init.Execute();
    }

    void Update()
    {
        if (!die)
        {
            sm.Update();
            if (view)
            {
                if (GetComponent<SphereCollider>().radius < radius * 2)
                {
                    GetComponent<SphereCollider>().radius *= 2;
                    viewAngle *= 1.5f;
                }
            }
            else
            {
                if (GetComponent<SphereCollider>().radius == radius * 2)
                {
                    GetComponent<SphereCollider>().radius /= 2;
                    viewAngle /= 1.5f;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<FpsControllerLPFP>() && !die)
        {
            playerInRange = true;
            player = other.GetComponent<FpsControllerLPFP>();
            LineOfSight();
            zombieTree._init.Execute();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FpsControllerLPFP>())
        {
            playerInRange = false;
            playerInSight = false;
            zombieTree._init.Execute();
        }
    }

    bool LineOfSight()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, dirToPlayer, out hit, visionRange.radius, _lm))
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

    public void OnAnimatorAttack()
    {
        //audioSource.PlayOneShot(attackSound);
    }

    public void ActionAttack()
    {
        sm.SetState<ZombieAttackState>();
    }

    public void ActionSeek()
    {
        sm.SetState<ZombieSeekState>();
    }

    public void ActionPatrol()
    {
        sm.SetState<ZombiePatrolState>();
    }

    public void ActionIdle()
    {
        sm.SetState<ZombieIdleState>();
    }

    public void ActionDie()
    {
        sm.SetState<ZombieDieState>();
    }

    public bool QuestionAttackRange()
    {
        view = true;
        return Vector3.Distance(transform.position, player.transform.position) < distanceToAttack;
    }

    public bool QuestionPlayerOnSight()
    {
        return LineOfSight();
    }

    public bool QuestionHaveLife()
    {
        return die;
    }
}
