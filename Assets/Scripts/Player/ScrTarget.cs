using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrTarget : MonoBehaviour
{
    public Camera cam;
    public GameObject OwnGameObject;



    public void Awake()
    {
        this.cam = FindAnyObjectByType<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        this.OwnGameObject = GetComponent<GameObject>();

        StartCoroutine(this.scaleAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if(this.cam != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldMousePosition = cam.ScreenToWorldPoint(mousePosition);
            this.transform.position = worldMousePosition;
        }
        else
        {
        this.cam = FindAnyObjectByType<Camera>();
        }
    }

    private IEnumerator scaleAnimation()
    {

        for(int i = 0; i < 10; i++)
        {
            this.transform.localScale = new Vector3(0.3f + (0.02f * i),0.3f + (0.02f * i),transform.localScale.z);
            yield return new WaitForSeconds(0.05f);
        }

        
        for(int i = 0; i < 10; i++)
        {
            this.transform.localScale = new Vector3(0.5f - (0.02f * i),0.5f - (0.02f * i),transform.localScale.z);
            yield return new WaitForSeconds(0.05f);
        }

        
        StartCoroutine(this.scaleAnimation());
    }
}
