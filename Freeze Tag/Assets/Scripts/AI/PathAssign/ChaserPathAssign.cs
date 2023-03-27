using System.Collections.Generic;

public static class ChaserPathAssign
{
    public static List<Node> GetPath(Node chaserNode, Node evaderNode)
    {
        if (evaderNode == null) { return new List<Node>(); }
        List<Node> path = AStar.Find(chaserNode, evaderNode, true);
        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;
    }
}
