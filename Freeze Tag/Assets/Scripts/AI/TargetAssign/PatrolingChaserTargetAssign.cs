using UnityEngine;
public static class PatrolingChaserTargetAssign
{
    public static Node GetTarget(Character self)
    {
        WayPoint wayPoint = self.GetComponent<WayPoint>(); 
        wayPoint.Next();
        return wayPoint.WayPoints[wayPoint.LastWayPointIndex];
    }
}
