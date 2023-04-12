using System.Collections;
using System.Collections.Generic;
using TocClient;
using UnityEngine;
using UnityEngine.UI;

public class BezierArrows : MonoBehaviour
{
    [Tooltip("头部箭头")]
    public GameObject ArrowHeadPrefab;
    [Tooltip("节点箭头")]
    public GameObject ArrowNodePrefab;
    [Tooltip("节点数量")]
    public int ArrowNodeNum;
    [Tooltip("节点缩放")]
    public float ScaleFactor;
    [Tooltip("箭头根节点")]
    public RectTransform Origin;

    public bool StartDraw;
    private List<RectTransform> arrowNodes = new();
    private List<Vector2> controlPoints = new();
    private readonly List<Vector2> controlPointFactors = new List<Vector2>  { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };

    public void Init()
    {
        for (int i = 0; i < this.ArrowNodeNum; i++)
            this.arrowNodes.Add(Instantiate(this.ArrowNodePrefab,Origin.transform).GetComponent<RectTransform>());
        this.arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));
        for (int i = 0; i < 4; ++i)
            this.controlPoints.Add(Vector2.zero);
    }
    private void Update()
    {
        if (!StartDraw)
            return;
        this.controlPoints[0] = new Vector2(this.Origin.position.x,this.Origin.position.y);
        this.controlPoints[3] = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3]-this.controlPoints[0])*this.controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[1];

        for (int i = 0; i < this.arrowNodes.Count; i++)
        {
            var t = Mathf.Log(1f * i / (this.arrowNodes.Count - 1) + 1f, 2f);
            Debug.Log("I=" + i);
            this.arrowNodes[i].position =
                Mathf.Pow(1 - t, 3) * this.controlPoints[0] +
                3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1] +
                3 * (1 - t) * Mathf.Pow(t, 2) * this.controlPoints[2] +
                Mathf.Pow(t, 3) * this.controlPoints[3];
            if (i>0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
                this.arrowNodes[i].rotation = Quaternion.Euler(euler);
            }
            var scale = this.ScaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
            this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
        }
        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
    public void TurnRed()
    {
        foreach (var stem in arrowNodes)
        {
            stem.GetComponent<Image>().color = Color.red;
        }
    }

    public void TurnWhite()
    {
        foreach (var stem in arrowNodes)
        {
            stem.GetComponent<Image>().color = Color.white;
        }
    }
    public void TurnGreen()
    {
        foreach (var stem in arrowNodes)
        {
            stem.GetComponent<Image>().color = Color.green;
        }
    }
}
