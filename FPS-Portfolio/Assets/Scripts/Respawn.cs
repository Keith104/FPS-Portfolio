using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform RespawnPoint;
    [SerializeField] bool playerDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDead)
        {
            RespawnPlayer();
            playerDead = false;
        }
    }
    
    void RespawnPlayer()
    {
        PlayerTransform.position = RespawnPoint.position;
    }
}
