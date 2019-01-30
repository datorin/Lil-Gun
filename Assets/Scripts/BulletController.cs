using DefaultNamespace;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _time;
    private Vector2 _direction;
    private int _damage;

    private void Update()
    {
        _time -= Time.deltaTime;

        if(_time <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(Values.EnemyTag))
        {
            other.GetComponent<IEnemy>().Hitted(1, _direction);
        }

        if(other.gameObject.CompareTag(Values.PlayerTag)) return;
        Destroy(gameObject);
    }
    
    public Vector2 Direction
    {
        set { _direction = value; }
    }
    
    public int Damage
    {
        set { _damage = value; }
    }
}