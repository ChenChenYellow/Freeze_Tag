using UnityEngine;
public class Steering
{
    public Vector3 Linear { get; set; }
    public Quaternion Angular { get; set; }
    public Steering(Vector3 linear, Quaternion angular)
    {
        Linear = linear;
        Angular = angular;
    }
    public void Add(Steering steering)
    {
        Linear += steering.Linear;
        Angular *= steering.Angular;
    }
}
