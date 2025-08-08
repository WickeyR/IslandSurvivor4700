using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI; //crafting tools
    //public GameObject boatScreenUI; //crafting the escape boat
    //public GameObject paddleScreenUi; //crafting paddle for boat
    public List<string> inventoryList = new List<string>();
    public bool isOpen; //check if screen is open
    //create reference for the crafting buttons
    Button toolsBTN;
    Button craftHammerBTN;
    //create reference for requirements
    TextMeshProUGUI hammerReq1;
    TextMeshProUGUI hammerReq2;
    //item blueprints
    public ItemBlueprint Hammer;

    //singleton pattern
    public static CraftingSystem Instance { get; set; }
    private void Awake(){
        if(Instance!=null && Instance != this){
            Destroy(gameObject);
        }//end of if
        else{
            Instance = this;
        }//end of else
        //make blueprint
        GameObject bp = new GameObject("Hammer");
        Hammer = bp.AddComponent<ItemBlueprint>();
        Hammer.itemName = "Hammer";
        Hammer.req1 = "Wood (Psst Right Click!)";
        Hammer.req2 = "Rock (Psst Right Click!)";
        Hammer.req1Amount = 1;
        Hammer.req2Amount = 1;
        Hammer.numOfReqs = 2;
    }//end of Awake

    // Start is called before the first frame update
    /*
    void Start(){
        isOpen = false; //keep screen closed at first
        toolsBTN = craftingScreenUI.transform.Find("toolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory();}); //when button is pressed, the tools screen will open
        //for the hammer
        hammerReq1 = toolsScreenUI.transform.Find("Hammer").transform.Find("hammerReq1").GetComponent<Text>();
        hammerReq2 = toolsScreenUI.transform.Find("Hammer").transform.Find("hammerReq2").GetComponent<Text>();
        craftHammerBTN = toolsScreenUI.transform.Find("Hammer").transform.Find("craftHammerBTN").GetComponent<Button>();
        craftHammerBTN.onClick.AddListener(delegate {CraftItem(Hammer);}); //craft item when button is clicked
    }//end of Start
    */

    void Start()
    {
        isOpen = false;

        // Temporarily activate tools screen to allow finding children
        bool wasActive = toolsScreenUI.activeSelf;
        toolsScreenUI.SetActive(true);

        // Do the Find() setup
        toolsBTN = craftingScreenUI.transform.Find("toolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        Transform hammerTransform = toolsScreenUI.transform.Find("Hammer");
        hammerReq1 = hammerTransform.Find("hammerReq1").GetComponent<TextMeshProUGUI>();
        hammerReq2 = hammerTransform.Find("hammerReq2").GetComponent<TextMeshProUGUI>();
        craftHammerBTN = hammerTransform.Find("craftHammerBTN").GetComponent<Button>();

        craftHammerBTN.onClick.AddListener(delegate { CraftItem(Hammer); });

        // Return UI to its previous state (invisible again)
        toolsScreenUI.SetActive(wasActive);
    }


    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        { //check if C key is pressed and if the crafting system is opened
            craftingScreenUI.SetActive(true); //becomes visible
            isOpen = true;
            Cursor.lockState = CursorLockMode.None; //can use mouse
            Cursor.visible = true;
        }//end of if
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        { //will close inventory
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false); //close just in case

            if (!InventorySystem.Instance.isOpen){ //only lock if both screens are closed
                Cursor.lockState = CursorLockMode.Locked; //can not use mouse
                Cursor.visible = false;
            }//end of if

            isOpen = false;
        }//end of else if
    }//end of Update

    private void OpenToolsCategory(){
        craftingScreenUI.SetActive(false); //close crafting screen
        toolsScreenUI.SetActive(true); //open tools crafting screen
        RefreshReqs(); //show most update requirements when tools screen is pulled up
    }//end of OpenToolsCategory

    private void CraftItem(ItemBlueprint craftable){
        InventorySystem.Instance.AddToInventory(craftable.itemName); //add whatever you made into the inventory
        if(craftable.numOfReqs == 1){
            InventorySystem.Instance.RemoveItem(craftable.req1, craftable.req1Amount);//remove used materials from inventory
        }//end of if
        else if(craftable.numOfReqs == 2){
            InventorySystem.Instance.RemoveItem(craftable.req1, craftable.req1Amount);//remove used materials from inventory
            InventorySystem.Instance.RemoveItem(craftable.req2, craftable.req2Amount);//remove used materials from inventory
        }//end of else if
        StartCoroutine(calculate()); //to refresh the list properly
        RefreshReqs(); 
    }//end of CraftItem

    public IEnumerator calculate(){
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.RecalculateList();
    }//end of calculate

    private void RefreshReqs(){
        //crafting materials
        int rockCount = 0; //amount of rocks needed
        int woodCount = 0; //amount of wood need
        inventoryList = InventorySystem.Instance.itemList;

        //list what the player has on the crafting screen
        foreach(string iN in inventoryList){ //iN = item name
            switch(iN){
                case "Rock (Psst Right Click!)":
                    rockCount++;
                    break;
                case "Wood (Psst Right Click!)":
                    woodCount++;
                    break;
            }//end of switch statement
        }//end  of foreach

        //Hammer display
        hammerReq1.text = "One piece of Wood [" + woodCount + "]";
        hammerReq2.text = "One Rock [" + rockCount + "]";
        //check if the player can craft
        if (woodCount >= Hammer.req1Amount && rockCount >= Hammer.req2Amount){
            craftHammerBTN.gameObject.SetActive(true);
        }//end of if
        else{
            craftHammerBTN.gameObject.SetActive(false);
        }//end of else
    }//end of RefreshReqs
}//end of CraftingSystem
