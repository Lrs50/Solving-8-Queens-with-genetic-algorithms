using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private List<float> valueList;
    private RectTransform graphContainer;

    void Awake(){
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        ShowGraph(valueList);
    }

    private GameObject CreateCircle(float xPos, float yPos){
        Vector2 anchoredPosition = new Vector2(xPos, yPos);
        GameObject gameObject = new GameObject("GraphCircle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(1, 2);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private float maxValue(List<float> valueList){
        float maximum = 0;
        for (int i = 0; i < valueList.Count; i++){
            if (Mathf.Abs(valueList[i]) > maximum)
                maximum = Mathf.Abs(valueList[i]);
        }
        return maximum;
    }

    private void ShowGraph(List<float> valueList){
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = maxValue(valueList);
        float xSize = graphContainer.sizeDelta.x/(valueList.Count - 1);

        GameObject lastCircle = null;
        for (int i = 0; i < valueList.Count; i++){
            float xPos = i * xSize;
            float yPos = (valueList[i] / yMaximum) * graphHeight/2 + graphHeight/2;
            GameObject circleGameObject = CreateCircle(xPos, yPos);
            if (lastCircle != null){
                CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, new Vector2(xPos, yPos));
            }
            lastCircle = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB){
        GameObject gameObject = new GameObject("DotConnection", typeof(LineRenderer));
        gameObject.transform.SetParent(graphContainer, false);
        LineRenderer Line = gameObject.GetComponent<LineRenderer>();
        Line.startColor = new Color(1, 1, 1, 0.3f);
        Line.endColor = new Color(1, 1, 1, 0.3f);
        Line.startWidth = 0.05f;
        Line.endWidth = 0.05f;
        Line.positionCount = 2;
        Line.useWorldSpace = false;
        Line.material = lineMaterial;
        Line.SetPosition(0, new Vector3(dotPositionA.x - 50f, dotPositionA.y - 50f, -0.01f));
        Line.SetPosition(1, new Vector3(dotPositionB.x - 50f , dotPositionB.y - 50f, -0.01f));
    }
}
