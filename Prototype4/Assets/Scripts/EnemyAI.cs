using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public Rigidbody enemyRb;
    public GameObject player;
    public float speed = 3.0f; 


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookingDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookingDirection * speed);
    }
}
