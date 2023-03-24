using System.Collections.Generic;

public static class RescuerEvaderPathAssign
{
    public static List<Node> GetPath(Node rescuerNode, Node frozenNode)
    {
        List<Node> path = AStar.Find(rescuerNode, frozenNode);
        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;

    }
}

