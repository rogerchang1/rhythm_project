using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BeatMapParser : MonoBehaviour
{
    //External text file beat map
    public TextAsset beatMap;
    public float[][][] notes;
    public List<List<List<float>>> notesList;


    void Awake()
    {
        parseBeatMapToArray();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Parse TextAsset BeatMap (external text file) to a float[][][] array for holding notes data.
    void parseBeatMapToArray()
    {
        //beatMap = Resources.Load("BeatMaps/beatMap1.txt") as TextAsset;
        string textMapString = beatMap.text.Trim();
        string[] textMapArr = textMapString.Split('\n');
        int[] laneIdxArr;

        if (textMapArr.Length > 0)
        {
            //First value in the Beat Map must be the number of lanes.
            int numLanes;
            if (Int32.TryParse(textMapArr[0].Trim(), out numLanes))
            {
                notes = new float[numLanes][][];
                laneIdxArr = new int[numLanes];
                //notesList = new List<List<List<float>>>();

                //creating each lane;
                for (int i = 0; i < numLanes; i++)
                {
                    notes[i] = new float[textMapArr.Length - 1][];
                    // notesList.Add(new List<List<float>>());
                }

                //Construct notes array
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
                                if (noteType > 0)
                                {
                                    notes[j - 1][laneIdxArr[j - 1]] = new float[] { beat, noteType };
                                    List<float> l = new List<float>() { beat, noteType };
                                    //notesList[j - 1].Add(l);
                                    laneIdxArr[j - 1]++;
                                }
                                else
                                {
                                    //Invalid note, decrease array size for the lane by 1.
                                    Array.Resize(ref notes[j - 1], notes[j - 1].Length - 1);
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
