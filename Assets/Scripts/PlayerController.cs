﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IInteractable
{
    public static PlayerController Instance;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _trigger;

    [SerializeField] private int _maxHealthPoints;
    private int _currentHealthPoints;
    
    private int _curePointsNeeded = 3;
    private int _currentCurePoints;

    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _fallGravity;
    [SerializeField] private float _normalGravity;

    private bool _jumpRequest;
    private bool _onGround;
    private float _halfHeight = 0.5f;
    private float _heightTolerance = 0.2f;

    [SerializeField] private int _airJumps;
    private int _actualAirJumps;

    private float _movement;
    private float _lastMovement;

    private void Awake()
    {
        Instance = this;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _onGround = false;
        _lastMovement = 1;
        _actualAirJumps = _airJumps;
        _currentHealthPoints = _maxHealthPoints;
    }

    // Use this for initialization
    void Update()
    {
        _animator.SetFloat("YVelocity", _rigidbody.velocity.y);
        
        if (_currentHealthPoints < _maxHealthPoints && _currentCurePoints == _curePointsNeeded)
        {
            _currentHealthPoints += 1;
            _currentCurePoints = 0;
        }

        if (_currentCurePoints > 3)
        {
            _currentCurePoints = 3;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            _animator.SetBool("isJumping", true);
            
            if (_onGround)
            {
                _jumpRequest = true;
                _onGround = false;
            }
            else if (_actualAirJumps > 0)
            {
                _jumpRequest = true;
                _actualAirJumps--;
            }
        }

        _movement = Input.GetAxis("Horizontal");

        if (Mathf.Abs(_movement) > 0)
        {
            _lastMovement = _movement;
            _animator.SetBool("isMoving", true);
            if (_movement < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (Math.Abs(_rigidbody.velocity.y) > 0.01f)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }

        if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && _gun != null)
        {
            _gun.GetComponent<GunController>().Shoot(Vector2.right * _lastMovement);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_jumpRequest)
        {
            _rigidbody.gravityScale = _normalGravity;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddForce(Vector3.up * _jumpVelocity, ForceMode2D.Impulse);
            _jumpRequest = false;
        }

        if (_rigidbody.velocity.y <= 0 && !_onGround)
        {
            _rigidbody.gravityScale = _fallGravity;
        }

        if (Math.Abs(_movement) > 0.01f)
        {
            //_rigidbody.AddForce(Vector2.right * _movement * _movementSpeed, ForceMode2D.Impulse);
            _rigidbody.position += Vector2.right * _movement * _movementSpeed * Time.deltaTime;
        }
    }

    public Vector2 GetCurrentRoom()
    {
        return RoomManager.GetRoom(transform.position);
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Values.GroundTag))
        {
            foreach (var point in other.contacts)
            {
                if (point.normal.y > 0.9f && Math.Abs(_rigidbody.velocity.y) < 0.01f)
                {
                    _onGround = true;
                    _animator.SetBool("isJumping", false);
                    _rigidbody.gravityScale = _normalGravity;
                    _actualAirJumps = _airJumps;
                }
            }
        }


        if (other.gameObject.CompareTag(Values.EnemyTag))
        {
            foreach (var point in other.contacts)
            {
                Debug.Log(point.normal);
                if (point.normal.y > 0.9f)
                {
                    _jumpRequest = true;
                    other.transform.GetComponent<IInteractable>().Hitted(2, Vector2.down);
                    if (_gun != null)
                    {
                        _gun.GetComponent<GunController>().Reload();
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _onGround = false;
    }

    public GameObject Gun
    {
        get { return _gun; }
        set { _gun = value; }
    }

    public void Hitted(int damage, Vector2 direction)
    {
        _currentHealthPoints -= damage;
        if (_currentHealthPoints <= 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    
    public int CurrentHealthPoints
    {
        get { return _currentHealthPoints; }
    }
    
    public int MaxHealthPoints
    {
        get { return _maxHealthPoints; }
    }
    
    public int CurrentCurePoints
    {
        get { return _currentCurePoints; }
        set { _currentCurePoints = value; }
    }
    
}