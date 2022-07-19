using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
	private Animator animator;
	private bool isGemTaken;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isGemTaken && other.CompareTag("Player"))
		{
			FollowingPoint.singletonPoint.GetGem();
			animator.SetTrigger("Taken");
			isGemTaken = true;
			Invoke(nameof(DestroyGem), 0.5f);
		}
	}

	private void DestroyGem() => Destroy(gameObject);
}
