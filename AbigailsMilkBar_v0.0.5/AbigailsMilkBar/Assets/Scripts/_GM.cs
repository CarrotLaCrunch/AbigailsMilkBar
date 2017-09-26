using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Class to manage menu functions save/load 
// in Game Time and money will be managed here as-well


// this class matched client with worker (with mouse input obv)

public class _GM : MonoBehaviour {

    public float sleepTime;
    //tweakable time in MINUTES
    public float spawnGap;
    //tweakable time in MINUTES
    public float maxWait;
    //tweakable time in MINUTES
    public float workerSpawnAgainAfter;

    // Player's money
    public long money;
    

    public GameObject[] _clients;
    public GameObject[] _workers;

    public float startTime;
    public float lastSpawnTime;

    public GameObject matchMe1;
    public GameObject matchMe2;
    public GameObject stairs;
    

    public _GM()
    {
        workerSpawnAgainAfter = 2f;
        spawnGap = 1.5f;
        maxWait = 1f;
    }


	// Use this for initialization
	void Start () {
        sleepTime = 5f;
        startTime = Time.time;
        lastSpawnTime = 0f;

        _clients=GameObject.FindGameObjectsWithTag("client");
        _workers=GameObject.FindGameObjectsWithTag("worker");

        foreach (GameObject o in _clients)
        {
            
            o.SetActive(false);

        }

    }
	

