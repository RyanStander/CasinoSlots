using System.Collections.Generic;
using SlotDisplay;
using UnityEngine;

namespace SlotFunctionality
{
    /// <summary>
    /// Manages the game logic of the slot machine.
    /// </summary>
    public class SlotGameManager : MonoBehaviour
    {
        [SerializeField] private SlotLayoutManager slotLayoutManager;
        [SerializeField] private GameObject[] slotSymbols;
        [SerializeField] private GameObject slots;
        [SerializeField] private Vector3 symbolOffset = new(0, 0, -1);
        [SerializeField,Range(-2f,-0.01f)] private float startingSpinSpeed = 0.1f;

        private GameObject[,] symbols;
        private List<GameObject> matchLines = new();

        private float topRowYPos;
        private float bottomRowYPos;

        private void OnValidate()
        {
            if (slotLayoutManager == null)
                slotLayoutManager = FindObjectsByType<SlotLayoutManager>(FindObjectsSortMode.None)[0];

            if (slots == null)
                slots = GameObject.Find("Slots");
        }

        private void Start()
        {
            symbols = new GameObject[slotLayoutManager.ReelCount, slotLayoutManager.RowCount];
            SpinTheWheel();

            topRowYPos = slotLayoutManager.SlotRows[0].transform.localPosition.y;
            bottomRowYPos = slotLayoutManager.SlotRows[^1].transform.localPosition.y-slotLayoutManager.RowSpacing;
        }

        private void Update()
        {
            //take the objs and move them down on the y axis
            foreach (var obj in slotLayoutManager.SlotRows)
            {
                obj.transform.position += new Vector3(0, startingSpinSpeed, 0);

                var rowTransform = obj.transform;

                if (rowTransform.localPosition.y <= bottomRowYPos)
                {
                    rowTransform.localPosition =
                        new Vector3(obj.transform.localPosition.x, topRowYPos, rowTransform.localPosition.z);
                }
            }
        }

        public void SpinTheWheel()
        {
            //Clear lines from previous spin
            foreach (var line in matchLines)
            {
                Destroy(line);
            }

            matchLines.Clear();

            for (var i = 0; i < slotLayoutManager.ReelCount; i++)
            {
                for (var j = 0; j < slotLayoutManager.RowCount; j++)
                {
                    var gridPosition = slotLayoutManager.SlotBoard[i, j];

                    foreach (Transform child in gridPosition.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    var pieceType = slotSymbols[Random.Range(0, slotSymbols.Length)];
                    var thisPiece = Instantiate(pieceType, gridPosition.transform.position + symbolOffset,
                        Quaternion.identity);
                    thisPiece.name = pieceType.name;
                    thisPiece.transform.parent = gridPosition.transform;
                    symbols[i, j] = thisPiece;
                }
            }

            CheckForMatches();
        }

        private void DrawLine(Vector3 start, Vector3 end)
        {
            var myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            var lineRenderer = myLine.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            matchLines.Add(myLine);
        }

        private void CheckForMatches()
        {
            #region Vertical Matches

            for (var i = 0; i < slotLayoutManager.ReelCount; i++)
            {
                for (var j = 0; j < slotLayoutManager.RowCount; j++)
                {
                    if (j != 0 && symbols[j, i].name != symbols[j - 1, i].name)
                        break;
                    if (j == slotLayoutManager.RowCount - 1)
                    {
                        DrawLine(symbols[0, i].transform.position + symbolOffset,
                            symbols[slotLayoutManager.RowCount - 1, i].transform.position + symbolOffset);
                    }
                }
            }

            #endregion

            #region Horizontal Matches

            for (var i = 0; i < slotLayoutManager.RowCount; i++)
            {
                var matchLength = 1;
                var matchBegin = symbols[i, 0];
                GameObject matchEnd;
                for (var j = 0; j < slotLayoutManager.ReelCount - 1; j++)
                {
                    if (symbols[i, j].name == symbols[i, j + 1].name)
                    {
                        matchLength++;
                    }
                    else
                    {
                        if (matchLength >= 3)
                        {
                            matchEnd = symbols[i, j];
                            DrawLine(matchBegin.transform.position + symbolOffset,
                                matchEnd.transform.position + symbolOffset);
                        }

                        matchBegin = symbols[i, j + 1];
                        matchLength = 1;
                    }
                }

                if (matchLength >= 3)
                {
                    matchEnd = symbols[i, slotLayoutManager.ReelCount - 1];
                    DrawLine(matchBegin.transform.position + symbolOffset, matchEnd.transform.position + symbolOffset);
                }
            }

            #endregion
        }
    }
}
