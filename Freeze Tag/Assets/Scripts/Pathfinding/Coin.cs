using UnityEngine;
public class Coin : MonoBehaviour
{
    public bool Acquired = false;
    private Room currentRoom;
    public Room CurrentRoom
    {
        get { return currentRoom; }
        set
        {
            if (value == CurrentRoom) { return; }

            MyNode.Neighbors.Clear();
            if (currentRoom != null)
            {
                currentRoom.CoinLeave(this);
            }

            currentRoom = value;
            MyNode.Neighbors.AddRange(currentRoom.Nodes);
            currentRoom.CoinEnter(this);
        }
    }
    private Node MyNode;
    private void Awake()
    {
        MyNode = GetComponent<Node>();
    }
    public void Aquire()
    {
        Acquired = true;
        GetComponent<Renderer>().enabled = false;
    }
}
