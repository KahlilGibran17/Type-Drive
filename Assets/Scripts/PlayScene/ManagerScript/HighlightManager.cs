using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public GameObject sparePart1;
    public GameObject sparePart2;
    public GameObject sparePart3;
    public GameObject sparePart4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)){
            sparePart1.SetActive(true);
            sparePart2.SetActive (false);
            sparePart3.SetActive (false);
            sparePart4.SetActive (false);
          }
        if (Input.GetKeyDown(KeyCode.F2)){
            sparePart1.SetActive (false);
            sparePart2.SetActive (true);
            sparePart3.SetActive (false);
            sparePart4.SetActive (false);
    }
        if (Input.GetKeyDown(KeyCode.F3)){
            sparePart1.SetActive (false);
            sparePart2.SetActive (false);
            sparePart3.SetActive (true);
            sparePart4.SetActive (false);
    }
        if (Input.GetKeyDown(KeyCode.F4)){
            sparePart1.SetActive (false);
            sparePart2.SetActive (false);
            sparePart3.SetActive (false);
            sparePart4.SetActive (true);
    }   
    }
}
