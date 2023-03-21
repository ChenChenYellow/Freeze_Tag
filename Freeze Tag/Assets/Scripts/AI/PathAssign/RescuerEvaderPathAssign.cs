using System.Collections.Generic;

public static class RescuerEvaderPathAssign
{
    public static List<Node> GetPath(Character rescuer, Character frozen)
    {
        List<Node> path = AStar.Find(rescuer.Node, frozen.Node);
        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;

    }
}

