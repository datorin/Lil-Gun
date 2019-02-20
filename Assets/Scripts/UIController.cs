using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _filledHeart;
	[SerializeField] private GameObject _emptyHeart;
	[SerializeField] private GameObject _heartsPosition;
	[SerializeField] private GameObject _gun;
	[SerializeField] private GameObject _filledBullets;
	[SerializeField] private GameObject _emptyBullets;
	[SerializeField] private GameObject _bulletsPosition;
	[SerializeField] private GameObject _curePoints3;
	[SerializeField] private GameObject _curePoints2;
	[SerializeField] private GameObject _curePoints1;
	[SerializeField] private GameObject _curePoints0;
	[SerializeField] private GameObject _curePosition;

	private int _currentHealthPoints;
	private int _maxHealthPoints;
	
	private int _currentBullets;
	private int _maxBullets;

	private int _currentCurePoints;

	private GameObject[] _hearts;
	private GameObject[] _bullets;
	private GameObject _cure;
	
	// Use this for initialization
	void Start () {
		_maxHealthPoints = _player.GetComponent<PlayerController>().MaxHealthPoints;
		_hearts = new GameObject[_maxHealthPoints];

		for (var i = 0; i < _maxHealthPoints; i++)
		{
			var heart = Instantiate(_emptyHeart, _heartsPosition.transform.position, Quaternion.identity);
			heart.transform.SetParent(transform, true);
			heart.transform.localScale = new Vector3(0.3f, 0.3f, 1);
			heart.transform.localPosition = 
				new Vector2(_heartsPosition.transform.localPosition.x + (16.2f * i), _heartsPosition.transform.localPosition.y);
			_hearts[i] = heart;
		}
		
		_maxBullets = _gun.GetComponent<GunController>().Bullets;
		_bullets = new GameObject[_maxBullets];

		for (var i = 0; i < _maxBullets; i++)
		{
			var bullet = Instantiate(_emptyBullets, _bulletsPosition.transform.position, Quaternion.identity);
			bullet.transform.SetParent(transform, true);
			bullet.transform.localScale = new Vector3(0.3f, 0.3f, 1);
			bullet.transform.localPosition = 
				new Vector2(_bulletsPosition.transform.localPosition.x + (9.4f * i), _bulletsPosition.transform.localPosition.y);
			_bullets[i] = bullet;
		}

		_cure = Instantiate(_curePoints0, _curePosition.transform.position, Quaternion.identity);
		_cure.transform.SetParent(transform,true);
		_cure.transform.localScale = new Vector3(0.15f, 0.15f, 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		_currentHealthPoints = _player.GetComponent<PlayerController>().CurrentHealthPoints;

		for (var i = 0; i < _maxHealthPoints; i++)
		{
			if (i < _currentHealthPoints)
			{
				_hearts[i].GetComponent<Image>().sprite = _filledHeart.GetComponent<Image>().sprite;
			}
			else
			{
				_hearts[i].GetComponent<Image>().sprite = _emptyHeart.GetComponent<Image>().sprite;
			}
		}
		
		_currentBullets = _gun.GetComponent<GunController>().ActualBullets;

		for (var i = 0; i < _maxBullets; i++)
		{
			if (i < _currentBullets)
			{
				_bullets[i].GetComponent<Image>().sprite = _filledBullets.GetComponent<Image>().sprite;
			}
			else
			{
				_bullets[i].GetComponent<Image>().sprite = _emptyBullets.GetComponent<Image>().sprite;
			}
		}

		_currentCurePoints = _player.GetComponent<PlayerController>().CurrentCurePoints;

		switch (_currentCurePoints)
		{
			case 1:
				_cure.GetComponent<Image>().sprite = _curePoints1.GetComponent<Image>().sprite;
				break;
			case 2:
				_cure.GetComponent<Image>().sprite = _curePoints2.GetComponent<Image>().sprite;
				break;
			case 3:
				_cure.GetComponent<Image>().sprite = _curePoints3.GetComponent<Image>().sprite;
				break;
			default:
				_cure.GetComponent<Image>().sprite = _curePoints0.GetComponent<Image>().sprite;
				break;
		}
	}
}
