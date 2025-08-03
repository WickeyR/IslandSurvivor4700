using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragNDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public static GameObject item; //item being dragged
    Vector3 startPosition;
    Transform startParent;

    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }//end of Awake

    //what will happen when you start to click and start drag
    public void OnBeginDrag(PointerEventData eventData){
        Debug.Log("OnBeginDrag"); //check if it works
        canvasGroup.alpha = .6f; //make item transparent so you know item has been selected
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false; //prevent item slot from being moved
        startPosition = transform.position; //what is the parent (currently slot 1)
        startParent = transform.parent;
        transform.SetParent(transform.root); //no longer a child of the slot and just in the root 
        item = gameObject;
    }//end of OnBeginDrag

    //what happens during the drag
    public void OnDrag(PointerEventData eventData){
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta; //delta = speed of the mouse
    }//end of OnDrag

    //what happens at the end of the drag
    public void OnEndDrag(PointerEventData eventData){
        item = null;

        //moving the object into new slots
        //will go back to last parent if object is not close enough to another slot
        if (transform.parent == startParent || transform.parent == transform.root){
            transform.position = startPosition;
            transform.SetParent(startParent);
        }//end of if

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f; //no longer transparent
        canvasGroup.blocksRaycasts = true; //don't need this feature anymore
    }//end of OnEndDrag
}//end of ItemDragNDrop class
