using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private void Update()
    {
        obtainedSpear = PlayerPrefs.GetInt("SPEAR") == 1;
        obtainedSLASH = PlayerPrefs.GetInt("SLASH") == 1;
        obtainedMAP = PlayerPrefs.GetInt("MAP") == 1;
        hasBOOK = PlayerPrefs.GetInt("HASBOOK") == 1;
        obtainedHeal = PlayerPrefs.GetInt("HEAL") == 1;


        inMactanSFX = PlayerPrefs.GetInt("inMactan") == 1;
        inTondoSFX = PlayerPrefs.GetInt("inTondo") == 1;
        inIfugaoSFX = PlayerPrefs.GetInt("inIfugao") == 1;
        inCaveSFX = PlayerPrefs.GetInt("inCave") == 1;
        inSpaceSFX = PlayerPrefs.GetInt("inSpace") == 1;
        inSQSFX = PlayerPrefs.GetInt("inSQ") == 1;
    }
    //OTHER
    public bool isPaused = false;
    public bool Transitioning = false;
    public bool inParkourState = false;
    public bool isNPC = false;
    public bool canPause = true;
    public bool canOpenJournal = true;
    public bool newSkill = false;
    public bool newJournalChapter = false;

    //movement bool
    public bool jumping = false;
    public bool dashing = false;
    public bool running = false;
    public bool walking = false;

    //status bool
    public bool invincible = false;
    public bool canMove = true;
    public bool isAlive = true;
    public bool canAttack = true;
    public bool killedABoss = false;
    public bool SkillBOSS = false;
    public bool hasBOOK = false;

    //parry,block
    public bool blocking = false;
    public bool parry = false;

    //recoil
    public bool recoilingX, recoilingY;
    public bool lookingRight;


    //KEY CHECK
    public bool hasKey = false;

    //obtainskills
    public bool obtainedMAP = false;
    public bool obtainedSpear = false;
    public bool obtainedSLASH = false;
    public bool obtainedHeal = false;
    public bool obtainedEndure = false; // pag nag lamp lang siya ma rerecharge ONE TIME USE!!

    //SKILLS
    public bool SpearDash = false;
    public bool SLASH = false;
    public bool HPBUFF = false;

    //locations
    public bool inIfugao = false;
    public bool inMactan = false;
    public bool inTondo = false;
    public bool inOliver = false;
    public bool inCave = false;
    public bool inIfugaoSFX = false;
    public bool inMactanSFX = false;
    public bool inTondoSFX = false;
    public bool inOliverSFX = false;
    public bool inCaveSFX = false;
    public bool inSpaceSFX = false;
    public bool inSQSFX = false;

}
