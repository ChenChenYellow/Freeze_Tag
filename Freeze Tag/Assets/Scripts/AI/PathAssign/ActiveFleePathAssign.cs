using UnityEngine;
using System.Collections.Generic;
public static class ActiveFleePathAssign
{
    public static List<Node> GetPath(Node chaserNode, Node evaderNode)
    {
        List<Node> path = new List<Node>();
        if (evaderNode.Neighbors.Count <= 0)
        {
            return path;
        }
        // Get the angle Door-Evader-Chaser
        // if angle < 45 
        // then don't move,
        // else, the larger the angle the better
        Vector3 EC = chaserNode.transform.position - evaderNode.transform.position;
        Node targetNode = evaderNode;
        float score = 45;
        foreach (Node node in evaderNode.Neighbors)
        {
            if (!node.IsStatic || node == evaderNode) { continue; }
            if (node.gameObject.layer != 0) { continue; }
            if (Vector3.Distance(node.transform.position, evaderNode.transform.position) < 0.5f) { continue; }
            Vector3 ED = node.transform.position - evaderNode.transform.position;
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
