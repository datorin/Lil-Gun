using UnityEngine;
public static class Functions
{
    public static float CalculateAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}