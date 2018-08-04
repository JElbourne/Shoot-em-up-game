using UnityEngine;
using System.Collections.Generic;
using Rts2DEngine.Units;

namespace Rts2DEngine
{
    namespace MouseSelection
    {
        public class SelectionManager : MonoBehaviour
        {

            #region variables

            public static SelectionManager instance;
            //floats
            public float boxWidth;
            public float boxHeight;
            public float boxTop;
            public float boxLeft;
            //Vector2
            public Vector2 boxStart;
            public Vector2 boxFinish;
            public Vector2 mouseDragStartPosition;
            //Vector3
            public Vector3 currentMousePoint;
            public Vector3 mouseDownPoint;
            //GUI
            public GUIStyle mouseDragSkin;
            //list and arrays
            public List<GameObject> currentSelectedObjects = new List<GameObject>();
            //bool
            public bool mouseDragging;
            //gameObjects
            public GameObject selectedObject;
            //FSM
            public enum SelectFSM // Finite State Machine = FSM
            {
                clickOrDrag,
                clickSelect,
                clickDeselct
            }
            public SelectFSM selectFSM; 

            #endregion

            void Awake()
            {
                instance = this;
            }

            // Update is called once per frame
            void Update()
            {
                SelectUnitsFSM();
            }

            void OnGUI()
            {
                if (mouseDragging)
                {
                    GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), "", mouseDragSkin);
                }
            }

            private void SelectUnitsFSM() // Finite State Machine = FSM
            {
                switch(selectFSM)
                {
                    case SelectFSM.clickOrDrag:
                        ClickOrDrag();
                        break;
                    case SelectFSM.clickSelect:
                        SelectSingleObject();
                        break;
                    case SelectFSM.clickDeselct:
                        DeselectAll();
                        break;
                }
            }

            private void ClickOrDrag()
            {
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    currentMousePoint = hit.point;
                    mouseDragStartPosition = Input.mousePosition;

                    if (hit.collider.gameObject.tag == "Unit")
                    {
                        selectedObject = hit.collider.gameObject;
                        selectFSM = SelectFSM.clickSelect;
                    }
                    else if(hit.collider.gameObject.tag == "Ground")
                    {
                        selectFSM = SelectFSM.clickDeselct;
                    }
                }
                else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
                {
                    if (hit.collider.gameObject.tag == "Unit" && !currentSelectedObjects.Contains(hit.collider.gameObject))
                    {
                        AddToCurrentSelectedObjects(hit.collider.gameObject);
                    } else if (hit.collider.gameObject.tag == "Unit" && currentSelectedObjects.Contains(hit.collider.gameObject))
                    {
                        RemoveFromCurrentSelectedObjects(hit.collider.gameObject);
                    }
                }
                else if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
                {
                    if(UserDraggingByPosition(mouseDragStartPosition, Input.mousePosition))
                    {
                        mouseDragging = true;
                        DrawDragBox();
                        SelectObjectsInDrag();
                    }
                }
                else if (Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftShift))
                {
                    mouseDragging = false;
                }
            }

            private void DrawDragBox()
            {
                boxWidth = UnityEngine.Camera.main.WorldToScreenPoint(mouseDownPoint).x - UnityEngine.Camera.main.WorldToScreenPoint(currentMousePoint).x;
                boxHeight = UnityEngine.Camera.main.WorldToScreenPoint(mouseDownPoint).y - UnityEngine.Camera.main.WorldToScreenPoint(currentMousePoint).y;
                boxLeft = Input.mousePosition.x;
                boxTop = (Screen.height - Input.mousePosition.y) - boxHeight; // need to invert y

                if (boxWidth > 0 && boxHeight < 0f)
                {
                    boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                else if (boxWidth > 0 && boxHeight > 0f)
                {
                    boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + boxHeight);
                }
                else if(boxWidth < 0f && boxHeight < 0f)
                {
                    boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y);
                }
                else if(boxWidth < 0f && boxHeight > 0f)
                {
                    boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y + boxHeight);
                }
                boxFinish = new Vector2(boxStart.x + Mathf.Abs(boxWidth), boxStart.y - Mathf.Abs(boxHeight));
            }

            private bool UserDraggingByPosition(Vector2 dragStartPoint, Vector2 newPoint)
            {
                if ((newPoint.x > dragStartPoint.x || newPoint.x < dragStartPoint.x) ||
                    (newPoint.y > dragStartPoint.y || newPoint.y < dragStartPoint.y))
                {
                    return true;
                } else
                {
                    return false;
                }
            }

            private void SelectObjectsInDrag()
            {
                for (int i=0; i < UnitManager.instance.units.Count; i++)
                {
                    if (UnitManager.instance.units[i].GetComponent<Unit>().renderer.isVisible)
                    {
                        Vector2 unitScreenPosition = UnityEngine.Camera.main.WorldToScreenPoint(UnitManager.instance.units[i].transform.position);
                        if (unitScreenPosition.x < boxFinish.x && unitScreenPosition.y > boxFinish.y && unitScreenPosition.x > boxStart.x && unitScreenPosition.y < boxStart.y)
    
                        {
                            AddToCurrentSelectedObjects(UnitManager.instance.units[i]);
                        }
                        else
                        {
                            RemoveFromCurrentSelectedObjects(UnitManager.instance.units[i]);
                        }
                    }
                }
            }

            private void SelectSingleObject()
            {
                if (selectedObject != null)
                {
                    if(currentSelectedObjects.Count > 0)
                    {
                        RemoveSelectedObjects();
                    }
                    else if (currentSelectedObjects.Count == 0)
                    {
                        AddToCurrentSelectedObjects(selectedObject);
                        selectFSM = SelectFSM.clickOrDrag;
                    }
                }
            }

            private void AddToCurrentSelectedObjects(GameObject objectToAdd)
            {
                if(!currentSelectedObjects.Contains(objectToAdd))
                {
                    currentSelectedObjects.Add(objectToAdd);
                    objectToAdd.transform.Find("SelectionCircle").gameObject.SetActive(true);
                }
            }

            private void RemoveFromCurrentSelectedObjects(GameObject objectToRemove)
            {
                if (currentSelectedObjects.Contains(objectToRemove))
                {
                    objectToRemove.transform.Find("SelectionCircle").gameObject.SetActive(false);
                    currentSelectedObjects.Remove(objectToRemove);
                }
            }

            private void DeselectAll()
            {
                if(currentSelectedObjects.Count > 0)
                {
                    RemoveSelectedObjects();
                }
                else if(currentSelectedObjects.Count == 0)
                {
                    selectFSM = SelectFSM.clickOrDrag;
                }
            }

            private void RemoveSelectedObjects()
            {
                for (int i = 0; i < currentSelectedObjects.Count; i++)
                {
                    currentSelectedObjects[i].transform.Find("SelectionCircle").gameObject.SetActive(false);
                    currentSelectedObjects.Remove(currentSelectedObjects[i]);
                }
            }
        }

    }
}