using UnityEngine;
public static class ActiveEvaderTargetAssign
{
    public static LayerMask CharacterMask = LayerMask.GetMask("Chaser", "PatrolingChaser");
    public static float Radius = 7;
    public static Node GetTarget(Node selfNode)
    {
        Node ret = null;
        float retDistance = 0;
        // Get the nearest Chaser
        foreach (Collider collider in Physics.OverlapSphere(selfNode.transform.position, Radius, CharacterMask))
        {
            Node temp = collider.GetComponent<Node>();
            float distance = Vector3.Distance(selfNode.transform.position, collider.transform.position);
            if (ret == null || distance < retDistance)
            {
                ret = temp;
                retDistance = distance;
            }
        }
        return ret;
    }
}
