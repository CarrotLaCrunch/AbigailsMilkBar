using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Worker : MonoBehaviour {
    //sleptAt
    public float sleptAt;
    public float sleepingFor;

    Vector2 buttonPos;
    bool buttonPosGrabbed;

    public bool showSleepButton;

    //re spawn 
    public Transform[] reSpawnPoints;
    public GameObject reSpawns;

    // STATS MENU TEXTURE
    public Texture menuTexture;

    public float deadTime;
    public float deadAt;

    public GameObject clients;
    public GameObject workers;

    public bool mouseOver;

    public Stats stats;
    public int   Level;
    public long  Experience;
    public long  MaxExperienceTier;
    public long  Energy;
    public long  MaxEnergy;

    public long[] _ExperienceMaxArr;
    public long[] _EnergyMaxArr;


    public bool hideSTATS;
    
    // classes bool is to store true only on those index where worker character/type lies
    public bool[] classes;


    // true when worker is occupied
    public bool occupied;
    public bool floatyMove;
    public float speed;

    // float to target for now
    public Transform target;


    public Worker()
    {
        buttonPosGrabbed = false;
        showSleepButton = false;
        sleepingFor = 0f;
        sleptAt = 0f;
        deadAt = 0f;
        deadTime = 0f;
        mouseOver = false;

        occupied = false;

        floatyMove = false;


        hideSTATS = true;

        // Initialises worker's stats with stats default constructor
        stats = new Stats();
        Level = 1;
        Experience = 0;

        _ExperienceMaxArr = new long[9];
        _ExperienceMaxArr[0] = 100; _ExperienceMaxArr[1] = 100;
        _ExperienceMaxArr[2] = 250; _ExperienceMaxArr[3] = 500;
        _ExperienceMaxArr[4] = 1000;_ExperienceMaxArr[5] = 2500;
        _ExperienceMaxArr[6] = 5000;_ExperienceMaxArr[7] = 7500;
        _ExperienceMaxArr[8] = 10000;

        MaxExperienceTier = _ExperienceMaxArr[Level-1 ];



        _EnergyMaxArr = new long[9]; // 9 MAX energies for v0.01
        
        _EnergyMaxArr[0] = 100; _EnergyMaxArr[1] = 150;
        _EnergyMaxArr[2] = 200; _EnergyMaxArr[3] = 300;
        _EnergyMaxArr[4] = 500; _EnergyMaxArr[5] = 750;
        _EnergyMaxArr[6] = 1000; _EnergyMaxArr[7] = 10000;
        _EnergyMaxArr[8] = 99999;

        // worker gets max energy according to its level
        Energy = MaxEnergy = _EnergyMaxArr[Level - 1];
        // These classes have no major effect currently in v0.01 and are 9 for now
        classes = new bool[10];
        for (int i = 0; i < 10; i++)
            classes[i] = false;
        
    }
 
    // Returns level corresponding to EXP
    public int updateLevel()
    {
        if (Experience < 100)
            return 1;
        else if (Experience < 250)
            return 2;
        else if (Experience < 500)
            return 3;
        else if (Experience < 1000)
            return 4;
        else if (Experience < 2500)
            return 5;
        else if (Experience < 5000)
            return 6;
        else if (Experience < 7500)
            return 7;
        else if (Experience < 10000)
            return 8;
     
        return 9; // Anything greater Stays level 9 for v0.01
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
        return "bad_index";
    }

    // returns string based on worker class/characterType. ((Multiple classes seperated by a comma))
    public string iam(){
        string ret = "";
        for(int i = 0; i < 10; i++)
        {
            if (classes[i])  
                ret += getName(i)+",";
        }
        return ret.TrimEnd(',');  // returns string based on worker class/characterType. ((Multiple classes seperated by a comma))
    }

    //Sets Max EXP bracket for worker
    public void setMaxExp()
    {
        // gets next experience bracket according to current level
        MaxExperienceTier= _ExperienceMaxArr[Level - 1];
    }

    //Sets Max Energy for worker
    public void setMaxEnergy()
    {
        //Worker max energy resets according to its level
        MaxEnergy = _EnergyMaxArr[Level - 1];
    }

    //Increase EXP as work is done
    public void IncExp(long var)
    {

        if (Experience + var >= _ExperienceMaxArr[8])
        {
            Debug.Log("Max Experience!");
            Experience = _ExperienceMaxArr[8];
            
        }
        else
            Experience += var;

        // changing exp changes level   
        Level = updateLevel();
        //changing level changes max energy
        setMaxEnergy();
        // hopping to next level changes max experience required - tier  
        setMaxExp();
    }

    // checks if worker has enough energy for required work
    public bool EnergyLeftForWork(long work)
    {
        if (Energy - work < 0)
            return false; // No
        return true; // yes she can work
    }

    // doing work wastes energy
    public void DecEnergy(long var)
    {
        Energy = -var;
    }

    // sleep/changeDay and get energy to max
    public void ResetEnergy()
    {
        // worker gets max energy according to its level   (after sleeping / another day)
        Energy = _EnergyMaxArr[Level - 1];
    }


    void OnGUI()
    {







        if (showSleepButton)
        {

            if(!buttonPosGrabbed)
                buttonPos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            if (GUI.Button(new Rect(buttonPos.x,buttonPos.y , 90, 30), "Go to Bed"))
                explicitSleep();
            buttonPosGrabbed = true;
            
        }



       



        if (!hideSTATS && mouseOver)
        {

            GUI.DrawTexture(new Rect(0, 0, 100, 190), menuTexture, ScaleMode.StretchToFill, true, 0.0F);


            GUI.Label(new Rect(0, 0, 100, 190),

                "Stats:" +
                "\nAggression: " + stats.Aggression +
                "\nCharisma: " + stats.Charisma +
                "\nDexterity: " + stats.Dexterity +
                "\nIntelligence: " + stats.Intelligence +
                "\nSoftness: " + stats.Softness +
                "\nStrength: " + stats.Strength +
                "\n\nClass:\n" + iam()
                );

        }
        else
            hideSTATS = true;

    }


    // monoBehavior function
    void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            showSleepButton = true;
        }
       


        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }

    // monoBehavior function
    void OnMouseDown()
    {
        
        hideSTATS = false;
        
        
    }

    public Transform getMySpawnPoint()
    {
        int index = -1;
        bool present = true;

        while (present)
        {
            present = false;
            index = Random.Range(0, (reSpawns.transform.childCount) - 1);
            foreach (Transform child in workers.transform)
            {
                if (child.gameObject.activeSelf && reSpawnPoints[index] == child.gameObject.transform)
                {
                    present = true;
                    break;
                }
            }
        }

        return reSpawnPoints[index];
    }


    public void spawnAgain()
    {

        transform.position = getMySpawnPoint().position;
        occupied = floatyMove = false;
        deadTime = 0f;
        IncExp(50);
        DecEnergy(10);
        
    }



    // Use this for initialization
    void Start () {
        //preffered stats attributes of a client
        stats.Aggression = Random.Range(1, 10);
        stats.Charisma = Random.Range(1, 10);
        stats.Dexterity = Random.Range(1, 10);
        stats.Intelligence = Random.Range(1, 10);
        stats.Softness = Random.Range(1, 10);
        stats.Strength = Random.Range(1, 10);






        reSpawnPoints = new Transform[reSpawns.transform.childCount];
        int k = 0;
        foreach (Transform child in reSpawns.transform)
        {
            reSpawnPoints[k] = child.gameObject.transform;
            k++;
        }


    }

    void moveWorker()
    {
        if (floatyMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            if (transform.position == target.position)
                floatyMove = false;
        }
    }

    void workerEnergyCheck()
    {
        if (!floatyMove && !occupied && !EnergyLeftForWork(10)){

            // Automatically goes to sleep
            gameObject.SetActive(false);
            sleptAt = Time.time;
            sleepingFor = 0f;

        }
            
    }

    void explicitSleep()
    {
        sleptAt = Time.time;
        sleepingFor = 0f;
        gameObject.SetActive(false);
      
    }

    void sleepButtonDisable()
    {
        if (Input.GetMouseButton(0))
        {
            showSleepButton = false;
            buttonPosGrabbed = false;
        }

    }

    // Update is called once per frame
    void Update () {

        workerEnergyCheck();


        moveWorker();


        sleepButtonDisable();

    }
}
