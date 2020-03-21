using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Image image;
    public GameObject Canvas;
    public 
    void Start()
    {
        
    }
    void Update()
    {
        GameObject tempPrefab = Canvas.GetComponent<PathUISelect>().prefabClone;
        string a = tempPrefab.GetComponent<AnnCar>().valueSignal.ToString();
        if(a=="red")
            image.color = Color.red;
        if (a == "yellow")
            image.color = Color.yellow;
        if (a == "green")
            image.color = Color.green;
        if (a == "none")
            image.color = Color.white;
    }
    // Update is called once per frame
  
    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
