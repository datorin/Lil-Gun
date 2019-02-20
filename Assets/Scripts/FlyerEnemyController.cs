using System.Threading;
using UnityEngine;

public class FlyerEnemyController : MonoBehaviour, IInteractable
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _recoil;
    [SerializeField] private float _cooldown;
    [SerializeField] private GameObject _emitter;
    [SerializeField] private GameObject _bullet;

    private int _actualHealth;
    private float _actualCooldown;

    private Vector2 _movementDirection;
    private float _distance = 5;
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
        _actualCooldown -= Time.deltaTime;
        
        var hit = Physics2D.Raycast(_emitter.transform.position, Vector2.down, _distance);
        if (hit.transform.CompareTag(Values.PlayerTag) && _actualCooldown < 0)
        {
            var bullet = Instantiate(_bullet, transform.position, Quaternion.AngleAxis(Functions.CalculateAngle(Vector2.down),Vector3.forward));
            bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 5, ForceMode2D.Impulse);
            bullet.GetComponent<BulletController>().Direction = _movementDirection;
            bullet.GetComponent<BulletController>().Damage = _damage;
            
            _actualCooldown = _cooldown;
        }

        bool isGrounded = Physics2D.Linecast(transform.TransformPoint(Vector2.right * _detection), 
                                             transform.TransformPoint(Vector2.right *_detection) + Vector3.down * 3.75f,
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
            PlayerController.Instance.CurrentCurePoints += 1;
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
                Debug.Log(point.normal);
                
                if (point.normal.y < 0.1f)
                {
                    other.collider.GetComponent<IInteractable>().Hitted(_damage, _movementDirection);
                }
            }
        }
    }
}