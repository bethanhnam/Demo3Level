using DG.Tweening;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameStage : MonoBehaviour
{
    public static MiniGameStage Instance;
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

    public Vector3 targetScale;

    public GameObject pointer;
    public TutorPointer pointerTutor;

    public bool canInteract = true;

    public bool isWining = false;
    public bool isLosing = false;
    public bool isScaling = false;

    private void Start()
    {
        Instance = this;
        ClearData();
        numOfIronPlates = ironPlates.Length;
    }
    private void OnEnable()
    {
        Instance = this;
        resetData();
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

                    canInteract = true;

                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        isScaling = false;
                    });
                    EverythingStayStill(false);
                });
            });
        });
        GamePlayPanelUIManager.Instance.ShowNotice(false);
    }

    public void InitForMinigame(int level)
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

                    canInteract = true;

                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        isScaling = false;
                    });
                    EverythingStayStill(false);
                });
            });
        });
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
                    ironPlates[i].rigidbody2D1.gravityScale = 1;

            }
        }
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
                    });
                    EverythingStayStill(false);

                    canInteract = true;
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
        transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
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

    //public void ChangeSize(Sprite sprite)
    //{
    //	Vector2 sizeSprite = sprite.rect.size;

    //	float sWidth = sizeSprite.x / 500f;
    //	float sHeight = sizeSprite.y / 500f;

    //	sprRenderItem.transform.localScale = Vector2.one * (1 / Mathf.Max(sWidth, sHeight));
    //	sprRenderItem.sprite = sprite;
    //}

    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
              
                        Click();
            }
        }
        CheckHoleAvailable();
        //Hack();
        if (isWining)
        {

            if (UIManagerNew.Instance.GamePlayPanel.gameObject.activeSelf)
            {
                UIManagerNew.Instance.GamePlayPanel.Close();
            }
        }
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
                curHole = cubeHit[i].collider.gameObject.GetComponent<Hole>();
                setHoleInIron(curHole.transform.position);
                if (curHole.CheckNail() && curNail != curHole.getNail())
                {
                    curNail = curHole.getNail();
                    Debug.Log("Lấy đinh");
                    curNail.check();
                    curNail.PickUp(curHole.transform.position);
                }

                goto lb100;
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
                    curNail.Unselect();
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
                        preHole.getNail().Unselect();
                    }
                    preHole = curHole;
                    curNail = curHole.getNail();
                    curNail.check();
                    curNail.PickUp(curHole.transform.position);
                    var clickeffect = Instantiate(ParticlesManager.instance.pickUpStartParticle, curHole.transform.position, Quaternion.identity);
                    Destroy(clickeffect, 0.4f);
                    Debug.Log("chạy qua chọn đinh mới bth");
                }
                else
                {
                    if (curNail != null && curHole.isOsccupied == false && CheckHoleIsAvailable())
                    {
                        hasUndo = false;
                        Debug.Log("Đẩy được đinh vào");
                        // continue code
                        SaveGameObject();
                        Debug.Log("chạy qua save object bth");
                        //curNail.SetTrigger();
                        curNail.SetNewPos(curHole.transform.position);
                        Debug.Log("chạy qua set new pos bth");
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
            if (GameManagerNew.Instance.isStory)
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
                    Debug.Log("numOfIronPlates" + numOfIronPlates);
                    Debug.Log("chay vào đây khi hết thanh gỗ");
                    if (!UIManagerNew.Instance.PausePanel.gameObject.activeSelf && !UIManagerNew.Instance.UndoPanel.gameObject.activeSelf && !UIManagerNew.Instance.DeteleNailPanel.gameObject.activeSelf && !UIManagerNew.Instance.ExtralHolePanel.gameObject.activeSelf)
                    {
                        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_win, () =>
                        {
                            AdsControl.Instance.ActiveBlockFaAds(false);
                            Debug.Log("sau khi show ads");
                            //UIManagerNew.Instance.GamePlayPanel.Close();
                            DOVirtual.DelayedCall(0.3f, () =>
                            {
                                AudioManager.instance.PlaySFX("CompletePanel");
                                if (LevelManagerNew.Instance.stage == 0) {
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
                    else
                    {
                        return;
                    }
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
            if (!GameManagerNew.Instance.isStory)
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
    public void Hack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (var nail in nailControls)
            {
                Destroy(nail.gameObject);
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
}
