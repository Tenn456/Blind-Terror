using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float roamSpeed = 2f; // Speed at which the monster roams
    public float roamTime = 3f; // Time to roam before deciding to wait
    public float minWaitTime = 1f; // Minimum wait time before roaming again
    public float maxWaitTime = 3f; // Maximum wait time before roaming again
    public float deathRange = 3f;
    public AudioClip bite;
    public AudioClip scream;
    private AudioSource audioSource;
    public Player player;
    public Rigidbody rb;
    private float roamTimer; // Timer for how long the monster roams
    private float waitTimer; // Timer for how long the monster waits
    private bool isRoaming = true;
    private Vector3 roamDirection; // Direction the monster is roaming towards
    public float teleportDistance; // Distance until teleportation
    public float teleportRange; // The range around the player that the monster will teleport to
    public float distanceToPlayer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        roamTimer = roamTime;
        SetRoamDirection();
        waitTimer = Random.Range(minWaitTime, maxWaitTime); // Set initial wait time
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > teleportDistance)
        {
            Debug.Log(distanceToPlayer);
            Debug.Log(teleportDistance);
            TeleportMonsterAroundPlayer();
        }

        if (isRoaming)
        {
            // Decrease roam timer while the monster is roaming
            roamTimer -= Time.deltaTime;

            if (roamTimer <= 0)
            {
                // Stop roaming and start waiting
                isRoaming = false;
                waitTimer = Random.Range(minWaitTime, maxWaitTime); // Set a new random wait time
            }

            // Roam in the current direction
            transform.Translate(roamDirection * roamSpeed * Time.deltaTime);

            // Check if the player is within the death range
            if (distanceToPlayer <= deathRange)
            {
                GameOver();
            }
        }
        else
        {
            // Decrease wait timer while the monster is waiting
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                // Start roaming again after waiting
                isRoaming = true;
                roamTimer = roamTime;  // Reset the roaming timer
                SetRoamDirection();    // Set a new random direction for roaming
            }
        }
    }

    void SetRoamDirection()
    {
        float randomDirectionX = Random.Range(-1f, 1f);
        float randomDirectionZ = Random.Range(-1f, 1f);
        roamDirection = new Vector3(randomDirectionX, 0f, randomDirectionZ).normalized;
    }

    // Used ChatGPT to figure out how to make sure that the monster does not teleport within the death range.
    void TeleportMonsterAroundPlayer()
    {
        Vector3 randomOffset;
        float newDistanceToPlayer;

        do
        {
            // Generate a random offset within the teleport range
            randomOffset = new Vector3(Random.Range(-teleportRange, teleportRange), 0f, Random.Range(-teleportRange, teleportRange));

            // Calculate the new potential position
            Vector3 newPosition = player.transform.position + randomOffset;

            // Calculate the distance to the player from the new position
            newDistanceToPlayer = Vector3.Distance(newPosition, player.transform.position);

            // If the distance is greater than 3 units, we will teleport there
            if (newDistanceToPlayer > 3f)
            {
                transform.position = newPosition;  // Teleport the monster to the new position
                break; // Exit the loop if the teleport position is valid
            }

        } while (newDistanceToPlayer <= 3f); // Continue looping until we find a valid position

    }

    void GameOver()
    {
        if (player.isMoving)
        {
            audioSource.PlayOneShot(bite);
            audioSource.PlayOneShot(scream);
            player.gameOver = true;
        }
    }
}
