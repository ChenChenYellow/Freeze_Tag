using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    //private static float Allowance = 0.01f;
    private static float chaserAvoidFactor = 10.0f;
    public static List<Node> Find(Node startNode, Node endNode, bool usingIsNearbyChaser)
    {
        List<Marker> OpenList;
        List<Marker> CloseList;
        Marker startMarker = new Marker(startNode, 0, Vector3.Distance(startNode.transform.position, endNode.transform.position), null);
        OpenList = new List<Marker>();
        CloseList = new List<Marker>();
        OpenList.Add(startMarker);

        List<Node> ret = new List<Node>();

        while (OpenList.Count > 0)
        {
            Marker lowestF = OpenList[0];
            foreach (Marker marker in OpenList)
            { if (marker.f < lowestF.f) { lowestF = marker; } }

            OpenList.Remove(lowestF);
            Node lowestFNode = lowestF.node;
            foreach (Node neighbor in lowestF.node.Neighbors)
            {
                if (neighbor == endNode)
                {
                    ret.Add(neighbor);
                    Marker temp = lowestF;
                    Debug.DrawLine(neighbor.transform.position, temp.node.transform.position, Color.red, 1);
                    while (temp != null)
                    {
                        ret.Add(temp.node);
                        if (temp.parent != null)
                        {
                            Debug.DrawLine(temp.node.transform.position, temp.parent.node.transform.position, Color.red, 1);
                        }
                        temp = temp.parent;
                    }
                    ret.Reverse();
                    return ret;
                }
                float additionalG = Vector3.Distance(lowestFNode.transform.position, neighbor.transform.position);
                if (usingIsNearbyChaser && neighbor.IsNearbyChaser)
                {
                    additionalG *= chaserAvoidFactor;
                }
                float g = lowestF.g + additionalG;
                float h = Vector3.Distance(neighbor.transform.position, endNode.transform.position);
                Marker m = new Marker(neighbor, g, h, lowestF);

                bool existBetterInOpenList = false,
                    existBetterInCloseList = false;

                foreach (Marker marker in OpenList)
                {
                    if (marker.node == m.node
                        && marker.f < m.f)
                    {
                        existBetterInOpenList = true;
                        break;
                    }
                }
                if (existBetterInOpenList) { continue; }

                foreach (Marker marker in CloseList)
                {
                    if (marker.node == m.node
                        && marker.f < m.f)
                    {
                        existBetterInCloseList = true;
                        break;
                    }
                }
                if (existBetterInCloseList) { continue; }

                OpenList.Add(m);
            }
            CloseList.Add(lowestF);
        }
        return ret;
    }
}

public class Marker
{
    public Node node;
    public float g, h;
    public float f { get { return g + h; } }
    public Marker parent;
    public Marker(Node node, float g, float h, Marker parent)
    {
        this.node = node;
        this.g = g;
        this.h = h;
        this.parent = parent;
    }
}