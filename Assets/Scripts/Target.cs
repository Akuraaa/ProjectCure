using UnityEngine;

public class Target : MonoBehaviour
{
    public int health;
    private int currentHealth;
    protected Animator _anim;
    public AudioSource _audio;
    public AudioClip _receiveDamage;
    public bool _isDead = false;
    protected bool _isReceivingDamage = false;
	private bool isDead;
	public float timeToDie;

    private void Awake()
    {
        currentHealth = health;
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
				_anim.Play("ReceiveDamage", 0, 0);
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
		if (_anim != null && isDead == false)
		{
			_anim.SetTrigger("Die");
			isDead = true; 
		}
        Destroy(gameObject, timeToDie);
    }
}
