using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    Vector2 offset;

    int[,] map = {
    {0,0,5,1,3 },
    {5,0,3,4,3 },
    {1,0,4,4,8 },
    {1,0,5,8,8 }};

    public GameObject[] panels;

    public GameObject panelPrefab;


    // Use this for initialization
    void Start()
    {
        offset.x = -1 * map.GetLength(1) / 2.0f + 0.5f;
        offset.y = -1 * map.GetLength(0) / 2.0f + 0.5f;
        panels = new GameObject[map.Length];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                panels[i * map.GetLength(1) + j] = Instantiate(panelPrefab, new Vector3(j+offset.x, i+offset.y, 0), Quaternion.identity);
                panels[i * map.GetLength(1) + j].GetComponent<Panel>().SetColor(map[i,j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
