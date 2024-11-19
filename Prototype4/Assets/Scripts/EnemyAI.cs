using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody enemyRb;
    public GameObject playerGoal;
    public float speed = 3.0f;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        if (playerGoal == null)
        {
            playerGoal = GameObject.FindGameObjectWithTag("PlayerGoal");
        }
    }

    void Update()
    {
        Vector3 directionToPlayerGoal = (playerGoal.transform.position - transform.position).normalized;

        enemyRb.AddForce(directionToPlayerGoal * speed);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
