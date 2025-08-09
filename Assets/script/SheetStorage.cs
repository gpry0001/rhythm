using System.Collections.Generic; // List<T> ���
using System.IO;                  // ���� ����� (Path.Combine, File.Exists, File.Delete, File.Create, StreamWriter) ���
using System.Linq;                // LINQ (OrderBy) ���
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static TreeEditor.TreeEditorHelper;

public class SheetStorage : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject notePrefab;

    public int Linecnt;
    public float bpm;
    //public float lineYOffset = 0f;

    public InputField fileNameInput;

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
                Vector3 keyPosition = new Vector3(Mathf.Round(linePosition.x * 10) / 10, Mathf.Round(linePosition.y * 10) / 10, 0);
                if (activeNotes.ContainsKey(keyPosition))
                {
                    GameObject existingNote = activeNotes[keyPosition];
                    Destroy(existingNote);
                    activeNotes.Remove(keyPosition);
                    Debug.Log("��Ʈ ����");
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(linePosition.x, linePosition.y, 0);
                    GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                    // ��Ʈ�� �߰��� ���� �ݿø��� ��ġ�� Ű�� ����մϴ�.
                    activeNotes.Add(keyPosition, newNote);
                    Debug.Log("��Ʈ ����");
                }
            }
        }
    }
    public void SaveFromInput()
    {
        SaveMapData(fileNameInput.text);
    }

    public void LoadFromInput()
    {
        LoadMapData(fileNameInput.text);
    }

    public void SaveMapData(string fileName)
    {
        // ��ųʸ��� �ִ� ��Ʈ�� MapData.notes ����Ʈ�� �߰�
        mapData.notes.Clear(); // ���� ������ �ʱ�ȭ
        foreach (var entry in activeNotes)
        {
            float notePositionX = entry.Key.x;
            float notePositionY = entry.Key.y;
            string direction = "";
            int lane = 0;

            // X ��ǥ�� 0�� �ƴϸ� �¿� ����
            if (Mathf.Abs(notePositionX) > 0.1f) // 0.1f�� ���� ����
            {
                if (notePositionX > 0)
                {
                    direction = "Right";
                    lane = 1; // ������ ����
                }
                else
                {
                    direction = "Left";
                    lane = 2; // ���� ����
                }
            }
            // Y ��ǥ�� 0�� �ƴϸ� ���Ʒ� ����
            else if (Mathf.Abs(notePositionY) > 0.1f)
            {
                if (notePositionY > 0)
                {
                    direction = "Up";
                    lane = 3; // ���� ����
                }
                else
                {
                    direction = "Down";
                    lane = 4; // �Ʒ��� ����
                }
            }

            NoteData newNote = new NoteData(notePositionY, lane, direction, entry.Key);
            mapData.notes.Add(newNote);
        }

        string directoryPath = Path.Combine(Application.dataPath, "notejson");
        // ���ϸ��� �Ű�����(fileName)�� �޾ƿɴϴ�.
        string filePath = Path.Combine(directoryPath, fileName + ".json");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("�� ������ ���� �Ϸ�: " + filePath);
    }
    public void LoadMapData(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, "notejson", fileName + ".json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            mapData = JsonUtility.FromJson<MapData>(json);

            // ���� ��Ʈ ������Ʈ ��� ���� ��, �ε�� �����ͷ� �ٽ� ����
            foreach (var noteObj in activeNotes.Values)
            {
                Destroy(noteObj);
            }
            activeNotes.Clear();

            foreach (NoteData note in mapData.notes)
            {
                Vector3 spawnPosition = new Vector3(note.position.x, note.position.y, 0);
                // ��Ʈ�� �����ϰ� ��ųʸ��� �߰�
                GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                activeNotes.Add(note.position, newNote);
            }
            Debug.Log("�� ������ �ε� �Ϸ�");
        }
        else
        {
            Debug.LogError("������ ã�� �� �����ϴ�: " + filePath);
        }
    }

}
