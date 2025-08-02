using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject InteractionInfoUI; //allows us to turn UI element on and off
    Text interaction_text; //changing the text value directly in the Inspector
    public float choppingDistance = 3f;

    private void Start(){
        interaction_text = InteractionInfoUI.GetComponent<Text>(); //assigned to internal text component
    }//end of start

    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //casts a ray from the center of the screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){ //checks if the ray hit anything and stores information in hit if something was hit
            var selectionTransform = hit.transform; //grabs hit object

            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>()){
                //grabs the item name to display
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                InteractionInfoUI.SetActive(true);
            }//end of if
            else{//hit but no script on item
                InteractionInfoUI.SetActive(false); //shows nothing if object is not interactable
            }//end of else
        }//end of if
        else{ //deactivate text if nothing is hit (i.e., sky)
            InteractionInfoUI.SetActive(false);
        }

        //for chopping down trees
        if (Input.GetMouseButtonDown(0)){ //use left click to chop (0-left, 1-right, 2-scroller)
            //chop tree if player is within chopping distance and it is an interactable tree
            if (hit.collider.CompareTag("Tree") && hit.distance<=choppingDistance){
                TreeDrops drops = hit.collider.GetComponent<TreeDrops>(); //get the tree component
                if (drops != null){
                    drops.Chop();
                }//end of if
            }//end of if
        }//end of if
    }//end of Update
}//end of SelectionManager