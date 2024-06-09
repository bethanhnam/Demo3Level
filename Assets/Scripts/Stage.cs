using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public static Stage Instance;
    public GameObject holeToUnlock;
    public SpriteRenderer sprRenderItem;
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
    private NailControl curNail;
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
    private void Start()
    {
        Instance = this;
        ClearData();
        numOfIronPlates = ironPlates.Length;
    }
    private void OnEnable()
    {
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
        Instance = this;
        resetData();
        //if(GamePlayPanelUIManager.Instance.gameObject.activeSelf == false)
        //{
        //	
        //}
    }
    public void Init(int level)
    {
        gameObject.SetActive(true);
        Vector3 targetSclae = transform.localScale;
        transform.localScale = Vector3.one;
        transform.DOScale(GameManagerNew.Instance.TargetScale + new Vector3(0.1f,0.1f,0), 0.5f).OnComplete(() =>
        {
            transform.DOScale(GameManagerNew.Instance.TargetScale - new Vector3(0.1f, 0.1f, 0), 0.4f).OnComplete(() => {
                transform.DOScale(GameManagerNew.Instance.TargetScale, 0.5f);
            });
        });
        GamePlayPanelUIManager.Instance.ShowNotice(false);
    }
    public Vector3 setTargetScale(GameObject gameObject)
    {
        targetScale = gameObject.transform.localScale;
        return gameObject.transform.localScale;
    }
    public void Close(bool isDes)
    {
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

        if (Input.GetMouseButtonDown(0))
        {
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
        Hack();
        check1();
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
                    curNail.check();
                    curNail.PickUp(curHole.transform.position);
                    if (isLvTutor)
                    {
                        pointerTutor.SetPos(1);
                    }
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
                }
                else
                {
                    if (curNail != null && curHole.isOsccupied == false && CheckHoleIsAvailable())
                    {
                        // continue code
                        SaveGameObject();
                        //curNail.SetTrigger();
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
                            isLvTutor = false;
                            pointerTutor.DisablePointer();
                        }
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
        //selectedHole = cubeHit[hole].transform.gameObject;
        ////try
        ////{
        ////if (Notice.Instance.gameObject.activeSelf)
        ////{
        ////	StartCoroutine(DisappearNotice());
        ////}
        ////}
        ////catch
        ////{

        ////}
        //preHingeJoint2D.Clear();
        //AudioManager.instance.PlaySFX("PickUpScrew");
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
            AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_win, () =>
            {
                UIManagerNew.Instance.GamePlayPanel.Close();
                LevelManagerNew.Instance.NextStage();
                DOVirtual.DelayedCall(0.3f, () => { 
                    UIManagerNew.Instance.CompleteUI.Appear(sprRenderItem.sprite);
                });
            }, null);
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
                        GamePlayPanelUIManager.Instance.showPointer(false);
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
            SavePreData();


        }
        hasSave = true;
        if (LevelManagerNew.Instance.stage >=3)
        {
            GamePlayPanelUIManager.Instance.UndoButton.interactable = true;
        }
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

            hasSave = false;
            GamePlayPanelUIManager.Instance.UndoButton.interactable = false;
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
        Debug.Log(numOfHoleNotAvailable.Count);
        numOfHoleNotAvailable.Clear();
        Debug.Log(numOfHoleNotAvailable.Count);
    }
    public void ResetBooster()
    {
        UIManagerNew.Instance.DeteleNailPanel.numOfUsed = 1;
        UIManagerNew.Instance.UndoPanel.numOfUsed = 1;
        //UIManagerNew.Instance.RePlayPanel.numOfUsed = 1;
        UIManagerNew.Instance.LosePanel.hasUse = false;
        UIManagerNew.Instance.DeteleNailPanel.LockOrUnlock(true);
        UIManagerNew.Instance.UndoPanel.LockOrUnlock(true);
        TutorUnscrew();
        TutorLevel1();
        DeactiveDeleting();
    }

    //private void TutorUndo()
    //{
    //    if (LevelManagerNew.Instance.stage >= 1 )
    //    {
    //        UIManagerNew.Instance.UndoPanel.LockOrUnlock(false);
    //        GamePlayPanelUIManager.Instance.boosterBar.InteractableBT(GamePlayPanelUIManager.Instance.boosterBar.UndoBT);
    //    }
    //}

    private void TutorUnscrew()
    {
        if (LevelManagerNew.Instance.stage == 3)
        {
            //tuto undo 
            isTutor = true;
            GamePlayPanelUIManager.Instance.ActiveBlackPic(true);
            UIManagerNew.Instance.DeteleNailPanel.LockOrUnlock(false);
            
            //anim unlock
            //var x = Instantiate(ParticlesManager.instance.StarParticleObject, GamePlayPanelUIManager.Instance.DeteleButton.transform.position, Quaternion.identity, this.transform);
            //ParticleSystem particle = x.transform.GetChild(0).GetComponent<ParticleSystem>();
            //var shape = particle.shape;
            //         shape.sprite = GamePlayPanelUIManager.Instance.DeteleButton.image.sprite;
            //         Destroy(x, 1f);
            Invoke("showUnscrewTuTor", 1.3f);

        }
        else if (LevelManagerNew.Instance.stage > 3)
        {
            UIManagerNew.Instance.DeteleNailPanel.LockOrUnlock(false);
            GamePlayPanelUIManager.Instance.boosterBar.InteractableBT(GamePlayPanelUIManager.Instance.boosterBar.deteleBT);
        }
        else if (LevelManagerNew.Instance.stage < 3)
        {
            UIManagerNew.Instance.DeteleNailPanel.LockOrUnlock(true);
            GamePlayPanelUIManager.Instance.boosterBar.UninteractableBT(GamePlayPanelUIManager.Instance.boosterBar.deteleBT);
        }
    }
    private void TutorLevel1()
    {
        if (LevelManagerNew.Instance.stage == 0 && pointerTutor != null )
        {
            //tuto undo 
            isLvTutor = true;
            if(pointerTutor.gameObject.activeSelf == false)
            {
                Invoke("showLevel1Tutor",1f);
            }

        }
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
    public void check1()
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
        if (holes.Length != 0 && numOfHoleNotAvailable.Count == holes.Length)
        {
           
            if (checked1 == false)
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
        if (SaveSystem.instance.unscrewPoint == 0)
        {
            SaveSystem.instance.unscrewPoint = 1;
        }
        GamePlayPanelUIManager.Instance.boosterBar.disableDeteleWatchAdsBT();
        GamePlayPanelUIManager.Instance.boosterBar.SetPoiterPos(0);
        GamePlayPanelUIManager.Instance.boosterBar.InteractableBT(GamePlayPanelUIManager.Instance.boosterBar.deteleBT);
        GamePlayPanelUIManager.Instance.boosterBar.ShowPointer(true);
    }
    public void showLevel1Tutor()
    {
        pointerTutor.gameObject.SetActive(true);
    }
    public void DeactiveTutor()
    {
        if (isTutor) {
            isTutor = false;
        }
    }

    //public void showUndoTuTor()
    //{
    //    if(SaveSystem.instance.undoPoint == 0)
    //    {
    //        SaveSystem.instance.undoPoint = 1;
    //    }
    //    GamePlayPanelUIManager.Instance.boosterBar.disableUndoWatchAdsBT();
    //    GamePlayPanelUIManager.Instance.boosterBar.SetPoiterPos(1);
    //    GamePlayPanelUIManager.Instance.boosterBar.InteractableBT(GamePlayPanelUIManager.Instance.boosterBar.UndoBT);
    //    GamePlayPanelUIManager.Instance.boosterBar.ShowPointer(true);
    //}
}
