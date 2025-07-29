using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    Animator animator;

    public float moveSpeed = 0.2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    int WalkDirection;

    public bool checkIsWalking;

    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();

        //give the animal random walk and wait durations so that they're not all do the same thing
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);
        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection(); //chooses starting direction
    }//end of Start

    // Update is called once per frame
    void Update(){
        if (checkIsWalking)
        {
            animator.SetBool("isWalking", true);

            walkCounter -= Time.deltaTime;

            //resets the rotation of the creature's positioning based on WalkDirection
            switch (WalkDirection){
                case 0:
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0f, 90, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0f, -90, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0f, 180, 0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }//end of switch

            //will continue to walk until the walkCounter reaches 0
            if (walkCounter <= 0){
                //switches position back to default idle position
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                checkIsWalking = false;
                //stop movement
                transform.position = stopPosition;
                animator.SetBool("isWalking", false);
                //reset the waitCounter
                waitCounter = waitTime;
            }//end of if
        }//end of if
        //will not walk until the waitCounter reaches 0
        else{
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0){
                ChooseDirection();
            }//end of if
        }//end of else
    }//end of Update


    public void ChooseDirection(){
        /*
         * picks a random number from 0-3
         * possible directions to face: forward, backwards, left, and right
         */
        WalkDirection = Random.Range(0, 4);

        checkIsWalking = true;
        walkCounter = walkTime;
    }//end of ChooseDirection
}//end of AIMovement