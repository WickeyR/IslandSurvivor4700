using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatDrops : MonoBehaviour
{
    public GameObject goatMeatDrops; //hold goat meat prefab
    public int goatHealth = 10; //how many hits it takes to kill the goat
    public int numDrops = 1; //will recieve one piece of meat for killing the goat? undecided
    public AudioSource swordSound; //sword swining sound when attacking

    // Start is called before the first frame update
    void Start(){
        swordSound = GetComponent<AudioSource>();
    }//end of Start

    public void Swing(){
        if(swordSound != null){ //check if sword sound effect is attached
            swordSound.Play(); //play swinging attack noise if there is an effect attacked
        }//end of if
        goatHealth--; //decrease goat's life with each hit landed
        if(goatHealth <= 0){ //goat is dead at 0 health
            KillGoat();
            Destroy(gameObject); //replace with meat drops
        }//end of if
    }//end of Swing

    private void KillGoat(){ //goat is dead so player gets drops
        for(int i=0; i<numDrops; i++){
            Vector3 meatDropPosition = Random.insideUnitSphere * 0.5f; //drop meat at the goat's original position
            meatDropPosition.y = 0; //ensure drop remains on the floor
            Instantiate(goatMeatDrops, transform.position + meatDropPosition, Quaternion.identity);
        }//end of for loop
    }//end of KillGoat
}//end of GoatDrops
