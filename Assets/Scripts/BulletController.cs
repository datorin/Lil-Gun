using TMPro;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private string _myTag;
    [SerializeField] private string _hitTag;
    [SerializeField] private float _time;
    private Vector2 _direction;
    [SerializeField] private int _damage;

    private void Update()
    {
        _time -= Time.deltaTime;

        if (_time <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(_hitTag))
        {
            other.GetComponent<IInteractable>().Hitted(_damage, _direction);
        }

        if (other.gameObject.CompareTag(_myTag)) return;
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