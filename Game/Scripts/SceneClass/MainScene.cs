using UnityEngine;
using System.Collections;

public class MainScene : Scene {
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform movedTransform;     // 카메라가 옮겨질 위치
    [SerializeField]
    private GameObject dirLight;     // 전체광원
    [SerializeField]
    private GameObject mainText;

    [SerializeField]
    private Shader outlines;
    [SerializeField]
    private SkinnedMeshRenderer[] characters;

    [SerializeField]
    private Collider[] cols;

    public override void Initialize()
    {
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }

        StartCoroutine(MoveCamera_initPosition());

        if (!SoundManager.soundMgr.audioSource.isPlaying)
            SoundManager.soundMgr.audioSource.Play();
    }

    public override void Updated()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.sceneMgr.ChangeScene(SceneState.TITLE);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Xbot"))
            {
                AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
                DifficultyScene.selectIdx = 0;
                characters[0].material.shader = outlines;
                characters[1].material.shader = Shader.Find("Standard");
            }

            else if (hit.collider != null && hit.collider.gameObject.CompareTag("Ybot"))
            {
                AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
                DifficultyScene.selectIdx = 1;
                characters[1].material.shader = outlines;
                characters[0].material.shader = Shader.Find("Standard");
            }

            else if (hit.collider.gameObject.name != "Cube")
                DifficultyScene.selectIdx = 2;
        }
    }

    public override void Exit()
    {
        characters[0].material.shader = Shader.Find("Standard");
        characters[1].material.shader = Shader.Find("Standard");
        mainText.SetActive(false);
    }

    // 타이틀 -> 메인화면
    IEnumerator MoveCamera_initPosition()
    {
        float time = 0;
        Vector3 curPos = cameraTransform.position;
        Vector3 curRot = new Vector3(cameraTransform.rotation.x, cameraTransform.rotation.y, cameraTransform.rotation.z);

        while(true)
        {
            time += Time.deltaTime * 2;

            if (time > 1.2f)
            {
                dirLight.SetActive(true);
                mainText.SetActive(true);
                DifficultyScene.selectIdx = 2;
                break;
            }

            else
            {
                cameraTransform.position = Vector3.Lerp(curPos, movedTransform.position, time);
                cameraTransform.rotation = Quaternion.Euler(Vector3.Lerp(curRot, Vector3.zero, time));
            }

            yield return null;
        }

        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }
    }

    public void Button_Back()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SceneManager.sceneMgr.ChangeScene(SceneState.TITLE);
    }

    public void Button_Stage()
    {
        if (DifficultyScene.selectIdx != 2)
        {
            AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
            SceneManager.sceneMgr.ChangeScene(SceneState.DIFFICULTY);
        }
    }
}
