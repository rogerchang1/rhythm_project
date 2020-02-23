using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BeatMapParser : MonoBehaviour
{
    public TextAsset beatMap;
    public float[][][] notes;
    public List<List<List<float>>> notesList;


    // Start is called before the first frame update
    void Start()
    {
        generateNoteArray();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void generateNoteArray()
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
                notesList = new List<List<List<float>>>();

                //creating each lane;
                for (int i = 0; i < numLanes; i++)
                {
                    notes[i] = new float[textMapArr.Length - 1][];
                    notesList.Add(new List<List<float>>());
                }

                //Construct notes array
                int idx = 0;
                for (int i = 1; i < textMapArr.Length; i++)
                {
                    string[] beatLine = textMapArr[i].Split('/');

                    //Each line in the file needs to have the beat position as the first value. The values after the beat position represent the note type. Delimiter is '/'.
                    //The total number of values in the text file line should equal to numLanes + 1.
                    if (beatLine.Length == numLanes + 1)
                    {
                        float beat;
                        if (float.TryParse(beatLine[0].Trim(), out beat))
                        {
                            
                            for (int j = 1; j < beatLine.Length; j++)
                            {
                                float noteType = 0;
                                float.TryParse(beatLine[j].Trim(), out noteType);
                                Debug.Log(beat+","+ noteType);
                                notes[j - 1][idx] = new float[] { beat, noteType };
                                List<float> l = new List<float>() { beat, noteType };
                                notesList[j - 1].Add(l);
                            }
                            idx++;
                        }
                    }
                }

                for (int i = 0; i < notes.Length; i++)
                {
                    // notes[i] = sortLane(notes[i]);
                }
            }

        }
    }

    private float[][] sortLane(float[][] lane)
    {
        float[] beats = new float[lane[0].Length];
        float[] noteTypes = new float[lane[0].Length];
        float[][] output;
        for(int i = 0; i < lane.Length; i++)
        {
            beats[i] = lane[i][0];
            noteTypes[i] = lane[i][1];
        }
        Array.Sort(beats, noteTypes);
        output = new float[beats.Length][];
        for (int i = 0; i < beats.Length; i++)
        {
            output[i] = new float[] { beats[i], noteTypes[i] };
        }
        return output;
    }

}
