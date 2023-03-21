using UnityEngine;
public abstract class SteeringMovement : MonoBehaviour
{
    public virtual Steering GetSteering(SteeringAgent agent)
    {
        return new Steering(Vector3.zero, Quaternion.identity);
    }
}
