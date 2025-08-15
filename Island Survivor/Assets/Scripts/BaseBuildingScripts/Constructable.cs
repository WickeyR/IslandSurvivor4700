using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructable : MonoBehaviour
{
    //Validation
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt;
    public bool detectedGhostMemeber;

    //Material related
    private Renderer mRenderer;
    public Material redMaterial;
    public Material greenMaterial;
    public Material defaultMaterial;

    public List<GameObject> ghostList = new List<GameObject>();

    public BoxCollider solidCollider; //We need to drag this collider manualy into the inspector

    private void Start(){
        mRenderer = GetComponent<Renderer>();

        mRenderer.material = defaultMaterial;
        foreach (Transform child in transform){
            ghostList.Add(child.gameObject);
        }//end of foreach
    }//end of Start

    void Update(){
        if (isGrounded && isOverlappingItems == false){
            isValidToBeBuilt = true;
        }//end of if
        else{
            isValidToBeBuilt = false;
        }//end of else
    }//end of Update

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable")){
            isGrounded = true;
        }//end of if
        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable")){
            isOverlappingItems = true;
        }//end of if
        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable")){
            detectedGhostMemeber = true;
        }//end of if
    }//end of OnTriggerEnter

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Ground") && gameObject.CompareTag("activeConstructable")){
            isGrounded = false;
        }//end of if
        if (other.CompareTag("Tree") || other.CompareTag("pickable") && gameObject.CompareTag("activeConstructable")){
            isOverlappingItems = false;
        }//end of if
        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable")){
            detectedGhostMemeber = false;
        }//end of if
    }//end of OnTriggerExit

    public void SetInvalidColor(){
        if (mRenderer != null){
            mRenderer.material = redMaterial;
        }//end of if
    }//end of SetInvalidColor

    public void SetValidColor(){
        mRenderer.material = greenMaterial;
    }//end of SetValidColor

    public void SetDefaultColor(){
        mRenderer.material = defaultMaterial;
    }//end of SetDefaultColor

    public void ExtractGhostMembers(){
        foreach (GameObject item in ghostList){
            item.transform.SetParent(transform.parent, true);
            //  item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
            item.gameObject.GetComponent<GhostItem>().isPlaced = true;
        }//end of foreach
    }//end of ExtractGhostMembers
}//end of Constructable