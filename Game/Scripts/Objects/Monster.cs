using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
    [SerializeField]
    private Note note_Prefab;
    [SerializeField]
    private Transform startTransform;
    [SerializeField]
    private Transform[] attackTransform;
    public List<Note> notes;

    public bool b_attackEnd;
    public static float hp;

    public void Init()
    {
        b_attackEnd = false;
        hp = XmlTools.data.monsterHP;

        notes = new List<Note>(XmlTools.data.noteCount);

        for (int i = 0; i < XmlTools.data.noteCount; i++)
        {
            Note clone = Note.Instantiate(note_Prefab);
            clone.name = "Note";
            clone.Init(XmlTools.data.noteType[i], XmlTools.data.noteScore[i], XmlTools.data.esName[i]);
            clone.transform.position = startTransform.position;
            clone.transform.parent = startTransform;
            clone.gameObject.SetActive(false);

            notes.Add(clone);
        }
    }

    /// <summary>
    /// 공격!
    /// </summary>
    /// <returns></returns>
    public IEnumerator Attack()
    {
        int i = 0;
        while (true)
        {
            if (SoundManager.soundMgr.audioSource.time >= XmlTools.data.appearTime[i])
            {
                notes[i].gameObject.SetActive(true);
                StartCoroutine(notes[i].Move_Note(attackTransform[Random.Range(0, 5)].position));

                if (i < XmlTools.data.noteCount - 1)
                    i++;

                else if (i == XmlTools.data.noteCount - 1)
                {
                    b_attackEnd = true;
                    break;
                }
            }
            yield return null;
        }
    }
}
