using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private bool notActivatedUnit;
	[SerializeField]
	private Animator animator;

	private Rigidbody rb;
	private ParticleSystem particle;

	private bool isDead;
	private Vector3 pointToFollowing = Vector3.zero;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		particle = GetComponent<ParticleSystem>();

		if (notActivatedUnit)
			FollowingPoint.singletonPoint.AddNotActivatedUnit(gameObject);
	}

	void Update()
	{
		animator.transform.eulerAngles = new Vector3(-90, 0, 0);
		animator.transform.localPosition = Vector3.zero;

		if (isDead || notActivatedUnit)
			return;

		if (pointToFollowing != Vector3.zero)
			transform.localPosition = Vector3.Lerp(transform.localPosition, pointToFollowing, Time.fixedDeltaTime * 10f);
	}

	public void Death()
	{
		if (!isDead)
		{
			isDead = true;

			animator.enabled = false;
			GetComponent<Collider>().isTrigger = false;
			rb.isKinematic = false;
			rb.useGravity = true;
			particle.Play();
			FollowingPoint.singletonPoint.RemoveUnit(this);

			Invoke(nameof(DestroyUnit), 3f);
		}
	}

	public void ActivateUnit()
	{
		FollowingPoint.singletonPoint.RemoveNotActivatedUnit(gameObject);
		transform.parent = FollowingPoint.singletonPoint.transform;
		notActivatedUnit = false;

		StartRun();
		FollowingPoint.singletonPoint.AddUnit(this);
		particle.Play();
	}

	public void StartRun() => animator.SetTrigger("Run");

	public void StartDance() => animator.SetTrigger("Finish");

	private void DestroyUnit() => Destroy(gameObject);

	public void AssignPoint(Vector3 point) => pointToFollowing = point;

	private void OnTriggerEnter(Collider other)
	{
		if (!isDead && other.CompareTag("Player"))
		{
			if (FollowingPoint.singletonPoint.IsUnitNotActivated(other.gameObject))
			{
				other.GetComponent<Player>().ActivateUnit();
			}
		}
	}
}
