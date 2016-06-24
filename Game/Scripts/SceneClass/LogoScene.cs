using UnityEngine;
using System.Collections;

public class LogoScene : Scene{
    private float time;
    [SerializeField]
    private GameObject logoText;

    public override void Initialize()
    {
        time = 0;
        logoText.SetActive(true);
    }

    public override void Updated()
    {
        time += Time.deltaTime;

        if (time > 5.0f)
            SceneManager.sceneMgr.ChangeScene(SceneState.TITLE);
    }

    public override void Exit()
    {
        logoText.SetActive(false);
    }
}
