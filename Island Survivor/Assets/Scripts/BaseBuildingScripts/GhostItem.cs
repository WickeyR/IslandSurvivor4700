using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostItem : MonoBehaviour
{
    public BoxCollider solidCollider; //set manually
    public Renderer mRenderer;
    private Material semiTransparentMat; //Used for debug - instead of the full transparent
    private Material fullTransparentMat;
    private Material selectedMaterial;
    public bool isPlaced;
    // A flag for the deletion algorithm
    public bool hasSamePosition = false;

    private void Start(){
        mRenderer = GetComponent<Renderer>();
        semiTransparentMat = ConstructionSystem.Instance.ghostSemiTransparentMat;
        fullTransparentMat = ConstructionSystem.Instance.ghostFullTransparentMat;
        selectedMaterial = ConstructionSystem.Instance.ghostSelectedMat;
        mRenderer.material = semiTransparentMat; //change to semi if in debug else full
        //We disable the solid box collider - while it is not yet placed
        //(unless we are in construction mode - see update method)
        solidCollider.enabled = false;
    }//end of Start

    private void Update(){
        if (ConstructionSystem.Instance.inConstructionMode){
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ConstructionSystem.Instance.player.GetComponent<Collider>()); //diable player collision with ghosts if in construction mode
        }//end of if

        //We need the solid collider so the ray cast will detect it
        if (ConstructionSystem.Instance.inConstructionMode && isPlaced){
            solidCollider.enabled = true;
        }//end of if
        if (!ConstructionSystem.Instance.inConstructionMode){
            solidCollider.enabled = false;
        }//end of if
        //Triggering the material
        if (ConstructionSystem.Instance.selectedGhost == gameObject){ //check if the ghost is the selected ghost
            mRenderer.material = selectedMaterial; //Green 
        }//end of if
        else{
            mRenderer.material = fullTransparentMat; //change to semi transparent if in debug else full
        }//end of else
    }//end of Update
}//end of GhostItem
