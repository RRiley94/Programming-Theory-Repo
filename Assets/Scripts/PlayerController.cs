using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameManager
{
    private bool isOnGround = true;
    public float jumpForce;
    private Rigidbody playerRb;
    [SerializeField] GameObject powerupPrefab;
    public bool hasPowerup = false;
    private float spawnRange = 20;
    public float speed;
    private const float turnSpeed = 240.0f;
    protected Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {//code to pull player rigidbody for jump force
        playerRb = GetComponent<Rigidbody>();
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        m_Animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        float forwardInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        //code for using space to jump, isOnGround is a bool (true or false value) to prevent multiple jumping
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {//code to make player jump
            playerRb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
            m_Animator.SetBool("Jump_b", true);
            isOnGround = false;
        }
        else if (isOnGround == true)
        {
            m_Animator.SetBool("Jump_b", false);
        }
        //code for basic player movement with main camera attached using prototype 1 followplayer script.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerRb.AddRelativeForce(Vector3.forward * forwardInput * speed, ForceMode.Impulse);
            m_Animator.SetFloat("Speed_f", playerRb.velocity.magnitude);
            playerRb.transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        }
        else
        {
            if (isOnGround)
            {
                playerRb.velocity = Vector3.zero;
                m_Animator.SetFloat("Speed_f", 0);
            }
            
        }
        //inherits from game manager script
        WallBoundary();
    }


    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomSpawnPos = new Vector3(spawnPosX, 1, spawnPosZ);
        return randomSpawnPos;
    }
    private void OnCollisionEnter(Collision collision)
    {//code to allow player to jump once on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy") && !hasPowerup)
        {
            m_Animator.SetBool("Death_b", true);
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(DespawnPlayerSequence());
            
        }
    }

    IEnumerator DespawnPlayerSequence()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    IEnumerator PowerupCountdown()
    {
        yield return new WaitForSeconds(10);
        hasPowerup = false;
    }
    IEnumerator PowerupSpawn()
    {
        yield return new WaitForSeconds(10);
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdown());
            StartCoroutine(PowerupSpawn());
        }
        if (other.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Destroy(other.gameObject);
        }
    }
}
