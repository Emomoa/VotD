using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LabyrinthCreator : MonoBehaviour
{
    public Transform firstLabyrinthModuleStartPosition; // Koppla en empty till den här där första labyrinten ska börja. 
    public GameObject door;
    [Header("Labyrinth Modules: ")]
    public GameObject[] room1Modules; // tutorial labyrint modulerna
    public GameObject[] room2Modules;
    public GameObject[] room3Modules;
    public GameObject[] room4Modules;

    [Header("Room Id, (1-4)")]
    public int labyrintToCreate = 1; // rum 1 är tutorial, 2 är nästa labyrint sen 3 och 4


    void Start()
    {
        CreateLabyrinth();

    }


    void CreateLabyrinth()
    {
        GameObject[] roomModulesToPickFrom;
        if(labyrintToCreate==1)
        {
            roomModulesToPickFrom = room1Modules;
        }else if(labyrintToCreate==2)
        {
            roomModulesToPickFrom = room2Modules;
        }else if(labyrintToCreate==3)
        {
            roomModulesToPickFrom = room3Modules;
        }
        else
        {
            roomModulesToPickFrom = room4Modules;
        }
        //
        int module1Index = Random.Range(0, roomModulesToPickFrom.Length);
        int module2Index = Random.Range(0, roomModulesToPickFrom.Length);
        while(module1Index == module2Index)
        {
            module2Index = Random.Range(0, roomModulesToPickFrom.Length);
        }
        if( (roomModulesToPickFrom[module1Index] != null) && (roomModulesToPickFrom[module2Index] != null) )
        {
            GameObject mod1 = Instantiate(roomModulesToPickFrom[module1Index],firstLabyrinthModuleStartPosition.position,firstLabyrinthModuleStartPosition.rotation);
            GameObject mod2 = Instantiate(roomModulesToPickFrom[module2Index],mod1.GetComponent<Module>().endPoint.position,mod1.GetComponent<Module>().endPoint.rotation);

            Instantiate(door,mod2.GetComponent<Module>().endPoint.position, roomModulesToPickFrom[module2Index].GetComponent<Module>().endPoint.rotation);


            // sen ska även waypoints placeras ut, fast waypoints ska kanske redan vara placerade i modulerna?

        }
        else
        {
            print("Något blev fel i CreateLabyrinth metoden");
        }
        
    }



}
