using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameManager
{
    public float speed;
    public float zBound = 24;
    public float xBound = 24;
    private Rigidbody enemyRb;
    private GameObject player;
    private Animator enemy_Animator;
    private bool hasPowerup;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        enemy_Animator = gameObject.GetComponentInChildren<Animator>();
        enemy_Animator.SetBool("Attack", false);
    }
    public override void WallBoundary()
    {
        ZBound = 40;
        XBound = 40;
    }
    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();

        hasPowerup = player.GetComponent<PlayerController>().hasPowerup;

        if((player.transform.position.x - transform.position.x) < 2 && (player.transform.position.z - transform.position.z) < 2)
        {
            enemy_Animator.SetBool("Attack", true);
        } 
        else if (player.transform.position.x - transform.position.x > 2 && player.transform.position.z - transform.position.z > 2)
        {
            enemy_Animator.SetBool("Attack", false);
        }
        WallBoundary();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && hasPowerup == true)
        {
            enemy_Animator.SetBool("Dead", true);
            enemy_Animator.SetBool("Attack", false);
            enemyRb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(DespawnSequence());
        }
    }

        IEnumerator DespawnSequence()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    void MoveTowardsPlayer()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.velocity = (lookDirection * speed);
        transform.LookAt(player.transform, Vector3.up);
        enemy_Animator.SetFloat("MoveSpeed", enemyRb.velocity.magnitude);
    }
}
