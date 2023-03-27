using UnityEngine;
using System.Collections.Generic;
public class WayPoint : MonoBehaviour
{
    public List<Node> WayPoints;
    public int LastWayPointIndex;
    public int Next()
    {
        int newIndex = LastWayPointIndex + 1;
        if (newIndex >= WayPoints.Count) { newIndex = 0; }
        return newIndex;
    }
    public void Reset()
    {
        float minDistance = float.MaxValue;
        for (int i = 0; i < WayPoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, WayPoints[i].transform.position);
            if (distance < minDistance)
            {
                LastWayPointIndex = i;
                minDistance = distance;
            }
        }
    }
}
