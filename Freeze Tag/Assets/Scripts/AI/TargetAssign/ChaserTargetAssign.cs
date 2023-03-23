using UnityEngine;
public static class ChaserTargetAssign
{
    public static LayerMask CharacterMask = LayerMask.GetMask("Player", "ActiveEvader", "PassiveEvader", "RescuerEvader", "UnfrozenEvader");
    public static float MaxSpotRadius = 40, MinSpotRadius = 5, Angle = 45;
    public static Character GetTarget(Character self)
    {
        Character ret = null;
        float retDistance = 0;

        foreach (Collider collider in Physics.OverlapSphere(self.transform.position, MaxSpotRadius, CharacterMask))
        {
            Character temp = collider.GetComponent<Character>();
            float distance = Vector3.Distance(self.transform.position, collider.transform.position);
            if (distance > MinSpotRadius)
            {
                //Debug.Log(distance);
                Vector3 direction = collider.transform.position - self.transform.position;
                float angle = Vector3.Angle(direction, self.transform.forward);
                //Debug.Log(angle);
                if (angle > MaxSpotRadius)
                {
                    //Debug.Log("Continue");
                    continue;
                }
                Debug.DrawRay(self.transform.position, direction, Color.blue, 3);
                if (Physics.Raycast(self.transform.position, direction, out RaycastHit hit, MaxSpotRadius, Physics.AllLayers))
                {
                    if (hit.collider.gameObject != collider.gameObject) { continue; }
                }
                continue;
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
