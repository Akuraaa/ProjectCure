using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Target
{
	private bool aggro;
	public float timeToAggro;

	private Vector3 Direction;
	public Transform player;
	public FPSController character;
	public Transform[] waypoints;
	private int waypointIndex = 0;

	private bool followTarget;
	private float followRange;

	public float attackRange;
	public float visionRange;
	private bool getRotation;

	int playerMask = 1 << 8;
	int wallMask = 1 << 10;

	public bool cooldown;
	public float cooldowntime = 1;
	private float _cooldowntime = 1;

	private Vector3 rotation;
	private Vector3 lookplayer;

	[SerializeField] AudioClip attackSound;

	public AnimationClip _zombieHit;
	
	private void Start()
	{
		_anim = GetComponent<Animator>();
		_cooldowntime = cooldowntime;
		timeToDie = 7f;
		cooldowntime = _zombieHit.length * 2;
		_cooldowntime = _zombieHit.length * 2;
	}


	private void Update()
	{
		Debug.Log(health);
		//target = new Vector3 (player.position.x,transform.position.y, player.position.z);
		if (!_isDead)
		{
			invulnerabilityTime -= Time.deltaTime;
			lookplayer = new Vector3(player.position.x, transform.position.y, player.position.z);
			followRange = Vector3.Distance(transform.position, player.position);

			if (followRange < attackRange && !cooldown)
			{
				speed = 0;
				_anim.SetTrigger("Attack");
				cooldown = true;
				//Attack();
			}
			else
            {
				speed = 2;
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

		if (aggro)
		{
			followTarget = true;
			_anim.SetBool("IsRunning", true);
			_anim.SetBool("IsWalking", false);
			transform.rotation = Quaternion.LookRotation(lookplayer - transform.position);
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
					_anim.SetBool("IsRunning", false);
				}

				else if (Physics.Raycast(transform.position, Direction, out hit, followRange, playerMask))
				{
					followTarget = true;
					_anim.SetBool("IsRunning", true);
					_anim.SetBool("IsWalking", false);
					transform.rotation = Quaternion.LookRotation(lookplayer - transform.position);
					transform.position += transform.forward * speed * Time.deltaTime;
					getRotation = false;
				}

				else
				{
					followTarget = false;
					_anim.SetBool("IsWalking", true);
					_anim.SetBool("IsRunning", false);
				}

			}
			else
			{
				followTarget = false;
				_anim.SetBool("IsWalking", true);
				_anim.SetBool("IsRunning", false);

			}

			if (!followTarget)
			{
				Waypoints();
				_anim.SetBool("IsWalking", true);
				_anim.SetBool("IsRunning", false);

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

	public void OnAnimatorMeleeAttack()
	{
		_audio.PlayOneShot(attackSound);
		cooldown = true;
		player.GetComponent<FPSController>().SendMessage("TakeDamage", damage);
	}
}
