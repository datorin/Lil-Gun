using UnityEngine;

public class BasicEnemyController : MonoBehaviour, IEnemy
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _recoil;

    private int _actualHealth;

    private Vector2 _movementDirection;

    public Vector2 MovementDirection
    {
        get { return _movementDirection; }
    }

    private float _distance = 50;

    private readonly float _detection = 0.3f;
    
    // Use this for initialization
    void Start()
    {
        _actualHealth = _health;
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementDirection = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _distance, LayerMask.GetMask(Values.PlayerLayer))

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

        _rigidbody.position += _movementDirection * _speed * Time.deltaTime;
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