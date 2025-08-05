using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHit : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress;
    public GameObject eff;
    void Start()
    {

        if (transform.position.x > 0)
        {
            keyToPress = KeyCode.RightArrow;
        }
        else if (transform.position.x < 0)
        {
            keyToPress = KeyCode.LeftArrow;
        }
        else if (transform.position.y > 0)
        {
            keyToPress = KeyCode.UpArrow;
        }
        else if (transform.position.y < 0)
        {
            keyToPress = KeyCode.DownArrow;
        }
    }
    void Update()
    {
        if (canBePressed && Input.GetKeyDown(keyToPress))
        {
            gameObject.SetActive(false);
            Instantiate(eff, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Button"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Button"))
        {
            canBePressed = false;
            // Miss ���� ����: ��Ʈ�� ���� ������ ����� Miss ó���ϰ� �ı�
            Destroy(gameObject);
        }
    }
}
