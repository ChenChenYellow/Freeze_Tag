using UnityEngine;
using System.Collections.Generic;
public static class RescuerEvaderTargetAssign
{
    public static LayerMask FrozenCharacterMask = LayerMask.GetMask("FrozenEvader");
    public static LayerMask ChaserCharacterMask = LayerMask.GetMask("Chaser, PatrolingChaser");
    public static float FrozenCharacterRadius = 16, ChaserCharacterRadius = 20, Angle = 20;
    public static Node GetTarget(Node selfNode)
    {
        Node ret = null;
        float retDistance = 0;
        List<Vector3> chaserDirections = new List<Vector3>();
        foreach (Collider collider in Physics.OverlapSphere(selfNode.transform.position, ChaserCharacterRadius, ChaserCharacterMask))
        {
            Vector3 direction = collider.transform.position - selfNode.transform.position;
            chaserDirections.Add(direction);
        }
        foreach (Collider collider in Physics.OverlapSphere(selfNode.transform.position, FrozenCharacterRadius, FrozenCharacterMask))
        {
            Node temp = collider.GetComponent<Node>();
            float distance = Vector3.Distance(selfNode.transform.position, collider.transform.position);
            Vector3 frozenCharacterDirection = collider.transform.position - selfNode.transform.position;
            bool flag = false;
            foreach (Vector3 chaserDirection in chaserDirections)
            {
                float angle = Vector3.Angle(chaserDirection, frozenCharacterDirection);
                if (angle < Angle)
                {
                    flag = true;
                    break;
                }
            }
            if (flag) { continue; }
            if (ret == null || distance < retDistance)
            {
                ret = temp;
                retDistance = distance;
            }
        }
        return ret;
    }
}