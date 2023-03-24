using System.Collections.Generic;
public static class PatrolingChaserPathAssign
{
    public static List<Node> GetPath(Node selfNode)
    {
        WayPoint wayPoint = selfNode.GetComponent<WayPoint>();

        List<Node> path = AStar.Find(selfNode, wayPoint.WayPoints[wayPoint.LastWayPointIndex]);

        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;
    }
}
