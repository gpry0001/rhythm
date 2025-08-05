using System.Collections.Generic; // List<T> 사용
using System.IO;                  // 파일 입출력 (Path.Combine, File.Exists, File.Delete, File.Create, StreamWriter) 사용
using System.Linq;                // LINQ (OrderBy) 사용
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
                if (activeNotes.ContainsKey(linePosition))
                {
                    GameObject existingNote = activeNotes[linePosition];
                    Destroy(existingNote);
                    activeNotes.Remove(linePosition);
                    Debug.Log("노트 삭제");
                }
                else
                {
                    Vector3 spawnPosition = new Vector3(linePosition.x, linePosition.y + lineYOffset, 0);
                    GameObject newNote = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                    activeNotes.Add(linePosition, newNote);
                    Debug.Log("노트 생성");
                }
            }
        }
    }
    public void SaveMapData(string fileName)
    {
        // 딕셔너리에 있는 노트를 MapData.notes 리스트에 추가
        mapData.notes.Clear(); // 기존 데이터 초기화
        foreach (var entry in activeNotes)
        {
            // 실제 게임에 필요한 정보(시간, 레인 등)를 추출해서 NoteData 객체로 만듭니다.
            // 아래는 예시이므로, 실제 게임의 타임라인 및 레인 규칙에 맞게 로직을 수정해야 합니다.
            NoteData newNote = new NoteData(entry.Key.y, 0, "Up"); // Y축 위치를 시간으로, 레인을 0으로 가정
            mapData.notes.Add(newNote);
        }

        string json = JsonUtility.ToJson(mapData, true); // ToJson(객체, 예쁘게 출력할지 여부)
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json);
        Debug.Log("맵 데이터 저장 완료: " + path);
    }
    public void LoadMapData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            mapData = JsonUtility.FromJson<MapData>(json);

            // 기존 노트 오브젝트 모두 삭제 후, 로드된 데이터로 다시 생성
            foreach (var noteObj in activeNotes.Values)
            {
                Destroy(noteObj);
            }
            activeNotes.Clear();

            foreach (NoteData note in mapData.notes)
            {

                Debug.Log($"노트 로드: Time={note.time}, Lane={note.lane}");
            }
            Debug.Log("맵 데이터 로드 완료");
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + path);
        }
    }

}
