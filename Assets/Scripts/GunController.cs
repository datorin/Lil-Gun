using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	private Rigidbody2D _rigidbody;
	
	[SerializeField] private GameObject _bullet;
	[SerializeField] private int _damage;
	[SerializeField] private int _bullets;
	[SerializeField] private float _cooldown;
	[SerializeField] private float _bulletSpeed;
	[SerializeField] private float _recoil;

	[SerializeField] private float _gunLaunchForce;
	[SerializeField] private float _gunLaunchTorque;
	[SerializeField] private int _gunLaunchDamage;
	[SerializeField] private float _gunGravity;
	[SerializeField] private float _waitTriggerTime;

	private int _actualBullets;
	private float _actualCooldown;
	private Vector2 _direction;

	// Use this for initialization
	void Awake ()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		
		_actualBullets = _bullets;
		_actualCooldown = _cooldown;
	}
	
	// Update is called once per frame
	void Update ()
	{		
		if(_actualCooldown < 0) return;
		_actualCooldown -= Time.deltaTime;
	}

	public void Shoot(Vector2 direction)
	{
		_direction = direction.normalized;
		
		if (_actualBullets <= 0)
		{
			transform.parent.GetComponent<Animator>().SetBool("hasGun",false);
			transform.parent.GetComponent<PlayerController>().Gun = null;
			transform.parent = null;
			
			var _rigidbody = gameObject.AddComponent<Rigidbody2D>();
			_rigidbody.gravityScale = 0;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
			_rigidbody.AddForce(_direction * _gunLaunchForce, ForceMode2D.Impulse);
			_rigidbody.AddTorque(_direction.x * _gunLaunchTorque * -1);
			GetComponent<SpriteRenderer>().enabled = true;
			StartCoroutine(waitAndTrigger(_waitTriggerTime, _rigidbody));
		} 
		else if (_actualCooldown <= 0)
		{
			var bullet = Instantiate(_bullet, transform.position + (Vector3) _direction * 1, 
					Quaternion.AngleAxis(Functions.CalculateAngle(_direction),Vector3.forward));
			bullet.GetComponent<Rigidbody2D>().AddForce(_direction * _bulletSpeed, ForceMode2D.Impulse);
			bullet.GetComponent<BulletController>().Direction = _direction;
			bullet.GetComponent<BulletController>().Damage = _damage;
		
			Recoil(direction);
		
			_actualCooldown = _cooldown;
			_actualBullets--;	
		}
	}

	private void Recoil(Vector2 direction)
	{
		direction = direction * new Vector2(-1, -1);
		transform.parent.GetComponent<Rigidbody2D>().AddForce(direction * _recoil, ForceMode2D.Impulse);
	}

	public void Reload()
	{
		_actualBullets = _bullets;
	}

	private IEnumerator waitAndTrigger(float waitTime, Rigidbody2D _rigidbody)
	{
		yield return new WaitForSeconds(waitTime);
		transform.Find("GunTrigger").gameObject.SetActive(true);
		_rigidbody.gravityScale = _gunGravity;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag(Values.EnemyTag))
		{
			other.transform.GetComponent<IEnemy>().Hitted(_gunLaunchDamage, _direction);
		}
	}
}
