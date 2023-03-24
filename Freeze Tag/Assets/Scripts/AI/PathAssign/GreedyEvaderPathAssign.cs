using System.Collections.Generic;
public static class GreedyEvaderPathAssign
{
    public static List<Node> GetPath(Node selfNode, Node coinNode)
    {
        if (coinNode == null) { return new List<Node>(); }
        List<Node> path = AStar.Find(selfNode, coinNode);
        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;
    }
}
