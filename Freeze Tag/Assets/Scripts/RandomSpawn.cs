using UnityEngine;
using System.Collections.Generic;
public class RandomSpawn : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public GameObject SpawnObject;
    public int ObjectCount;
    public List<Node> WayPoints;
    public void Spawn()
    {
        bool hasChaser = false;
        if (ObjectCount > SpawnPoints.Count)
        {
            Debug.Log("Not enough Spawn Points");
            return;
        }
        while (ObjectCount > 0)
        {
            int index = Random.Range(0, SpawnPoints.Count);
            Vector3 spawnPoint = SpawnPoints[index].position;
            GameObject characterObject = Instantiate(SpawnObject, spawnPoint, Quaternion.identity, this.transform.parent);
            Character character = characterObject.GetComponent<Character>();
            if (character != null)
            {
                if (hasChaser)
                {
                    character.State = Character.CharacterState.PassiveEvader;
                }
                else
                {
                    character.State = Character.CharacterState.Chaser;
                    hasChaser = true;
                }
            }
            WayPoint wayPoint = characterObject.GetComponent<WayPoint>();
            wayPoint.WayPoints = WayPoints;
            wayPoint.LastWayPointIndex = 0;
            SpawnPoints.RemoveAt(index);
            ObjectCount--;
        }
    }
    public void Start()
    {
        Spawn();
    }
}