using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Neighbors;
    public List<Room> Rooms;
    public bool IsStatic;
    public float Radius;
    public LayerMask CharacterMask;
    private void OnTriggerEnter(Collider other)
    {
        if (IsStatic)
        {
            SteeringAgent agent = other.GetComponent<SteeringAgent>();
            if (agent != null)
            {
                if (agent.CurrentPath.Count > 1) { agent.NextNodeInPath(); }
                else
                {
                    Character character = other.GetComponent<Character>();
                    if (character != null) { character.UpdateNow(); }
                }
                WayPoint wayPoint = other.GetComponent<WayPoint>();
                if (wayPoint != null && wayPoint.WayPoints[wayPoint.LastWayPointIndex] == this) { wayPoint.Next(); }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsStatic)
        {

        }
    }
    private void FixedUpdate()
    {
    }
}
