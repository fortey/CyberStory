using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
	struct GUILine
	{
		public Vector2 startPt;
		public Vector2 endPt;
	}
	[System.Serializable]
	public struct Pair
	{
		public RectTransform start;
		public RectTransform end;
	}
	public Pair[] pairs;

	private List<Pair> mPairs = new List<Pair>();
	private int count;

	public GameObject WinText;
	//private bool isWin;

	public Texture2D texture;

	private GUILine newline;
	private bool mouseDown;
	private bool mouseUp;
	private bool shift;
	private ArrayList lines;
	private float length;
	private RectTransform currentNode = null;

	void Start()
	{
		lines = new ArrayList();
		CreateMini();
	}
	public void StartPuzzle()
	{
		mPairs.Clear();
		count = 0;
		//isWin = false;
		WinText.SetActive(false);
		ChangeCurrentNode(null);
		lines = new ArrayList();
	}

	void Update()
	{

	}

	void OnGUI()
	{
		foreach (GUILine line in lines)
		{
			setLinePoints(line);
			DrawLine(line.startPt, line.endPt);
		}
	}

	public void SelectNode(RectTransform node)
	{
		if (currentNode == node)
			ChangeCurrentNode(null);
		else if (currentNode == null)
			ChangeCurrentNode(node);
		else if (mPairs.Count(p => p.start == currentNode && p.end == node) > 0)
			ChangeCurrentNode(node);
		else
		{
			if (pairs.Count(p => (p.start == currentNode && p.end == node) || (p.start == node && p.end == currentNode)) == 0)
			{
				WrongLine(currentNode, node);
				return;
			}

			mPairs.Add(new Pair { start = currentNode, end = node });
			mPairs.Add(new Pair { end = currentNode, start = node });

			newline = new GUILine();
			newline.startPt = currentNode.position;
			newline.endPt = node.position;
			addGUILine(newline);

			count++;
			ChangeCurrentNode(node);

			if (count == pairs.Length)
			{
				Win();
			}
		}

	}

	Vector2 setPoint(Vector2 point)
	{
		point.x = (int)point.x;
		point.y = Screen.height - (int)point.y;
		return point;
	}

	void addGUILine(GUILine line)
	{
		lines.Add(line);
	}

	void setLinePoints(GUILine line)
	{
		line.startPt = setPoint(line.startPt);
		line.endPt = setPoint(line.endPt);
		length = (line.startPt - line.endPt).magnitude;
	}

	void DrawLine(Vector2 pointA, Vector2 pointB)
	{
		pointA = setPoint(pointA);
		pointB = setPoint(pointB);
		Texture2D lineTex = new Texture2D(1, 1);
		lineTex = texture;
		Matrix4x4 matrixBackup = GUI.matrix;
		float width = 10.0f;
		GUI.color = Color.black;

		float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * 180f / Mathf.PI;

		GUIUtility.RotateAroundPivot(angle, pointA);
		GUI.DrawTexture(new Rect(pointA.x - 3, pointA.y - width / 2, length + 3, width), lineTex);
		GUI.matrix = matrixBackup;
	}

	void Win()
	{
		WinText.SetActive(true);
		//isWin = true;
		GlobalVariables.instance.Cristals += 5;
	}

	void WrongLine(RectTransform node1, RectTransform node2)
	{
		StartCoroutine(WrongNode(node1));
		StartCoroutine(WrongNode(node2));
	}

	IEnumerator WrongNode(RectTransform node)
	{
		var nodeImage = node.GetComponent<UnityEngine.UI.Image>();
		var startColor = nodeImage.color;
		var timer = 0f;
		var time = 0.5f;
		var t = 0f;
		while (timer < time)
		{
			timer += Time.deltaTime;
			t = timer / time;
			nodeImage.color = Color.Lerp(startColor, Color.red, t);
			yield return new WaitForEndOfFrame();
		}
		timer = 0f;
		while (timer < time)
		{
			timer += Time.deltaTime;
			t = timer / time;
			nodeImage.color = Color.Lerp(Color.red, startColor, t);
			yield return new WaitForEndOfFrame();
		}
		nodeImage.color = startColor;
	}

	void ChangeCurrentNode(RectTransform node)
	{
		if (currentNode != null)
		{
			currentNode.GetComponent<Image>().color = Color.white;
		}
		currentNode = node;
		if (currentNode != null)
		{
			currentNode.GetComponent<Image>().color = Color.grey;
		}
	}

	public void OnOutsideClick()
	{
		ChangeCurrentNode(null);
	}

	void CreateMini()
	{
		var offsetY = Screen.height - Screen.height / 5 - 50;
		foreach (var pair in pairs)
		{
			newline = new GUILine();
			newline.startPt = pair.start.position;
			newline.startPt.x /= 5;
			newline.startPt.y /= 5;
			newline.startPt.y += offsetY;
			newline.endPt = pair.end.position;
			newline.endPt.x /= 5;
			newline.endPt.y /= 5;
			newline.endPt.y += offsetY;
			addGUILine(newline);
		}
	}
}
