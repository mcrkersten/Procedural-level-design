using UnityEngine;

public static class RangeAttackCalculations
{
    public static Vector3 GetVelocityForProjectile(Transform from, Transform target, float projectileSpeed)
    {
        float throwAngle;
        if (CalculateThrowAngle(from.position, target.position + new Vector3(0,1,0), projectileSpeed, out throwAngle))
        {
            Vector3 throwDirection = (target.position - from.position);
            throwDirection.y = 0;
            throwDirection = Vector3.RotateTowards(throwDirection, Vector3.up, throwAngle, throwAngle).normalized;
            return throwDirection * projectileSpeed;
        }
        return Vector3.zero;
    }

    static bool CalculateThrowAngle(Vector3 from, Vector3 to, float speed, out float angle)
    {
        float xx = to.x - from.x;
        float xz = to.z - from.z;
        float x = Mathf.Sqrt(xx * xx + xz * xz);
        float y = from.y - to.y;

        float v = speed;
        float g = Physics.gravity.y;

        float sqrt = (v * v * v * v) - (g * (g * (x * x) + 2 * y * (v * v)));

        // Not enough range
        if (sqrt < 0)
        {
            Debug.Log("Oops");
            angle = 0.0f;
            return false;
        }

        angle = Mathf.Atan(((v * v) - Mathf.Sqrt(sqrt)) / (g * x));
        return true;
    }
}
    