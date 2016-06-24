using UnityEngine;
using System.Collections;

public class MusicSelectScene : Scene {
    [SerializeField]
    private GameObject musicSelectText;

    public MusicManager musicMgr;

    public override void Initialize()
    {
        musicSelectText.SetActive(true);
        musicMgr.gameObject.SetActive(true);
        musicMgr.init();
    }

    public override void Updated()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.sceneMgr.ChangeScene(SceneState.DIFFICULTY);

        musicMgr.Update();
    }

    public override void Exit()
    {
        musicMgr.gameObject.SetActive(false);
        musicMgr.deleteMusic();
        musicSelectText.SetActive(false);
    }

    public void Button_Back()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SceneManager.sceneMgr.ChangeScene(SceneState.DIFFICULTY);
    }

    public void Button_GameStart()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SceneManager.sceneMgr.ChangeScene(SceneState.LOADING);
    }
}
