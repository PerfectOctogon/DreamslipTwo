using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    public static bool playerWeaponEnabled = false;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float teleportDistance = 5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool canDoubleJump = true;
    bool canDash = true;

    private AudioSource playerAudio;
    public AudioClip teleportSound;
    public AudioClip doubleJumpSound;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) { velocity.y = -2f; canDoubleJump = true; }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move *  speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || (!isGrounded && canDoubleJump))){
            if (!isGrounded && canDoubleJump)
            {
                playerAudio.clip = doubleJumpSound;
                playerAudio.Play();
                canDoubleJump = false; 
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(TeleportDash());
        }

        if (Input.GetKeyDown(KeyCode.C) && playerWeaponEnabled)
        {
            TogglePlayerAbilities.ToggleInsomniaBeamEmitter();
        }
    }

    private IEnumerator TeleportDash() {
        if (canDash)
        {
            canDash = false;
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
            Vector3 rotatedDirection = transform.rotation * inputDirection;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rotatedDirection, out hit, teleportDistance, groundMask))
            {
                Vector3 teleportPosition = hit.point + hit.normal * 0.1f;
                teleportPosition.y = transform.position.y;
                transform.position = teleportPosition;
            }
            else
            {
                Vector3 teleportPosition = transform.position + rotatedDirection * teleportDistance;
                transform.position = teleportPosition;
            }
            playerAudio.clip = teleportSound;
            playerAudio.Play();
            yield return new WaitForSeconds(1.5f);
            canDash = true;
        }
    }
}
