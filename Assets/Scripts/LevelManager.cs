using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    public static LevelManager _lm;
    public GameObject comboDisplay, accuracyDisplay, scoreDisplay, songObject, judgementBar, characterTest, startText,healthBar, feverBar, ringEffect;
    public int comboCounter, maxComboCounter, noteCounter;

    public int characterScoreModifier, comboModifier, healModifier;

    public float scoreCounter;

    public const int NOTE_NORMAL = 1;
    public const int NOTE_HOLD = 2;
    public const int NOTE_RELEASE = 3;

    public int[] accuracyTrackers;

    public KeyCode pause, activateFever;
    public bool isPause, songActive, feverActive, showResult;

    private const float SCORE_POINT = 1;

    public float maxFever, currentFever, currentHealth, maxHealth, totalAccuracy;
    private const float FEVER_LIMIT = 100f;

    void Awake()
    {
        if (!_lm)
        {
            _lm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!showResult)
        {
            comboDisplay = (GameObject)Instantiate(comboDisplay);
            accuracyDisplay = (GameObject)Instantiate(accuracyDisplay);
            scoreDisplay = (GameObject)Instantiate(scoreDisplay);
            judgementBar = (GameObject)Instantiate(judgementBar);
            //judgementBar = (GameObject)Instantiate(judgementBar, GameObject.FindGameObjectWithTag("UICanvasTag").transform);
            //songObject = (GameObject)(Instantiate(Resources.Load("SongObjects/B B B B")) as GameObject);
            loadSong(GameManager._gm.songNameToLoad);
            startText = (GameObject)Instantiate(startText);
            startText.GetComponent<BlinkController>().setTempo(songObject.GetComponent<SongManager>().bpm);
            healthBar.GetComponent<HealthBar>().setMaxHealth((int)maxHealth);
            feverBar.GetComponent<FeverBar>().setMaxFever(maxFever);
            showResult = false;
        }
        comboCounter = 0;
        maxComboCounter = 0;
        scoreCounter = 0;
        isPause = false;
        songActive = false;
        feverActive = false;
        characterScoreModifier = 0;
        healModifier = 0;
        noteCounter = 0;
        comboModifier = 1;
        maxFever = FEVER_LIMIT;
        currentFever = 0;
        maxHealth = 100;
        totalAccuracy = 0;
        currentHealth = maxHealth;
        accuracyTrackers = new int[11];

    }

    // Update is called once per frame
    void Update()
    {
        if (!showResult)
        {
            //Press any key to start the song.
            if (!songActive && Input.anyKeyDown)
            {
                songObject.GetComponent<SongManager>().startSong();
                Destroy(startText);
                //for testing purposes  
                //endLevel();
            }
            else if (songActive)
            {
                if (Input.GetKeyDown(pause))
                {
                    pauseLevel();
                }

                //ACTIVATE FEVER
                if (Input.GetKeyDown(activateFever) && !feverActive && currentFever >= FEVER_LIMIT)
                {
                    feverActive = true;
                    Instantiate(ringEffect, feverBar.transform.position, feverBar.transform.rotation);
                }

                //Decrement Activated Fever Bar by time.
                if (feverActive && currentFever >= 0)
                {
                    currentFever -= 15f * Time.deltaTime;
                    feverBar.GetComponentInChildren<FeverBar>().setFever(currentFever);
                }
                else
                {
                    feverActive = false;
                }

                //Song Ends
                if (songActive && songObject.GetComponent<AudioSource>().time == 0 && !songObject.GetComponent<AudioSource>().isPlaying)
                {
                    endLevel();
                }
            }
        }
    }

    public void NoteHit(float laneModifier, string accuracy)
    {
        increaseCombo();
        setAccuracyDisplay(accuracy);
        increaseScore(laneModifier);
        increaseFever(laneModifier / 100f);
        healHealth(laneModifier);
        noteCounter++;
        calculateTotalAccuracy();
    }

    public void NoteMiss()
    {
        resetCombo();
        setAccuracyDisplay("MISS");
        increaseScore(0);
        deductHealth();
        noteCounter++;
        calculateTotalAccuracy();
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
    }

    public void deductHealth()
    {
        int maxHealth = healthBar.GetComponentInChildren<HealthBar>().getMaxHealth();
        int health = healthBar.GetComponentInChildren<HealthBar>().getHealth();
        health -= (int)(maxHealth * .1);
        currentHealth = health;
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

    public void healHealth(float laneModifier)
    {
        currentHealth += healModifier * (2 * (laneModifier / 100f));
        healthBar.GetComponentInChildren<HealthBar>().setHealth((int)currentHealth);
    }

    private void calculateTotalAccuracy()
    {
        totalAccuracy = 0;
        int accuracySum = 0;
        for (int i = 0; i < LevelManager._lm.accuracyTrackers.Length; i++)
        {
            accuracySum += LevelManager._lm.accuracyTrackers[i] * (i * 10);
        }
        if (LevelManager._lm.noteCounter != 0)
            totalAccuracy = (accuracySum / (LevelManager._lm.noteCounter * 100f)) * 100f;
    }

    private void endLevel()
    {
        Destroy(judgementBar);
        Destroy(accuracyDisplay);
        Destroy(scoreDisplay);
        Destroy(comboDisplay);
        Destroy(songObject);
        Destroy(GameObject.FindGameObjectWithTag("PartyObjectTag"));
        Destroy(GameObject.FindGameObjectWithTag("PartyObjectTag"));
        Destroy(GameObject.FindGameObjectWithTag("UICanvasTag"));

        foreach (GameObject lane in GameObject.FindGameObjectsWithTag("LaneTag"))
        {
            Destroy(lane);
        }

        songActive = false;
        showResult = true;
        GameManager._gm.LoadResult();
    }

    private void pauseLevel()
    {
        isPause = !isPause;

        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        songObject.GetComponent<SongManager>().pauseSong(isPause);
    }

    public void loadSong(string songName)
    {
        songObject = (GameObject)(Instantiate(Resources.Load("SongObjects/"+songName)) as GameObject);
    }

}
