using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

    //External file beat map
    public TextAsset beatMap;

    public KeyCode speedUp, speedDown;

    // Start is called before the first frame update
    void Start()
    {

        //string[] lane1 = new string[] { "20","24", "28", "32", "36", "40", "44", "48", "52", "56", "60", "64", "68", "72", "76", "80", "84", "88", "92", "96", "100" };//kick
        //string[] lane2 = new string[] { "18", "19", "19.5", "22", "26", "30", "38", "42", "46", "50", "51", "51.5", "54", "58", "62", "66", "70", "74", "78", "82", "86", "90", "94", "98" }; //snare
        string[] lane1 = new string[] { "20","24", "28", "32", "40", "57", "58", "62", "66", "66.5", "67", "67.5","70","79","86","91","93" };
        string[] lane2 = new string[] { "18", "19", "19.5", "22", "26", "30", "46", "50", "51", "51.5", "54", "59", "61", "63", "68+69","72","74","78","80+83","84+85","88","90","94","96" }; //snare
        string[] lane3 = new string[] { "47", "60", "75","77","95" };
        string[] lane4 = new string[] { "43", "44", "70","79","86","95" };
        string[] lane5 = new string[] {  "36","38","41","42", "68+69","71","73","74","78", "80+83", "84+85","87","89","90","94","96" };
        string[] lane6 = new string[] { "34","34.5", "35","35.5","39", "45","48+49","52", "55+56",  "64+65", "75", "76","77","92","93" };
        string[] emptyLane = new string[] {};
        string[] testLane = new string[] { "12+16", "24"};

        //notes = new float[][][] { noteParser(testLane), noteParser(emptyLane), noteParser(emptyLane), noteParser(emptyLane)};
        //notes = new float[][][] { noteParser(lane1), noteParser(lane2), noteParser(lane3),noteParser(lane4),noteParser(lane5),noteParser(lane6)};
        //notes = transform.GetComponent<BeatMapParser>().notes;
        parseBeatMapToArray();
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
                int noteType = 0;
                if (notesInLane[nextIndex].Length == 2)
                {
                    noteType = (int)notesInLane[nextIndex][1];

                }
                if(noteType != 0)
                {
                    GameObject n = (GameObject)Instantiate(note);
                
                    //resize each note to scale with judgement bar and number of lanes
                    n.transform.localScale -= new Vector3(n.transform.localScale.x * (1 - (judgementBar_width / notes.Length)), 0, 0);

                    //if it's a hold note, indicate if it's hold or release.

                    switch (noteType)
                    {
                        case GameManager2.NOTE_NORMAL:
                            n.GetComponent<NoteObject2>().noteType = noteType;
                            break;
                        case GameManager2.NOTE_HOLD:
                            n.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f, 1f);
                            n.GetComponent<NoteObject2>().noteType = GameManager2.NOTE_HOLD;
                            n.transform.Find("HoldRelease").GetComponent<TextMesh>().text = "hold";
                            break;
                        case GameManager2.NOTE_RELEASE:
                            n.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f, 1f);
                            n.GetComponent<NoteObject2>().noteType = noteType;
                            n.transform.Find("HoldRelease").GetComponent<TextMesh>().text = "release";
                            break;
                        default:
                            n.GetComponent<NoteObject2>().noteType = noteType;
                            break;
                    }

                    //initialize the fields of the music note
                    n.GetComponent<NoteObject2>().beatOfThisNote = notesInLane[nextIndex][0] + 1;                
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }

    //Parse TextAsset BeatMap (external text file) to a float[][][] array for holding notes data.
    void parseBeatMapToArray()
    {
        //beatMap = Resources.Load("BeatMaps/beatMap1.txt") as TextAsset;
        string textMapString = beatMap.text.Trim();
        string[] textMapArr = textMapString.Split('\n');

        if (textMapArr.Length > 0)
        {
            //First value in the Beat Map must be the number of lanes.
            int numLanes;
            if (Int32.TryParse(textMapArr[0].Trim(), out numLanes))
            {
                notes = new float[numLanes][][];
                //notesList = new List<List<List<float>>>();

                //creating each lane;
                for (int i = 0; i < numLanes; i++)
                {
                    notes[i] = new float[textMapArr.Length - 1][];
                   // notesList.Add(new List<List<float>>());
                }

                //Construct notes array
                int idx = 0;
                for (int i = 1; i < textMapArr.Length; i++)
                {
                    string[] beatLine = textMapArr[i].Split('/');

                    //Each line in the file needs to have the beat position as the first value. The values after the beat position represent the note type. Delimiter is '/'.
                    //The total number of values in the text file line should equal to numLanes + 1.
                    if (beatLine.Length >= numLanes + 1)
                    {
                        float beat;
                        if (float.TryParse(beatLine[0].Trim(), out beat))
                        {

                            for (int j = 1; j < numLanes + 1; j++)
                            {
                                float noteType = 0;
                                float.TryParse(beatLine[j].Trim(), out noteType);
                                notes[j - 1][idx] = new float[] { beat, noteType };
                                List<float> l = new List<float>() { beat, noteType };
                                //notesList[j - 1].Add(l);
                            }
                            idx++;
                        }
                        else
                        {
                            //Invalid line in text file, decrease array size for all lanes by 1
                            for (int j = 0; j < numLanes; j++)
                            {
                                Array.Resize(ref notes[j], notes[j].Length - 1);
                            }
                        }
                    }
                    else
                    {
                        //Invalid line in text file, decrease array size for all lanes by 1
                        for (int j = 0; j < numLanes; j++)
                        {
                            Array.Resize(ref notes[j], notes[j].Length - 1);
                        }
                    }
                }

                for (int i = 0; i < notes.Length; i++)
                {
                    Array.Sort(notes[i], (a, b) => compareForSort(a, b));
                }
            }

        }
    }

    private int compareForSort(float[] a, float[] b)
    {
        if (a[0] != b[0])
        {
            return a[0].CompareTo(b[0]) > 0 ? 1 : -1;
        }
        return 0;
    }

}
