using UnityEngine;
using System.Collections;

// 씬의 상태들을 나타내기 위한 열거형.
public enum SceneState { LOGO, TITLE, MAIN, DIFFICULTY, MUSICSELECT, LOADING, PLAY, RESULT }

public class SceneManager : MonoBehaviour {
    // 참조할 일이 많기 때문에 이렇게 선언함.
    public static SceneManager sceneMgr = null;

    [SerializeField]
    private Scene[] scenes;         // 씬들을 담을 변수.
    private Scene curScene;         // 현재 실행 중인 씬.
    private SceneState curState;    // 현재 씬의 상태가 무슨 씬인지 알려줌.
    
    // 초기화.
    public void Initialize()
    {
        if (sceneMgr == null) sceneMgr = this;
        else if (sceneMgr != this) Destroy(this.gameObject);

        ChangeScene(SceneState.LOGO);
    }

    // 업데이트.
    public void Updated()
    {
        if (curScene == null) return;

        curScene.Updated();
    }

    /// <summary>
    /// 씬을 변경할 때 쓸 함수다.
    /// </summary>
    /// <param name="state"> 변경하고 싶은 씬의 상태 </param>
    public void ChangeScene(SceneState state)
    {
        // 만약 바꿀 씬이 현재의 씬과 같다면 종료.
        if (scenes[(int)state] == curScene) return;

        // 현재 씬이 Null이 아니라면 Exit() 함수 호출.
        if (curScene != null) curScene.Exit();

        for(int i = 0; i < scenes.Length; i++)
        {
            if (i == (int)state)
            {
                curState = state;
                scenes[i].gameObject.SetActive(true);
                curScene = scenes[(int)state];
                curScene.Initialize();
            }

            else
                scenes[i].gameObject.SetActive(false);
        }
    }
}
