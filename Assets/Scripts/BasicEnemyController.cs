using System.Threading;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour, IInteractable
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _chaceSpeed;
    private float _speed;
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
    private bool _isPlayer;

    private bool _isHitted;
    
    // Use this for initialization
    void Start()
    {
        _actualHealth = _health;
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementDirection = Vector2.left;
        _speed = _normalSpeed;
        _idleTime = Random.Range(_minIdleTime, _maxIdleTime);
    }

    // Update is called once per frame
    void Update()
    {
        var hit = Physics2D.Raycast(transform.position, _movementDirection, _distance);
        if (hit.transform.CompareTag(Values.PlayerTag))
        {
            _speed = _chaceSpeed;
            _isPlayer = true;
        }
        else
        {
            _isPlayer = false;
        }

        bool isGrounded = Physics2D.Linecast(transform.TransformPoint(Vector2.right * _detection), 
                                             transform.TransformPoint(Vector2.right *_detection) + Vector3.down * 1f,
                                             LayerMask.GetMask(Values.GroundLayer));
        
        Debug.DrawLine(transform.TransformPoint(Vector2.right * _detection), 
            transform.TransformPoint(Vector2.right *_detection) + Vector3.down * 1f);
        
        bool isBlocked = Physics2D.Linecast(transform.TransformPoint(Vector2.right * _detection), 
                                            transform.TransformPoint(Vector2.right *_detection) + (Vector3)_movementDirection * 0.2f,
                                            LayerMask.GetMask(Values.GroundLayer));

        if (!isGrounded || isBlocked)
        {
            transform.Rotate(0,180,0);
            _movementDirection *= Vector2.left;
            _speed = _normalSpeed;
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
            _speed = _normalSpeed;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(Values.PlayerTag))
        {
            foreach (var point in other.contacts)
            {
                
                if (point.normal.y > -0.1f && _isHitted == false)
                {
                    _isHitted = true;
                    
                    other.collider.GetComponent<IInteractable>().Hitted(_damage, _movementDirection);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag(Values.PlayerTag))
        {
            _isHitted = false;
        }
    }
}