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
            AStar aStar = new AStar();
            aStar.Find(StartNode, EndNode);
        }
    }
}
