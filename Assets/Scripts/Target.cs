using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int health;
    public float damage;
    public float speed;
    public Animator _anim;
    public AudioSource _audio;
    public AudioClip _receiveDamage;
    public bool _isDead = false;
    protected bool _isReceivingDamage = false;
	public float timeToDie;
    public float invulnerabilityTime = 1;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }


    public virtual void TakeDamage(int amount)
    {
         if (health > 0)
         {
             health -= amount;
             if(_audio != null)
             {
             	_audio.PlayOneShot(_receiveDamage);
             }
             if(_anim != null)
             {
                StartCoroutine(StunCorroutine());
             }
         }
    
         if (health <= 0)
         {
            Die();  
         }
    }

    void Die()
    {
        _anim.SetBool("IsDead", true);
        _isDead = true;
        Destroy(gameObject, timeToDie);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>())
        {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerStats>().hitPlayer = true;
        }
    }

    IEnumerator StunCorroutine()
    {
        speed = 0;
        _anim.Play("ZombieHit", 0, 0);
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorClipInfo(0).Length);
        speed = .5f;
    }
}
