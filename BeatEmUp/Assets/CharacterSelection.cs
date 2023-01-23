using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{

    [SerializeField] Transform[] characters;
    [SerializeField] GameObject loaderScene;

    Vector3 offset;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 1.5f, 0);
        index = Random.Range(0, characters.Length);
        transform.position = characters[index].transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = characters[index].transform.position + offset;
        
        if (Input.GetKeyDown("d") && index < characters.Length)
        {
            index += 1;
        }

        if (Input.GetKeyDown("q") && index >= 0)
        {
            index -= 1;
        }

        if (Input.GetKeyDown("d") && index >= characters.Length)
        {
            index = 0;
        }

        if (Input.GetKeyDown("q") && index <= 0)
        {
            index = 4;
        }

        Debug.Log(index);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            loaderScene.GetComponent<SceneLoader>().LoadScene2();
        }

    }
}
