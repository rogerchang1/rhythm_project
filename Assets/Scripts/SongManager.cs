using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SongManager : MonoBehaviour
{

    //song's current position in seconds
    public float songPosition;
    
    //testing
    public float audioSourceTime;
    public float lastAudioSourceTime;
    public float audioDSPTime;

    //song's current position in beats
    public float songPosInBeats;

    //how long a beat is
    private float secPerBeat;

    //how much time has passed since start of song in seconds
    public float dsptimesong;

    //blank audio at the start of the song
    public float offset;

    //beats per minute
    public float bpm;

    //keep all the position-in-beats of notes in the song
    //public float[][] notes;
    public float[][][] notes;

    //the index of the next note to be spawned
    private int[] nextIndexArr;

    //how many beats to show in advance. also be a scroll speed modifier
    public int beatsShownInAdvance;

    public GameObject note, lane, notehold;
    private GameObject[] lanes;

    //judgement bar positions
    private float judgementBar_width, judgementBar_xPos_left, judgementBar_yPos;

    //pauses the song
    public bool isPause;

    public KeyCode speedUp, speedDown;

    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        notes = transform.GetComponent<BeatMapParser>().notes;
        nextIndexArr = new int[notes.Length];
        lanes = new GameObject[notes.Length];
        GameObject judgementBar = LevelManager._lm.judgementBar;
        judgementBar_width = judgementBar.GetComponent<SpriteRenderer>().bounds.size.x;
        judgementBar_xPos_left = judgementBar.GetComponent<RectTransform>().transform.position.x - (judgementBar_width / 2);
        judgementBar_yPos = judgementBar.GetComponent<RectTransform>().transform.position.y;
        secPerBeat = 60f / bpm;

        for (int i = 0; i < notes.Length; i++)
        {
            nextIndexArr[i] = 0;
            GameObject l = (GameObject)Instantiate(lane);

            //set x position of lane with judgement bar and number of lanes.
            float posX = judgementBar_xPos_left + (i * (judgementBar_width / notes.Length)) + (judgementBar_width / notes.Length) / 2f;
            l.transform.localScale -= new Vector3(l.transform.localScale.x * (1 - (judgementBar_width / notes.Length)), 0, 0);
            l.transform.position = new Vector3(posX, (float)Math.Floor((9f- judgementBar_yPos)/2f), 0);
            l.transform.GetComponent<SpriteRenderer>().color = new Color(l.transform.GetComponent<SpriteRenderer>().color.r, l.transform.GetComponent<SpriteRenderer>().color.b, l.transform.GetComponent<SpriteRenderer>().color.g,0);
            l.GetComponent<LaneController>().sm = this;
            l.GetComponent<LaneController>().laneId = i;
            
            //switch case for temporarily making KeyCodes. 
            switch (i)
            {
                case 0:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.A;
                    break;
                case 1:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.S;
                    break;
                case 2:
                    if(notes.Length == 4)
                        l.GetComponent<LaneController>().keyToPress = KeyCode.Semicolon;
                    if (notes.Length == 6)
                        l.GetComponent<LaneController>().keyToPress = KeyCode.D;
                    break;
                case 3:
                    if (notes.Length == 4)
                        l.GetComponent<LaneController>().keyToPress = KeyCode.Quote;
                    if (notes.Length == 6)
                        l.GetComponent<LaneController>().keyToPress = KeyCode.L;
                    break;
                case 4:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.Semicolon;
                    break;
                case 5:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.Quote;
                    break;
                default:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.Space;
                    break;
            }
            lanes[i] = l;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager._lm.songActive)
        { 
            if (!isPause)
            {
                songPosition = (float)AudioSettings.dspTime - dsptimesong - offset;
                //testing out sync stuff
                /*songPosition += (float)AudioSettings.dspTime - dsptimesong;
                dsptimesong = (float)AudioSettings.dspTime;
                if (lastAudioSourceTime != GetComponent<AudioSource>().time)
                {
                    songPosition = (songPosition + (GetComponent<AudioSource>().time - offset)) / 2;
                    lastAudioSourceTime = GetComponent<AudioSource>().time;
                }*/
                songPosInBeats = songPosition / secPerBeat;
                audioSourceTime = GetComponent<AudioSource>().time;
                audioDSPTime = (float)AudioSettings.dspTime;

                for (int laneIdx = 0; laneIdx < notes.Length; laneIdx++)
                {

                    float[][] notesInLane = notes[laneIdx];
                    int nextIndex = nextIndexArr[laneIdx];
                    if (nextIndex < notesInLane.Length && notesInLane[nextIndex][0] < songPosInBeats + beatsShownInAdvance)
                    {
                        int noteType = 0;

                        //Get the note type 
                        if (notesInLane[nextIndex].Length == 2)
                        {
                            noteType = (int)notesInLane[nextIndex][1];
                        }

                        if (noteType != 0)
                        {
                            GameObject n = (GameObject)Instantiate(note);

                            //note color code
                            switch (laneIdx)
                            {
                                case 1:
                                    n.GetComponent<SpriteRenderer>().color = new Color(.25f, .7f, 1f, 1f);
                                    n.transform.Find("noteholdobject").gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(.25f, .7f, 1f, 1f);
                                    break;
                                case 2:
                                    if (notes.Length == 4)
                                    {
                                        n.GetComponent<SpriteRenderer>().color = new Color(.25f, .7f, 1f, 1f);
                                        n.transform.Find("noteholdobject").gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(.25f, .7f, 1f, 1f);
                                    }
                                    break;
                                case 4:
                                    if (notes.Length == 6)
                                    {
                                        n.GetComponent<SpriteRenderer>().color = new Color(.25f, .7f, 1f, 1f);
                                        n.transform.Find("noteholdobject").gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(.25f, .7f, 1f, 1f);
                                    }
                                    break;
                                default:
                                    break;
                            }

                            //resize each note to scale with judgement bar and number of lanes
                            n.transform.localScale -= new Vector3(n.transform.localScale.x * (1 - (judgementBar_width / notes.Length)), 0, 0);

                            
                            //initialize the fields of the music note
                            n.GetComponent<NoteObject2>().beatOfThisNote = notesInLane[nextIndex][0] + 1 + (GameManager.calibration/secPerBeat);
                            n.GetComponent<NoteObject2>().endBeatOfThisNote = n.GetComponent<NoteObject2>().beatOfThisNote;

                            //if it's a hold note, indicate if it's hold or release.
                            switch (noteType)
                            {
                                case LevelManager.NOTE_NORMAL:
                                    n.GetComponent<NoteObject2>().noteType = noteType;
                                    break;
                                case LevelManager.NOTE_HOLD:
                                    //n.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f, 1f);
                                    n.GetComponent<NoteObject2>().noteType = LevelManager.NOTE_HOLD;
                                    n.transform.Find("HoldRelease").GetComponent<TextMesh>().text = "hold";
                                    n.transform.Find("noteholdobject").gameObject.SetActive(true);
                                    break;
                                case LevelManager.NOTE_RELEASE:
                                    //n.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f, 1f);
                                    n.GetComponent<NoteObject2>().noteType = noteType;
                                    n.transform.Find("HoldRelease").GetComponent<TextMesh>().text = "release";
                                    lanes[laneIdx].GetComponent<LaneController>().changeLastNoteEndBeatValue(n.GetComponent<NoteObject2>().beatOfThisNote);
                                    break;
                                default:
                                    n.GetComponent<NoteObject2>().noteType = noteType;
                                    break;
                            }


                            n.GetComponent<NoteObject2>().sm = this;
                            n.GetComponent<NoteObject2>().lane = laneIdx;
                            float lanePos = lanes[laneIdx].transform.position.x;
                            n.GetComponent<NoteObject2>().spawnPos = new Vector2(lanePos, judgementBar_yPos + 9f); //arbitrarily 9f... because.. I don't know.
                            n.GetComponent<NoteObject2>().judgePos = new Vector2(lanePos, judgementBar_yPos);
                            n.GetComponent<NoteObject2>().removePos = new Vector2(lanePos, judgementBar_yPos - 9f);
                            lanes[laneIdx].GetComponent<LaneController>().noteList.AddLast(n);
                        }
                        nextIndexArr[laneIdx] = nextIndex + 1;
                    }
                }
            }
            if (Input.GetKeyDown(speedUp) && beatsShownInAdvance > 1)
                beatsShownInAdvance -= 1;
            if (Input.GetKeyDown(speedDown) && beatsShownInAdvance < 20)
                beatsShownInAdvance += 1;
        }
    }

    public void startSong()
    {
        //dsptimesong = (float)AudioSettings.dspTime;
        //testing out sync stuff
        dsptimesong = (float)AudioSettings.dspTime - offset;
        lastAudioSourceTime = 0;

        GetComponent<AudioSource>().Play();
        LevelManager._lm.songActive = true;
    }
    public void pauseSong(bool pause)
    {
        isPause = pause; 
        if (pause)
        {
            isPause = true;
            GetComponent<AudioSource>().Pause();
        }
        else
        {
            isPause = false;
            dsptimesong = (float)AudioSettings.dspTime - songPosition - offset;
            //dsptimesong = (float)AudioSettings.dspTime;
            GetComponent<AudioSource>().Play();
        }
    }
}
