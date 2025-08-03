using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
    public GameObject inventoryScreenUI; //reference to UI
    public bool isOpen; //check if inventory screen is open
    //public bool isFull; //check if inventory is full
    public List<GameObject> slots = new List<GameObject>(); //slots in the inventory (not the object)
    public List<string> itemList = new List<string>(); //names of items in slots
    private GameObject addedItem; //item you want to add into the inventory
    private GameObject equippedSlot; //slot you want to put the item in

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }//end of if
        else{
            Instance = this;
        }//end of else
    }//end of Awake

    void Start(){
        isOpen = false;
        populateSlotList();
    }//end of Start

    //attach game objects to the slots in the list
    private void populateSlotList(){
        //searching for transforms in inventoryScreenUI's transform
        foreach(Transform child in inventoryScreenUI.transform){
            if (child.CompareTag("Slot")){
                slots.Add(child.gameObject); //add if child of inventoryUI is a slot
            }//end of if
        }//end of foreach
    }//end of PopulateSlotList

    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && !isOpen){ //check if E key is pressed and if the inventory system is opened
            //Debug.Log("E is pressed"); //check if worked
            inventoryScreenUI.SetActive(true); //becomes visible
            isOpen = true;
            Cursor.lockState = CursorLockMode.None; //can use mouse
            Cursor.visible = true;
        }//end of if
        else if (Input.GetKeyDown(KeyCode.E) && isOpen){ //will close inventory
            inventoryScreenUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked; //can not use mouse
            Cursor.visible = false;
        }//end of else if
    }//end of Update

    public void addToInventory(string itemName){
        equippedSlot = nextEmptySlot();
        addedItem = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), equippedSlot.transform.position, equippedSlot.transform.rotation);
        addedItem.transform.SetParent(equippedSlot.transform); //set item as a child of the slot
        itemList.Add(itemName);
    }//end of addToInventory

    public bool checkFull(){
        int counter = 0; //number of filled slots
        foreach(GameObject slot in slots){
            if(slot.transform.childCount > 0){
                counter += 1; //increase if slot has a child
            }//end of if
        }//end of foreach
        if(counter == 24) //amount of slots
        {
            return true;
        }//end of if
        else
        {
            return false;
        }//end of if
    }//end of checkFull

     GameObject nextEmptySlot(){
        foreach(GameObject slot in slots){
            if(slot.transform.childCount == 0){ //no children = empty
                return slot;
            }//end of if
        }//end of foreach
        return new GameObject();
    }//end of nextEmptySlot
}//end of InventorySystem class
