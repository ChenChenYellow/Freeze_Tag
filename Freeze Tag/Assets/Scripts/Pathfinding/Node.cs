using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Neighbors;
    public bool IsStatic;
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
                    if (character != null)
                    {
                        character.UpdateNow();
                        switch (character.State)
                        {
                            case Character.CharacterState.Chaser:
                            case Character.CharacterState.PatrolingChaser:
                                break;
                            default:
                                Coin coin = GetComponent<Coin>();
                                if (coin != null && !coin.Acquired) { coin.Aquire(); }
                                break;
                        }
                    }
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
