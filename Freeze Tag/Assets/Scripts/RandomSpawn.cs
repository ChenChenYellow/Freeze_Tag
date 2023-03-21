using UnityEngine;
using System.Collections.Generic;
public class RandomSpawn : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public GameObject SpawnObject;
    public int ObjectCount;
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
            GameObject gameObject = GameObject.Instantiate(SpawnObject, spawnPoint, Quaternion.identity, this.transform.parent);
            Character character = gameObject.GetComponent<Character>();
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
            SpawnPoints.RemoveAt(index);
            ObjectCount--;
        }
    }
    public void Start()
    {
        Spawn();
    }
}