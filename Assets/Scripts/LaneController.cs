﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    
    public LinkedList<GameObject> noteList;
    public KeyCode keyToPress;
    public SongManager sm;
    public int laneId;
    public float firstNoteBeat; //debug purposes
    public GameObject noteTemp; //used to temporarily keep track of long note's starting beat.

    private bool removeReleaseNote;

    private float limit;
    private const float LIMIT_MODIFIER = .3f;
    // Start is called before the first frame update
    void Start()
    {
        noteList = new LinkedList<GameObject>();
        limit = sm.beatsShownInAdvance * LIMIT_MODIFIER; //limit used to be just 2f, but I think that's unbalanced with the scroll speed modifier.
        removeReleaseNote = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager._lm.isPause)
        {
            //Show SpriteRenderer
            if (Input.GetKey(keyToPress) || Input.GetKeyDown(keyToPress))
            {
                transform.GetComponent<SpriteRenderer>().color = transform.GetComponent<SpriteRenderer>().color = new Color(transform.GetComponent<SpriteRenderer>().color.r, transform.GetComponent<SpriteRenderer>().color.b, transform.GetComponent<SpriteRenderer>().color.g, .75f);
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().color = transform.GetComponent<SpriteRenderer>().color = new Color(transform.GetComponent<SpriteRenderer>().color.r, transform.GetComponent<SpriteRenderer>().color.b, transform.GetComponent<SpriteRenderer>().color.g, 0);
            }
            if (noteList.Count != 0)
            {
                GameObject firstNote = noteList.First.Value;
                NoteObject2 n = firstNote.GetComponent<NoteObject2>();
                firstNoteBeat = n.beatOfThisNote;
                if (removeReleaseNote)
                {
                    noteMiss();
                    noteList.RemoveFirst();
                    Destroy(firstNote);
                    removeReleaseNote = false;
                }
                //regular hit
                if (Input.GetKeyDown(keyToPress) && n.noteType == LevelManager.NOTE_NORMAL)
                {
                    if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit)
                    {
                        noteHit(n);
                        //Instantiate(HitEffect, firstNote.transform.position, HitEffect.transform.rotation);
                        noteList.RemoveFirst();
                        Destroy(firstNote);
                    }
                }

                //long hit hold
                if (Input.GetKeyDown(keyToPress) && n.noteType == LevelManager.NOTE_HOLD && n.isHit == false)
                {
                    if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit)
                    {
                        noteHit(n);
                        n.isHit = true;
                        n.pauseAtJudgeBar = true;
                        noteTemp = firstNote;
                        noteList.RemoveFirst();
                    }
                }

                //long hit release
                if (!Input.GetKey(keyToPress) && n.noteType == LevelManager.NOTE_RELEASE && noteTemp != null)
                {
                    if (Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats) <= limit && noteTemp.GetComponent<NoteObject2>().isHit == true)
                    {
                        noteHit(n);
                        noteList.RemoveFirst();

                        Destroy(firstNote);
                        Destroy(noteTemp);

                    }
                    else
                    {
                        //Miss Release
                        noteMiss();
                        Destroy(noteTemp);
                        removeReleaseNote = true;
                    }
                }

                //Note Miss
                if (sm.songPosInBeats - n.beatOfThisNote > limit && n.isHit == false)
                {
                    noteMiss();
                    noteList.RemoveFirst();
                    Destroy(firstNote);
                    if(firstNote.GetComponent<NoteObject2>().noteType == LevelManager.NOTE_HOLD)
                    {
                        removeReleaseNote = true;
                    }
                    if (noteTemp != null)
                    {   
                        Destroy(noteTemp);
                    }
                }
            }
        }
    }
    private float RoundUp(float toRound)
    {
        if (toRound % 10 == 0) return toRound;
        return (10 - toRound % 10) + toRound;
    }

    //Display accuracy on note hit and increase combo count
    private void noteHit(NoteObject2 n)
    {
        
        float noteScore = RoundUp(Mathf.Ceil(((limit - Mathf.Abs(n.beatOfThisNote - sm.songPosInBeats)) / limit) * 100f));
        string noteScoreString = "";
        if(100f - noteScore < 10)
        {
            noteScoreString = "!MIKAN!";
        }
        else if(n.beatOfThisNote < sm.songPosInBeats)
            noteScoreString = "SLOW " + noteScore.ToString();
        else
            noteScoreString = "FAST " + noteScore.ToString();

        LevelManager._lm.NoteHit(noteScore, noteScoreString);
    }

    private void noteMiss()
    {
        LevelManager._lm.NoteMiss();
    }

    public void changeLastNoteEndBeatValue(float newEndBeatOfThisNote)
    {
        if(noteList.Count > 0)
            noteList.Last.Value.GetComponent<NoteObject2>().endBeatOfThisNote = newEndBeatOfThisNote;
    }

}
