using UnityEngine;
public static class PatrolingChaserTargetAssign
{
    public static Node GetTarget(Node selfNode)
    {
        WayPoint wayPoint = selfNode.GetComponent<WayPoint>(); 
        return wayPoint.WayPoints[wayPoint.Next()];
    }
}
