using System.Threading;
using UnityEngine;

public class CasterEnemyController : MonoBehaviour, IEnemy
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _emitter;
    [SerializeField] private float _cooldown;
    private float _actualCooldown;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _recoil;

    private int _actualHealth;

    private Vector2 _movementDirection;
    private float _distance = 50;
    private readonly float _detection = 0.3f;

    private float _walkTime;
    [SerializeField] private float _maxWalkTime;
    [SerializeField] private float _minWalkTime;
    private float _idleTime;
    [SerializeField] private float _maxIdleTime;
    [SerializeField] private float _minIdleTime;
    private bool _isIdle;
    private bool _isWalk;
    
    // Use this for initialization
    void Start()
    {
        _actualHealth = _health;
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementDirection = Vector2.left;
        _idleTime = Random.Range(_minIdleTime, _maxIdleTime);
        _actualCooldown = _cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        _actualCooldown -= Time.deltaTime;
        
        var hit = Physics2D.Raycast(_emitter.transform.position, _movementDirection, _distance);
        if (hit.transform.CompareTag(Values.PlayerTag) && _actualCooldown < 0)
        {
            var bullet = Instantiate(_bullet, transform.position, Quaternion.AngleAxis(Functions.CalculateAngle(_movementDirection),Vector3.forward));
            bullet.GetComponent<Rigidbody2D>().AddForce(_movementDirection * 5, ForceMode2D.Impulse);
            bullet.GetComponent<BulletController>().Direction = _movementDirection;
            bullet.GetComponent<BulletController>().Damage = _damage;
            
            _actualCooldown = _cooldown;
        }

        bool isGrounded = Physics2D.Linecast(transform.TransformPoint(Vector2.right * _detection), 
                                             transform.TransformPoint(Vector2.right *_detection) + Vector3.down * 0.75f,
                                             LayerMask.GetMask(Values.GroundLayer));
        
        bool isBlocked = Physics2D.Linecast(transform.TransformPoint(Vector2.right * _detection), 
                                            transform.TransformPoint(Vector2.right *_detection) + (Vector3)_movementDirection * 0.2f,
                                            LayerMask.GetMask(Values.GroundLayer));

        if (!isGrounded || isBlocked)
        {
            transform.Rotate(0,180,0);
            _movementDirection *= Vector2.left;
        }

        if (_walkTime > 0 && _idleTime <= 0)
        {
            _isWalk = true;
            _isIdle = false;
        }else if (_walkTime <= 0 && _idleTime > 0)
        {
            _isIdle = true;
            _isWalk = false;
        }

        if (_isWalk)
        {
            _rigidbody.position += _movementDirection * _speed * Time.deltaTime;
            _walkTime -= Time.deltaTime;
            if(_idleTime > 0) return;
            _idleTime = Random.Range(_minIdleTime, _maxIdleTime);
            transform.Rotate(0,180,0);
            _movementDirection *= Vector2.left;
        }

        if (_isIdle)
        {
            _idleTime -= Time.deltaTime;
            if(_walkTime > 0) return;
            _walkTime = Random.Range(_minWalkTime, _maxWalkTime);
        }
    }

    public void Hitted(int damage, Vector2 direction)
    {
        _actualHealth -= damage;
        Push(direction);
        if (_actualHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Push(Vector2 direction)
    {
        _rigidbody.AddForce(direction * _recoil, ForceMode2D.Impulse);
    }
}