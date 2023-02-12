using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    public CinemachineConfiner2D[] confiners;
    private int currentConfinerIndex = 0;

    void Awake()
    {
        // set the first confiner as active at the start of the game
        confiners[currentConfinerIndex].gameObject.SetActive(true);
    }

    public void SwitchConfiner(int stageIdx)
    {
        Debug.Log("Now Index is : ");
        Debug.Log(stageIdx);
        // deactivate the current confiner
        confiners[currentConfinerIndex].gameObject.SetActive(false);

        // set the new confiner as active
        currentConfinerIndex = stageIdx;
        confiners[currentConfinerIndex].gameObject.SetActive(true);
    }
}
