using UnityEngine;
using System.Collections;

public class LoadingScene : Scene
{
    [SerializeField]
    private GameObject[] characters;
    private Animator animator;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private GameObject portal;
    private Vector3 portalPos;
    public Vector3 originCharPos;

    [SerializeField]
    private GameObject portal_another;

    private float time;

    public override void Initialize()
    {
        time = 0;
        portalPos = new Vector3(portal.transform.position.x, characters[DifficultyScene.selectIdx].transform.position.y, portal.transform.position.z);
        originCharPos = characters[DifficultyScene.selectIdx].transform.position;
        animator = characters[DifficultyScene.selectIdx].GetComponent<Animator>();
        animator.SetBool("isGameStart", true);
        StartCoroutine(XmlTools.LoadData("XMLData/stageData_001"));
    }

    public override void Updated()
    {
        time += Time.deltaTime * 0.3f;

        characters[DifficultyScene.selectIdx].transform.position = Vector3.Lerp(originCharPos, portalPos, time);

        if (characters[DifficultyScene.selectIdx].transform.position.z - cameraTransform.position.z > 10)
            cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, characters[DifficultyScene.selectIdx].transform.position.z - 10);

        if (time > 1.0f)
        {
            //soundMgr.audioSource.Stop();
            SceneManager.sceneMgr.ChangeScene(SceneState.PLAY);
        }

    }

    public override void Exit()
    {
        animator.SetBool("isReady", true);
        characters[DifficultyScene.selectIdx].transform.position = new Vector3(portal_another.transform.position.x, originCharPos.y, portal_another.transform.position.z + 50);
    }
}
