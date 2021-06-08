using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int health;
    public float damage;
    public float speed;
    private int currentHealth;
    public Animator _anim;
    public AudioSource _audio;
    public AudioClip _receiveDamage;
    public bool _isDead = false;
    protected bool _isReceivingDamage = false;
	private bool isDead;
	public float timeToDie;

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
            _isDead = true;
            Die();
         }
     }
    
     public virtual void Die()
     {
        _anim.Play("ZombieDeath");
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
