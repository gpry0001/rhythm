using System.Collections.Generic; // List<T> ���
using System.IO;                  // ���� ����� (Path.Combine, File.Exists, File.Delete, File.Create, StreamWriter) ���
using System.Linq;                // LINQ (OrderBy) ���
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class SheetStorage : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject notePrefab;

    public int Linecnt;
    public float bpm;
    public float lineYOffset = 0f;

    private MapData mapData; // MapData ��ü�� ��Ʈ �����͸� ����
    private Dictionary<Vector3, GameObject> activeNotes = new Dictionary<Vector3, GameObject>();

    private void Start()
    {
        mapData = new MapData(bpm); // �� ������ �ʱ�ȭ
        float secondsPerBeat = 60 / bpm * 10;

        for (int i = 0; i < Linecnt; i++) {
            Instantiate(linePrefab, new Vector3(0, 6 + i* secondsPerBeat, 0), Quaternion.identity);
            Instantiate(linePrefab, new Vector3(0, -6 + -i * secondsPerBeat, 0), Quaternion.identity);
            Instantiate(linePrefab, new Vector3(6 + i * secondsPerBeat, 0, 0), Quaternion.Euler(0, 0, 90f));
            Instantiate(linePrefab, new Vector3(-6 + -i * secondsPerBeat, 0, 0), Quaternion.Euler(0, 0, 90f));

        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero); 

            if (hit.collider != null && hit.collider.CompareTag("Line"))
            {
                Vector3 linePosition = hit.collider.transform.position;
                if (activeNotes.ContainsKey(linePosition))
                {
                    GameObject existingNote = activeNotes[linePosition];
                    Destroy(existingNote);
                    activeNotes.Remove(linePosition);
                    Debug.Log("��Ʈ ����");
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(linePosition.x, linePosition.y + lineYOffset, 0);
                    GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                    activeNotes.Add(linePosition, newNote);
                    Debug.Log("��Ʈ ����");
                }
            }
        }
    }
    public void SaveMapData(string fileName)
    {
        // ��ųʸ��� �ִ� ��Ʈ�� MapData.notes ����Ʈ�� �߰�
        mapData.notes.Clear(); // ���� ������ �ʱ�ȭ
        foreach (var entry in activeNotes)
        {
            // ���� ���ӿ� �ʿ��� ����(�ð�, ���� ��)�� �����ؼ� NoteData ��ü�� ����ϴ�.
            // �Ʒ��� �����̹Ƿ�, ���� ������ Ÿ�Ӷ��� �� ���� ��Ģ�� �°� ������ �����ؾ� �մϴ�.
            NoteData newNote = new NoteData(entry.Key.y, 0, "Up"); // Y�� ��ġ�� �ð�����, ������ 0���� ����
            mapData.notes.Add(newNote);
        }

        string json = JsonUtility.ToJson(mapData, true); // ToJson(��ü, ���ڰ� ������� ����)
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json);
        Debug.Log("�� ������ ���� �Ϸ�: " + path);
    }
    public void LoadMapData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            mapData = JsonUtility.FromJson<MapData>(json);

            // ���� ��Ʈ ������Ʈ ��� ���� ��, �ε�� �����ͷ� �ٽ� ����
            foreach (var noteObj in activeNotes.Values)
            {
                Destroy(noteObj);
            }
            activeNotes.Clear();

            foreach (NoteData note in mapData.notes)
            {

                Debug.Log($"��Ʈ �ε�: Time={note.time}, Lane={note.lane}");
            }
            Debug.Log("�� ������ �ε� �Ϸ�");
        }
        else
        {
            Debug.LogError("������ ã�� �� �����ϴ�: " + path);
        }
    }

}
