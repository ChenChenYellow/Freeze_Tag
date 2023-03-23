using UnityEngine;
using System.Collections.Generic;
public class WayPoint : MonoBehaviour
{
    public List<Node> WayPoints;
    public int LastWayPointIndex;
    public void Next()
    {
        int newIndex = LastWayPointIndex + 1;
        if (newIndex >= WayPoints.Count) { newIndex = 0; }
        LastWayPointIndex = newIndex;
    }
}
