using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDrops : MonoBehaviour
{
    public GameObject woodDrops; //to hold logs prefab that will drop after hitting the tree
    public GameObject fruitDrops; //to hold the fruit dropped from trees
    public int treeLife = 5; //will take 5 hits to take down the tree (tree has 5 life points)
    public int numWoodDrops = 1; //will give 1 pack of 6 logs of wood (count prefab logs image)
    public int numFoodDrops = 1; //will give 1 pack of food
    public AudioSource audioSource; //chopping sound effect when tree is hit
    //public ParticleSystem chopEffect; //will output smoke to let player know tree has been chopped sucessfully

    //Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //chopEffect = GetComponent<ParticleSystem>();
    }//end of Start

    public void Chop(){
        if (audioSource != null){
            audioSource.Play(); //play the chopping sound if there is audio
        }
        treeLife--; //decrease life with each hit

        if (treeLife <= 0){
            killTree();
            Destroy(gameObject); //replace the tree with the wood drops
        }//end of if
    }//end of Chop

    private void killTree(){
        //special effect to let user know tree has been sucessfully chopped
        /*
        if (chopEffect != null){
            chopEffect.Play(); //release smoke
        }
        */
        for(int i=0; i<numWoodDrops; i++){
            Vector3 woodDropPosition = Random.insideUnitSphere * 0.5f; //drop wood at the tree's original position
            woodDropPosition.y = 0; //ensure drop remains on the floor
            Instantiate(woodDrops, transform.position + woodDropPosition, Quaternion.identity);
        }//end of for loop

        for (int j=0; j<numFoodDrops; j++){
            Vector3 fruitDropPosition = Random.insideUnitSphere * 1.75f; //drop wood at the tree's original position
            fruitDropPosition.y = 0.05f; //ensure drop remains on the floor
            Instantiate(fruitDrops, transform.position + fruitDropPosition, Quaternion.identity);
        }//end of for loop
    }//end of killTree

    //Update is called once per frame
    void Update(){
        
    }//end of Update
}//end of WoodDrop
