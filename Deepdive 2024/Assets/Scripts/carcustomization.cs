using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carcustomization : MonoBehaviour
{
    public GameObject caroutside;
    public GameObject carinside;

    public Color outsideColor;
    public Color insideColor;

    private Renderer outsideRenderer;
    private Renderer insideRenderer;


    void Start()
    {
        outsideRenderer = caroutside.GetComponent<Renderer>();
        insideRenderer = carinside.GetComponent<Renderer>();


        outsideColor = new Color(PlayerPrefs.GetFloat("OutsideColorR"), PlayerPrefs.GetFloat("OutsideColorG"), PlayerPrefs.GetFloat("OutsideColorB"));
        insideColor = new Color(PlayerPrefs.GetFloat("InsideColorR"), PlayerPrefs.GetFloat("InsideColorG"), PlayerPrefs.GetFloat("InsideColorB"));

        outsideRenderer.material.color = outsideColor;
        insideRenderer.material.color = insideColor;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            SaveColors();
        }
    }

    public void SaveColors()
    {
        PlayerPrefs.SetFloat("OutsideColorR", outsideRenderer.material.color.r);
        PlayerPrefs.SetFloat("OutsideColorG", outsideRenderer.material.color.g);
        PlayerPrefs.SetFloat("OutsideColorB", outsideRenderer.material.color.b);
        PlayerPrefs.SetFloat("InsideColorR", insideRenderer.material.color.r);
        PlayerPrefs.SetFloat("InsideColorG", insideRenderer.material.color.g);
        PlayerPrefs.SetFloat("InsideColorB", insideRenderer.material.color.b);
    }

    public void blue()
    {
        insideRenderer.material.color = Color.blue;
    }

    public void red()
    {
        insideRenderer.material.color = Color.red;
    }
    public void black()
    {
        insideRenderer.material.color = Color.black;
    }
    public void yelow()
    {
        insideRenderer.material.color = Color.yellow;
    }
    public void green()
    {
        insideRenderer.material.color = Color.green;
    }
    public void white()
    {
        insideRenderer.material.color = Color.white;
    }    
    public void mainblue()
    {
        outsideRenderer.material.color = Color.blue;
    }

    public void mainred()
    {
        outsideRenderer.material.color = Color.red;
    }
    public void mainblack()
    {
        outsideRenderer.material.color = Color.black;
    }
    public void mainyelow()
    {
        outsideRenderer.material.color = Color.yellow;
    }
    public void maingreen()
    {
        outsideRenderer.material.color = Color.green;
    }
    public void mainwhite()
    {
        outsideRenderer.material.color = Color.white;
    }
}
