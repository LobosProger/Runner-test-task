using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
	private List<Vector3> points = new List<Vector3>();
	[SerializeField]
	private LineRenderer line;
	[SerializeField]
	private float stroke = 1f;
	private bool startedDrawing;
	private void Update()
	{
		line.startWidth = stroke;
		line.endWidth = stroke;

		if (startedDrawing)
		{
			points.Add((Input.mousePosition - new Vector3(1185.3f, 260.9f, 0.0f)));
			line.positionCount++;
			line.SetPosition(line.positionCount - 1, (Input.mousePosition - new Vector3(1185.3f, 260.9f, 0.0f)));
		}

		else
		{
			line.positionCount = 0;
			FollowingPoint.singletonPoint.SetPoint(points);
			points.Clear();
		}
	}

	public void StopingDrawing() => startedDrawing = false;
	public void StartingDrawing() => startedDrawing = true;
}
