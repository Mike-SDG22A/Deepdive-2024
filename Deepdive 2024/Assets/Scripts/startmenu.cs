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

    public Camera main;
    public Camera inside;
    public Camera outside;

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
        outside.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        customization.gameObject.SetActive(true);
        Startmenu.gameObject.SetActive(false);
    }
    public void Back()
    {
        main.gameObject.SetActive(true);
        outside.gameObject.SetActive(false);
        Startmenu.gameObject.SetActive(true);
        setings.gameObject.SetActive(false);
        cirquitselector.gameObject.SetActive(false);

    }

    public void Extirior ()
    {
        customization.gameObject.SetActive(false);
        extirior.gameObject.SetActive(true);
    }
    public void Intirior ()
    {
        inside.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        customization.gameObject.SetActive(false);
        intirior.gameObject.SetActive(true);
    }

    public void backc()
    {
        inside.gameObject.SetActive(false);
        main.gameObject.SetActive(false);
        outside.gameObject.SetActive(true);
        intirior.gameObject.SetActive(false);
        extirior.gameObject.SetActive(false);
        customization.gameObject.SetActive(true);
            }

    public void Quit()
    {        
        Application.Quit();
    }

}
