using UnityEngine;
public class ObstacleCollision : SteeringMovement
{
    public LayerMask ObstacleMask;
    public float Radius;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        foreach (Collider collider in Physics.OverlapSphere(agent.Position, Radius, ObstacleMask))
        {

            Vector3 hitPoint = collider.ClosestPoint(agent.Position);
            float distance = Vector3.Distance(agent.Position, hitPoint);
            float intensity = Radius / distance;
            Vector3 desiredVelocity = agent.Position - hitPoint;
            desiredVelocity = agent.MaxSpeed * intensity * desiredVelocity.normalized;
            //Debug.Log(agent.name + " " + desiredVelocity);
            Vector3 steering = desiredVelocity - agent.Velocity;
            ret.Linear = steering;
            Debug.DrawRay(hitPoint, desiredVelocity, Color.grey, 1);
        }
        return ret;
    }
}
