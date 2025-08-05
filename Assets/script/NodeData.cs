using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time; // 노트가 등장하는 시간 (초)
    public int lane;   // 노트가 위치할 레인 (0, 1, 2, 3 등으로 구분)
    public float angle; // 노트의 회전 각도 (선택사항)
    public string direction; // 노트의 방향 (e.g., "Up", "Down", "Left", "Right")

    public NoteData(float time, int lane, string direction)
    {
        this.time = time;
        this.lane = lane;
        this.direction = direction;
    }
}
