using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class GameManager : MonoBehaviour
{
    public GameObject player;
    int rowNum = 3;
    int colNum = 3;
    public GameObject[] mapPrefab;
    public GameObject[] monster;
    public GameObject[] capsule;
    public GameObject[] item;
    GameObject[] map = new GameObject[9];
    public static float time;
    float gameOverTime = 60f;
    bool isGameOver;
    bool isMove;
    // Start is called before the first frame update
    void Start()
    {
       
        for (int i = 0; i < mapPrefab.Length; i++)
        { map[i] = Instantiate(mapPrefab[i], player.transform.position + new Vector3(100 * (i / 3), 100 * (i % 3), 0), mapPrefab[i].transform.rotation); }

        isMove = false;


        isGameOver = false;
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!isMove) MapMove();
        GameOver();
        
    }

    /* void MonsterRespawn()
     {
         Instantiate(monster[0]);
     }*/

    // 1. 맵 이미지 생성
    // 2. 4개 맵 이미지 출력
    // 3. 위쪽 방향으로 이동 처리

    void MapMove()
    {
        Vector3 currentPos = map[0].transform.position;
       
        float cameraHeight = Camera.main.orthographicSize;
        Debug.Log("높이: " + cameraHeight);
        float cameraWidth = cameraHeight * Camera.main.aspect;
        Debug.Log("너비: " + cameraWidth);

        float camLeftSidePos = currentPos.x - cameraWidth;
        float upperLeftPos = currentPos.x - map[0].GetComponent<RectTransform>().rect.width / 2;


        if (player.transform.position.x - cameraWidth <= currentPos.x - map[0].GetComponent<RectTransform>().rect.width / 2)
        {
            map[6].transform.position = map[0].transform.position + new Vector3(-100, 0, 0);
            map[7].transform.position = map[1].transform.position + new Vector3(-100, 0, 0);
            map[8].transform.position = map[2].transform.position + new Vector3(-100, 0, 0);
            Debug.Log("맵 이동");
        }

        // 카메라의  오른쪽 방향 이동 판정만 제대로 구현.
     
    }

    void CapsuleRespawn()
    {

    }

    void GameOver()
    {
        if (time >= gameOverTime)
        {
            Debug.Log(time + "게임 종료");
            isGameOver = true;
        }
    }
    /*void MonsterProduction()
    {
        for (time = 0.0f; time < gameOverTime; time += Time)
        Instantiate(monster, )
    }*/

   /* void MapProduce()
    {

    }*/
}
