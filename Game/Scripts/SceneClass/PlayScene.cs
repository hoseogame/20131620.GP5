using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayScene : Scene {
    public Transform cameraTransform;
    [SerializeField]
    private GameObject[] characters;
    [SerializeField]
    private GameObject window;
    [SerializeField]
    private GameObject playText;        // 플레이씬 UI
    [SerializeField]
    private Image fadein_Img;           // 페이드인 위한 이미지
    [SerializeField]
    private RectTransform current_Bar;          // 현재 BGM 상태

    public LoadingScene loadingScene;

    public bool b_ending;
    private float time;
    private float audioTime;

    private bool openWindow;
    public Monster monster;

    [SerializeField]
    private Image monsterhp;
    [SerializeField]
    private Text monsterhpText;
    [SerializeField]
    private Transform monsterTransform;
    
    [SerializeField]
    private Image playerLife;
    [SerializeField]
    private Text lifeText;
    public static int life;

    public static int score;
    
    public override void Initialize()
    {
        life = 2;
        score = 0;
        audioTime = 0;
        monster.Init();
        openWindow = false;
        b_ending = false;
        time = 0;
        StartCoroutine(FadeIn());
        playText.SetActive(true);
        window.SetActive(false);

        current_Bar.sizeDelta = new Vector2(0, 20);

        cameraTransform.position = new Vector3(characters[DifficultyScene.selectIdx].transform.position.x, 
                                               characters[DifficultyScene.selectIdx].transform.position.y + 10, 
                                               characters[DifficultyScene.selectIdx].transform.position.z - 6);

        cameraTransform.rotation = Quaternion.Euler(new Vector3(15, 0, 0));
        SoundManager.soundMgr.audioSource.clip = SoundManager.soundMgr.bgmList["Stage1"];

        lifeText.text = "100%";
        playerLife.rectTransform.sizeDelta = new Vector2(250, 20);
    }

    public override void Updated()
    {
        #region UI
        if (Input.GetKeyDown(KeyCode.Escape))
            openWindow = !openWindow;

        if (openWindow)
            window.SetActive(true);
        else
            window.SetActive(false);

        playerLife.rectTransform.sizeDelta = new Vector2(250 - 250 / 3 * (2 - life) , 20);
        lifeText.text = ((int)((playerLife.rectTransform.sizeDelta.x / 250) * 100)).ToString("000") + "%";

        if (SoundManager.soundMgr.audioSource.clip != null)
        {
            if (SoundManager.soundMgr.audioSource.time >= SoundManager.soundMgr.audioSource.clip.length - 0.1f && !b_ending)
            {
                current_Bar.sizeDelta = new Vector2(1280, 20);
                b_ending = true;
            }
            else if (SoundManager.soundMgr.audioSource.time <= SoundManager.soundMgr.audioSource.clip.length - 0.1f && !b_ending)
            {
                audioTime = SoundManager.soundMgr.audioSource.time;
                current_Bar.sizeDelta = new Vector2(((audioTime / SoundManager.soundMgr.audioSource.clip.length) * 1280), 20);
            }

            if (SoundManager.soundMgr.audioSource.time == 0 && b_ending && time > 3.0f || life < 0)
            {
                SceneManager.sceneMgr.ChangeScene(SceneState.RESULT);
            }
        }

        if (b_ending)
            time += Time.deltaTime;
        #endregion

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            if (hit.collider.name == "Note")
            {
                hit.transform.parent.gameObject.SendMessage("CheckNote", monsterTransform.position);
            }
        }
    }

    public override void Exit()
    {
        for (int i = 0; i < monster.notes.Count; i++ )
        {
            if (monster.notes[i] != null)
                Destroy(monster.notes[i].gameObject);
        }
        cameraTransform.gameObject.GetComponent<AudioSource>().clip = null;
        playText.SetActive(false);
        window.SetActive(false);

        SoundManager.soundMgr.audioSource.clip = SoundManager.soundMgr.bgmList["Main"];
    }

    public void Button_Yes()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        SoundManager.soundMgr.audioSource.Stop();

        characters[DifficultyScene.selectIdx].GetComponent<Animator>().SetBool("isReady", false);
        characters[DifficultyScene.selectIdx].GetComponent<Animator>().SetBool("isGameStart", false);

        characters[DifficultyScene.selectIdx].transform.position = loadingScene.originCharPos;

        SceneManager.sceneMgr.ChangeScene(SceneState.MAIN);
    }

    public void Button_No()
    {
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["UITouch"], Camera.main.transform.position);
        openWindow = false;
    }

    IEnumerator FadeIn()
    {
        fadein_Img.gameObject.SetActive(true);
        fadein_Img.color = new Color(0, 0, 0, 1);
        monsterhp.rectTransform.sizeDelta = new Vector2(0, 20);
        Vector2 originScale = monsterhp.rectTransform.sizeDelta;

        float time = 0;
        while(true)
        {
            time += Time.deltaTime * 0.5f;
            monsterhp.rectTransform.sizeDelta = Vector2.Lerp(originScale, new Vector2(250, 20), time);
            playerLife.rectTransform.sizeDelta = Vector2.Lerp(originScale, new Vector2(250, 20), time);
            fadein_Img.color = new Color(0, 0, 0, 1 - time);
            
            monsterhpText.text = ((int)((monsterhp.rectTransform.sizeDelta.x / 250) * 100)).ToString("000") + "%";
            lifeText.text = ((int)((playerLife.rectTransform.sizeDelta.x / 250) * 100)).ToString("000") + "%";
            if (time > 1.2f)
            {
                fadein_Img.gameObject.SetActive(false);
                SoundManager.soundMgr.audioSource.Play();
                StartCoroutine(monster.Attack());
                break;
            }
            yield return null;
        }
    }

}
