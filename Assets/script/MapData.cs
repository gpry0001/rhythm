using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public float bpm;
    public List<NoteData> notes;

    public MapData(float bpm)
    {
        this.bpm = bpm;
        this.notes = new List<NoteData>();
    }
}
