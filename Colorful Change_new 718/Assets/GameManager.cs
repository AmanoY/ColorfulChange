using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    int gyo;
    int retsu;
    public GameObject start;
    public GameObject clearText;
    public GameObject black;
    public GameObject failed;
    public GameObject gray;
    public GameObject effect;


    SpriteRenderer firstSprite;
    Color firstColor;

    bool isFirst = true;
    int firstPanelNumX;
    int firstPanelNumY;


    //内部データ
    private int[,] map;
    public Sprite[] mameSprites;
    public GameObject mamePrefab;
    public GameObject[] mameInstances;

    int[] ansCopy;

    Text scoreText;
    int score;

    void GanerateParticle(int x, int y)
    {
        Debug.Log(x + "," + y);
        Instantiate(effect, new Vector3(y, -x, -2), Quaternion.identity);
    }

    /// <summary>
    /// 再描画.
    /// </summary>
    void ReDraw()
    {
        //内部データに対応するマメの画像に差し替える.
        for (int i = 0; i < gyo; i++)
        {
            for (int j = 0; j < retsu; j++)
            {
                this.mameInstances[i * gyo + j].GetComponent<SpriteRenderer>().sprite = mameSprites[map[i, j]];
            }
        }
    }

    /// <summary>
    /// パネルを入れ替える.
    /// </summaryfirst
    /// <param name="num">パネル番号.</param>
    public void SwapPanel(int num)
    {
        if (score > 0)
        {
            //   0  1  2  3
            //0  0  1  2  3
            //1  4  5  6  7
            //2  8  9 10 11
            //3 12 13 14 15

            Debug.Log(num);
            //1回目と2回目で処理を分ける.
            if (isFirst)
            {
                //1回目のときの処理.
                //パネルの番号を記録する.
                firstPanelNumX = num % gyo;
                firstPanelNumY = num / gyo;

                firstSprite = mameInstances[num].GetComponent<SpriteRenderer>();
                firstColor = firstSprite.color;

                firstColor.a = 0.5f;
                firstSprite.color = firstColor;

                //次は1回目ではない.
                isFirst = false;
            }
            else
            {
                //2回目のときの処理.
                //次は1回目である.
                isFirst = true;

                int numX = num % gyo;
                int numY = num / gyo;
                //内部データを入れ替える.



                //Debug.Log("x:" + firstPanelNumX + " y:" + firstPanelNumY);
                //Debug.Log("x:" + numX + " y:" + numY);

                int temp = map[firstPanelNumY, firstPanelNumX];

                map[firstPanelNumY, firstPanelNumX] = map[numY, numX];
                map[numY, numX] = temp;



                //再描画する
                ReDraw();

                if (numX - 2 >= 0)
                {
                    if (map[numY, numX] == map[numY, numX - 2])
                    {
                        //Debug.Log("おなじ");
                        map[numY, numX - 1] = map[numY, numX];
                        GanerateParticle(numY, numX - 1);

                        this.GetComponent<AudioSource>();
                        ReDraw();
                    }
                }

                if (numX + 2 < retsu)
                {
                    if (map[numY, numX] == map[numY, numX + 2])
                    {
                        //Debug.Log("おなじ");
                        map[numY, numX + 1] = map[numY, numX];

                        GanerateParticle(numY, numX + 1);
                        this.GetComponent<AudioSource>();

                        ReDraw();
                    }
                }

                if (numY - 2 >= 0)
                {
                    if (map[numY, numX] == map[numY - 2, numX])
                    {
                        //Debug.Log("おなじ");
                        map[numY - 1, numX] = map[numY, numX];

                        GanerateParticle(numY - 1, numX);
                        this.GetComponent<AudioSource>();
                        ReDraw();
                    }
                }
                if (numY + 2 < gyo)
                {
                    if (map[numY, numX] == map[numY + 2, numX])
                    {
                        ////Debug.Log("おなじ");
                        map[numY + 1, numX] = map[numY, numX];

                        GanerateParticle(numY + 1, numX);
                        this.GetComponent<AudioSource>();

                        ReDraw();
                    }
                }

                //クリアチェック
                //int ans = 0;
                bool canClear = true;
                int[] colorCount = new int[10];
                for (int i = 0; i < gyo; i++)
                {
                    for (int j = 0; j < retsu; j++)
                    {
                        colorCount[map[i, j]]++;
                      
                    }
                }
                for (int i = 0; i < colorCount.Length; i++)
                {
                    if (colorCount[i] != ansCopy[i])
                    {
                        canClear = false;
                    }
                }
                if (canClear)
                {
                    // GanerateParticle();
                    decrease();
                    Invoke("ClearMethod", 1.0f);

                }
                else
                {
                    addScore();
                }

                firstColor.a = 1.0f;
                firstSprite.color = firstColor;
            }
        }
    }

    /*End Of Swap Panel*/

    public void init(int[,] smapCopy, int[] ans, int tekazu)
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        score = tekazu;
        scoreText.text = score.ToString();
        map = smapCopy;
        gyo = smapCopy.GetLength(0);
        retsu = smapCopy.GetLength(1);
        ansCopy = ans;
    }

    void Start()
    {

        black.SetActive(false);
        clearText.SetActive(false);
        failed.SetActive(false);
        gray.SetActive(false);
        //マメインスタンスを保存する配列を用意する.
        mameInstances = new GameObject[gyo * retsu];
        //マメをインスタンス化する.
        for (int i = 0; i < gyo; i++)
        {
            for (int j = 0; j < retsu; j++)
            {
                mameInstances[i * gyo + j] = Instantiate(mamePrefab, new Vector3(j, -i, 0), Quaternion.identity) as GameObject;
            }
        }
        //再描画する.
        ReDraw();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClearMethod()
    {
        Debug.Log("Clear");
        black.SetActive(true);
        clearText.SetActive(true);
        Invoke("LoadScene", 2.0f);

    }

    public void FailedMethod()
    {
        failed.SetActive(true);
        gray.SetActive(true);
        Invoke("LoadScene", 2.0f);

    }

    void LoadScene()
    {
        SceneManager.LoadScene("select");
    }


    void decrease()
    {
        score--;
        scoreText.text = score.ToString();
    }

    void addScore()
    {
        decrease();
        if (score <= 0)
        {
            Invoke("FailedMethod", 0.5f);
        }
    }
}


