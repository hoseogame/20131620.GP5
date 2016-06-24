using UnityEngine;
using System.Collections;

// 초기 카메라 위치
// 0, -14.03f, -8.55f
public class TitleScene : Scene {
    [SerializeField]
    private Transform cameraTransform;     // 카메라
    private Vector3 originPos;
    private Vector3 originRot;

    // 인트로 텍스트
    public GameObject introText;
    [SerializeField]
    // 전체광원
    private GameObject dirLight;

    private bool b_canStart;
    public static bool b_init = false;

    public override void Initialize()
    {
        if (!b_init)
        {
            originPos = new Vector3(0, -11.2f, -5f);
            originRot = new Vector3(-8.8f, 0, 0);

            cameraTransform.position = new Vector3(originPos.x, originPos.y, originPos.z);
            cameraTransform.rotation = Quaternion.Euler(originRot);

            b_init = true;
        }

        StartCoroutine(MoveCamera());
    }

    public override void Updated()
    {
        if (Input.GetMouseButtonDown(0))
        {
            introText.SetActive(false);
            SceneManager.sceneMgr.ChangeScene(SceneState.MAIN);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.QuitGame();
        }
    }

    public override void Exit()
    {
        
    }

    // 메인화면 -> 타이틀
    IEnumerator MoveCamera()
    {
        b_canStart = false;

        Vector3 curPos = cameraTransform.position;
        float time = 0;

        while(true)
        {
            time += Time.deltaTime * 2;

            cameraTransform.position = Vector3.Lerp(curPos, originPos, time);
            cameraTransform.rotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, originRot, time));

            if (time > 1.0f)
            {
                dirLight.SetActive(false);
                introText.SetActive(true);
                b_canStart = true;
                SoundManager.soundMgr.audioSource.clip = SoundManager.soundMgr.bgmList["Main"];
                SoundManager.soundMgr.audioSource.Play();
                break;
            }

            yield return null;
        }
    }
}
