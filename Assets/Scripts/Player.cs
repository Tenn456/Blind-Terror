using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 movement;
    public AudioClip footsteps;
    private AudioSource audioSource;
    public bool isMoving = false;
    public bool gameOver = false;
    public bool footstepsPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!gameOver)
        {
            // Get input from arrow keys or WASD for 3D movement
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            movement = new Vector3(horizontal, 0f, vertical).normalized;

            isMoving = movement.magnitude > 0; // Check if the player is moving (any input in horizontal, vertical, or up/down directions)

            if (isMoving)
            {
                if (!footstepsPlaying)
                {
                    audioSource.PlayOneShot(footsteps);
                    footstepsPlaying = true;
                }
                
            }
            else
            {
                audioSource.Stop();
                footstepsPlaying = false;
            }
        }
        else
        {
            isMoving = false;
            audioSource.Stop();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
}
