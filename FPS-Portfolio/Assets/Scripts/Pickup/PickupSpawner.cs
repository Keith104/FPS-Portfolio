using System;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject pickupToSpawn;
    [SerializeField] Transform spawnedInHere;
    [SerializeField] Transform[] spawnPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearSpawned()
    {
        for (int index = spawnedInHere.childCount - 1; index >= 0; --index)
            Destroy(spawnedInHere.GetChild(index).gameObject);
    }

    public void Spawn()
    {
        ClearSpawned();

        for(int index = 0; index < spawnPos.Length; ++index) 
            Instantiate(pickupToSpawn, spawnPos[index].transform.position, spawnPos[index].transform.rotation, spawnedInHere);
    }
}
