using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    
    public GameObject ballStay;
    [SerializeField]
    private GameObject ball;
    public int y;
    [SerializeField]
    private int totalScore = 0;
    public void SpawnAt(Vector3 position, Quaternion rotation)
    {
        Instantiate(ball, position, rotation);
    }
    

    // Update is called once per frame
    void Update()
    {
        if(y == 1) 
        {
            Instantiate(ball, ballStay.transform.position, ballStay.transform.rotation);
            y= 0;
        }
    }
}
