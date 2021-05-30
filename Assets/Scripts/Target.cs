using UnityEngine;

public class Target : MonoBehaviour
{
    public int health;
    public float damage;
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
             	_anim.Play("ZombieHit", 0, 0);
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
}
