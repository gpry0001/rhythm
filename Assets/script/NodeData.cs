using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time;
    public int lane;
    public string direction;

    // �� �κ��� �߰����ּ���!
    public Vector3 position;

    // �����ڵ� �Բ� �����ؾ� �մϴ�.
    public NoteData(float time, int lane, string direction, Vector3 position)
    {
        this.time = time;
        this.lane = lane;
        this.direction = direction;
        this.position = position;
    }
}
