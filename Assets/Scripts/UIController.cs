using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _filledHeart;
	[SerializeField] private GameObject _emptyHeart;
	[SerializeField] private GameObject _position;

	private int _currentHealthPoints;
	private int _maxHealthPoints;

	private GameObject[] _hearts;

	// Use this for initialization
	void Start () {
		_maxHealthPoints = _player.GetComponent<PlayerController>().MaxHealthPoints;
		_hearts = new GameObject[_maxHealthPoints];

		for (var i = 0; i < _maxHealthPoints; i++)
		{
			var heart = Instantiate(_emptyHeart, _position.transform.position, Quaternion.identity);
			heart.transform.SetParent(transform, true);
			heart.transform.localScale = new Vector3(0.3f, 0.3f, 1);
			heart.transform.localPosition = 
				new Vector2(_position.transform.localPosition.x + (16.2f * i), _position.transform.localPosition.y);
			_hearts[i] = heart;
		}
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
	}
}
