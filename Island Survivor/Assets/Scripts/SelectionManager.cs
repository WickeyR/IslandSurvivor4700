using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; internal set; }
    public GameObject InteractionInfoUI; //allows us to turn UI element on and off
    Text interaction_text; //changing the text value directly in the Inspector
    public bool onTarget; //cursor pointing at object check
    public GameObject selectedObject; //the object the cursor is pointing to
    public float choppingDistance = 3f; //player must be in this range to chop a tree down
    public float goatDistance = 3f; //player must be very close to the goat to hit it

    private void Start(){
        onTarget = false;
        interaction_text = InteractionInfoUI.GetComponent<Text>(); //assigned to internal text component
    }//end of start

    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //casts a ray from the center of the screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        { //checks if the ray hit anything and stores information in hit if something was hit
            var selectionTransform = hit.transform; //grabs hit object
            InteractableObject thing = selectionTransform.GetComponent<InteractableObject>();

            if (thing != null && thing.playerInRange)
            {
                //grabs the item name to display
                onTarget = true;
                selectedObject = thing.gameObject;
                interaction_text.text = thing.GetItemName();
                InteractionInfoUI.SetActive(true);
            }//end of if
            else
            {//hit but no script on item
                onTarget = false;
                InteractionInfoUI.SetActive(false); //shows nothing if object is not interactable
            }//end of else
        }//end of if
        else{ //deactivate text if nothing is hit (i.e., sky)
            onTarget = false;
            InteractionInfoUI.SetActive(false);
        }

        //for chopping down trees
        if (Input.GetMouseButtonDown(0))
        { //use left click to chop (0-left, 1-right, 2-scroller)
            //chop tree if player is within chopping distance and it is an interactable tree
            if (hit.collider.CompareTag("Tree") && hit.distance <= choppingDistance)
            {
                TreeDrops drops = hit.collider.GetComponent<TreeDrops>(); //get the tree component
                if (drops != null)
                {
                    drops.Chop(); //call chopping method to hit tree
                }//end of if
            }//end of if
        }//end of if

        /* Currently not working due to animation issues (?)
        //for hitting goats
        if (Input.GetMouseButtonDown(0)){
            if (hit.collider.CompareTag("Goat") && hit.distance <= choppingDistance){
                GoatDrops meat = hit.collider.GetComponent<GoatDrops>(); //get the tree component
                if (meat != null){
                    meat.Swing(); //call hitting goat method
                }//end of if
            }//end of if
        }//end of if
        */
    }//end of Update
}//end of SelectionManager