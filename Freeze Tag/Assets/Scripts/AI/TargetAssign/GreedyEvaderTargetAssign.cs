using UnityEngine;
public static class GreedyEvaderTargetAssign
{
    public static LayerMask CoinMask = LayerMask.GetMask("Coin");
    public static float MaxSpotRadius = 40;
    public static Node GetTarget(Node selfNode)
    {
        Node ret = null;
        float retDistance = 0;

        foreach (Collider collider in Physics.OverlapSphere(selfNode.transform.position, MaxSpotRadius, CoinMask))
        {
            Coin coin = collider.GetComponent<Coin>();
            if (coin == null || coin.Acquired) { continue; }
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
