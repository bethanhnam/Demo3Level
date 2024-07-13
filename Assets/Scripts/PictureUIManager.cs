using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PictureUIManager : MonoBehaviour
{

    [SerializeField]
    private ItemInStage[] stage;

    private int level;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public SkeletonGraphic[] characters;

    public GameObject windowObj;

    public String[] animationStage = { "idle_sad", "sad-happy", "happy", "idle_happy", "tremble", "tremble_happy" };
    public ItemInStage[] Stage { get => stage; set => stage = value; }
    public int Level { get => level; set => level = value; }
    public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }

    //bool
    public bool hasWindow = false;

    //tutor pic
    public PicTutor picTutor;
    public bool hasFixed = false;

    //[Button("TransferValue")]
    //private void Test()
    //{
    //    for (int i = 0; i < stage.Length; i++)
    //    {
    //        var stagetest = stage[i];
    //        stagetest.listObjLock.Clear();
    //        for (int j = 0; j < stage[i].ObjLock.Length; j++)
    //        {
    //            stagetest.listObjLock.Add(new());
    //            stagetest.listObjLock[j].objLock = new();
    //            stagetest.listObjLock[j].objLock.Add(new());
    //            stagetest.listObjLock[j].objLock[0] = stagetest.ObjLock[j];
    //            stagetest.listObjLock[j].objunLock = new();
    //            stagetest.listObjLock[j].objunLock.Add(new());
    //            stagetest.listObjLock[j].objunLock[0] = stagetest.ObjunLock[j];
    //            stagetest.listObjLock[j].objBtn = new();
    //            stagetest.listObjLock[j].objBtn.Add(new());
    //            stagetest.listObjLock[j].objBtn[0] = stagetest.ObjBtn[j];

    //        }
    //    }
    //}

    public void Init(int _level)
    {

        level = _level;
        if (level >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
        {
            Debug.LogWarning("level "+ level);
            level = 0;
        }

        for (int i = 0; i < stage.Length; i++)
        {
            if (i < DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
            {
                for (int j = 0; j < stage[i].listObjLock.Count; j++)
                {
                    for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                    {
                        if (stage[i].listObjLock[j].objBtn[k].activeSelf)
                        {
                            stage[i].listObjLock[j].objBtn[k].SetActive(false);
                        }
                    }
                    for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                    {
                        if (stage[i].listObjLock[j].objLock[k].activeSelf)
                        {
                            stage[i].listObjLock[j].objLock[k].SetActive(false);
                        }
                    }
                    for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                    {
                        if (!stage[i].listObjLock[j].objunLock[k].activeSelf)
                        {
                            stage[i].listObjLock[j].objunLock[k].SetActive(true);
                        }
                    }
                }
            }
            else
            {
                if (i == DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
                {
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                            {
                            
                                if (stage[i].listObjLock[j].objBtn[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objBtn[k].SetActive(false);

                                }
                                PlayerPrefs.SetInt("lastLevelActived", level);
                                PlayerPrefs.SetInt("lastLevelStageActived", DataLevelManager.Instance.DataLevel.Data[level].IndexStage);
                            }
                        }
                        else
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                            {
                                if (!stage[i].listObjLock[j].objBtn[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objBtn[k].SetActive(true);
                                }
                            }

                        }

                    }
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                            {
                            
                                if (stage[i].listObjLock[j].objLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objLock[k].SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                            {
                                if (!stage[i].listObjLock[j].objLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objLock[k].SetActive(true);
                                }
                            }

                        }

                    }
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                            {
                          
                                if (!stage[i].listObjLock[j].objunLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objunLock[k].SetActive(true);
                                }
                            
                            }
                        }
                        else
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                            {
                                if (stage[i].listObjLock[j].objunLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objunLock[k].SetActive(false);
                                }
                            }

                        }
                    }
                }
                else
                {
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                        {
                            if (stage[i].listObjLock[j].objBtn[k].activeSelf)
                            {
                                stage[i].listObjLock[j].objBtn[k].SetActive(false);
                            }
                        }
                        for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                        {
                            if (stage[i].listObjLock[j].objLock[k].activeSelf)
                            {
                                stage[i].listObjLock[j].objLock[k].SetActive(true);
                            }
                        }
                        for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                        {
                            if (stage[i].listObjLock[j].objunLock[k].activeSelf)
                            {
                                stage[i].listObjLock[j].objunLock[k].SetActive(false);
                            }
                        }
                    }
                }
            }
        }
        SetStarText();
        Debug.Log("chạy xong setStarText");
        CheckForWindow();
        Debug.Log("chạy xong checkForWindow");

    }
    public void CheckForWindow()
    {
        Debug.Log("LevelManagerNew.Instance.LevelBase.Level " + LevelManagerNew.Instance.LevelBase.Level);
        if (LevelManagerNew.Instance.LevelBase.Level == 0)
        {
            if (windowObj.gameObject != null && windowObj.gameObject.activeSelf)
            {
                hasWindow = true;
                Debug.Log("chạy qua has window true");
            }
            else
            {
                PlayerPrefs.SetInt("windowFixed", 1);
                hasWindow = false;
                Debug.Log("chạy qua has window fal trong true");
            }
        }
        else
        {
            PlayerPrefs.SetInt("windowFixed", 1);
            hasWindow = false;
            Debug.Log("chạy qua has window fal");
        }
    }
    public void ChangeReaction(float time, string t, bool loop, bool hasWindow)
    {
        StartCoroutine(ChangeReaction1(time, t, loop, hasWindow));
    }
    IEnumerator ChangeReaction1(float time, string t, bool loop, bool hasWindow)
    {
        yield return new WaitForSeconds(time);
        if (characters != null)
        {
            foreach (var character in characters)
            {
                character.gameObject.SetActive(true);
                character.AnimationState.SetAnimation(0, t, loop);
            }
        }
    }
    public void HiddenButton()
    {
        for (int j = 0; j < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock.Count; j++)
        {
            for (int k = 0; k < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn.Count; k++)
            {
                stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn[k].SetActive(false);
            }
        }
    }
    public void DisplayButton()
    {
        for (int j = 0; j < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock.Count; j++)
        {
            for (int k = 0; k < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn.Count; k++)
            {
                if (!DataLevelManager.Instance.DataLevel.Data[level].Stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].DataItmeLevel[j].IsUnlock)
                {
                    stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn[k].SetActive(true);
                    stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn[k].transform.localScale = Vector3.zero;
                    stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn[k].transform.DOScale(1f, 0.2f);
                    stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn[k].transform.GetChild((1)).GetComponent<Image>().SetNativeSize();
                }
            }
        }
        SetStarText();

    }
    public void SetStarText()
    {
        for (int j = 0; j < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock.Count; j++)
        {
            for (int k = 0; k < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn.Count; k++)
            {
                var starText = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[j].Star;
                stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].listObjLock[j].objBtn[k].GetComponentInChildren<LevelButton>().SetStarText(starText);
            }
        }
    }
    public void ChangeItem(GameObject obj)
    {
        int bouncingImage = Animator.StringToHash("FixedObjBouncing");
        obj.GetComponent<Animator>().Play(bouncingImage);
    }

    public void Close()
    {
        CanvasGroup.blocksRaycasts = false;
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
        CanvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    public void Open()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
        CanvasGroup.DOFade(1, 0.3f).OnComplete(() =>
        {

            CanvasGroup.blocksRaycasts = true;
        });
    }
    public void DisableCV()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    public void EnableCV()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    public void ChangeItemOnly(int _level, bool showBT = true)
    {
        level = _level;
        if (level >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
        {
            level = 0;
        }

        for (int i = 0; i < stage.Length; i++)
        {
            if (i < DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
            {
                for (int j = 0; j < stage[i].listObjLock.Count; j++)
                {
                    for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                    {
                        if (stage[i].listObjLock[j].objBtn[k].activeSelf)
                        {
                            stage[i].listObjLock[j].objBtn[k].SetActive(false);
                        }
                    }
                    for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                    {
                        if (stage[i].listObjLock[j].objLock[k].activeSelf)
                        {
                            stage[i].listObjLock[j].objLock[k].SetActive(false);
                        }
                    }
                    for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                    {
                        if (!stage[i].listObjLock[j].objunLock[k].activeSelf)
                        {
                            stage[i].listObjLock[j].objunLock[k].SetActive(true);
                        }
                    }
                }
                Debug.Log("chạy xong đổi các item stage trước");
            }
            else
            {
                if (i == DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
                {
                    //for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    //{
                    //    for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                    //    {
                    //        if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
                    //        {
                    //            if (stage[i].listObjLock[j].objBtn[k].activeSelf)
                    //            {
                    //                stage[i].listObjLock[j].objBtn[k].SetActive(false);

                    //            }
                    //            PlayerPrefs.SetInt("lastLevelActived", level);
                    //            PlayerPrefs.SetInt("lastLevelStageActived", DataLevelManager.Instance.DataLevel.Data[level].IndexStage);
                    //        }
                    //        else
                    //        {
                    //            if (!stage[i].listObjLock[j].objBtn[k].activeSelf)
                    //            {
                    //                stage[i].listObjLock[j].objBtn[k].SetActive(true);
                    //            }

                    //        }
                    //    }

                    //}
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                            {
                            
                                if (stage[i].listObjLock[j].objLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objLock[k].SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                            {
                                if (!stage[i].listObjLock[j].objLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objLock[k].SetActive(true);
                                }
                            }

                        }
                    }
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                            {
                          
                                if (!stage[i].listObjLock[j].objunLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objunLock[k].SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                            {
                                if (stage[i].listObjLock[j].objunLock[k].activeSelf)
                                {
                                    stage[i].listObjLock[j].objunLock[k].SetActive(false);
                                }
                            }

                        }
                    }
                    Debug.Log("chạy xong đúng stage");
                }
                else
                {
                    for (int j = 0; j < stage[i].listObjLock.Count; j++)
                    {
                        for (int k = 0; k < stage[i].listObjLock[j].objBtn.Count; k++)
                        {
                            if (stage[i].listObjLock[j].objBtn[k].activeSelf)
                            {
                                stage[i].listObjLock[j].objBtn[k].SetActive(false);
                            }
                        }
                        for (int k = 0; k < stage[i].listObjLock[j].objLock.Count; k++)
                        {
                            if (stage[i].listObjLock[j].objLock[k].activeSelf)
                            {
                                stage[i].listObjLock[j].objLock[k].SetActive(true);
                            }
                        }
                        for (int k = 0; k < stage[i].listObjLock[j].objunLock.Count; k++)
                        {
                            if (stage[i].listObjLock[j].objunLock[k].activeSelf)
                            {
                                stage[i].listObjLock[j].objunLock[k].SetActive(false);
                            }
                        }
                    }
                    Debug.Log("chạy xong đóng hết item");
                }
            }
        }
        HiddenButton();
        Debug.Log("chạy xong changeitemonly");
        StartCoroutine(NormalInit(showBT));
    }
    IEnumerator NormalInit(bool showBT)
    {
        yield return new WaitForSeconds(.5f);
        Init(level);
        if (showBT)
        {
            Debug.Log("chạy xong showBT");
            DisplayButton();
        }
        //if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        //{
        //	UIManagerNew.Instance.ButtonMennuManager.Appear();
        //}
        if (!GameManagerNew.Instance.CheckSliderValueAndDisplay())
        {
            Debug.Log("chạy vào trong normalInit");
            UIManagerNew.Instance.ButtonMennuManager.ActiveCVGroup();
        }
        Debug.Log("chạy xong normalInit");
    }

    public Vector3 GetCurrentPosItem()
    {
        return stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].listObjLock[GameManagerNew.Instance.Level].objunLock[0].transform.position;
    }
    public void DisableCharacter()
    {
        foreach (var character in characters)
        {
            Destroy(character.gameObject);
        }
    }
    public void SetHasWindowFirstTime()
    {
        if (PlayerPrefs.GetInt("windowFixed") == 1)
        {
            hasWindow = false;
            ChangeReaction(0, "idle_sad", true, GameManagerNew.Instance.PictureUIManager.hasWindow);
        }
    }
    //  public void DisplayPicStory(int _level, bool showBT = true)
    //  {
    //      level = _level;
    //      if (level >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
    //      {
    //          level = 0;
    //      }

    //      for (int i = 0; i < stage.Length; i++)
    //      {
    //          if (i < DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
    //          {
    //              for (int j = 0; j < stage[i].ObjBtn.Length; j++)
    //              {
    //                  if (stage[i].ObjBtn[j].activeSelf)
    //                  {
    //                      stage[i].ObjBtn[j].SetActive(false);
    //                  }
    //              }

    //              for (int j = 0; j < stage[i].ObjLock.Length; j++)
    //              {
    //                  if (stage[i].ObjLock[j].activeSelf)
    //                  {
    //                      stage[i].ObjLock[j].SetActive(false);
    //                  }
    //              }

    //              for (int j = 0; j < stage[i].ObjunLock.Length; j++)
    //              {
    //                  if (!stage[i].ObjunLock[j].activeSelf)
    //                  {
    //                      stage[i].ObjunLock[j].SetActive(true);
    //                  }
    //              }
    //          }
    //          else
    //          {
    //              if (i == DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
    //              {
    //                  //for (int j = 0; j < stage[i].ObjBtn.Length; j++)
    //                  //{
    //                  //	if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
    //                  //	{
    //                  //		if (stage[i].ObjBtn[j].activeSelf)
    //                  //		{
    //                  //			stage[i].ObjBtn[j].SetActive(false);
    //                  //		}
    //                  //	}
    //                  //	else
    //                  //	{
    //                  //		if (!stage[i].ObjBtn[j].activeSelf)
    //                  //		{
    //                  //			stage[i].ObjBtn[j].SetActive(true);
    //                  //		}
    //                  //	}
    //                  //}

    //                  for (int j = 0; j < stage[i].ObjLock.Length; j++)
    //                  {
    //                      if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
    //                      {
    //                          if (stage[i].ObjLock[j].activeSelf)
    //                          {
    //                              stage[i].ObjLock[j].SetActive(false);
    //                          }
    //                      }
    //                      else
    //                      {
    //                          if (!stage[i].ObjLock[j].activeSelf)
    //                          {
    //                              stage[i].ObjLock[j].SetActive(true);
    //                          }
    //                      }
    //                  }

    //                  for (int j = 0; j < stage[i].ObjunLock.Length; j++)
    //                  {
    //                      if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
    //                      {
    //                          if (!stage[i].ObjunLock[j].activeSelf)
    //                          {
    //                              stage[i].ObjunLock[j].SetActive(true);
    //                          }
    //                          hasFixed = true;
    //                      }
    //                      else
    //                      {
    //                          if (stage[i].ObjunLock[j].activeSelf)
    //                          {
    //                              stage[i].ObjunLock[j].SetActive(false);
    //                          }
    //                      }
    //                  }
    //              }
    //              else
    //              {
    //                  for (int j = 0; j < stage[i].ObjBtn.Length; j++)
    //                  {
    //                      if (stage[i].ObjBtn[j].activeSelf)
    //                      {
    //                          stage[i].ObjBtn[j].SetActive(false);
    //                      }
    //                  }

    //                  for (int j = 0; j < stage[i].ObjLock.Length; j++)
    //                  {
    //                      if (stage[i].ObjLock[j].activeSelf)
    //                      {
    //                          stage[i].ObjLock[j].SetActive(true);
    //                      }
    //                  }

    //                  for (int j = 0; j < stage[i].ObjunLock.Length; j++)
    //                  {
    //                      if (stage[i].ObjunLock[j].activeSelf)
    //                      {
    //                          stage[i].ObjunLock[j].SetActive(false);
    //                      }
    //                  }
    //              }
    //          }
    //      }
    //EnableCV();
    //  }
}
[Serializable]
public class ItemInStage
{
    //[SerializeField]
    //private GameObject[] objLock;
    //[SerializeField]
    //private GameObject[] objunLock;
    //[SerializeField]
    //private GameObject[] objBtn;

    [SerializeField] public List<ItemStage> listObjLock;


    //public GameObject[] ObjLock { get => objLock; set => objLock = value; }
    //public GameObject[] ObjunLock { get => objunLock; set => objunLock = value; }
    //public GameObject[] ObjBtn { get => objBtn; set => objBtn = value; }
}

[Serializable]
public class ItemStage
{
    [SerializeField]
    public List<GameObject> objLock;
    [SerializeField]
    public List<GameObject> objunLock;
    [SerializeField]
    public List<GameObject> objBtn;
}