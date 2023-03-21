using UnityEngine;
using System.Collections.Generic;
public class SteeringAgent : MonoBehaviour
{
    public List<SteeringMovement> SteeringMovements;
    public float MaxSpeed;
    public Vector3 Velocity = Vector3.zero;
    public List<Node> CurrentPath;
    public float Drag = 0.1f;
    public Vector3 Position { get { return transform.position; } }
    public Vector3 TargetPosition
    {
        get { return CurrentTargetTransform == null ? Vector3.zero : CurrentTargetTransform.position; }
    }
    public Transform CurrentTargetTransform
    {
        get
        {
            if (CurrentPath.Count > 0)
            {
                return CurrentPath[0].transform;
            }
            return null;
        }
    }
    private Steering GetSteeringSum()
    {
        Steering ret = new Steering(Vector3.zero, Quaternion.identity);
        foreach (SteeringMovement steeringMovement in SteeringMovements)
        {
            ret.Add(steeringMovement.GetSteering(this));
        }
        return ret;
    }
    private void Update()
    {
        Velocity *= (1 - Drag);

        Steering finalSteering = GetSteeringSum();
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * finalSteering.Angular, Time.deltaTime);
        Velocity += finalSteering.Linear;
        Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);
        transform.position += Velocity * Time.deltaTime;
    }
    private void Start()
    {
        CurrentPath = new List<Node>();
    }
    public void NextNodeInPath()
    {
        if (CurrentPath.Count > 1)
        {
            CurrentPath.RemoveAt(0);
        }
    }
}
