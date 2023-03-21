﻿using UnityEngine;
public class Seek : SteeringMovement
{
    public float MaxViewArc;
    public float CloseDistance;
    public float ViewArcDecreaseFactor;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);

        if (agent.CurrentTargetTransform == null) { return ret; }
        Vector3 desiredVelocity = agent.TargetPosition - agent.Position;
        float viewArc = (1 - ViewArcDecreaseFactor * (agent.Velocity.magnitude / agent.MaxSpeed)) * MaxViewArc;
        //Debug.Log(viewArc);
        if (Vector3.Angle(agent.transform.forward, desiredVelocity.normalized) < viewArc
            || Vector3.Distance(agent.Position, agent.TargetPosition) < CloseDistance)
        {
            desiredVelocity = desiredVelocity.normalized * agent.MaxSpeed;
            Vector3 steering = desiredVelocity - agent.Velocity;
            ret.Linear = steering;
            //Debug.Log("In view range");
        }
        return ret;
    }
}
