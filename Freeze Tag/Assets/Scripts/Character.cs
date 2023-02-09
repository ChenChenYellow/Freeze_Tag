using UnityEngine;
public class Character : MonoBehaviour
{
    public Room CurrentRoom;
    private Node node;

    public void Start()
    {
        node = GetComponent<Node>();
    }
    public void SetCurrentRoom(Room room)
    {
        if (room == CurrentRoom) { return; }

        Room temp = CurrentRoom;
        if (temp != null)
        {
            Debug.Log("SetCurrentRoom Start");
            Debug.Log(temp.Nodes.Count);
        }
        Debug.Log(node.Neighbors.Count);
        node.Neighbors.Clear();
        Debug.Log(node.Neighbors.Count);
        if (temp != null)
        {
            Debug.Log("SetCurrentRoom Mid");
            Debug.Log(temp.Nodes.Count);
        }
        if (CurrentRoom != null)
        {
            CurrentRoom.CharacterLeave(this);
        }

        CurrentRoom = room;
        node.Neighbors.AddRange(CurrentRoom.Nodes);
        CurrentRoom.CharacterEnter(this);

        if (temp != null)
        {
            Debug.Log("SetCurrentRoom End");
            Debug.Log(temp.Nodes.Count);
        }
    }
}
