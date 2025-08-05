using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttoncontroller : MonoBehaviour
{
    private AudioSource audioSource;
    private SpriteRenderer sp;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode KeyToPress;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트를 찾을 수 없습니다!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyToPress))
        {
            audioSource.Play();
            sp.sprite = pressedImage;
        }
        if (Input.GetKeyUp(KeyToPress))
        {
            sp.sprite = defaultImage;
        }
    }

}
