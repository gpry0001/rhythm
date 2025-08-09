using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNoteData", menuName = "rhythm/Note Data")]
public class testnodedata : ScriptableObject
{
    public string songName;
    public List<NoteInfo> notes;
}

[System.Serializable]
public class NoteInfo
{
    public float time; // ��Ʈ Ÿ�̹� (�� ����)
    public int lane;   // ��Ʈ ��ġ
}
