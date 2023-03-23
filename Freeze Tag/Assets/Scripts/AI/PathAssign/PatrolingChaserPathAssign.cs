using System.Collections.Generic;
public static class PatrolingChaserPathAssign
{
    public static List<Node> GetPath(Character self)
    {
        WayPoint wayPoint = self.GetComponent<WayPoint>();

        List<Node> path = AStar.Find(self.Node, wayPoint.WayPoints[wayPoint.LastWayPointIndex]);

        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;
    }
}
