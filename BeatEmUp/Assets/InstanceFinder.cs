using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceFinder : MonoBehaviour
{

    public static InstanceFinder instance;

    private void Awake()
    {
        if(instance!= null)
        {
            instance = this;

        }
    }

    public Transform collectibleDestination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
