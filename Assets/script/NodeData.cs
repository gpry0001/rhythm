using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time;
    public int lane;
    public string direction;

    // 이 부분을 추가해주세요!
    public Vector3 position;

    // 생성자도 함께 수정해야 합니다.
    public NoteData(float time, int lane, string direction, Vector3 position)
    {
        this.time = time;
        this.lane = lane;
        this.direction = direction;
        this.position = position;
    }
}
