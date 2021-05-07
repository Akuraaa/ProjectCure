using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Target
{

	public Target parentScript;
	public float speed = 4f;

	private bool aggro;
	public float timeToAggro;

	private Vector3 Direction;
	public Transform player;
	public PlayerController character;
	public Transform[] waypoints;
	private int waypointIndex = 0;

	private bool followTarget;
	private float followRange;
	private float _followRange;

	private float attackRange;


	private bool getRotation;

	int playerMask = 1 << 8;
	int wallMask = 1 << 10;

	public bool stun;
	private float stunTime = 1;

	public bool cooldown;
	public float cooldowntime = 1;
	private float _cooldowntime = 1;

	private Vector3 rotation;

	private Rigidbody rigid;
	private MeshCollider mesh;
	private Vector3 lookplayer;

	private void Start()
	{
		parentScript = GetComponent<Target>();
		_cooldowntime = cooldowntime;
		rigid = GetComponent<Rigidbody>();
		mesh = GetComponent<MeshCollider>();
		timeToDie = 7f;
	}


	private void Update()
	{
		//target = new Vector3 (player.position.x,transform.position.y, player.position.z);
		if (!parentScript._isDead)
		{
			lookplayer = new Vector3(player.position.x, transform.position.y, player.position.z);
			followRange = Vector3.Distance(transform.position, player.position);

			if (followRange < 5 && !cooldown)
			{
				_anim.SetBool("IsAttacking", true);
				//Attack();
			}

			if (stun)
			{
				Stun();
			}
			if (cooldown)
			{
				Cooldown();
			}

			if (!cooldown && !stun)
			{
				Move();
			}
		}
		else
		{
			gameObject.layer = LayerMask.NameToLayer("EnemyDeath");
		}
	}

	void Stun()
	{
		stunTime -= Time.deltaTime;
		if (stunTime <= 0)
		{
			_anim.SetBool("IsWalking", true);
			stunTime = 1;
			stun = false;
		}
	}


	void Move()
	{


		if (aggro)
		{
			followTarget = true;
			_anim.SetBool("IsRunning", true);
			_anim.SetBool("IsWalking", false);


			//player.position
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
			if (followRange < 20)
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
					Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
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
			_anim.SetBool("IsAttacking", false);
			_anim.SetBool("IsWalking", true);
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
			//waypoints[waypointIndex].position
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
			_anim.Play("ReceiveDamage", 0, 0);
			stun = true;

		}

		if (health <= 0)
		{
			_isDead = true;
			Die();
		}

	}


	public void OnAnimatorAttack()
	{
		stun = true;
		cooldown = true;
		//_anim.SetBool("IsCooldown", true);

		//player.SendMessage("TakeDamage", damage);
		player.GetComponent<PlayerController>().SendMessage("TakeDamage", damage);
	}
}
