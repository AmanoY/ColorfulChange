using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Panel : MonoBehaviour {
    public Transform[] panelTransforms;
    public bool isDruging;
    
    private Vector3 startPos;

    public int colorNumber = 0;

    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
   // Text ScoreText;

    //ドラッグ中移動
    private void OnMouseDown()
    {
        Debug.Log("down");


        isDruging = true;
    }

    
    private void OnMouseUp()
    {
        isDruging = false;
        transform.position = startPos;
    }

    public void SetColor(int value)
    {
        colorNumber = value;
        spriteRenderer.sprite = sprites[colorNumber];
    }

    public void SetStartPos(Vector3 pos)
    {
        startPos = pos;
    }

    private Transform FindSwapTarget()
    {
        Transform targetTransform=null;
        for (int i = 0; i < panelTransforms.Length; i++)
        {
            if (panelTransforms[i]==this.transform)
            {
                //Debug.Log("それは自分");
            }
            else
            {
                Vector3 d = this.transform.position - panelTransforms[i].position;
                if (d.magnitude<0.5f)
                {
                    targetTransform = panelTransforms[i];
                }
            }
        }
        return targetTransform;
    }

    // Use this for initialization
    void Start () {
        //int panelCount = GameObject.FindObjectsOfType<Panel>().Length;
        //パネルクラスを持つものをすべて探して配列化
        Panel[] panels = GameObject.FindObjectsOfType<Panel>();
        //パネルの数だけTransform配列を作る
        panelTransforms = new Transform[panels.Length];
        //パネルを登録する.
        for (int i = 0; i < panels.Length; i++)
        {
            panelTransforms[i] = panels[i].transform;
        }

        startPos = transform.position;

        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

	}
	
	//ドラッグ中にパネル入れかえ
	void Update () {
        if (isDruging == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10.0f;
            transform.position =Camera.main.ScreenToWorldPoint(mousePos);

            Transform target = FindSwapTarget();
            if (target!=null)
            {
                Vector3 tempPos = target.transform.position;
                target.transform.position = startPos;
                target.GetComponent<Panel>().SetStartPos(startPos);
                startPos = tempPos;
            }            
        }        
	}
}
