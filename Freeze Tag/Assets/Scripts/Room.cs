using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Node> Nodes;
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character == null) { return; }
        character.SetCurrentRoom(this);
    }
    public void CharacterLeave(Character character)
    {
        Debug.Log("Leave Start");
        Debug.Log(Nodes.Count);
        Node characterNode = character.GetComponent<Node>();
        foreach (Node node in Nodes)
        {
            node.Neighbors.Remove(characterNode);
        }
        Debug.Log("Leave End");
        Debug.Log(Nodes.Count);
    }
    public void CharacterEnter(Character character)
    {
        //Debug.Log("Enter Start");
        //Debug.Log(Nodes.Count);
        Node characterNode = character.GetComponent<Node>();
        foreach (Node node in Nodes)
        {
            node.Neighbors.Add(characterNode);
        }
        //Debug.Log("Enter End");
        //Debug.Log(Nodes.Count);
    }
}
