using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isOnGround = true;
    public float jumpForce;
    protected Rigidbody playerRb;
    public GameObject powerupPrefab;
    public bool hasPowerup = false;
    private float spawnRange = 20;
    public float speed;
    private const float turnSpeed = 240.0f;
    protected float zBound = 24;
    protected float xBound = 24;
    private GameObject focalPoint;
    // Start is called before the first frame update
    void Start()
    {//code to pull player rigidbody for jump force
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float forwardInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        //code for using space to jump, isOnGround is a bool (true or false value) to prevent multiple jumping
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {//code to make player jump
            playerRb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
        //code for basic player movement with main camera attached using prototype 1 followplayer script.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerRb.AddRelativeForce(Vector3.forward * forwardInput * speed, ForceMode.Impulse);
            playerRb.transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        }
        else
        {
            playerRb.velocity = Vector3.zero;
        }
        //wall boundaries for player
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

        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Destroy(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Enemy") && !hasPowerup)
        {
            Destroy(gameObject);
        }
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

    protected void WallBoundary()
    {
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        else if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
    }
}
