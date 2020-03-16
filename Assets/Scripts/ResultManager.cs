using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{

    public static ResultManager _rm;
    public float totalAccuracy;
    public GameObject totalAccuracyValue, maxComboValue;


    void Start()
    {
        _rm = this;
        Debug.Log(LevelManager._lm.totalAccuracy.ToString());
        GameObject totalAccuracyValue = GameObject.FindGameObjectWithTag("TotalAccuracyValueTag");
        totalAccuracyValue.GetComponent<TextMeshProUGUI>().SetText(LevelManager._lm.totalAccuracy.ToString());
        GameObject.FindGameObjectWithTag("MaxComboValueTag").GetComponent<TextMeshProUGUI>().SetText(LevelManager._lm.maxComboCounter.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        //Press any key to exit level's result screen.
        if (Input.anyKeyDown)
        {
            //Destroy(LevelManager._lm);
            Destroy(GameObject.FindGameObjectWithTag("LevelManagerTag"));
            GameManager._gm.LoadWorld();
        }
    }

    
}
