using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    
    public bool startPlaying;
    public BeatScroller bs;
    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public Text scoreText;
    public Text multiText;
    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;


    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        multiplierTracker = 0;
        totalNotes = FindObjectsOfType<NoteObject>().Length;
        resultsScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                bs.hasStarted = true;
                theMusic.Play();
            }
        }
        else
        {
            if (!theMusic.isPlaying && !resultsScreen.activeInHierarchy)
            {
                resultsScreen.SetActive(true);
                normalsText.text = "" + normalHits;
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = missedHits.ToString();
                float totalHits = (normalHits + goodHits + perfectHits);
                float percentHits = (totalHits / totalNotes) * 100f;
                percentHitText.text = percentHits.ToString("F1") + "%";

                string rankVal = "F";
                if(percentHits > 40)
                {
                    rankVal = "D";
                    if(percentHits > 55)
                    {
                        rankVal = "C";
                        if(percentHits > 70)
                        {
                            rankVal = "B";
                            if(percentHits > 85)
                            {
                                rankVal = "A";
                                if(percentHits > 95)
                                {
                                    rankVal = "S";
                                }
                            }
                        }
                    }
                }

                rankText.text = rankVal;
                finalScoreText.text = currentScore.ToString();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
        
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
        multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                currentMultiplier++;
                multiplierTracker = 0;
            }
            //scoreText.text = "Score: " + currentScore;
            //currentScore += scorePerNote * currentMultiplier;
            multiText.text = "Multiplier: x"+currentMultiplier;
        }
    }

    public void NormalHit()
    {
        currentScore += scorePerNote;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote;
        NoteHit();
        perfectHits++;
    }

    public void NoteMiss()
    {
        Debug.Log("Missed Note");
        multiplierTracker = 0;
        currentMultiplier = 1;
        multiText.text = "Multiplier: x" + currentMultiplier;
        missedHits++;
    }
}
