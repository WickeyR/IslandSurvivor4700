using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerInRange;

    //displays object name
    public string GetItemName(){
        return ItemName;
    }//end of GetItemName()

    //pick up the item with left click into inventory
    void Update(){
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange){
            Debug.Log("Item added to inventory"); //delete later
            Destroy(gameObject); //disappear from view and is added to inventory
        }//end of if
    }//end of Update

    //check if player is in range of item
    private void OnTriggerEnter(Collider other){
        //compare other and if player then text will show up
        if (other.CompareTag("Player")){
            playerInRange = true;
        }//end of if
    }//end of OnTriggerEnter

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            playerInRange = false;
        }//end of if
    }//end of OnTriggerExit
}//end of InteractableObject