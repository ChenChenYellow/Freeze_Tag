using UnityEngine;
using System.Collections.Generic;
public class RandomSpawn : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public GameObject ChaserPrefab, EvaderPrefab, CoinPrefab;
    public int ChaserCount, EvaderCount;
    public List<Node> WayPoints;
    public void Spawn()
    {
        if (ChaserCount > SpawnPoints.Count)
        {
            Debug.Log("Not enough Spawn Points");
            return;
        }
        while (SpawnPoints.Count > 0)
        {
            int index = Random.Range(0, SpawnPoints.Count);
            Vector3 spawnPoint = SpawnPoints[index].position;
            if (ChaserCount > 0 || EvaderCount > 0)
            {
                GameObject characterObject = Instantiate(ChaserPrefab, spawnPoint, Quaternion.identity, this.transform.parent);
                Character character = characterObject.GetComponent<Character>();
                if (character != null)
                {
                    if (EvaderCount > 0)
                    {
                        character.State = Character.CharacterState.PassiveEvader;
                        EvaderCount--;
                    }
                    else
                    {
                        character.State = Character.CharacterState.Chaser;
                        ChaserCount--;
                    }
                }
                WayPoint wayPoint = characterObject.GetComponent<WayPoint>();
                wayPoint.WayPoints = WayPoints;
                wayPoint.LastWayPointIndex = 0;
            }
            else
            {
                GameObject coinObject = Instantiate(CoinPrefab, spawnPoint, Quaternion.identity, this.transform.parent);
            }
            SpawnPoints.RemoveAt(index);
        }
    }
    public void Start()
    {
        Spawn();
    }
}