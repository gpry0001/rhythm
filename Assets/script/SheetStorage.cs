using System.Collections.Generic; // List<T> 사용
using System.IO;                  // 파일 입출력 (Path.Combine, File.Exists, File.Delete, File.Create, StreamWriter) 사용
using System.Linq;                // LINQ (OrderBy) 사용
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

    private MapData mapData; // MapData 객체로 노트 데이터를 관리
    private Dictionary<Vector3, GameObject> activeNotes = new Dictionary<Vector3, GameObject>();

    private void Start()
    {
        mapData = new MapData(bpm); // 맵 데이터 초기화
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
                    Debug.Log("노트 삭제");
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(linePosition.x, linePosition.y, 0);
                    GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                    // 노트를 추가할 때도 반올림된 위치를 키로 사용합니다.
                    activeNotes.Add(keyPosition, newNote);
                    Debug.Log("노트 생성");
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
        // 딕셔너리에 있는 노트를 MapData.notes 리스트에 추가
        mapData.notes.Clear(); // 기존 데이터 초기화
        foreach (var entry in activeNotes)
        {
            float notePositionX = entry.Key.x;
            float notePositionY = entry.Key.y;
            string direction = "";
            int lane = 0;

            // X 좌표가 0이 아니면 좌우 라인
            if (Mathf.Abs(notePositionX) > 0.1f) // 0.1f는 오차 범위
            {
                if (notePositionX > 0)
                {
                    direction = "Right";
                    lane = 1; // 오른쪽 레인
                }
                else
                {
                    direction = "Left";
                    lane = 2; // 왼쪽 레인
                }
            }
            // Y 좌표가 0이 아니면 위아래 라인
            else if (Mathf.Abs(notePositionY) > 0.1f)
            {
                if (notePositionY > 0)
                {
                    direction = "Up";
                    lane = 3; // 위쪽 레인
                }
                else
                {
                    direction = "Down";
                    lane = 4; // 아래쪽 레인
                }
            }

            NoteData newNote = new NoteData(notePositionY, lane, direction, entry.Key);
            mapData.notes.Add(newNote);
        }

        string directoryPath = Path.Combine(Application.dataPath, "notejson");
        // 파일명을 매개변수(fileName)로 받아옵니다.
        string filePath = Path.Combine(directoryPath, fileName + ".json");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("맵 데이터 저장 완료: " + filePath);
    }
    public void LoadMapData(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, "notejson", fileName + ".json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            mapData = JsonUtility.FromJson<MapData>(json);

            // 기존 노트 오브젝트 모두 삭제 후, 로드된 데이터로 다시 생성
            foreach (var noteObj in activeNotes.Values)
            {
                Destroy(noteObj);
            }
            activeNotes.Clear();

            foreach (NoteData note in mapData.notes)
            {
                Vector3 spawnPosition = new Vector3(note.position.x, note.position.y, 0);
                // 노트를 생성하고 딕셔너리에 추가
                GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                activeNotes.Add(note.position, newNote);
            }
            Debug.Log("맵 데이터 로드 완료");
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + filePath);
        }
    }

}
