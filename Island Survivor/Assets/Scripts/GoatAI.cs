using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//https://www.youtube.com/watch?v=xtJgi8SblIk
public class GoatAI : MonoBehaviour
{
    GameObject character;
    NavMeshAgent goat; //attacker (a very aggressive goat)
    [SerializeField] LayerMask groundLayer, playerLayer;

    //goat will be peacefully roaming before it senses the player
        //Vector3 destPt; //walking towards this point
        //bool walkPtSet; //does the goat already have a destination?
        //[SerializeField] float walkRange;
    Animator animator;
    public float moveSpeed = 0.2f;
    Vector3 stopPosition;
    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    int WalkDirection;
    public bool checkIsWalking;

    //state change from roaming to defend/attack
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    // Start is called before the first frame update
    void Start(){
        goat = GetComponent<NavMeshAgent>();
        character = GameObject.Find("Character");

        //for goat's initial movement
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
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer); //check a radius of a sphere for the player
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer); //check a radius of a sphere for the player
        if(!playerInSight && !playerInAttackRange){
            Roam();
        }//end of if
        if(playerInSight && !playerInAttackRange){ //chase so goat can attack
            Debug.Log("Position: " + transform.position + " | Velocity: " + goat.velocity);
            Chase();
        }//end of if
        if(playerInSight && playerInAttackRange){
            Attack();
        }//end of if
    }//end of Update

    void Roam(){
        if(!checkIsWalking){
            waitCounter -= Time.deltaTime;
            if(waitCounter <= 0){
                ChooseDirection(); //choose a random direction to walk in
            }//end of if
        }//end of if
        else{
            walkCounter -= Time.deltaTime;
            if(walkCounter <= 0){
                stopPosition = transform.position; //stop when walk time is over
                checkIsWalking = false;
                waitCounter = waitTime;
                goat.SetDestination(transform.position); //stop NavMeshAgent from moving
            }//end of if
        }//end of else
    }//end of Roam

    //chase the player
    void Chase(){
        goat.SetDestination(character.transform.position); //walk towards player
    }//end of Chase

    //attack the player in self defense move
    void Attack(){

    }//end of Attack

    /*
    void SearchForDest(){
        float z = Random.Range(-walkRange, walkRange);
        float x = Random.Range(-walkRange, walkRange);
        destPt = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        //check if destination is possible
        if(Physics.Raycast(destPt, Vector3.down, groundLayer)){
            walkPtSet = true;
        }//end of if
    }//end of SearchForDest
    */

    public void ChooseDirection()
    {
        /*
         * picks a random number from 0-3
         * possible directions to face: forward, backwards, left, and right
         */
        WalkDirection = Random.Range(0, 4);

        checkIsWalking = true;
        walkCounter = walkTime;
    }//end of ChooseDirection
}//end of GoatAI




