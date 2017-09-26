using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Prefrence
{
    public Stats stats;
     
    // classes bool is to store true only on those index where worker character/type lies
    public bool[] classes;

    public Prefrence()
    {
        stats = new Stats();

        // These classes have no major effect currently in v0.01 and are 9 for now
        classes = new bool[10];
        for (int i = 0; i < 10; i++)
            classes[i] = false;

    }

    // Returns Class/CharacterType corresponding to the index
    public string getName(int index)
    {
        switch (index)
        {
            case 0:
                return "Beast";
            case 1:
                return "Monster";
            case 2:
                return "Golem";
            case 3:
                return "Oni";
            case 4:
                return "Celestial";
            case 5:
                return "Demon";
            case 6:
                return "Sylvan";
            case 7:
                return "Goo";
            case 8:
                return "Myth";
            case 9:
                return "human";
        }
        Debug.Log(index);
        return "BAD_INDEX";
    }

    // returns string based on worker class/characterType. ((Multiple classes seperated by a comma))
    public string iwant()
    {
        string ret = "";
        for (int i = 0; i < 10; i++)
        {
            if (classes[i])
                ret += getName(i) + ",";
        }
        return ret.TrimEnd(',');  // returns string based on worker class/characterType. ((Multiple classes seperated by a comma))
    }

}


public class Client : MonoBehaviour {

    public bool mouseOver;

    public float waitingTime;
    public float spawnTime;

    // STATS MENU TEXTURE
    public Texture menuTexture;

    // true when a client is matched
    public bool occupied;
    public bool floatyMove;
    public float speed;

    // float to target for now
    public Transform target;



    public Prefrence prefrence;
    // all clients are human for now v0.01

    // HIDE/UNHIDE PREFERENCE
    public bool hidePREF;

    public GameObject workers;
    public GameObject clients;

    public GameObject GM;

    //public int spawnPoint; //0 for chair, 1 for somewhere else (i.e. against a wall)
    public Transform[] spawnPoints;
    

    // stool spawn points
    public GameObject parentStool;
    // other spawn points
    public GameObject parentPoints;
    public Client()
    {
        spawnTime = 0f;
        waitingTime =0f;
        mouseOver = false;
        prefrence = new Prefrence();
        hidePREF = true;

        occupied = false;
        floatyMove = false;
    }
    public void resetBasics()
    {
        spawnTime = 0f;
        waitingTime = 0f;
        mouseOver = false;
        hidePREF = true;

        occupied = false;
        floatyMove = false;
    }
    public Transform getMySpawnPoint()
    {
        int index=-1;
        bool present = true;

        while (present) {
            present = false;
            index = Random.Range(0, (parentStool.transform.childCount + parentPoints.transform.childCount) - 1);
            foreach (Transform child in clients.transform)
            {
                if(spawnPoints[index] == child.gameObject.transform)
                {
                    present = true;
                    break;
                }
            }
        }

        return spawnPoints[index];
    }

    // Use this for initialization
    void Start () {

       // spawnPoint = Random.Range(0, 1);
        


        spawnPoints = new Transform[parentStool.transform.childCount    + parentPoints.transform.childCount];
        int k = 0;
        foreach (Transform child in parentStool.transform)
        {
            spawnPoints[k] = child.gameObject.transform;
            k++;
        }
               
        foreach (Transform child in parentPoints.transform)
        {
            spawnPoints[k] = child.gameObject.transform;
            k++;
        }

      
       transform.position= getMySpawnPoint().position;



        //preffered stats attributes of a client
        prefrence.stats.Aggression = Random.Range(1, 10);
        prefrence.stats.Charisma = Random.Range(1, 10);
        prefrence.stats.Dexterity = Random.Range(1, 10);
        prefrence.stats.Intelligence = Random.Range(1, 10);
        prefrence.stats.Softness = Random.Range(1, 10);
        prefrence.stats.Strength = Random.Range(1, 10);




        //preffered classes/character type of a client
        prefrence.classes[Random.Range(0, 9)]=true;
        // preffered index set to true
    }

    public void resetPref()
    {
        prefrence = new Prefrence();
        //preffered stats attributes of a client
        prefrence.stats.Aggression = Random.Range(1, 10);
        prefrence.stats.Charisma = Random.Range(1, 10);
        prefrence.stats.Dexterity = Random.Range(1, 10);
        prefrence.stats.Intelligence = Random.Range(1, 10);
        prefrence.stats.Softness = Random.Range(1, 10);
        prefrence.stats.Strength = Random.Range(1, 10);




        //preffered classes/character type of a client
        prefrence.classes[Random.Range(0, 9)] = true;
        // preffered index set to true
    }


    void OnGUI()
    {
        if (!hidePREF && mouseOver )
        {

            GUI.DrawTexture(new Rect(0, 0, 100, 190), menuTexture, ScaleMode.StretchToFill, true, 0.0F);

            GUI.Label(new Rect(0, 0, 100, 190), 
                "Preference: " +
                "\n\n  --Stats--"+
                "\nAggression: "+ prefrence.stats.Aggression +
                "\nCharisma: " + prefrence.stats.Charisma +
                "\nDexterity: " + prefrence.stats.Dexterity +
                "\nIntelligence: " + prefrence.stats.Intelligence+
                "\nSoftness: " + prefrence.stats.Softness +
                "\nStrength: " + prefrence.stats.Strength +
                "\n\n--Class--\n"+ prefrence.iwant()
                
                );

            
        }
        else
            hidePREF = true;

        

    }

    // monoBehavior function
    void OnMouseOver()
    {
       
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }

    // monoBehavior function
    void OnMouseDown()
    {
        hidePREF = false;
    }

    void moveClient()
    {
        if (floatyMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            if (transform.position == target.position)
                floatyMove = false;
        }
    }

    void updateWaitingTime()
    {
        waitingTime = Time.time - spawnTime;
    }


    // Update is called once per frame
    void Update () {



        moveClient();

        updateWaitingTime();
        

    }
}
