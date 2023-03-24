using System.Collections.Generic;
public static class UnfrozenEvaderPathAssign
{
    public static List<Node> GetPath(Node selfNode, Node evaderNode)
    {
        List<Node> path = new List<Node>();
        if (evaderNode == null) { return path; }
        path.Add(evaderNode);
        return path;
    }
}