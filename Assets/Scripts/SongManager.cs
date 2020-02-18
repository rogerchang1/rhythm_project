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
    //public float[][] notes;
    public float[][][] notes;

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
        //string[] lane1 = new string[] { "20","24", "28", "32", "36", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "100" };//kick
        //string[] lane2 = new string[] { "18", "19", "19.5", "22", "26", "30", "38", "42", "46", "50", "51", "51.5", "54", "58", "62", "66", "70", "74", "78", "82", "86", "90", "94", "98" }; //snare
        string[] lane1 = new string[] { "20","24", "28", "32", "40", "57", "58", "62", "66", "66.5", "67", "67.5","70","79","86","91","93" };
        string[] lane2 = new string[] { "18", "19", "19.5", "22", "26", "30", "46", "50", "51", "51.5", "54","61", "63","68+69","72","74","78","80+83","84+85","88","90","94","96" }; //snare
        string[] lane3 = new string[] { "47", "59","75","77","95" };
        string[] lane4 = new string[] { "43", "44","70","79","86","95" };
        string[] lane5 = new string[] {  "36","38","41","42", "60","68+69","71","73","74","78", "80+83", "84+85","87","89","90","94","96" };
        string[] lane6 = new string[] { "34","34.5", "35","35.5","39", "45","48+49","52", "55+56", "64+65", "75", "76","77","92","93" };
        float[][] emptyLane = new float[][] {};
        float[][] testLane = new float[][] { new float [] {12f,16f }, new float[] { 24f}, new float[] { 28f }, new float[] { 32f }, new float[] { 36f }, };

        //notes = new float[][][] { noteParser(lane1), noteParser(lane2), noteParser(lane3), noteParser(lane4)};
        notes = new float[][][] { noteParser(lane1), noteParser(lane2), noteParser(lane3),noteParser(lane4),noteParser(lane5),noteParser(lane6)};
        nextIndexArr = new int[notes.Length];
        lanes = new GameObject[notes.Length];
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

            float[][] notesInLane = notes[laneIdx];
            int nextIndex = nextIndexArr[laneIdx];
            if (nextIndex < notesInLane.Length && notesInLane[nextIndex][0] < songPosInBeats + beatsShownInAdvance)
            {
                for (int i = 0; i < notesInLane[nextIndex].Length; i++)
                {
                    GameObject n = (GameObject)Instantiate(note);
                
                    //resize each note to scale with judgement bar and number of lanes
                    n.transform.localScale -= new Vector3(n.transform.localScale.x * (1 - (judgementBar_width / notes.Length)), 0, 0);

                    //if it's a hold note, indicate if it's hold or release.
                    if (notesInLane[nextIndex].Length > 1)
                    {
                        n.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f, 1f);
                        if (i == 0)
                        {
                            n.GetComponent<NoteObject2>().noteType = 1;
                            n.transform.Find("HoldRelease").GetComponent<TextMesh>().text = "hold";
                        }
                        else
                        {
                            n.GetComponent<NoteObject2>().noteType = 2;
                            n.transform.Find("HoldRelease").GetComponent<TextMesh>().text = "release";
                        }
                        
                    }
                    else
                    {
                        n.GetComponent<NoteObject2>().noteType = 0;
                    }

                    //initialize the fields of the music note
                    n.GetComponent<NoteObject2>().beatOfThisNote = notesInLane[nextIndex][i] + 1;                
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

    //turn a string array of note beats into float[][]. Just to make it easier to input and design note charts....
    private float[][] noteParser(string[] input)
    {
        float[][] output = new float[input.Length][];
        for(int i = 0; i < input.Length; i++)
        {
            string[] inputStr = input[i].Split('+');
            float[] temp = new float[inputStr.Length];
            for(int j = 0; j<inputStr.Length;j++)
            {
                temp[j] = float.Parse(inputStr[j]);
            }
            output[i] = temp;
        }
        return output;
    }
}
