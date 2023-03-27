using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Neighbors;
    public bool IsStatic, IsNearbyChaser;
    private void OnTriggerEnter(Collider other)
    {
        if (IsStatic)
        {
            SteeringAgent agent = other.GetComponent<SteeringAgent>();
            if (agent != null)
            {
                if (agent.CurrentPath.Count > 1) { agent.NextNodeInPath(); }
                Character character = other.GetComponent<Character>();
                if (character != null)
                {
                    character.UpdateNow();
                    switch (character.State)
                    {
                        case Character.CharacterState.Chaser:
                        case Character.CharacterState.PatrolingChaser:
                            WayPoint wayPoint = other.GetComponent<WayPoint>();
                            if (wayPoint != null)
                            {
                                if (wayPoint.WayPoints.Contains(this)) { wayPoint.LastWayPointIndex = wayPoint.WayPoints.IndexOf(this); }
                                //else { wayPoint.Reset(); }
                            }
                            break;
                        default:
                            Coin coin = GetComponent<Coin>();
                            if (coin != null && !coin.Acquired) { coin.Aquire(); }
                            break;
                    }
                }
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
