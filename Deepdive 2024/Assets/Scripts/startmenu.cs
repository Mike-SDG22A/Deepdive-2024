using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;


public class startmenu : MonoBehaviour
{
    public GameObject timer;
    public Canvas Startmenu;
    public Canvas setings;
    public Canvas cirquitselector;

    public Button start;
    public Button Setings;
    public Button quit;
    public Button lvl1;
    public Button back;

    void Start()
    {
        Startmenu.gameObject.SetActive(true);
    }

    void Update()
    {
    }

    public void StartGame()
    {
        cirquitselector.gameObject.SetActive(true);
        Startmenu.gameObject.SetActive (false);

    }
    public void lvlone()
    {
        cirquitselector.gameObject.SetActive(false);
    }

    public void seting()
    {
        Startmenu.gameObject.SetActive(false);
        setings.gameObject.SetActive(true);
    }

    public void Back()
    {
        Startmenu.gameObject.SetActive(true);
        setings.gameObject.SetActive(false);

    }
    public void Quit()
    {        

        // Sluit de toepassing af (werkt in build hopelijk)
        Application.Quit();
    }

}
