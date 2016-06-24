using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultScene : Scene {
    [SerializeField]
    private GameObject resultText;
    [SerializeField]
    private GameObject[] characters;
    public LoadingScene loadingScene;
    public Text score;

    public override void Initialize()
    {
        resultText.SetActive(true);     
    }

    public override void Updated()
    {
        score.text = PlayScene.score.ToString();
    }

    public override void Exit()
    {
        resultText.SetActive(false);
    }

    public void Button_CharacterSelect()
    {

        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        characters[DifficultyScene.selectIdx].GetComponent<Animator>().SetBool("isReady", false);
        characters[DifficultyScene.selectIdx].GetComponent<Animator>().SetBool("isGameStart", false);

        characters[DifficultyScene.selectIdx].transform.position = loadingScene.originCharPos;

        SceneManager.sceneMgr.ChangeScene(SceneState.MAIN);
    }

    public void Button_ReStart()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SceneManager.sceneMgr.ChangeScene(SceneState.PLAY);
    }
}
