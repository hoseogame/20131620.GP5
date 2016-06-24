using UnityEngine;
using System.Collections;

// 게임 매니저 클래스. 게임에 대한 관리를 한다.
public class GameManager : MonoBehaviour {
    public static GameManager gameMgr = null;
    [SerializeField]
    private SceneManager sceneMgr;
    [SerializeField]
    private SoundManager soundMgr;

    // 게임 매니저 클래스를 싱글톤으로 만든다.
    void Awake()
    {
        if (gameMgr == null) gameMgr = this;
        else if (gameMgr != this) Destroy(this.gameObject);

        Initialize();
    }

    // 모든 초기화는 여기서 이루어진다.
    void Initialize()
    {
        Screen.SetResolution(1280, 720, true);
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        sceneMgr.Initialize();
        soundMgr.Init();
    }

    // 모든 업데이트는 여기서 이루어진다.
    void Update()
    {
        sceneMgr.Updated();
    }

    // 게임 종료시에 호출할 함수.
    public static void QuitGame()
    {
        Application.Quit();
    }
}
