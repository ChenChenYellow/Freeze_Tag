using System.Collections.Generic;
public static class PatrolingChaserPathAssign
{
    public static List<Node> GetPath(Node selfNode, Node targetNode)
    {
        WayPoint wayPoint = selfNode.GetComponent<WayPoint>();

        List<Node> path = AStar.Find(selfNode, targetNode, false);

        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;
    }
}
