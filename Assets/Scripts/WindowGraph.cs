using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private List<float> valueListY;
    [SerializeField] private List<float> valueListX;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private RectTransform graphContainer;

    void Awake(){
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        ShowGraph(valueListY, valueListX);
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

    private float maxAbsValue(List<float> valueList){
        float maximum = 0;
        for (int i = 0; i < valueList.Count; i++){
            if (Mathf.Abs(valueList[i]) > maximum)
                maximum = Mathf.Abs(valueList[i]);
        }
        return maximum;
    }

    private float maxValue(List<float> valueList){
        float maximum = 0;
        for (int i = 0; i < valueList.Count; i++){
            if (valueList[i] > maximum)
                maximum = valueList[i];
        }
        return maximum;
    }

    private float minValue(List<float> valueList){
        float minimum = 0;
        for (int i = 0; i < valueList.Count; i++){
            if (valueList[i] < minimum)
                minimum = valueList[i];
        }
        return minimum;
    }

    private void ShowGraph(List<float> valueListY, List<float> valueListX){
        valueListX.Sort((a, b) => a.CompareTo(b)); //Sort para que a parte horizontal fique sempre crescente
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float xMaximum = maxValue(valueListX);
        float yMaximumAbs = maxAbsValue(valueListY);
        float xMinimum = minValue(valueListX);
        float yMinimum = minValue(valueListY);
        Debug.Log(xMinimum);
        Debug.Log(xMaximum);
        float xSize = graphWidth/(xMaximum - xMinimum);
        float ySize = graphHeight/yMaximumAbs   ;

        GameObject lastCircle = null;
        for (int i = 0; i < valueListY.Count && i < valueListX.Count; i++){
            float xPos = 0;
            if (xMinimum < 0)
                xPos = (valueListX[i] - xMinimum) * xSize;
            else
                xPos = valueListX[i] * xSize;
            float yPos = valueListY[i] * ySize/2 + graphHeight/2;
            GameObject circleGameObject = CreateCircle(xPos, yPos);
            if (lastCircle != null){
                CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, new Vector2(xPos, yPos));
            }
            lastCircle = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPos - 0.7f, -1f);
            labelX.GetComponent<Text>().text = valueListX[i].ToString();

            RectTransform labelY = Instantiate(labelTemplateX);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            if (valueListY[i] < 0)
                labelY.anchoredPosition = new Vector2(-2.4f, yPos + 2f);
            else
                labelY.anchoredPosition = new Vector2(-2f, yPos + 2f);
            labelY.GetComponent<Text>().text = valueListY[i].ToString();

            if (Mathf.Abs(valueListY[i]) != yMaximumAbs){
                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(0f, yPos);
            }

            if (i != 0 && i != valueListY.Count - 1){
                RectTransform dashY = Instantiate(dashTemplateY);
                dashY.SetParent(graphContainer);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(xPos, 0f);
            }
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
        Line.sortingOrder = 1;
        Line.SetPosition(0, new Vector3(dotPositionA.x - 50f, dotPositionA.y - 50f, -0.01f));
        Line.SetPosition(1, new Vector3(dotPositionB.x - 50f , dotPositionB.y - 50f, -0.01f));
    }
}
