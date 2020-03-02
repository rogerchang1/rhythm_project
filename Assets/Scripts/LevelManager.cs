using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    public static LevelManager _i;
    public GameObject comboDisplay, accuracyDisplay, scoreDisplay, songObject, judgementBar, characterTest;
    public int comboCounter, maxComboCounter;

    public int characterScoreModifier;

    public float scoreCounter;

    public const int NOTE_NORMAL = 1;
    public const int NOTE_HOLD = 2;
    public const int NOTE_RELEASE = 3;

    public int[] accuracyTrackers;

    public KeyCode pause;
    public bool isPause, songActive;

    private const float SCORE_POINT = 1;

    // Start is called before the first frame update
    void Start()
    {
        _i = this;
        comboDisplay = (GameObject)Instantiate(comboDisplay);
        accuracyDisplay = (GameObject)Instantiate(accuracyDisplay);
        scoreDisplay = (GameObject)Instantiate(scoreDisplay);
        judgementBar = (GameObject)Instantiate(judgementBar);
        //songObject = (GameObject)Instantiate(songObject);
        songObject = (GameObject)(Instantiate(Resources.Load("SongObjects/B B B B")) as GameObject);
        characterTest = (GameObject)Instantiate(characterTest);
        comboCounter = 0;
        maxComboCounter = 0;
        scoreCounter = 0;
        isPause = false;
        songActive = false;
        characterScoreModifier = 0;
        accuracyTrackers = new int[11];
    }

    // Update is called once per frame
    void Update()
    {
        if (!songActive && Input.anyKeyDown)
        {
            songObject.GetComponent<SongManager>().startSong();
        }
        else if (songActive)
        {
            if (Input.GetKeyDown(pause))
            {
                isPause = !isPause;
                songObject.GetComponent<SongManager>().pauseSong(isPause);
            }
            if (isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }


            //Song Ends
            if (songActive && songObject.GetComponent<AudioSource>().time == 0 && !songObject.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(judgementBar);
                Destroy(accuracyDisplay);
                Destroy(comboDisplay);
                Destroy(characterTest);
                Destroy(songObject);
                foreach (GameObject lane in GameObject.FindGameObjectsWithTag("LaneTag"))
                {
                    Destroy(lane);
                }

                songActive = false;
                GameManager._gm.LoadStart();
            }
        }
    }

    public void NoteHit()
    {
    }

    public void increaseCombo()
    {
        comboCounter++;
        comboDisplay.GetComponent<ComboDisplay>().setComboDisplay(comboCounter);
        if(comboCounter > maxComboCounter)
            maxComboCounter = comboCounter;
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
        scoreCounter += (laneModifier/100f) * (SCORE_POINT+characterScoreModifier);
        Debug.Log(laneModifier);
        accuracyTrackers[((int)laneModifier/10)]++;
        scoreDisplay.GetComponent<ScoreDisplay>().setScoreDisplay(scoreCounter, accuracyTrackers);
    }

}
