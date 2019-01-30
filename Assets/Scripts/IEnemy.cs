using UnityEngine;

namespace DefaultNamespace
{
    public interface IEnemy
    {
        void Hitted(int damage, Vector2 direction);
    }
}