using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName;
    public bool playerInRange;

    //displays object name
    public string GetItemName(){
        return itemName;
    }//end of GetItemName()

    //pick up the item with left click into inventory
    void Update(){
         if(Input.GetKeyDown(KeyCode.Mouse1) && playerInRange /*&& SelectionManager.Instance.onTarget*/){ //right click to add inventory
            if (InventorySystem.Instance.checkFull()){ //check if inventory is full before adding item
                InventorySystem.Instance.addToInventory(itemName); //add item name to inventory
                Destroy(gameObject); //disappear from view and is added to inventory
            }//end of if
            else{
                Debug.Log("Inventory is full");
            }//end of else
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