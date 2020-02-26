using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    
    public static GameManager2 _i;
    public GameObject comboDisplay, accuracyDisplay, scoreDisplay, songObject, judgementBar, characterTest;
    public int comboCounter;

    public int characterScoreModifier;

    public float scoreCounter;

    public const int NOTE_NORMAL = 1;
    public const int NOTE_HOLD = 2;
    public const int NOTE_RELEASE = 3;

    public KeyCode pause;
    public bool isPause;

    private const float SCORE_POINT = 1;

    // Start is called before the first frame update
    void Start()
    {
        _i = this;
        comboDisplay = (GameObject)Instantiate(comboDisplay);
        accuracyDisplay = (GameObject)Instantiate(accuracyDisplay);
        scoreDisplay = (GameObject)Instantiate(scoreDisplay);
        judgementBar = (GameObject)Instantiate(judgementBar);
        songObject = (GameObject)Instantiate(songObject);
        characterTest = (GameObject)Instantiate(characterTest);
        comboCounter = 0;
        scoreCounter = 0;
        isPause = false;
        characterScoreModifier = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pause))
        {
            isPause = !isPause;
            songObject.GetComponent<SongManager>().pauseSong(isPause);
        }
    }

    public void NoteHit()
    {
    }

    public void increaseCombo()
    {
        comboCounter++;
        comboDisplay.GetComponent<ComboDisplay>().setComboDisplay(comboCounter);
    }

    public void resetCombo()
    {
        if (comboCounter > 0)
        {
            comboCounter = 0;
            comboDisplay.GetComponent<ComboDisplay>().setComboDisplay(comboCounter);
        }
    }

    public void setAccuracyDisplay(string accuracy)
    {
        accuracyDisplay.GetComponent<AccuracyDisplay>().setAccuracyDisplay(accuracy);
    }

    public void increaseScore(float laneModifier)
    {
        scoreCounter += (float)System.Math.Round((laneModifier/100f) * (SCORE_POINT+characterScoreModifier));
        scoreDisplay.GetComponent<ScoreDisplay>().setScoreDisplay(scoreCounter);
    }

}
