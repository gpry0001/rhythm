using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titlebutton : MonoBehaviour
{
    private SpriteRenderer sp;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public GameObject backimage;
    public SpriteRenderer backsr;
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    void Start()
    {
        backimage.SetActive(false);
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sp.sprite = pressedImage;
            }
            if (Input.GetMouseButtonUp(0))
            {
                sp.sprite = defaultImage;

                if (CompareTag("Gamebutton"))
                {

                    StartCoroutine(CallScene(0, 1, "gamescene"));
                    
                }
                    
                //SceneManager.LoadScene("game");

                else if (CompareTag("Lodebutton"))
                {
                    StartCoroutine(CallScene(0, 1, "Nodesetting"));
                }
            }
        }
    }
    IEnumerator CallScene(float start, float end, string scenename)
    {
        GetComponent<Collider2D>().enabled = false;
        float currentTime = 0.0f;
        float percent = 0.0f;
        backimage.SetActive(true);
        while(percent < 1)
        {
            
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = backsr.color;
            color.a = Mathf.Lerp(start, end, percent);
            backsr.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        
        GetComponent<Collider2D>().enabled = true;
        Debug.Log(scenename);
        SceneManager.LoadScene(scenename);
        //backimage.SetActive(false);
    }
}
