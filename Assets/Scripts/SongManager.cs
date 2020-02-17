using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{

    //song's current position in seconds
    private float songPosition;

    //song's current position in beats
    public float songPosInBeats;

    //how long a beat is
    private float secPerBeat;

    //how much time has passed since start of song in seconds
    private float dsptimesong;

    //blank audio at the start of the song
    public float offset;

    //beats per minute
    public float bpm;

    //keep all the position-in-beats of notes in the song
    public float[][] notes;

    //the index of the next note to be spawned
    private int[] nextIndexArr;

    //how many beats to show in advance. also be a scroll speed modifier
    public int beatsShownInAdvance;

    public GameObject note, lane, judgementBar;
    private GameObject[] lanes;

    //judgement bar positions
    private float judgementBar_width, judgementBar_xPos_left, judgementBar_yPos;

    public KeyCode speedUp, speedDown;

    // Start is called before the first frame update
    void Start()
    {
        float[] lane1 = new float[] { 24f, 28f, 32f, 36f, 40f, 44f, 48f, 52f, 56f, 60f, 64f, 68f, 72f, 76f, 80f, 84f, 88f, 92f, 96f, 100f };//kick
        float[] lane2 = new float[] { 18f,19f,19.5f,22f,26f,30f,38f,42f,46f,50f,51f,51.5f,54f,58f,62f,66f,70f,74f,78f,82f,86f,90f,94f,98f}; //snare
        float[] lane3 = new float[] {34f,35f,66.5f,67.5f };
        float[] lane4 = new float[] {34.5f,35.5f,67f };
        float[] lane5 = new float[] {};
        float[] lane6 = new float[] {};

        notes = new float[][] { lane1, lane2, lane3, lane4 };
        notes = new float[][] { lane1, lane2, lane3, lane4, lane5, lane6 };
        nextIndexArr = new int[notes.Length];
        lanes = new GameObject[notes.Length];
        //judgementBar_width = judgementBar.GetComponent<RectTransform>().rect.width * .4f;//Why do I need to multiply by .4f? I don't know...
        judgementBar_width = judgementBar.GetComponent<SpriteRenderer>().bounds.size.x;
        judgementBar_xPos_left = judgementBar.GetComponent<RectTransform>().transform.position.x - (judgementBar_width / 2);
        judgementBar_yPos = judgementBar.GetComponent<RectTransform>().transform.position.y;

        for (int i = 0; i < notes.Length; i++)
        {
            nextIndexArr[i] = 0;
            GameObject l = (GameObject)Instantiate(lane);

            //set x position of lane with judgement bar and number of lanes.
            float posX = judgementBar_xPos_left + (i * (judgementBar_width / notes.Length)) + (judgementBar_width / notes.Length) / 2f;
            l.transform.position = new Vector3(posX, 0, 0);
            l.GetComponent<LaneController>().sm = this;
            l.GetComponent<LaneController>().laneId = i;
            switch (i)
            {
                case 0:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.A;
                    break;
                case 1:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.S;
                    break;
                case 2:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.Semicolon;
                    break;
                case 3:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.Quote;
                    break;
                default:
                    l.GetComponent<LaneController>().keyToPress = KeyCode.Space;
                    break;
            }
            lanes[i] = l;
        }
        secPerBeat = 60f / bpm;
        dsptimesong = (float)AudioSettings.dspTime;
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)AudioSettings.dspTime - dsptimesong - offset;
        songPosInBeats = songPosition / secPerBeat;

        if (Input.GetKeyDown(speedUp))
        {
            beatsShownInAdvance -= 1;
        }
        if (Input.GetKeyDown(speedDown))
        {
            beatsShownInAdvance += 1;
        }

        for (int laneIdx = 0; laneIdx < notes.Length; laneIdx++)
        {
            float[] notesInLane = notes[laneIdx];
            int nextIndex = nextIndexArr[laneIdx];
            if (nextIndex < notesInLane.Length && notesInLane[nextIndex] < songPosInBeats + beatsShownInAdvance)
            {
                GameObject n = (GameObject)Instantiate(note);

                //resize each note to scale with judgement bar and number of lanes
                n.transform.localScale -= new Vector3(n.transform.localScale.x * (1-(judgementBar_width / notes.Length)), 0 ,0);
                
                //initialize the fields of the music note
                n.GetComponent<NoteObject2>().beatOfThisNote = notesInLane[nextIndex] + 1;
                n.GetComponent<NoteObject2>().sm = this;
                n.GetComponent<NoteObject2>().lane = laneIdx;
                float lanePos = lanes[laneIdx].transform.position.x;
                n.GetComponent<NoteObject2>().spawnPos = new Vector2(lanePos, judgementBar_yPos+9f);
                //n.GetComponent<NoteObject2>().judgePos = new Vector2(lanePos, 0f);
                n.GetComponent<NoteObject2>().judgePos = new Vector2(lanePos, judgementBar_yPos);
                n.GetComponent<NoteObject2>().removePos = new Vector2(lanePos, judgementBar_yPos - 9f);
                lanes[laneIdx].GetComponent<LaneController>().noteList.AddLast(n);
                nextIndexArr[laneIdx] = nextIndex + 1;
            }
        }
    }
}
