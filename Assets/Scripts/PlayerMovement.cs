using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Playercontroller;
    public float movespeeed = 10f;
    public float runspeed = 10f;
    public float walkspeed = 5f;
    public float g;
    public Vector3 velocity;
    // private AudioSource AudioSource;
    // public AudioClip shootSound;
    public LayerMask ground;
    public bool onGround;
    public Transform CheckSpherePos;
    public float checkRadius;
    public float jumpHeight;
    public bool isWalking;
    // void Start()
    // {
    //     AudioSource = GetComponent<AudioSource>();
    // }
    void Update()
    {


        if (Input.GetKey(KeyCode.LeftShift))
        {
            movespeeed = runspeed;
            isWalking = false;

        }
        else
        {
            movespeeed = walkspeed;
            isWalking = true;

        }

        onGround = Physics.CheckSphere(CheckSpherePos.position, checkRadius, ground);
        if (onGround == true)
        {
            velocity.y = -1f;

        }
        else
        {
            velocity.y -= g * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            velocity.y = jumpHeight;


        }
        Playercontroller.Move(velocity);
        float x = Input.GetAxis("Horizontal") * movespeeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * movespeeed * Time.deltaTime;
        Playercontroller.Move(transform.forward * y);
        Playercontroller.Move(transform.right * x);
        // if (x != 0 || y != 0)
        // {

        //     StartCoroutine(SwitchAfterDelay());
        // }

    }
    // private IEnumerator SwitchAfterDelay()
    // {
    //     PlayShootSound();
    //     yield return new WaitForSeconds(1);
    //     PlayShootSound();
    // }
    // private void PlayShootSound()
    // {


    //     AudioSource.PlayClipAtPoint(shootSound, new Vector3(5, 5, 5));

    //     // AudioSource.clip = shootSound;
    //     // AudioSource.Play();
    // }
}
