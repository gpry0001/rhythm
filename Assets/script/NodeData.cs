using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float time; // ��Ʈ�� �����ϴ� �ð� (��)
    public int lane;   // ��Ʈ�� ��ġ�� ���� (0, 1, 2, 3 ������ ����)
    public float angle; // ��Ʈ�� ȸ�� ���� (���û���)
    public string direction; // ��Ʈ�� ���� (e.g., "Up", "Down", "Left", "Right")

    public NoteData(float time, int lane, string direction)
    {
        this.time = time;
        this.lane = lane;
        this.direction = direction;
    }
}
