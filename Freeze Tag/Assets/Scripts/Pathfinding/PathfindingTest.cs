using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    public Node StartNode, EndNode;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.Find(StartNode, EndNode, false);
        }
    }
}
