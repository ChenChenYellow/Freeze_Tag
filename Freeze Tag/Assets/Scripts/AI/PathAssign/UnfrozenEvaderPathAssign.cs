
using UnityEngine;
using System.Collections.Generic;
public static class UnfrozenEvaderPathAssign
{
    public static List<Node> GetPath(Character self, Character evader)
    {
        List<Node> path = new List<Node>();
        if (evader == null) { return path; }
        path.Add(evader.Node);
        return path;
    }
}