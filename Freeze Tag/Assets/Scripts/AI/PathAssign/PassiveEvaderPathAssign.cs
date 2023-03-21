using System.Collections.Generic;

public static class PassiveEvaderPathAssign
{
    public static List<Node> GetPath(Character self)
    {
        return new List<Node>() { self.Node };
    }
}

