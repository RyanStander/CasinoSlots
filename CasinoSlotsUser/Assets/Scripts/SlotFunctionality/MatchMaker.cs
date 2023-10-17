using System.Collections.Generic;
using SlotDisplay;
using UnityEngine;

namespace SlotFunctionality
{
    public class MatchMaker
    {
        private readonly Material lineMaterial;
        private readonly Vector3 symbolOffset;
        private readonly List<GameObject> matchLines = new();

        public MatchMaker(Material lineMaterial, Vector3 symbolOffset)
        {
            this.lineMaterial = lineMaterial;
            this.symbolOffset = symbolOffset;
        }

        public void ClearLines()
        {
            //Clear lines from previous spin
            foreach (var line in matchLines)
            {
                GameObject.Destroy(line);
            }

            matchLines.Clear();
        }

        private void DrawLine(Vector3 start, Vector3 end)
        {
            var myLine = new GameObject();
            myLine.transform.position = start;
            var lineRenderer = myLine.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.material = lineMaterial;
            matchLines.Add(myLine);
        }

        public void CheckForMatches(GameObject[,] symbols, SlotLayoutManager slotLayoutManager)
        {
            CheckForVerticalMatch(symbols, slotLayoutManager);

            CheckForHorizontalMatch(symbols, slotLayoutManager);
        }

        private void CheckForVerticalMatch(GameObject[,] symbols, SlotLayoutManager slotLayoutManager)
        {
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
        }

        private void CheckForHorizontalMatch(GameObject[,] symbols, SlotLayoutManager slotLayoutManager)
        {
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
        }
    }
}
