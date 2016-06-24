using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {
    public int musicCount;
    [SerializeField]
    private GameObject music_Obj;           // 음악 오브젝트
    [SerializeField]
    private GameObject[] musics;            // 음악들 관리할 배열
    [SerializeField]
    private Texture[] musicCover;

    public int curIdx;
    public Text musicName;

    // 터치 관련 변수
    private bool b_canDrag;
    private bool b_drag;
    private Vector2 originTouchPos;
    [SerializeField]
    private float correction;
    private Vector3[] originPos;

    public void init()
    {
        if (DifficultyScene.selectIdx == 1)
            transform.position = new Vector3(-2.5f, -8.9f, -1);
        else if (DifficultyScene.selectIdx == 0)
            transform.position = new Vector3(3, -8.9f, -1);


        originPos = new Vector3[musicCount];

        createMusic();
        curIdx = 0;
        b_canDrag = false;
        b_drag = false;
    }

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0) && !b_drag)
        {
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Music"))
            {
                originTouchPos = new Vector2(ray.origin.x, ray.origin.y);
                b_canDrag = true;
            }
        }

        if(b_canDrag && Input.GetMouseButtonUp(0))
        {
            if (originTouchPos.x - ray.origin.x > correction)
            {
                StartCoroutine(DragAnimation(Vector2.left));
                b_drag = true;
                b_canDrag = false;
            }

            else if (ray.origin.x - originTouchPos.x > correction)
            {
                StartCoroutine(DragAnimation(Vector2.right));
                b_drag = true;
                b_canDrag = false;
            }

            else
                b_canDrag = false;
        }

        musicName.text = "곡 이름 : " + musicCover[curIdx].name;
    }

    public void createMusic()
    {
        musics = new GameObject[musicCount];
        for (int i = 0; i < musicCount; i++)
        {
            musics[i] = GameObject.Instantiate(music_Obj);
            musics[i].transform.position = new Vector3(transform.position.x + (i * 7), transform.position.y + i * 3, i * 2.5f);
            musics[i].name = musicCover[i].name;
            musics[i].transform.parent = transform;
            musics[i].transform.FindChild("Cover").GetComponent<Renderer>().material.mainTexture = musicCover[i];
            originPos[i] = musics[i].transform.position;
        }
    }

    public void deleteMusic()
    {
        for (int i = 0; i < musicCount; i++)
        {
            Destroy(musics[i]);
        }
    }

    IEnumerator DragAnimation(Vector2 dir)
    {
        float time = 0;
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        while(true)
        {
            time += Time.deltaTime * 5;

            if (time > 1.2f)
            {
                if (dir == Vector2.right)
                    curIdx--;
                else
                    curIdx++;

                if (curIdx > 9) curIdx = 9;
                else if (curIdx < 0) curIdx = 0;

                for (int i = 0; i < musics.Length; i++)
                {
                    originPos[i] = musics[i].transform.position;
                }

                b_drag = false;
                break;
            }

            else
            {
                for (int i = 0; i < musics.Length; i++)
                {
                    if (dir == Vector2.right && curIdx > 0 && curIdx <= musics.Length - 1)
                    {
                        // 오른쪽 이동
                        if (i >= curIdx)
                        {
                            musics[i].transform.position = Vector3.Lerp(originPos[i], new Vector3(originPos[i].x + 7, originPos[i].y + 3, originPos[i].z + 2.5f), time);
                        }

                        else
                        {
                            musics[i].transform.position = Vector3.Lerp(originPos[i], new Vector3(originPos[i].x + 7, originPos[i].y - 3, originPos[i].z - 2.5f), time);
                        }
                    }

                    else if (dir == Vector2.left && curIdx >= 0 && curIdx < musics.Length - 1)
                    {
                        // 왼쪽 이동
                        if (i <= curIdx)
                        {
                            musics[i].transform.position = Vector3.Lerp(originPos[i], new Vector3(originPos[i].x - 7, originPos[i].y + 3, originPos[i].z + 2.5f), time);
                        }

                        else
                        {
                            musics[i].transform.position = Vector3.Lerp(originPos[i], new Vector3(originPos[i].x - 7, originPos[i].y - 3, originPos[i].z - 2.5f), time);
                        }
                    }
                }
            }

            yield return null;
        }
    }
}
