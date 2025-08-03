using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
    public GameObject inventoryScreenUI; //reference to UI
    public bool isOpen; //check if inventory screen is open

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
    }//end of Start

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
}//end of InventorySystem class
