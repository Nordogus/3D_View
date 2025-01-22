using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchModel : MonoBehaviour
{

    [SerializeField]
    GameObject[] models;
    GameObject lasObject;

    private void Start()
    {
        lasObject = models[Random.Range(0, models.Length)];
        lasObject.SetActive(true);
    }

    public void OnSwitchModel(string cmd)
    {
        Debug.Log(cmd);
        if(cmd != "!switch") return;

        if(lasObject != null)
            lasObject.SetActive(false);
        lasObject = models[Random.Range(0, models.Length)];
        lasObject.SetActive(true);
    }
}
