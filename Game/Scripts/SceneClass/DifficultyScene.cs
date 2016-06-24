using UnityEngine;
using System.Collections;

public enum DIFFICULTY { EASY = 0, NORMAL, HARD }

public class DifficultyScene : Scene {
    [SerializeField]
    private Transform cameraTransform;      // 카메라
    [SerializeField]
    private GameObject[] characters;        // 캐릭터
    [SerializeField]
    private Transform[] movedTransform;     // 카메라가 옮겨질 위치

    // 현재 선택한 캐릭터
    public static int selectIdx;

    public GameObject difficultyText;       // UI 텍스트
    public GameObject difficulties_Parent;  // 난이도 오브젝트 부모
    public GameObject[] difficulties_obj;   // 난이도 오브젝트들
    private Vector3[] originPos;            // 난이도 오브젝트 초기 위치

    [SerializeField]
    private Shader colorChange;

    [SerializeField]
    private MeshRenderer[] cubeMaterials;
    [SerializeField]
    private MeshRenderer[] textMatreials;

    public static DIFFICULTY curDiff;          // 현재 난이도

    // 터치관련
    private bool b_canDrag;
    private Vector2 originTouchPos;
    public float correction;
    private bool b_drag;
    
    public override void Initialize()
    {
        if (DifficultyScene.selectIdx == 1)
            difficulties_Parent.transform.position = new Vector3(-3, -11, -15);
        else if (DifficultyScene.selectIdx == 0)
            difficulties_Parent.transform.position = new Vector3(5, -11, -15);


        originPos = new Vector3[3];

        for (int i = 0; i < originPos.Length; i++)
        {
            originPos[i] = difficulties_obj[i].transform.position;
        }

        b_canDrag = false;
        b_drag = false;
        SetColor();
        StartCoroutine(MoveCamera());
    }

    public override void Updated()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.sceneMgr.ChangeScene(SceneState.MAIN);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0) && !b_drag)
        {
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Difficulty"))
            {
                originTouchPos = new Vector2(ray.origin.x, ray.origin.y);
                b_canDrag = true;
            }
        }

        if (b_canDrag)
        {
            if (originTouchPos.x - ray.origin.x > correction && Input.GetMouseButtonUp(0))
            {
                StartCoroutine(DragAnimation(Vector2.right));
                b_drag = true;
                b_canDrag = false;
            }

            else if (ray.origin.x - originTouchPos.x > correction && Input.GetMouseButtonUp(0))
            {
                StartCoroutine(DragAnimation(Vector2.left));
                b_drag = true;
                b_canDrag = false;
            }
        }
    }

    public override void Exit()
    {
        difficultyText.SetActive(false);
        difficulties_Parent.SetActive(false);
        characters[1 - selectIdx].SetActive(true);
    }

    // 카메라 이동
    IEnumerator MoveCamera()
    {
        float time = 0;
        Vector3 curPos = cameraTransform.position;

        while (true)
        {
            if (selectIdx != 2)
            {
                time += Time.deltaTime * 2;

                if (time > 1.0f || Mathf.Abs(cameraTransform.position.x - movedTransform[selectIdx].position.x) < 0.1f)
                {
                    characters[1 - selectIdx].SetActive(false);
                    difficultyText.SetActive(true);
                    difficulties_Parent.SetActive(true);
                    break;
                }

                else
                {
                    cameraTransform.position = Vector3.Lerp(curPos, movedTransform[selectIdx].position, time);
                }
            }
            yield return null;
        }
    }

    public void Button_Back()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SceneManager.sceneMgr.ChangeScene(SceneState.MAIN);
    }

    public void Button_MusicSelect()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SceneManager.sceneMgr.ChangeScene(SceneState.MUSICSELECT);
    }

    IEnumerator DragAnimation(Vector2 dir)
    {
        float time = 0;
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        while(true)
        {
            time += Time.deltaTime * 5;

            
            if(time > 1.3f)
            {
                for(int i = 0; i < difficulties_obj.Length; i++)
                {
                    originPos[i] = difficulties_obj[i].transform.position;
                }

                if(dir == Vector2.right)
                {
                    curDiff++;

                    if (curDiff == DIFFICULTY.HARD + 1)
                        curDiff = DIFFICULTY.EASY;
                }

                else
                {
                    curDiff--;

                    if (curDiff == DIFFICULTY.EASY - 1)
                        curDiff = DIFFICULTY.HARD;
                }

                SetColor();
                b_drag = false;
                break;
            }

            else
            {
                for(int i = 0; i < difficulties_obj.Length; i++)
                {
                    if(dir == Vector2.right)
                    {
                        if (i == 0)
                            difficulties_obj[i].transform.position = Vector3.Lerp(originPos[i], originPos[2], time);
                        else
                            difficulties_obj[i].transform.position = Vector3.Lerp(originPos[i], originPos[i - 1], time);
                    }

                    else
                    {
                        if (i == 2)
                            difficulties_obj[i].transform.position = Vector3.Lerp(originPos[i], originPos[0], time);
                        else
                            difficulties_obj[i].transform.position = Vector3.Lerp(originPos[i], originPos[i + 1], time);
                    }
                }
            }


            yield return null;
        }
    }

    private void SetColor()
    {
        for(int i = 0; i < difficulties_obj.Length; i++)
        {
            if (i == (int)curDiff)
            {
                textMatreials[i].material.shader = Shader.Find("Standard");
                cubeMaterials[i].material.shader = Shader.Find("Standard");
            }

            else
            {
                textMatreials[i].material.shader = colorChange;
                cubeMaterials[i].material.shader = colorChange;
            }
        }
    }
}
