using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowingPoint : MonoBehaviour
{
	[SerializeField]
	private float sensivity = 1f;
	[SerializeField]
	private List<Player> units = new List<Player>();
	public HashSet<GameObject> notActivatedUnits = new HashSet<GameObject>();

	[SerializeField]
	private Text text;
	private int amountOfCollectedGems = 0;
	public static FollowingPoint singletonPoint;

	private Rigidbody rb;
	private bool startedGame;
	private bool stopGame;

	[SerializeField]
	private GameObject panelGameOver;
	[SerializeField]
	private GameObject panelVictory;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		singletonPoint = this;
	}

	public void AddUnit(Player unit) => units.Add(unit);
	public void RemoveUnit(Player unit)
	{
		units.Remove(unit);
		if (units.Count == 0)
		{
			StopGame();
			Invoke(nameof(ShowGameOverPanel), 1.5f);
		}
	}

	public void AddNotActivatedUnit(GameObject unit) => notActivatedUnits.Add(unit);
	public void RemoveNotActivatedUnit(GameObject unit) => notActivatedUnits.Remove(unit);
	public bool IsUnitNotActivated(GameObject unit) => notActivatedUnits.Contains(unit);

	public void SetPoint(List<Vector3> positions)
	{
		if (stopGame)
			return;

		int stepToNextPoint = positions.Count / units.Count;
		int indexOfUnit = 0;
		for (int i = 0; i < positions.Count - 1; i += stepToNextPoint)
		{
			if (indexOfUnit == units.Count)
				break;
			else
			{
				units[indexOfUnit].AssignPoint(new Vector3(positions[i].x / sensivity, 0, positions[i].y / sensivity));
				indexOfUnit++;
			}
		}
	}

	public void GetGem()
	{
		amountOfCollectedGems++;
		text.text = amountOfCollectedGems.ToString();
	}

	public void StartGame()
	{
		if (startedGame)
			return;

		startedGame = true;
		rb.velocity = new Vector3(0, 0, 3);
		foreach (Player every in units)
			every.StartRun();
	}

	public void StopGame()
	{
		stopGame = true;
		rb.velocity = new Vector3(0, 0, 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Finish"))
		{
			foreach (Player every in units)
				every.StartDance();

			StopGame();
			Invoke(nameof(ShowVictoryPanel), 1.5f);
		}
	}

	private void ShowGameOverPanel() => panelGameOver.SetActive(true);

	private void ShowVictoryPanel() => panelVictory.SetActive(true);
}
