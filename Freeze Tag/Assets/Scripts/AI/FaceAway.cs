using UnityEngine;
public class FaceAway : SteeringMovement
{
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);

        Quaternion quaternion = Quaternion.identity;
        Vector3 direction = agent.Position - agent.TargetPosition;
        if (direction.normalized == agent.transform.forward
            || Mathf.Approximately(direction.magnitude, 0))
        {
            quaternion = agent.transform.rotation;
        }
        else
        {
            quaternion = Quaternion.LookRotation(direction);
        }
        Vector3 from = Vector3.ProjectOnPlane(agent.transform.forward, Vector3.up);
        Vector3 to = quaternion * Vector3.forward;
        float angleY = Vector3.SignedAngle(from, to, Vector3.up);
        ret.Angular = Quaternion.AngleAxis(angleY, Vector3.up);
        return ret;
    }
}


