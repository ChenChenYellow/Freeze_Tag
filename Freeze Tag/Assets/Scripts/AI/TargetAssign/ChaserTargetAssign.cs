using UnityEngine;
public static class ChaserTargetAssign
{
    public static LayerMask CharacterMask = LayerMask.GetMask("Player", "ActiveEvader", "PassiveEvader", "RescuerEvader", "UnfrozenEvader", "GreedyEvader");
    public static LayerMask RayCastMask = LayerMask.GetMask("Player", "ActiveEvader", "PassiveEvader", "RescuerEvader", "UnfrozenEvader", "GreedyEvader", "Wall");
    public static float MaxSpotRadius = 40, MinSpotRadius = 5, Angle = 70;
    public static Node GetTarget(Node selfNode)
    {
        Node ret = null;
        float retDistance = 0;

        foreach (Collider collider in Physics.OverlapSphere(selfNode.transform.position, MaxSpotRadius, CharacterMask))
        {
            Node temp = collider.GetComponent<Node>();
            float distance = Vector3.Distance(selfNode.transform.position, collider.transform.position);
            if (distance > MaxSpotRadius) { continue; }
            if (distance > MinSpotRadius)
            {
                Vector3 direction = collider.transform.position - selfNode.transform.position;
                float angle = Vector3.Angle(direction, selfNode.transform.forward);
                //Debug.Log(angle);
                if (angle > Angle) { continue; }
                if (Physics.Raycast(selfNode.transform.position, direction, out RaycastHit hit, MaxSpotRadius, RayCastMask))
                {
                    if (hit.collider.gameObject != collider.gameObject) { continue; }
                    Debug.DrawRay(selfNode.transform.position, direction, Color.blue, 3);
                }
            }
            if (ret == null || distance < retDistance)
            {
                ret = temp;
                retDistance = distance;
            }
        }
        return ret;
    }
}
