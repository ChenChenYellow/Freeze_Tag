using System.Collections.Generic;

public static class ChaserPathAssign
{
    public static List<Node> GetPath(Character chaser, Character evader)
    {
        if (evader == null) { return new List<Node>(); }
        List<Node> path = AStar.Find(chaser.Node, evader.Node);
        // Remove self node
        if (path.Count > 1) { path.RemoveAt(0); }
        return path;
    }
}
