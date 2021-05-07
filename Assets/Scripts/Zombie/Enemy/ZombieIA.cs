using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIA : MonoBehaviour
{
    public float speed;
    public float radius;
    public int hp = 100;

    public float minIdleTime = 3;
    public float maxIdleTime = 5;
    public float fireRate = 1.5f;
    public float viewAngle = 45;
    public float distanceToAttack;
    public LayerMask _lm;

    public PlayerController player;

    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypointTarget = 0;

    private bool playerInRange = false;
    private bool playerInSight = false;
    private bool view;
    private bool die;
    private bool haveLife;

    public Animator _anim;
    private SphereCollider visionRange;
    private AudioSource audioSource;
    public AudioClip receiveDamage, attackSound;

    public Material zombieMat;
    public float bloodAmount;

    public StateMachine sm;
    public EnemyDecisionTree zombieTree;

    void Awake()
    {
        sm = new StateMachine();
        sm.AddState(new ZombieAttackState(sm, this));
        sm.AddState(new ZombieDieState(sm, this));
        //sm.AddState(new ZombieIdleState(sm, this));
        sm.AddState(new ZombieSeekState(sm, this));
        sm.AddState(new ZombiePatrolState(sm, this));
        visionRange = GetComponent<SphereCollider>();
        _anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        zombieTree = new EnemyDecisionTree(this);
        zombieTree.SetNodes();
        die = false;
        
    }

    private void Start()
    {
        zombieTree._init.Execute();
        radius = GetComponent<SphereCollider>().radius;
        GetComponent<SphereCollider>().enabled = false;
    }

    void Update()
    {
        bloodAmount = Mathf.Lerp(0, 1, hp);
        zombieMat.SetFloat("BloodAmount", bloodAmount);
        if (!die)
        {
            haveLife = true;
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
        else
        {
            sm.SetState<ZombieDieState>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() && !die)
        {
            playerInRange = true;
            player = other.GetComponent<PlayerController>();
            LineOfSight();
            //zombieTree._init.Execute();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
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


    public void ReceiveDamage(int damage)
    {
        if (hp >= 0)
        {
            hp -= damage;
            //audioSource.playOneShot();
            _anim.Play("Zombie_ReceiveDamage", 0, 0);
        }
        else
        {
            die = true;
        }

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

    //public void ActionIdle()
    //{
    //    sm.SetState<ZombieIdleState>();
    //}

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
        if (hp <= 0)
        {
            die = true;
        }
        return haveLife;
    }
}
