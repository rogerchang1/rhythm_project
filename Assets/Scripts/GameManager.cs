using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager _gm;
    public static float calibration = 0f;
    public string songNameToLoad;

    void Awake()
    {
        if (!_gm)
        {
            _gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void LoadStart()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void LoadWorld()
    {
        SceneManager.LoadScene("WorldScene");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void setCalibration(float c)
    {
        calibration = Mathf.Round(c*1000f)/1000f;
    }
}
