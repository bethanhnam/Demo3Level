using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Stage : MonoBehaviour
{
    public static Stage Instance;
    public ExtraHoleButton holeToUnlock;
    public GameObject Square;
    // Start is called before the first frame update

    [SerializeField]
    public Hole[] holes;
    [SerializeField]
    public NailControl[] nailControls;
    [SerializeField]
    public IronPlate[] ironPlates;
    [SerializeField]
    public int numOfIronPlates;
    [SerializeField]
    public NailControl curNail;
    [SerializeField]
    private Hole curHole;
    [SerializeField]
    public List<NailDetector> nailDetectors = new List<NailDetector>();
    public List<IronPlate> selectedIrons = new List<IronPlate>();
    [SerializeField]
    private Hole preHole;

    //extra 
    public string layerName = "Hole";
    //deteleting
    [SerializeField]
    private Hole holeToDetele;
    [SerializeField]
    NailControl nailToDetele;
    public bool isDeteleting = false;
    public bool hasDelete;

    public bool hasUndo = true;

    // hingeJoint
    public List<HingeJoint2D> HingeJointBeforeRemove = new List<HingeJoint2D>();

    //undo 
    [SerializeField] Hole holeToUndo;
    [SerializeField] Hole holeBeforeUndo;
    [SerializeField] NailControl nailToUndo;
    [SerializeField] private GameObject holeBeforeMove;
    [SerializeField] private GameObject holeBeToReturn;
    public bool hasSave;
    public List<IronPlate> ironObjectsBeforemove = new List<IronPlate>();
    public List<Vector3> ironObjectsTransformBeforemove = new List<Vector3>();
    public List<Quaternion> ironObjectsRotationBeforemove = new List<Quaternion>();
    public List<NailControl> nailObjectsBeforemove = new List<NailControl>();
    public List<NailControl> nailObjectsBeforeBoom = new List<NailControl>();
    public List<NailControl> nailsJointBeforemove = new List<NailControl>();
    public List<Vector3> nailObjectsTransformBeforemove = new List<Vector3>();

    //notice 
    public List<Hole> numOfHoleNotAvailable = new List<Hole>();
    public bool checked1 = false;

    //tutor
    public bool isTutor = false;
    public bool isLvTutor = false;
    public Vector3 targetScale;

    public GameObject pointer;
    public TutorPointer pointerTutor;

    public bool canInteract = true;


    public bool checkForWinning = false;
    public bool isWining = false;
    public bool isLosing = false;
    public bool isScaling = false;

    //check for movement
    public bool isMoving = false;
    public Tween boosterTween;
    public Tween boosterTween1;

    //weekly event
    public String eventBarName;
    public int numOfEventItem = 0;

    private void Start()
    {
        Instance = this;
        ClearData();
        numOfIronPlates = ironPlates.Length;

    }
    private void OnEnable()
    {
        if (!GameManagerNew.Instance.isStory && LevelManagerNew.Instance.stage > 3 && !GameManagerNew.Instance.isMinigame)
        {
            StartCoroutine(CheckForClickContinuously());
        }
        try
        {
            if (GamePlayPanelUIManager.Instance.gameObject.activeSelf == false)
            {
                GamePlayPanelUIManager.Instance.Appear();
            }
        }
        catch
        {

        }
        checkForWinning = false;
        Instance = this;
        resetData();
        StartCoroutine(check());
        //InvokeRepeating("Check1", 0f, 1.5f);
    }
    IEnumerator check()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            check1();
        }
    }
    public void Init(int level)
    {
        isScaling = true;
        canInteract = false;
        gameObject.SetActive(true);
        Vector3 targetSclae = transform.localScale;
        transform.localScale = Vector3.one;
        transform.DOScale(GameManagerNew.Instance.TargetScale + new Vector3(0.1f, 0.1f, 0), 0.4f).OnComplete(() =>
        {
            transform.DOScale(GameManagerNew.Instance.TargetScale - new Vector3(0.1f, 0.1f, 0), 0.3f).OnComplete(() =>
            {
                transform.DOScale(GameManagerNew.Instance.TargetScale, 0.4f).OnComplete(() =>
                {
                    ChangeBarColor();
                    UIManagerNew.Instance.PausePanel.SetNumOfCollectItem();
                    if (LevelManagerNew.Instance.stage == 3)
                    {
                        if (PlayerPrefs.GetInt("GiveAwayUnscrew") != 0)
                        {
                            canInteract = true;
                        }
                    }
                    if (LevelManagerNew.Instance.stage == 4)
                    {
                        if (PlayerPrefs.GetInt("GiveAwayUndo") != 0)
                        {
                            canInteract = true;
                        }
                    }
                    else
                    {
                        canInteract = true;
                    }
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        isScaling = false;
                        EverythingStayStill(false);
                    });
                    if (!GamePlayPanelUIManager.Instance.gameObject.activeSelf)
                    {
                        GamePlayPanelUIManager.Instance.Appear();
                    }
                });
            });
        });
        GamePlayPanelUIManager.Instance.ShowNotice(false);
    }
    public void EverythingStayStill(bool status)
    {
        if ((status))
        {
            for (int i = 0; i < ironPlates.Length; i++)
            {
                if (ironPlates[i] != null)
                    ironPlates[i].rigidbody2D1.gravityScale = 0;
            }
        }
        else
        {
            for (int i = 0; i < ironPlates.Length; i++)
            {
                if (ironPlates[i] != null)
                {
                    ironPlates[i].rigidbody2D1.gravityScale = 1f;
                }
            }
        }
    }
    public void InitForStory(int level)
    {
        isScaling = true;
        canInteract = false;
        gameObject.SetActive(true);
        Vector3 targetSclae = transform.localScale;
        transform.localScale = Vector3.one;
        AudioManager.instance.PlaySFX("GamePlayLoading");
        transform.DOScale(GameManagerNew.Instance.TargetScale + new Vector3(0.1f, 0.1f, 0), 0.4f).OnComplete(() =>
        {
            transform.DOScale(GameManagerNew.Instance.TargetScale - new Vector3(0.1f, 0.1f, 0), 0.3f).OnComplete(() =>
            {
                transform.DOScale(GameManagerNew.Instance.TargetScale, 0.4f).OnComplete(() =>
                {

                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        isScaling = false;
                        EverythingStayStill(false);
                    });

                    canInteract = true;
                    TutorLevel1();

                });
            });
        });
        //GamePlayPanelUIManager.Instance.ShowNotice(false);
    }
    public void ScaleUpStage()
    {
        isScaling = true;
        canInteract = false;
        gameObject.SetActive(true);
        Vector3 targetSclae = transform.localScale;
        transform.localScale = Vector3.one;
        transform.DOScale(GameManagerNew.Instance.TargetScale + new Vector3(0.1f, 0.1f, 0), 0.4f).OnComplete(() =>
        {
            transform.DOScale(GameManagerNew.Instance.TargetScale - new Vector3(0.1f, 0.1f, 0), 0.3f).OnComplete(() =>
            {
                transform.DOScale(GameManagerNew.Instance.TargetScale, 0.4f).OnComplete(() =>
                {

                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        isScaling = false;
                        EverythingStayStill(false);
                    });
                    if (LevelManagerNew.Instance.stage == 3)
                    {
                        if (PlayerPrefs.GetInt("GiveAwayUnscrew") != 0)
                        {
                            canInteract = true;
                        }
                    }
                    if (LevelManagerNew.Instance.stage == 4)
                    {
                        if (PlayerPrefs.GetInt("GiveAwayUndo") != 0)
                        {
                            canInteract = true;
                        }
                    }
                });
            });
        });
    }
    public Vector3 setTargetScale(GameObject gameObject)
    {
        targetScale = gameObject.transform.localScale;
        return gameObject.transform.localScale;
    }
    public void Close(bool isDes)
    {
        EverythingStayStill(true);
        canInteract = false;
        isScaling = true;
        transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            if (isDes)
            {
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        });

    }

    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!GameManagerNew.Instance.isStory && !GameManagerNew.Instance.isMinigame)
                {
                    isMoving = true;
                    SetDefaultBoosterAim();
                }
                //booster anim
                if (!isTutor)
                {

                    if (isDeteleting)
                    {
                        GamePlayPanelUIManager.Instance.ButtonOff();
                        selectDeteleNail();

                    }
                    else
                    {

                        Click();

                    }
                }
            }
        }
        CheckHoleAvailable();
        Hack1();
        if (isWining && checkForWinning == false)
        {
            checkForWinning = true;
            if (UIManagerNew.Instance.GamePlayPanel.gameObject.activeSelf)
            {
                if (EventController.instance != null && EventController.instance.weeklyEvent != null && EventController.instance.weeklyEvent.eventStaus == WeeklyEventController.EventStaus.running)
                {
                    DOVirtual.DelayedCall(1.1f, () =>
                    {
                        UIManagerNew.Instance.GamePlayPanel.Close();
                    });
                }
                else
                {
                    UIManagerNew.Instance.GamePlayPanel.Close();
                }

            }
        }
        //Hack();
    }

    public void Click()
    {

        Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(posMouse, 0.5f, Vector3.forward, Mathf.Infinity);

        if (!nailDetectors.IsNullOrEmpty())
        {
            nailDetectors.Clear();
        }

        if (!selectedIrons.IsNullOrEmpty())
        {
            selectedIrons.Clear();
        }

        for (int i = 0; i < cubeHit.Length; i++)
        {
            if (cubeHit[i].transform.gameObject.tag == "Iron")
            {
                selectedIrons.Add(cubeHit[i].transform.GetComponent<IronPlate>());
            }
        }
        for (int i = 0; i < cubeHit.Length; i++)
        {
            if (cubeHit[i].transform.gameObject.tag == "Hole")
            {
                if (curNail == null)
                {
                    curHole = cubeHit[i].collider.gameObject.GetComponent<Hole>();
                    setHoleInIron(curHole.transform.position);
                    if (curHole.CheckNail() && curNail != curHole.getNail())
                    {
                        curNail = curHole.getNail();
                        Debug.Log("Lấy đinh");
                        curNail.check();
                        curNail.PickUp(curHole.getNail());
                        if (isLvTutor)
                        {
                            pointerTutor.SetPos(1);
                        }
                        goto lb100;
                    }
                    if (i == cubeHit.Length - 1)
                    {
                        goto lb100;
                    }
                }
                else
                {
                    curHole = cubeHit[i].collider.gameObject.GetComponent<Hole>();
                    setHoleInIron(curHole.transform.position);
                    if (curHole.CheckNail() && curNail != curHole.getNail())
                    {
                        curNail = curHole.getNail();
                        Debug.Log("Lấy đinh");
                        curNail.check();
                        curNail.PickUp(curHole.getNail());
                        if (isLvTutor)
                        {
                            pointerTutor.SetPos(1);
                        }
                    }
                    goto lb100;
                }
            }
        }
        AudioManager.instance.PlaySFX("Click");
        GameManagerNew.Instance.CreateFxClickFail(posMouse);
        curHole = null;

    lb100:

        if (curHole != null)
        {
            //neu tu bam vao cai dinh hien co
            if (curHole == preHole)
            {
                preHole = null;
                curHole = null;
                if (curNail != null)
                {
                    curNail.Unselect(curNail);
                    AudioManager.instance.PlaySFX("PushNail");
                    curNail = null;
                }


            }
            else
            {
                if (preHole == null)
                {
                    preHole = curHole;
                }
                if (curHole.CheckNail())
                {
                    //if (curNail != null)
                    //{
                    //	curNail.Unselect();
                    //}
                    if (preHole.getNail() != null)
                    {
                        preHole.getNail().Unselect(preHole.getNail());
                    }
                    preHole = curHole;
                    curNail = curHole.getNail();
                    curNail.check();
                    curNail.PickUp(curHole.getNail());
                    var clickeffect = Instantiate(ParticlesManager.instance.pickUpStartParticle, curHole.transform.position, Quaternion.identity);
                    Destroy(clickeffect, 0.4f);
                    Debug.Log("chạy qua chọn đinh mới bth");
                }
                else
                {
                    if (curNail != null && curHole.isOsccupied == false && CheckHoleIsAvailable())
                    {
                        hasUndo = false;
                        SaveGameObject();
                        curNail.SetNewPos(curHole.transform.position);
                        if (!nailDetectors.IsNullOrEmpty())
                        {
                            foreach (var nail in nailDetectors)
                            {
                                nail.hingeJoint2D.connectedBody = curNail.rigidbody2D;
                                nail.Nail = curNail;
                            }
                        }
                        if (!selectedIrons.IsNullOrEmpty())
                        {
                            foreach (var iron in selectedIrons)
                            {
                                iron.ResetHingeJoint();
                            }
                        }
                        curNail.RemoveHinge();
                        curHole.setNail(curNail);
                        preHole.setNail(null);
                        curNail = null;
                        curHole = null;
                        nailDetectors.Clear();
                        selectedIrons.Clear();
                        hasDelete = false;
                        if (isLvTutor)
                        {
                            Debug.Log("chạy vào tutor");
                            isLvTutor = false;
                            if (pointerTutor != null)
                            {
                                pointerTutor.gameObject.SetActive(false);
                            }
                            pointerTutor.DisablePointer();
                        }
                        Debug.Log("chạy qua click mới bth");
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX("Click");
                        GameManagerNew.Instance.CreateFxClickFail(posMouse);
                    }
                }
            }
        }
        else
        {
            //do nothing;
        }
    }
    public void setHoleInIron(Vector3 pos)
    {
        RaycastHit2D[] nailDetectorHits = Physics2D.CircleCastAll(pos, 0.1f, Vector3.forward, Mathf.Infinity);
        for (int i = 0; i < nailDetectorHits.Length; i++)
        {
            if (nailDetectorHits[i].transform.gameObject.tag == "HoleInIron")
            {
                nailDetectors.Add(nailDetectorHits[i].transform.GetComponent<NailDetector>());
            }
        }
    }
    public bool CheckHoleIsAvailable()
    {
        bool allin = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(curHole.transform.position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.transform.tag == "Iron")
            {
                if (collider.GetComponent<IronPlate>().hingeJoint2Ds.Length > 0)
                {
                    if (!collider.GetComponent<IronPlate>().checkHitPoint(curHole.transform.position))
                    {
                        allin = false;
                        return false;
                    }
                    else
                    {
                        allin = true;
                    }
                }
            }
        }
        return allin;
    }
    public void CheckDoneLevel()
    {
        if (numOfIronPlates <= 0)
        {
            if (GameManagerNew.Instance.isStory && !GameManagerNew.Instance.isMinigame)
            {
                //code phần complete khi hoàn thành màn story
                int videoIndex = PlayerPrefs.GetInt("videoIndex");
                PlayerPrefs.SetInt("videoIndex", VideoController.instance.videoIndex + 1);
                this.Close(true);
                UIManagerNew.Instance.StoryItem.DisplayItem(videoIndex + 1, () =>
                {
                    UIManagerNew.Instance.StoryItem.Disable();
                });

            }
            else
            {
                if (numOfIronPlates <= 0)
                {
                    isWining = true; 
                    UIManagerNew.Instance.GamePlayPanel.DeactiveCVGroup();

                    if (isDeteleting)
                    {
                        isDeteleting = false;
                        TurnRed(false);
                    }
                    UIManagerNew.Instance.GamePlayPanel.DeactiveBoosterEffect();
                    if (!UIManagerNew.Instance.PausePanel.gameObject.activeSelf && !UIManagerNew.Instance.UndoPanel.gameObject.activeSelf && !UIManagerNew.Instance.DeteleNailPanel.gameObject.activeSelf && !UIManagerNew.Instance.ExtralHolePanel.gameObject.activeSelf)
                    {
                        if (LevelManagerNew.Instance.stage == 0 || LevelManagerNew.Instance.stage == 1)
                        {
                            AdsControl.Instance.ActiveBlockFaAds(false);
                            Debug.Log("sau khi show ads");
                            DOVirtual.DelayedCall(0.3f, () =>
                            {
                                AudioManager.instance.PlaySFX("CompletePanel");
                                if (LevelManagerNew.Instance.stage == 0)
                                {
                                    DOVirtual.DelayedCall(0.3f, () =>
                                    {
                                        UIManagerNew.Instance.CompleteUI.Appear();
                                        canInteract = false;
                                    });
                                }
                                else
                                {

                                    UIManagerNew.Instance.CompleteUI.Appear();
                                    canInteract = false;
                                }
                            });
                        }
                        else
                        {
                            if (EventController.instance != null && EventController.instance.weeklyEvent != null && EventController.instance.weeklyEvent.eventStaus == WeeklyEventController.EventStaus.running)
                            {
                                DOVirtual.DelayedCall(1.1f, () =>
                                {
                                    Debug.Log("trước khi show ads");
                                    AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_win, () =>
                                    {
                                        AdsControl.Instance.ActiveBlockFaAds(false);
                                        Debug.Log("sau khi show ads");
                                        DOVirtual.DelayedCall(0.3f, () =>
                                        {
                                            AudioManager.instance.PlaySFX("CompletePanel");
                                            if (LevelManagerNew.Instance.stage == 0)
                                            {
                                                DOVirtual.DelayedCall(0.3f, () =>
                                                {
                                                    UIManagerNew.Instance.CompleteUI.Appear();
                                                    canInteract = false;
                                                });
                                            }
                                            else
                                            {

                                                UIManagerNew.Instance.CompleteUI.Appear();

                                                canInteract = false;
                                            }
                                        });
                                    }, null);
                                });
                            }
                            else
                            {
                                Debug.Log("trước khi show ads");
                                AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_win, () =>
                                {
                                    AdsControl.Instance.ActiveBlockFaAds(false);
                                    Debug.Log("sau khi show ads");
                                    DOVirtual.DelayedCall(0.3f, () =>
                                    {
                                        AudioManager.instance.PlaySFX("CompletePanel");
                                        if (LevelManagerNew.Instance.stage == 0)
                                        {
                                            DOVirtual.DelayedCall(0.3f, () =>
                                            {
                                                UIManagerNew.Instance.CompleteUI.Appear();
                                                canInteract = false;
                                            });
                                        }
                                        else
                                        {

                                            UIManagerNew.Instance.CompleteUI.Appear();

                                            canInteract = false;
                                        }
                                    });
                                }, null);
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            isWining = false;
        }
    }
    public void AfterPanel()
    {
        if (isWining)
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                CheckDoneLevel();
            });

        }
    }
    public void selectDeteleNail()
    {
        Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(posMouse, 0.2f, Vector3.forward);
        if (cubeHit.Length > 0)
        {
            foreach (RaycastHit2D ray in cubeHit)
            {
                if (ray.collider.tag == "Hole")
                {
                    //chọn hố
                    if (ray.collider.GetComponent<Hole>().CheckNail() == true)
                    {

                        holeToDetele = ray.collider.GetComponent<Hole>();
                        ClearData();
                        SaveGameObject();
                        nailToDetele = holeToDetele.getNail();
                        nailToDetele.check();
                        SaveGameObject();
                        holeToDetele.setNail(null);
                        nailToDetele.RemoveHinge();
                        nailToDetele.gameObject.SetActive(false);
                        setDeteleting(false);
                        hasDelete = true;
                        UIManagerNew.Instance.GamePlayPanel.ShowUnscrewEffect(holeToDetele.transform, null);
                        if (LevelManagerNew.Instance.stage == 3)
                        {
                            if (UIManagerNew.Instance.DeteleNailPanel.hasUseTutor == true)
                            {
                                UIManagerNew.Instance.DeteleNailPanel.hasUseTutor = false;
                                FirebaseAnalyticsControl.Instance.LogEventTutorialStatus(LevelManagerNew.Instance.stage, TutorialStatus.tut_unscrew_done);
                            }
                        }
                    }
                    //var Destroyeffect1 = Instantiate(destroyNailEffect, nailToDetele.transform.position, quaternion.identity);
                    //Destroy(Destroyeffect1, 0.5f);
                }
            }
        }
    }

    private void TurnRed(bool status)
    {
        for (int i = 0; i < nailControls.Length; i++)
        {
            nailControls[i].redNailSprite.enabled = status;
        }
    }
    public void setDeteleting(bool status)
    {
        if (status == true)
        {
            GamePlayPanelUIManager.Instance.ButtonOff();
        }
        else
        {
            GamePlayPanelUIManager.Instance.ButtonOn();
        }
        isDeteleting = status;
        TurnRed(status);
    }
    public void SaveGameObject()
    {

        //code cu
        {
            //code đoạn này ngu vl
            ClearData();
            Debug.Log("chạy qua ClearData");
            SavePreData();
            Debug.Log("chạy qua SavePreData");


        }
        hasSave = true;
        if (LevelManagerNew.Instance.stage >= 3 && !GameManagerNew.Instance.isStory && !GameManagerNew.Instance.isMinigame)
        {
            if (!hasUndo)
            {
                Debug.Log("chạy vào gameplaypanel");
                GamePlayPanelUIManager.Instance.UndoButton.interactable = true;
            }
        }
        Debug.Log("chạy qua save object");
        //TutorUndo();
    }

    private void SavePreData()
    {
        nailToUndo = curNail;
        holeBeforeUndo = curHole;
        holeToUndo = preHole;
        foreach (var iron in ironPlates)
        {
            if (iron.gameObject.activeSelf)
                ironObjectsBeforemove.Add(iron);
        }
        for (int i = 0; i < ironObjectsBeforemove.Count; i++)
        {
            ironObjectsTransformBeforemove.Add(ironObjectsBeforemove[i].transform.localPosition);
            ironObjectsRotationBeforemove.Add(ironObjectsBeforemove[i].transform.rotation);

        }
        if (nailObjectsBeforemove.IsNullOrEmpty())
        {
            for (int i = 0; i < nailControls.Length; i++)
            {
                nailObjectsBeforemove.Add(nailControls[i]);

            }
            for (int i = 0; i < nailControls.Length; i++)
            {
                nailObjectsTransformBeforemove.Add(nailObjectsBeforemove[i].transform.position);
            }
        }
        else
        {
            for (int i = 0; i < nailObjectsBeforemove.Count; i++)
            {
                nailObjectsBeforemove.Add(nailObjectsBeforemove[i]);

            }
            for (int i = 0; i < nailObjectsBeforemove.Count; i++)
            {
                nailObjectsTransformBeforemove.Add(nailObjectsBeforemove[i].transform.position);
            }
        }
        foreach (var iron in ironObjectsBeforemove)
        {
            foreach (var hinge in iron.hingeJoint2Ds)
            {
                if (hinge.connectedBody != null)
                {
                    HingeJointBeforeRemove.Add(hinge);
                    nailsJointBeforemove.Add(hinge.connectedBody.gameObject.GetComponent<NailControl>());
                }
            }
        }
    }

    public void Undo()
    {
        if (hasSave)
        {
            //code cu
            {
                for (int i = 0; i < ironObjectsBeforemove.Count; i++)
                {
                    ironObjectsBeforemove[i].transform.SetLocalPositionAndRotation(ironObjectsTransformBeforemove[i], ironObjectsRotationBeforemove[i]);
                    ironObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = true;
                    ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().isKinematic = true;
                    ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().freezeRotation = true;
                    ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    ironObjectsBeforemove[i].gameObject.SetActive(true);
                }
                if (hasDelete == false)
                {
                    foreach (var iron in ironObjectsBeforemove)
                    {
                        foreach (var hinge in iron.hingeJoint2Ds)
                        {
                            if (hinge.connectedBody != null)
                            {
                                hinge.connectedBody = null;
                                hinge.connectedAnchor = Vector2.zero;
                                hinge.enabled = false;
                            }
                        }
                    }
                    nailToUndo.gameObject.transform.position = holeToUndo.transform.position;
                    holeToUndo.setNail(nailToUndo);
                    holeBeforeUndo.setNail(null);
                }
                else
                {
                    RestoreDeteleData();
                }
                for (int i = 0; i < HingeJointBeforeRemove.Count; i++)
                {
                    HingeJointBeforeRemove[i].connectedBody = nailsJointBeforemove[i].rigidbody2D;
                    HingeJointBeforeRemove[i].connectedAnchor = Vector2.zero;
                    HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = true;
                    HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = false;
                }
                hasUndo = true;
                hasDelete = false;
                Continute();
                resetData();
            }
        }
    }

    private void RestoreDeteleData()
    {
        foreach (var iron in ironObjectsBeforemove)
        {
            foreach (var hinge in iron.hingeJoint2Ds)
            {
                if (hinge.connectedBody != null)
                {
                    hinge.connectedBody = null;
                    hinge.connectedAnchor = Vector2.zero;
                    hinge.enabled = false;
                }
            }
        }
        nailToDetele.gameObject.SetActive(true);
        holeToDetele.setNail(nailToDetele);
        nailToDetele.collider2D.isTrigger = true;
    }

    public void Continute()
    {
        for (int i = 0; i < ironObjectsBeforemove.Count; i++)
        {
            ironObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = false;
            ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().freezeRotation = false;
            ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().isKinematic = false;
            ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        for (int i = 0; i < nailObjectsBeforemove.Count; i++)
        {
            nailObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = false;
        }
        for (int i = 0; i < HingeJointBeforeRemove.Count; i++)
        {
            HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = true;
            HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = false;
            HingeJointBeforeRemove[i].enabled = true;
        }
        if (nailToDetele != null)
        {
            nailToDetele.collider2D.isTrigger = false;
        }
        numOfIronPlates = ironObjectsBeforemove.Count;
        ClearData();

    }
    public void ClearData()
    {
        try
        {
            nailObjectsBeforemove.Clear();
            nailObjectsTransformBeforemove.Clear();
            ironObjectsRotationBeforemove.Clear();
            ironObjectsBeforemove.Clear();
            ironObjectsTransformBeforemove.Clear();
            nailObjectsBeforeBoom.Clear();

            //mới add
            HingeJointBeforeRemove.Clear();
            nailsJointBeforemove.Clear();
            Debug.Log("chạy qua Clear");

            hasSave = false;
            if (!GameManagerNew.Instance.isStory && !GameManagerNew.Instance.isMinigame)
            {

                Debug.Log("chạy vào GamePlayPanelUIManager");
                GamePlayPanelUIManager.Instance.UndoButton.interactable = false;
            }
            Debug.Log("chạy hết Clear data");
        }
        catch (Exception e) { }
    }
    public void ChangeLayer()
    {
        int layer = LayerMask.NameToLayer(layerName); // Chuyển đổi tên layer thành ID layer
        if (layer != -1) // Kiểm tra xem layer có tồn tại không
        {
            if (holeToUnlock.gameObject != null)
            {
                holeToUnlock.gameObject.layer = layer; // Đặt layer cho GameObject

            }
        }
        else
        {
            Debug.LogError("Layer " + layerName + " does not exist!"); // In ra thông báo lỗi nếu layer không tồn tại
        }
    }
    public void resetData()
    {
        curNail = null;
        curHole = null;
        preHole = null;
        numOfHoleNotAvailable.Clear();
    }
    public void ResetBooster()
    {
        numOfEventItem = 0;
        UIManagerNew.Instance.DeteleNailPanel.numOfUsed = 1;
        UIManagerNew.Instance.UndoPanel.numOfUsed = 1;
        //UIManagerNew.Instance.RePlayPanel.numOfUsed = 1;
        UIManagerNew.Instance.LosePanel.hasUse = false;
        TutorUnscrew();
        DeactiveDeleting();


    }
    private void TutorUnscrew()
    {
        if (LevelManagerNew.Instance.stage == 3 && !GameManagerNew.Instance.isStory && GamePlayPanelUIManager.Instance.gameObject.activeSelf && PlayerPrefs.GetInt("GiveAwayUnscrew") == 0)
        {
            //tuto unscrew
            isTutor = true;
            Invoke("showUnscrewTuTor", 0.5f);
        }
        else if (LevelManagerNew.Instance.stage == 3 && !GameManagerNew.Instance.isStory && GamePlayPanelUIManager.Instance.gameObject.activeSelf && PlayerPrefs.GetInt("GiveAwayUnscrew") == 1)
        {
            UIManagerNew.Instance.GamePlayPanel.boosterBar.freeUnscrewImg.gameObject.SetActive(true);
        }
        if (LevelManagerNew.Instance.stage == 4 && !GameManagerNew.Instance.isStory && GamePlayPanelUIManager.Instance.gameObject.activeSelf && PlayerPrefs.GetInt("GiveAwayUndo") == 0)
        {
            UIManagerNew.Instance.GamePlayPanel.boosterBar.undoAddImg.gameObject.SetActive(false);
            UIManagerNew.Instance.GamePlayPanel.boosterBar.blockUndoImage.gameObject.SetActive(true);
            Invoke("ShowUndoTutor", 0.5f);
        }
        if (LevelManagerNew.Instance.stage == 4 && !GameManagerNew.Instance.isStory && GamePlayPanelUIManager.Instance.gameObject.activeSelf && PlayerPrefs.GetInt("GiveAwayUndo") == 1)
        {
            UIManagerNew.Instance.GamePlayPanel.boosterBar.blockUndoImage.gameObject.SetActive(false);
            if (SaveSystem.instance.undoPoint > 0)
            {
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoAddImg.gameObject.SetActive(false);
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoNumImg.gameObject.SetActive(true);
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoNumText.text = SaveSystem.instance.undoPoint.ToString();
            }
            else
            {
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoAddImg.gameObject.SetActive(true);
                UIManagerNew.Instance.GamePlayPanel.boosterBar.undoNumImg.gameObject.SetActive(false);
            }
        }
    }
    public void TutorLevel1()
    {
        if (GameManagerNew.Instance.isStory && pointerTutor != null)
        {
            isLvTutor = true;
            if (pointerTutor.gameObject.activeSelf == false)
            {
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    pointerTutor.gameObject.SetActive(true);
                });
            }

        }
    }
    public void Hack()
    {
            foreach (var nail in nailControls)
            {
                Destroy(nail.gameObject);
            }
    }

    public void Hack1()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (var nail in nailControls)
            {
                Destroy(nail.gameObject);
            }
        }
    }
    public void check1()
    {
        if (!GameManagerNew.Instance.isStory && !GameManagerNew.Instance.isMinigame)
        {
            if (holes.Length != 0 && numOfHoleNotAvailable.Count == holes.Length)
            {

                if (checked1 == false && !isScaling)
                {
                    checked1 = true;
                    Invoke("ShowNotice", 1f);
                }

            }
            else
            {
                GamePlayPanelUIManager.Instance.ShowNotice(false);
                checked1 = false;
            }
        }
    }

    private void CheckHoleAvailable()
    {
        foreach (var hole in holes)
        {
            if (hole.isOsccupied == true)
            {
                if (!numOfHoleNotAvailable.Contains(hole))
                {
                    numOfHoleNotAvailable.Add(hole);
                }
            }
            else
            {
                if (numOfHoleNotAvailable.Contains(hole))
                {
                    numOfHoleNotAvailable.Remove(hole);
                }
            }
        }
    }

    private void ShowNotice()
    {
        if (numOfHoleNotAvailable.Count == holes.Length)
        {
            GamePlayPanelUIManager.Instance.ShowNotice(true);

        }
    }
    public void DeactiveDeleting()
    {
        if (isDeteleting)
        {
            isDeteleting = false;
        }

    }
    public void showUnscrewTuTor()
    {
        UIManagerNew.Instance.NewBooster.SetValue(1);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
            {
                canInteract = false;
            }
            UIManagerNew.Instance.GamePlayPanel.boosterBar.freeUnscrewImg.gameObject.SetActive(false);
            UIManagerNew.Instance.NewBooster.Appear();
        });
    }

    public void ShowUndoTutor()
    {
        UIManagerNew.Instance.NewBooster.SetValue(0);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            if (Stage.Instance != null && Stage.Instance.gameObject.activeSelf)
            {
                canInteract = false;
            }
            UIManagerNew.Instance.GamePlayPanel.boosterBar.undoAddImg.gameObject.SetActive(false);
            UIManagerNew.Instance.GamePlayPanel.boosterBar.blockUndoImage.gameObject.SetActive(true);
            UIManagerNew.Instance.NewBooster.Appear();
        });
    }

    public void showLevel1Tutor()
    {
        pointerTutor.gameObject.SetActive(true);
    }
    public void DeactiveTutor()
    {
        isTutor = false;
    }
    IEnumerator CheckForClickContinuously()
    {
        while (true)
        {
            if (!GameManagerNew.Instance.isStory && !GameManagerNew.Instance.isMinigame)
            {
                isMoving = false;
                float timer = 5f;
                while (timer > 0)
                {
                    timer -= Time.deltaTime; // Giảm thời gian theo thời gian thực
                    yield return null; // Đợi khung hình tiếp theo
                }
                if (isMoving)
                {
                    // Thực hiện hành động khi người chơi bấm vào màn hình
                    SetDefaultBoosterAim();
                    yield return null; // Đợi khung hình tiếp theo
                }
                else
                {
                    LauchBoosterAim();
                }
                // Đợi 5 giây trước khi kiểm tra lại
                yield return new WaitForSeconds(5f);
            }
        }
    }

    private void SetDefaultBoosterAim()
    {
        //holeToUnlock.myAnimator.enabled = false;
        if (holeToUnlock.addImage.gameObject.activeSelf)
        {
            //this.holeToUnlock.addImage.transform.localScale = holeToUnlock.myScale;
        }
        GamePlayPanelUIManager.Instance.activeAnimation(GamePlayPanelUIManager.Instance.DeteleButtonAim, false);
        GamePlayPanelUIManager.Instance.DeteleButton.transform.DOScale(1.05f, 0.05f);
        GamePlayPanelUIManager.Instance.activeAnimation(GamePlayPanelUIManager.Instance.UndoButtonAim, false);
        GamePlayPanelUIManager.Instance.UndoButton.transform.DOScale(1.05f, 0.05f);
        if (boosterTween != null)
            DOTween.Kill(boosterTween);
        if (boosterTween1 != null)
            DOTween.Kill(boosterTween1);
    }

    private void LauchBoosterAim()
    {
        // Thực hiện hành động khi người chơi không bấm vào màn hình
        GamePlayPanelUIManager.Instance.activeAnimation(GamePlayPanelUIManager.Instance.DeteleButtonAim, true);
        boosterTween = DOVirtual.DelayedCall(3f, () =>
        {
            GamePlayPanelUIManager.Instance.activeAnimation(GamePlayPanelUIManager.Instance.DeteleButtonAim, false);
            GamePlayPanelUIManager.Instance.DeteleButton.transform.localScale = new Vector3(1.05f, 1.05f, 1);
            GamePlayPanelUIManager.Instance.activeAnimation(GamePlayPanelUIManager.Instance.UndoButtonAim, true);
            boosterTween1 = DOVirtual.DelayedCall(3f, () =>
            {
                GamePlayPanelUIManager.Instance.activeAnimation(GamePlayPanelUIManager.Instance.UndoButtonAim, false);
                GamePlayPanelUIManager.Instance.UndoButton.transform.localScale = new Vector3(1.05f, 1.05f, 1);
                //this.holeToUnlock.myAnimator.enabled = false;
            });
        });
    }

    public void ChangeBarColor()
    {
        if (EventController.instance.weeklyEvent != null && EventController.instance.weeklyEvent.eventStaus == WeeklyEventController.EventStaus.running)
        {
            if (EventController.instance.weeklyEvent.colorIndex == 0)
            {
                ChangeEachBarColor("IronLayer1", "IronLayer9");
            }
            if (EventController.instance.weeklyEvent.colorIndex == 1)
            {
                ChangeEachBarColor("IronLayer2", null);
            }
            if (EventController.instance.weeklyEvent.colorIndex == 2)
            {
                ChangeEachBarColor("IronLayer3", "layer10", "IronLayer8");
            }
            if (EventController.instance.weeklyEvent.colorIndex == 3)
            {
                ChangeEachBarColor("IronLayer4", null);
            }
            if (EventController.instance.weeklyEvent.colorIndex == 4)
            {
                ChangeEachBarColor("IronLayer5", "layer11");
            }
            if (EventController.instance.weeklyEvent.colorIndex == 5)
            {
                ChangeEachBarColor("IronLayer6", "layer12");
            }
            if (EventController.instance.weeklyEvent.colorIndex == 6)
            {
                ChangeEachBarColor("IronLayer7", null);
            }
        }

    }
    public void ChangeEachBarColor(string layer, string layer2, string layer3 = null)
    {
        for (int i = 0; i < ironPlates.Length; i++)
        {
            if (ironPlates[i].layer.CompareTo(layer) == 0 || ironPlates[i].layer.CompareTo(layer2) == 0 || ironPlates[i].layer.CompareTo(layer3) == 0)
            {
                if (ironPlates[i].shiningParticle == null)
                {
                    ironPlates[i].isEventItem = true;
                    ParticleSystem x = UIManagerNew.Instance.GamePlayPanel.eventItemShinning;

                    // Thiết lập ShapeModule từ ParticleSystem
                    ParticleSystem.ShapeModule shapeModule = x.shape;

                    // Thay đổi các thuộc tính của ShapeModule nếu cần
                    shapeModule.spriteRenderer = ironPlates[i].GetComponent<SpriteRenderer>();
                    shapeModule.scale = new Vector3(ironPlates[i].transform.localScale.x * 1.8f, ironPlates[i].transform.localScale.y, ironPlates[i].transform.localScale.z);

                    var y = Instantiate(x, ironPlates[i].transform);
                    ironPlates[i].shiningParticle = y;
                }
            }
        }
    }
}
