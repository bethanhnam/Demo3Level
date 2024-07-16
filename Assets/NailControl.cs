using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class NailControl : MonoBehaviour
{
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private Hole hole;
	[SerializeField]
	public SpriteRenderer nailSprite;
	[SerializeField]
	public SpriteRenderer redNailSprite;
	[SerializeField]
	public List<HingeJoint2D> hingeJoint2Ds = new List<HingeJoint2D>();
	public List<NailDetector> holeInIronControllers = new List<NailDetector>();
	public List<IronPlate> ironPlates = new List<IronPlate>();

	public Collider2D collider2D;
	public Rigidbody2D rigidbody2D;

    private int pickUp = Animator.StringToHash("SelectNail");
	private int unSelect = Animator.StringToHash("DeSelectNail");
	private int idle = Animator.StringToHash("idleBail");

	private void Start()
	{
        collider2D = GetComponent<Collider2D>();
		nailSprite = GetComponent<SpriteRenderer>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	public void PickUp(Vector3 pos)
	{
        AudioManager.instance.PlaySFX("PushNail");
        //AudioManager.instance.PlaySFX("PickUpScrew");
		nailSprite.enabled = false;
		anim.Play(pickUp, 0, 0);
		Stage.Instance.curNail.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 23;
	}

	public void SetNewPos(Vector3 pos)
	{
        Stage.Instance.curNail.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 22;
        AudioManager.instance.PlaySFX("PushNail");
        transform.position = pos;
        anim.Play(unSelect, 0, 0);
	}

	public void Unselect() {
        Stage.Instance.curNail.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 22;
        anim.Play(unSelect, 0, 0);
	}
	public void SetHole(Hole  _hole)
	{
		hole = _hole;
	}
	public void SetHingeJoint(HingeJoint2D _hingeJoint2D)
	{
		if (!hingeJoint2Ds.Contains(_hingeJoint2D)){
			hingeJoint2Ds.Add(_hingeJoint2D);
		}
	}
	public void RemoveHingeJoint(HingeJoint2D _hingeJoint2D)
	{
		if (hingeJoint2Ds.Contains(_hingeJoint2D))
		{
			hingeJoint2Ds.Remove(_hingeJoint2D);
		}
	}
	public void SetHoleInIron(NailDetector holeInIronController)
	{
		if (!holeInIronControllers.Contains(holeInIronController))
		{
			holeInIronControllers.Add(holeInIronController);
		}
	}
	public void RemoveHoleInIron(NailDetector holeInIronController)
	{
		if (holeInIronControllers.Contains(holeInIronController))
		{
			holeInIronControllers.Remove(holeInIronController);
		}
	}
	public void check()
	{
		RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(this.transform.position, 0.1f, Vector3.forward, Mathf.Infinity);

		for (int i = 0; i < cubeHit.Length; i++)
		{
			if (cubeHit[i].transform.gameObject.tag == "HoleInIron")
			{
				var holes = cubeHit[i].transform.GetComponent<NailDetector>();
				if(holes.Nail == this)
					{
						SetHoleInIron(holes);
						SetHingeJoint(holes.hingeJoint2D);
					}
			}
		}
	}
	public void RemoveHinge()
	{
		foreach(var hinge in hingeJoint2Ds)
		{
			hinge.connectedBody = null;
			hinge.enabled = false;
			hinge.connectedAnchor = Vector2.zero;
		}
		hingeJoint2Ds.Clear();
		holeInIronControllers.Clear();
		ironPlates.Clear();
	}
	public void SetNailToIron()
	{
		collider2D.isTrigger = false;
	}
	public void SetTrigger()
	{
		collider2D.isTrigger = true;
	}
}
