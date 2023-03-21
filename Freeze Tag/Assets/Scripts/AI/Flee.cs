using UnityEngine;
public class Flee : SteeringMovement
{
    public float FleeAtDistance;
    public float MaxViewArc;
    public float CloseDistance;
    public float ViewArcDecreaseFactor;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);

        if (Vector3.Distance(agent.TargetPosition, agent.Position) < FleeAtDistance)
        {
            Vector3 desiredVelocity = agent.Position - agent.TargetPosition;
            float viewArc = (1 - ViewArcDecreaseFactor * (agent.Velocity.magnitude / agent.MaxSpeed)) * MaxViewArc;
            if (Vector3.Angle(agent.transform.forward, desiredVelocity.normalized) < viewArc
            || Vector3.Distance(agent.Position, agent.TargetPosition) < CloseDistance)
            {
                desiredVelocity = desiredVelocity.normalized * agent.MaxSpeed;
                Vector3 steering = desiredVelocity - agent.Velocity;
                ret.Linear = steering;
            }
        }

        return ret;
    }
}
