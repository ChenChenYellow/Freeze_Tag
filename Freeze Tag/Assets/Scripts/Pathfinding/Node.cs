using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Neighbors;
    public List<Room> Rooms;
    public bool IsStatic;
    private void OnTriggerEnter(Collider other)
    {
        if (IsStatic)
        {
            SteeringAgent agent = other.GetComponent<SteeringAgent>();
            if (agent != null)
            {
                if (agent.CurrentPath.Count > 1)
                {
                    agent.NextNodeInPath();
                }
                else
                {
                    Character character = other.GetComponent<Character>();
                    if (character != null)
                    {
                        character.State = character.State;
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
}
