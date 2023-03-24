using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Node> Nodes;
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null) { CharacterEnter(character); }
        else
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin != null) { CoinEnter(coin); }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null) { CharacterLeave(character); }
        else
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin != null) { CoinLeave(coin); }
        }
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
    public void CoinEnter(Coin coin)
    {
        Node characterNode = coin.GetComponent<Node>();
        characterNode.Neighbors.AddRange(Nodes);
        foreach (Node node in Nodes)
        {
            node.Neighbors.Add(characterNode);
        }
        Nodes.Add(characterNode);
    }
    public void CoinLeave(Coin coin)
    {
        Node characterNode = coin.GetComponent<Node>();
        foreach (Node node in Nodes)
        {
            characterNode.Neighbors.Remove(node);
            node.Neighbors.Remove(characterNode);
        }
        Nodes.Remove(characterNode);
    }
}
