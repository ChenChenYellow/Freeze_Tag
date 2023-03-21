using UnityEngine;
public class ObstacleAvoid : SteeringMovement
{
    public LayerMask ObstacleMask;
    public float DistanceFromObstacle;
    public float DistanceToObstacle;
    public float Radius;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret =  base.GetSteering(agent);

        if (Physics.SphereCast(agent.Position, Radius, agent.Velocity.normalized, out RaycastHit hit, DistanceToObstacle, ObstacleMask))
        {
            //Debug.Log(hit.collider.name);
            Vector3 targetDirection = agent.Velocity.normalized - (2 * Vector3.Dot(agent.Velocity.normalized, hit.normal)) * hit.normal;
            targetDirection = targetDirection.normalized;
            Vector3 targetPosition = hit.point + (targetDirection * DistanceFromObstacle);

            Vector3 desiredVelocity = targetPosition - agent.Position;
            float intensity = DistanceToObstacle / hit.distance;
            desiredVelocity = agent.MaxSpeed * intensity * desiredVelocity.normalized;
            Vector3 steering = desiredVelocity - agent.Velocity;
            ret.Linear = steering;

            Debug.DrawRay(hit.point, hit.normal, Color.blue, 1);
            Debug.DrawLine(hit.point, targetPosition, Color.cyan, 1);
        }
        return ret;
    }
}
