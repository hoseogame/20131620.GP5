using UnityEngine;
using System.Collections;

public enum NoteType { NONE, TOUCH, DRAG_LEFT, DRAG_RIGHT, DRAG_UP, DRAG_DOWN }

public class Note : MonoBehaviour {
    public NoteType noteType;
    public Texture2D[] tex;
    public MeshRenderer meshRenderer;
    public int noteScore;
    public float appearTime;
    public AudioClip audioClip;
    public GameObject ring;

    public bool b_destroy;
    public float time;

    public void Init(string _type, int _score, string _esName)
    {
        b_destroy = false;
        time = 0;
        noteType = StrToNoteType(_type);
        noteScore = _score;
        meshRenderer.material.mainTexture = tex[(int)noteType];
        ring.GetComponent<MeshRenderer>().material.shader = Shader.Find("Outline");

        if(_esName != "NONE")
            audioClip = SoundManager.soundMgr.esList[_esName];
    }

    //public void SetDestroy(bool value)
    //{
    //    b_destroy = value;
    //}

    /// <summary>
    /// 문자열 -> BulletState 변경
    /// </summary>
    /// <param name="str"> 문자열 </param>
    /// <returns></returns>
    public NoteType StrToNoteType(string str)
    {
        switch (str)
        {
            case "TOUCH":
                return NoteType.TOUCH;

            case "DRAG_UP":
                return NoteType.DRAG_UP;

            case "DRAG_LEFT":
                return NoteType.DRAG_LEFT;

            case "DRAG_RIGHT":
                return NoteType.DRAG_RIGHT;

            case "DRAG_DOWN":
                return NoteType.DRAG_DOWN;

            default:
                return NoteType.NONE;
        }
    }

    public void CheckNote(Vector3 pos)
    {
        if ((time > 0 && time < 0.3f) || (time > 0.95f && time > 1.0f))
        {
            PlayScene.score += 0;
            Destroy(gameObject.transform.FindChild("Ring").gameObject);
            Destroy(gameObject.transform.FindChild("Note").GetComponent<Collider>());
        }

        else if ((time >= 0.3f && time <= 0.4f) || (time >= 0.8f && time <= 0.95f))
        {
            b_destroy = true;
            StopCoroutine("Move_Note");
            PlayScene.score += (int)(noteScore * 0.5f);
            StartCoroutine(Move_Note_Monster(pos));
        }

        else if (time > 0.4f && time < 0.8f)
        {
            b_destroy = true;
            StopCoroutine("Move_Note");
            PlayScene.score += noteScore;
            StartCoroutine(Move_Note_Monster(pos));
        }
    }

    /// <summary>
    /// 노트 이동
    /// </summary>
    /// <param name="pos"> 목적지 </param>
    /// <param name="count"> 보정수치 </param>
    /// <returns></returns>
    public IEnumerator Move_Note(Vector3 pos)
    {
        Vector3 originPos = transform.position;
        Vector3 originScale = ring.transform.localScale;
        if (audioClip != null)
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);

        while (!b_destroy)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, pos, time);
            if(ring != null)
                ring.transform.localScale = Vector3.Lerp(originScale, Vector3.one * 0.5f, time);
            yield return new WaitForSeconds(Time.deltaTime);

            if (time > 1.0f)
            {
                PlayScene.life--;
                Destroy(gameObject);
                break;
            }
        }
    }

    public IEnumerator Move_Note_Monster(Vector3 pos)
    {
        Vector3 originPos = transform.position;
        AudioSource.PlayClipAtPoint(SoundManager.soundMgr.esList["Attack"], Camera.main.transform.position);
        Destroy(ring);
        float time = 0;
        transform.FindChild("Note").GetComponent<MeshRenderer>().material.shader = Shader.Find("ColorChanage");

        while (true)
        {
            time += Time.deltaTime;

            transform.position = Vector3.Lerp(originPos, pos, time);

            if (time > 1.0f)
            {
                Destroy(gameObject);
                Monster.hp -= noteScore;
                break;
            }

            yield return null;
        }
    }
}
