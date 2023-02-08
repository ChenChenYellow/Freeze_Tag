using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public List<Marker> OpenList;
    public List<Marker> CloseList;
    public void Calculate(Vector3 currentPosition, Room currentRoom, Vector3 targetPosition, Room targetRoom)
    {/*
        if (currentRoom == targetRoom)
        {
            Debug.DrawLine(currentPosition, targetPosition, Color.green);
            return;
        }
        Marker startPosition = new Marker();
        startPosition.g = 0;
        startPosition.h = Vector3.Distance(currentPosition, targetPosition);
        startPosition.parent = null;

        foreach (Node node in currentRoom.Nodes)
        {
            Marker marker = new Marker();
            marker.g = Vector3.Distance(currentPosition, node.transform.position);
            marker.h = Vector3.Distance(node.transform.position, targetPosition);
            marker.parent = startPosition;
            OpenList.Add(marker);
        }*/

    }
    public void Find(Node startNode, Node endNode)
    {
        Marker startMarker = new Marker(startNode, 0, Vector3.Distance(startNode.transform.position, endNode.transform.position), null);
        OpenList = new List<Marker>();
        CloseList = new List<Marker>();
        OpenList.Add(startMarker);

        while (OpenList.Count > 0)
        {
            Marker lowestF = OpenList[0];
            foreach (Marker marker in OpenList)
            {
                if (marker.f < lowestF.f)
                {
                    lowestF = marker;
                }
            }
            OpenList.Remove(lowestF);
            Node lowestFNode = lowestF.node;
            foreach (Node neighbor in lowestF.node.Neighbors)
            {
                if (neighbor == endNode)
                {
                    Debug.Log("Find end");
                    Debug.Log(neighbor.name);
                    Marker temp = lowestF;
                    while (temp != null)
                    {
                        Debug.Log(temp.node.name);
                        temp = temp.parent;
                    }
                    
                    return;
                }
                float g = lowestF.g + Vector3.Distance(lowestFNode.transform.position, neighbor.transform.position);
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