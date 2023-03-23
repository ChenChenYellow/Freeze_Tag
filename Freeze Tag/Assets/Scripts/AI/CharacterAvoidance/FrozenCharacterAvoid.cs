using UnityEngine;
public class FrozenCharacterAvoid : SteeringMovement
{
    public LayerMask CharacterMask;
    public float Radius;
    public override Steering GetSteering(SteeringAgent agent)
    {
        Steering ret = base.GetSteering(agent);
        foreach (Collider collider in Physics.OverlapSphere(agent.Position, Radius, CharacterMask))
        {
            if (collider.gameObject == this.gameObject) { continue; }
            float intensity = Radius / Vector3.Distance(agent.Position, collider.transform.position);
            Vector3 desiredVelocity = agent.Position - collider.transform.position;
            desiredVelocity = agent.MaxSpeed * intensity * desiredVelocity.normalized;
            Vector3 steering = desiredVelocity - agent.Velocity;
            ret.Linear += steering;
            Debug.DrawLine(agent.Position, collider.gameObject.transform.position, Color.magenta, 1);
        }

        //Debug.Log(this.name + " Avoid " + ret.Linear);
        //Debug.Log("Character Avoid " + ret.Linear);
        return ret;
    }
}
