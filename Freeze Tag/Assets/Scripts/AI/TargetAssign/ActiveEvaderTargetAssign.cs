using UnityEngine;
public static class ActiveEvaderTargetAssign
{
    public static LayerMask CharacterMask = LayerMask.GetMask("Chaser", "PatrolingChaser");
    public static float Radius = 12;
    public static Character GetTarget(Character self)
    {
        Character ret = null;
        float retDistance = 0;
        // Get the nearest Chaser
        foreach (Collider collider in Physics.OverlapSphere(self.transform.position, Radius, CharacterMask))
        {
            Character temp = collider.GetComponent<Character>();
            float distance = Vector3.Distance(self.transform.position, collider.transform.position);
            if (ret == null || distance < retDistance)
            {
                ret = temp;
                retDistance = distance;
            }
        }
        return ret;
    }
}
