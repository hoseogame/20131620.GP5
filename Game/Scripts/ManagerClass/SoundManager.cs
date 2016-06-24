using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    public Dictionary<string, AudioClip> bgmList;
    public Dictionary<string, AudioClip> esList;
    public AudioSource audioSource;

    public static SoundManager soundMgr = null;

    public void Init()
    {
        if (soundMgr == null) soundMgr = this;
        else if (soundMgr != this) Destroy(this.gameObject);

        bgmList = new Dictionary<string, AudioClip>();
        esList = new Dictionary<string, AudioClip>();
        string bgmPath = "Sounds/BGM/";

        bgmList.Add("Main", (AudioClip)Resources.Load(bgmPath + "Main"));
        bgmList.Add("Stage1", (AudioClip)Resources.Load(bgmPath + "001"));

        string esPath = "Sounds/EffectSound/";
        esList.Add("en_blademaster_attack_far", (AudioClip)Resources.Load(esPath + "en_blademaster_attack_far"));
        esList.Add("en_blademaster_attack_near", (AudioClip)Resources.Load(esPath + "en_blademaster_attack_near"));
        esList.Add("en_blademaster_death", (AudioClip)Resources.Load(esPath + "en_blademaster_death"));
        esList.Add("en_blademaster_hit", (AudioClip)Resources.Load(esPath + "en_blademaster_hit"));
        esList.Add("Attack", (AudioClip)Resources.Load(esPath + "attack_fire"));
        esList.Add("UITouch", (AudioClip)Resources.Load(esPath + "obj_slide"));
    }
}
