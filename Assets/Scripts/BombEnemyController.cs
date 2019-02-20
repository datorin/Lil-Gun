using System.Collections;
using System.Threading;
using UnityEngine;

public class BombEnemyController : MonoBehaviour, IInteractable
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _recoil;

    private int _actualHealth;

    private Vector2 _movementDirection;
    private readonly float _detection = 0.3f;

    private GameObject _player;
    private bool _isPlayer;
    
    // Use this for initialization
    void Start()
    {
        _actualHealth = _health;
        _rigidbody = GetComponent<Rigidbody2D>();
        _movementDirection = Vector2.left;
        _player = PlayerController.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (_player.transform.position.x > transform.position.x && _movementDirection == Vector2.left ||
            _player.transform.position.x < transform.position.x && _movementDirection == Vector2.right)
        {
            transform.Rotate(0,180,0);
            _movementDirection *= Vector2.left;
        }

        if (PlayerController.Instance.GetCurrentRoom().Equals(RoomManager.GetRoom(transform.position)))
        {
            _rigidbody.position += _movementDirection * _speed * Time.deltaTime;   
        }
    }

    public void Hitted(int damage, Vector2 direction)
    {
        _actualHealth -= damage;
        Push(direction);
        if (_actualHealth <= 0)
        {
            _actualHealth = 1000;
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
                
                if (point.normal.y > -0.1f)
                {
                    other.collider.GetComponent<IInteractable>().Hitted(_damage, _movementDirection);           
                    Destroy(gameObject);
                }
            }
            

        }
    }
}