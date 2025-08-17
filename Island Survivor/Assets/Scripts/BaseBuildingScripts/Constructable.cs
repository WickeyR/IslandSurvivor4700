using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructable : MonoBehaviour
{
    //Check the item can be placed there or can be built
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt;
    public bool detectedGhostMember;
    //Materials
    private Renderer mRenderer;
    public Material redMaterial;
    public Material greenMaterial;
    public Material defaultMaterial;
    //List of ghosts
    public List<GameObject> ghostList = new List<GameObject>();
    public BoxCollider solidCollider; //We need to drag this collider manualy into the inspector

    private void Start(){
        mRenderer = GetComponent<Renderer>(); //can change the different materials in run time
        mRenderer.material = defaultMaterial; //make it the default look (wood)
        foreach (Transform child in transform) { //look for ghosts and add them to the list
            ghostList.Add(child.gameObject);
        }//end of foreach
    }//end of Start

    void Update(){
        //check if item is in the appropriate location so that it may be placed
        if (isGrounded && isOverlappingItems == false) {
            isValidToBeBuilt = true;
        }//end of if
        else {
            isValidToBeBuilt = false;
        }//end of else
       /*
       if (gameObject.name == "FoundationModel" || gameObject.name == "FireModel"){
            isValidToBeBuilt = isGrounded && !isOverlappingItems;
       }//end of if
       else if (gameObject.name == "WallModel"){
            isValidToBeBuilt = detectedGhostMember;
       }//end of else if
       else{
            isValidToBeBuilt = false;
       }//end of else
       */
    }//end of Update

    //when the item collides with something
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable")) {
            isGrounded = true; //item is on the ground
            Debug.Log("Hit Ground");
        }//end of if
        if (other.CompareTag("Tree") || other.CompareTag("Rock") || other.CompareTag("Drops") || other.CompareTag("Goat") && gameObject.CompareTag("activeConstructable")) {
            isOverlappingItems = true; //item is touching other items (trees, goats, drops, rocks)
            Debug.Log("Hit Tree");
        }//end of if
        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable")) {
            detectedGhostMember = true; //item is overlapping anothe ghost item
            Debug.Log("Hit Ghost");
        }//end of if
    }//end of OnTriggerEnter

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable")) {
            isGrounded = false; //not on the ground anymore
        }//end of if
        if (other.CompareTag("Tree") || other.CompareTag("Rock") || other.CompareTag("Drops") || other.CompareTag("Goat") && gameObject.CompareTag("activeConstructable")) {
            isOverlappingItems = false; //not overlapping anything
        }//end of if
        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable")) {
            detectedGhostMember = false; //not overlapping anything
        }//end of if 
    }//end of OnTriggerExit

    //turn the item red if it cannot be placed there
    public void SetInvalidColor(){
        if (mRenderer != null){
            mRenderer.material = redMaterial;
        }//end of if
    }//end of SetInvalidColor

    //turn the item green if it can be placed there
    public void SetValidColor(){
        mRenderer.material = greenMaterial;
    }//end of SetValidColor

    //turn the item back to its original color if it can be placed there
    public void SetDefaultColor(){
        mRenderer.material = defaultMaterial;
    }//end of SetDefaultColor
    
    //set all ghost items in the same root hierachry and not as children as whatever item the player is building
    public void ExtractGhostMembers(){
        foreach (GameObject item in ghostList){
            item.transform.SetParent(transform.parent, true);
            item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false; //for when construction is finished, no more ghosts
            item.gameObject.GetComponent<GhostItem>().isPlaced = true;
        }//end of foreach
    }//end of ExtractGhostMembers
}//end of Constructable
