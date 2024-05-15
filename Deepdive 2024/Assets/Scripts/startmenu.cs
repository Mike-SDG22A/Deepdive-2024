using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class startmenu : MonoBehaviour
{
    public GameObject timer;
    public Canvas Startmenu;
    public Canvas setings;
    public Canvas cirquitselector;
    public Canvas customization;
    public Canvas intirior;
    public Canvas extirior;

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
    public void LoadScene(int sceneIndex)
    {
        cirquitselector.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneIndex);
    }

    public void seting()
    {
        Startmenu.gameObject.SetActive(false);
        setings.gameObject.SetActive(true);
    }
    public void Customization () 
    {
        customization.gameObject.SetActive(true);
        Startmenu.gameObject.SetActive(false);
    }
    public void Back()
    {
        Startmenu.gameObject.SetActive(true);
        setings.gameObject.SetActive(false);
    }

    public void Extirior ()
    {
        customization.gameObject.SetActive(false);
        extirior.gameObject.SetActive(true);
    }
    public void Intirior ()
    {
        customization.gameObject.SetActive(false);
        intirior.gameObject.SetActive(true);
    }

    public void backc()
    {
        intirior.gameObject.SetActive(false);
        extirior.gameObject.SetActive(false);
        customization.gameObject.SetActive(true);
            }

    public void Quit()
    {        
        Application.Quit();
    }

}