    // gets right clicked objects and tries if they are paired ready to go upstairs 
    void checkPairing()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool goAhead = false;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "worker")
                {
                    matchMe1 = null;
                    matchMe1 = hit.transform.gameObject;

                    if (matchMe1.GetComponent<Worker>().occupied)
                        matchMe1 = null;
                }
                else if (hit.transform.tag == "client")
                {
                    matchMe2 = null;
                    matchMe2 = hit.transform.gameObject;

                    if (matchMe2.GetComponent<Client>().occupied)
                        matchMe2 = null;

                }
                else if (hit.transform.tag == "stairs")
                {
                    goAhead = true;
                }
            }


            if (matchMe1 != null && matchMe2 != null && goAhead)
            {
                Debug.Log("you've made a pair!");
                matchMe1.GetComponent<Worker>().occupied = true;
                matchMe1.GetComponent<Worker>().floatyMove = true;

                matchMe2.GetComponent<Client>().occupied = true;
                matchMe2.GetComponent<Client>().floatyMove = true;


                matchMe1 = matchMe2 = null;
            }



        }
    }

    // disables a pair when target is reached
    void disablePair()
    {
        GameObject G1 = null, G2 = null;
        foreach (GameObject c in _clients)
        {
            if (c.GetComponent<Client>().floatyMove == false && c.GetComponent<Client>().occupied == true && c.GetComponent<Client>().enabled == true)
            {
                G1 = c;
                break;
            }
        }
        foreach (GameObject c in _workers)
        {
            if (c.GetComponent<Worker>().floatyMove == false && c.GetComponent<Worker>().occupied == true && c.GetComponent<Worker>().enabled == true)
            {
                G2 = c;
                break;
            }
        }
        if (G1 != null && G2 != null)
        {
            G1.GetComponent<Client>().enabled = false;
            G1.GetComponent<SpriteRenderer>().enabled = false;

            G2.GetComponent<Worker>().enabled = false;
            G2.GetComponent<SpriteRenderer>().enabled = false;
            G2.GetComponent<Worker>().deadAt = Time.time;
        }

    }

    void enableClient()
    {
        
        if (Time.time - lastSpawnTime >= spawnGap*60)
        {
            
            lastSpawnTime = Time.time;
          
            // spawn a client (enable game object)
            foreach (GameObject o in _clients)
            {
               
                if (o != null) {
                    
                    if (!o.activeSelf)
                    {
                         
                        o.SetActive(true);
                        o.GetComponent<Client>().spawnTime = lastSpawnTime;
                        break;
                    }
                }

            }

        }
    }

    void disableClients()
    {
        foreach (GameObject o in _clients)
        {
            if (o != null)
            {

                if (o.activeSelf)
                {


                    if (o.GetComponent<Client>().waitingTime >= maxWait * 60)
                    {
                        o.GetComponent<Client>().waitingTime = 0;
                        o.SetActive(false);
                    }
                    
                }
            }
        }
    }

    void reSpawnWorker()
    {


        foreach (GameObject c in _workers)
        {
            if (!c.GetComponent<Worker>().enabled)
            {
                if(c.GetComponent<Worker>().deadTime>=workerSpawnAgainAfter*60)
                {
                    c.GetComponent<Worker>().spawnAgain();
                    c.GetComponent<Worker>().enabled = true;
                    c.GetComponent<SpriteRenderer>().enabled = true;
                    c.GetComponent<Worker>().deadAt = 0;
                }
            }
        }


    }

    void ammendWorkerDeadTime()
    {
        foreach (GameObject c in _workers)
        {
            if (!c.GetComponent<Worker>().enabled)
            {
                c.GetComponent<Worker>().deadTime = Time.time - c.GetComponent<Worker>().deadAt;
            }
        }
    }

    void workersSleepingTimeAdjust()
    {
        foreach (GameObject c in _workers)
        {
            if (!c.activeSelf) //check sleeping workers
            {
                c.GetComponent<Worker>().sleepingFor = Time.time - c.GetComponent<Worker>().sleptAt;
            }
        }
        
    }

    private void OnGUI()
    {
        //IF all girls are sleeping can select next day
        int counter = 0;
        foreach (GameObject c in _workers)
        {
            if (!c.activeSelf || c.GetComponent<Worker>().occupied  )
            {
                counter++;   
            }
        }
        if(counter == _workers.Length)
        {
            // all girls are sleeping or occupied (upstairs)


            //show button
            if (GUI.Button(new Rect(Screen.width-(Screen.width/4), Screen.width - (Screen.width / 4), 90, 30), "Next Day"))
                nextDay();  // next day on click




        }

    }

    void reSpawnWorkerExplicit()
    {


        foreach (GameObject c in _workers)
        {
            if (!c.GetComponent<Worker>().enabled)
            {
                if ( c.GetComponent<Worker>().occupied )
                {
                    c.GetComponent<Worker>().spawnAgain();
                    c.GetComponent<Worker>().enabled = true;
                    c.GetComponent<SpriteRenderer>().enabled = true;
                    c.GetComponent<Worker>().deadAt = 0;
                }
            }
        }


    }


    void clientsDespawnExplicit()
    {
        foreach (GameObject o in _clients)
        {
            o.SetActive(false);
            o.GetComponent<Client>().enabled = true;
            o.GetComponent<SpriteRenderer>().enabled = true;
            o.GetComponent<Client>().resetPref();
            o.GetComponent<Client>().resetBasics();
           

        }
    }



    void nextDay()
    {
        // next day is just clients despawned and workers respawned
        reSpawnWorkerExplicit();
        wakeWorkersExplicit();
        clientsDespawnExplicit();
    }

    void wakeWorkersExplicit()
    {
        foreach (GameObject c in _workers)
        {
                    
                    c.GetComponent<Worker>().ResetEnergy(); // reset energy
                    c.SetActive(true); // wakes worker up
        }
    }


    void wakeWorkers()
    {
        foreach (GameObject c in _workers)
        {
            if (!c.activeSelf) //check sleeping workers
            {
                if(c.GetComponent<Worker>().sleepingFor >= sleepTime*60 )  // sleeping for more than sleep time
                {
                    
                    c.GetComponent<Worker>().ResetEnergy(); // reset energy
                    c.SetActive(true); // wakes worker up
                    
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {

       // Debug.Log("secs passed: "+Time.time);

        disableClients(); // Clients despawns on wait of >1 min

        checkPairing(); // gets right clicked objects and tries if they are paired ready to go upstairs 

        disablePair();  // disables a pair when target is reached


        enableClient();

        ammendWorkerDeadTime();

        reSpawnWorker();

        workersSleepingTimeAdjust();

        wakeWorkers();



    }
}
