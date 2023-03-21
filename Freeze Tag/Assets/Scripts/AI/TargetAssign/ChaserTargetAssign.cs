using UnityEngine;
public static class ChaserTargetAssign 
{
    public static LayerMask CharacterMask = LayerMask.GetMask("Player", "ActiveEvader", "PassiveEvader", "RescuerEvader", "UnfrozenEvader");
    public static float Radius = 40;
    public static Character GetTarget(Character self)
    {
        Character ret = null;
        float retDistance = 0;
        int counter = 0;
        foreach (Collider collider in Physics.OverlapSphere(self.transform.position, Radius, CharacterMask))
        {
            //Debug.Log("Chaser Target Assign Loop " + counter);
            counter++;
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
