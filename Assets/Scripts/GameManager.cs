using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager _gm;
    public static float calibration = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _gm = this;
        DontDestroyOnLoad(_gm);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void LoadStart()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void setCalibration(float c)
    {
        calibration = Mathf.Round(c*10f)/10f;
    }
}
