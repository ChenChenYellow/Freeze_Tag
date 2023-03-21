using UnityEngine;
using System.Collections.Generic;
public static class ActiveFleePathAssign
{
    public static List<Node> GetPath(Character chaser, Character evader)
    {
        List<Node> path = new List<Node>();

        if (evader.Node.Neighbors.Count <= 0)
        {
            return path;
        }
        // Get the angle Door-Evader-Chaser
        // if angle < 45 
        // then don't move,
        // else, the larger the angle the better
        Vector3 EC = chaser.transform.position - evader.transform.position;
        Node targetNode = evader.Node;
        float score = 45;
        foreach (Node node in evader.Node.Neighbors)
        {
            if (!node.IsStatic) { continue; }
            Vector3 ED = node.transform.position - evader.transform.position;
            float tempScore = Vector3.Angle(ED, EC);
            if (tempScore > score)
            {
                score = tempScore;
                targetNode = node;
            }
        }
        path.Add(targetNode);
        return path;
    }
}
