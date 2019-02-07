using UnityEngine;

public class BasicEnemyController : MonoBehaviour, IEnemy
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _recoil;

    private int _actualHealth;

    // Use this for initialization
    void Start()
    {
        _actualHealth = _health;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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