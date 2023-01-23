using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
        
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(2);

    }

    public void LoadScene3()
    {
        SceneManager.LoadScene(3);

    }
}
