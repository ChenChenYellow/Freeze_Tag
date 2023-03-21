using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Node> Nodes;
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character == null) { return; }
        //character.CurrentRoom = this;
        CharacterEnter(character);
    }
    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character == null) { return; }
        CharacterLeave(character);
    }
    public void CharacterEnter(Character character)
    {
        Node characterNode = character.GetComponent<Node>();
        characterNode.Neighbors.AddRange(Nodes);
        foreach (Node node in Nodes)
        {
            node.Neighbors.Add(characterNode);
        }
        Nodes.Add(characterNode);

    }
    public void CharacterLeave(Character character)
    {
        Node characterNode = character.GetComponent<Node>();
        foreach (Node node in Nodes)
        {
            characterNode.Neighbors.Remove(node);
            node.Neighbors.Remove(characterNode);
        }
        Nodes.Remove(characterNode);
    }
}
