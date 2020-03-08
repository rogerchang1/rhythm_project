using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    public static LevelManager _i;
    public GameObject comboDisplay, accuracyDisplay, scoreDisplay, songObject, judgementBar, characterTest, startText,healthBar, feverBar, ringEffect;
    public int comboCounter, maxComboCounter;

    public int characterScoreModifier, comboModifier;

    public float scoreCounter;

    public const int NOTE_NORMAL = 1;
    public const int NOTE_HOLD = 2;
    public const int NOTE_RELEASE = 3;

    public int[] accuracyTrackers;

    public KeyCode pause, activateFever;
    public bool isPause, songActive, feverActive;

    private const float SCORE_POINT = 1;

    public float maxFever, currentFever = 0;
    private const float FEVER_LIMIT = 100f;

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
        startText = (GameObject)Instantiate(startText);
        comboCounter = 0;
        maxComboCounter = 0;
        scoreCounter = 0;
        isPause = false;
        songActive = false;
        feverActive = false;
        characterScoreModifier = 0;
        comboModifier = 1;
        maxFever = FEVER_LIMIT;
        accuracyTrackers = new int[11];
        startText.GetComponent<BlinkController>().setTempo(songObject.GetComponent<SongManager>().bpm);
        healthBar.GetComponent<HealthBar>().setMaxHealth(100);
        feverBar.GetComponent<FeverBar>().setMaxFever(maxFever);
    }

    // Update is called once per frame
    void Update()
    {
        if (!songActive && Input.anyKeyDown)
        {
            songObject.GetComponent<SongManager>().startSong();
            Destroy(startText);
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

            if (Input.GetKeyDown(activateFever) && !feverActive && currentFever >= FEVER_LIMIT)
            {
                feverActive = true;
                comboModifier = 5;
                Instantiate(ringEffect, feverBar.transform.position, feverBar.transform.rotation);
            }

            if (feverActive && currentFever >= 0){
                currentFever -= 15f * Time.deltaTime;
                feverBar.GetComponentInChildren<FeverBar>().setFever(currentFever);
            }
            else
            {
                feverActive = false;
                comboModifier = 1;
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
        comboCounter += comboModifier * 1;
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
        accuracyTrackers[((int)laneModifier/10)]++;
        scoreDisplay.GetComponent<ScoreDisplay>().setScoreDisplay(scoreCounter, accuracyTrackers);
        increaseFever(laneModifier / 100f);
    }

    public void deductHealth()
    {
        int maxHealth = healthBar.GetComponentInChildren<HealthBar>().getMaxHealth();
        int health = healthBar.GetComponentInChildren<HealthBar>().getHealth();
        health -= (int)(maxHealth * .1);
        healthBar.GetComponentInChildren<HealthBar>().setHealth(health);
    }

    public void increaseFever(float f)
    {
        float feverModifier = 2f;

        if (feverActive)
        {
            feverModifier = 1f;
        }
        
        if(currentFever < maxFever)
        {
            currentFever += (feverModifier * f);
            if (currentFever > maxFever) currentFever = maxFever;
            feverBar.GetComponentInChildren<FeverBar>().setFever(currentFever);
        }
    }

}
