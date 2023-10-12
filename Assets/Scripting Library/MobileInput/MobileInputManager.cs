using System;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    private const float c_AllowedClickDistanceDelta = 0.1f;
    private const float c_AllowedTimeForPinchDetection = 0.2f;
    private const float c_AllowedTimeForPinchDetectionPerTouch = 0.3f;

    private List<TouchData> _rawTouches = new List<TouchData>();
    private List<TouchData> _swipeTouches = new List<TouchData>();
    PinchData _pinchTouch;

    public static event Action<ClickInfo> OnClick;
    public static event Action<SwipeInfo> OnSwipe;
    public static event Action<PinchInfo> OnPinch;

#if UNITY_EDITOR
    private Vector3 swipeStartPos;
    private Vector3 swipeLastPos;
#endif

    private void Update()
    {
#if UNITY_EDITOR
        EditorUpdate();
#else
        MobileUpdate();
#endif
    }

    private void EditorUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            OnClick.Invoke(new ClickInfo()
            {
                StartPos = Input.mousePosition,
                EndPos = Input.mousePosition
            });
        }

        if (Input.GetMouseButtonDown(1))
        {
            swipeStartPos = Input.mousePosition;
            swipeLastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            OnSwipe?.Invoke(new SwipeInfo()
            {
                ID = -1,
                StartPosition = swipeStartPos,
                FinalPosition = Input.mousePosition,
                DeltaPosition = (Input.mousePosition - swipeLastPos) / Screen.dpi
            });
            swipeLastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            OnSwipe?.Invoke(new SwipeInfo()
            {
                ID = -1,
                StartPosition = swipeStartPos,
                FinalPosition = Input.mousePosition,
                DeltaPosition = (Input.mousePosition - swipeLastPos) / Screen.dpi,
                IsFinished = true
            });
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            OnPinch?.Invoke(new PinchInfo
            {
                Delta = Input.mouseScrollDelta.y
            });
        }
#endif
    }
    private void MobileUpdate()
    {
        if (_pinchTouch != null) _pinchTouch.UpdatedOnThisFrame = false;

        else if (_swipeTouches.Count > 1)
        {//Convert Swipe touches to Pinch touches
            for (int i = _swipeTouches.Count - 1; i > 0; i--)
            {
                var prev = _swipeTouches.Prev(i);
                var cur = _swipeTouches[i];
                if
                (
                    Mathf.Abs(prev.StartTime - cur.StartTime) < c_AllowedTimeForPinchDetection &&
                    Mathf.Abs(prev.StartTime - Time.realtimeSinceStartup) < c_AllowedTimeForPinchDetectionPerTouch
                )
                {
                    Touch prevTouch = new Touch(), curTouch = new Touch();
                    foreach (var t in Input.touches)
                    {
                        if (t.fingerId == prev.ID) prevTouch = t;
                        if (t.fingerId == cur.ID) curTouch = t;
                    }

                    if (prevTouch.phase == TouchPhase.Moved && curTouch.phase == TouchPhase.Moved)
                    {
                        var angle = Vector2.Angle(prevTouch.deltaPosition, curTouch.deltaPosition);

                        if (angle.Between(-1, 7) || angle.Between(173f, 181f))
                        {
                            OnSwipe?.Invoke(new SwipeInfo()
                            {
                                ID = prevTouch.fingerId,
                                StartPosition = _swipeTouches[i - 1].StartPos,
                                FinalPosition = prevTouch.position,
                                DeltaPosition = prevTouch.deltaPosition / Screen.dpi,
                                IsFinished = true
                            });
                            OnSwipe?.Invoke(new SwipeInfo()
                            {
                                ID = curTouch.fingerId,
                                StartPosition = _swipeTouches[i].StartPos,
                                FinalPosition = curTouch.position,
                                DeltaPosition = curTouch.deltaPosition / Screen.dpi,
                                IsFinished = true
                            });
                            _swipeTouches.RemoveAt(i);
                            _swipeTouches.RemoveAt(i - 1);

                            _pinchTouch = new PinchData(prev.ID, cur.ID, (prevTouch.position - curTouch.position).sqrMagnitude);
                        }
                    }
                }
            }
        }

        //Process new input
        foreach (var t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                _rawTouches.Add(new TouchData(t));
            }
            else if (t.phase == TouchPhase.Moved)
            {
                if (GetRawTouchWithID(t.fingerId, out var rawTouch, out var rawIndex) && DPIMagnitude(t.position, rawTouch.StartPos) > c_AllowedClickDistanceDelta)
                {
                    _swipeTouches.Add(rawTouch);
                    _rawTouches.RemoveAt(rawIndex);
                }
                else if (GetSwipeTouchIndex(t.fingerId, out var swipeTouch, out int swipeIndex))
                {
                    OnSwipe?.Invoke(new SwipeInfo()
                    {
                        ID = t.fingerId,
                        StartPosition = swipeTouch.StartPos,
                        FinalPosition = t.position,
                        DeltaPosition = t.deltaPosition / Screen.dpi
                    });
                }
                else if (_pinchTouch != null && !_pinchTouch.UpdatedOnThisFrame && _pinchTouch.IsInvolved(t.fingerId))
                {
                    if (GetPinchTouches(out var one, out var two))
                    {
                        var curFrameSqrMagnitude = (one.position - two.position).sqrMagnitude;

                        OnPinch?.Invoke(new PinchInfo()
                        {
                            Delta = (curFrameSqrMagnitude - _pinchTouch.PrevFrameSQRMagnitude) / Screen.dpi
                        });

                        _pinchTouch.PrevFrameSQRMagnitude = curFrameSqrMagnitude;
                    }
                }
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (GetRawTouchWithID(t.fingerId, out var rawTouche, out var index) && DPIMagnitude(t.position, rawTouche.StartPos) <= c_AllowedClickDistanceDelta)
                {
                    _rawTouches.RemoveAt(index);
                    OnClick?.Invoke(new ClickInfo() { StartPos = rawTouche.StartPos, EndPos = t.position });
                }
                else if (GetSwipeTouchIndex(t.fingerId, out var swipeTouch, out int swipeIndex))
                {
                    OnSwipe?.Invoke(new SwipeInfo()
                    {
                        ID = t.fingerId,
                        StartPosition = swipeTouch.StartPos,
                        FinalPosition = t.position,
                        DeltaPosition = t.deltaPosition / Screen.dpi,
                        IsFinished = true
                    });
                    _swipeTouches.RemoveAt(swipeIndex);
                }
                else if (_pinchTouch != null && _pinchTouch.IsInvolved(t.fingerId))
                {
                    _pinchTouch = null;
                }
            }
            else if (t.phase == TouchPhase.Canceled)
            {
                _rawTouches.RemoveAll(x => x.ID == t.fingerId);
                if (_pinchTouch != null && _pinchTouch.IsInvolved(t.fingerId)) _pinchTouch = null;

                if (GetSwipeTouchIndex(t.fingerId, out var swipeTouch, out int swipeIndex))
                {
                    OnSwipe?.Invoke(new SwipeInfo()
                    {
                        ID = t.fingerId,
                        StartPosition = swipeTouch.StartPos,
                        FinalPosition = t.position,
                        DeltaPosition = t.deltaPosition / Screen.dpi,
                        IsFinished = true
                    });
                    _swipeTouches.RemoveAt(swipeIndex);
                }
            }
        }
    }

    private bool GetRawTouchWithID(int id, out TouchData rawTouch, out int index) => GetTouchWithID(id, _rawTouches, out rawTouch, out index);
    private bool GetSwipeTouchIndex(int id, out TouchData swipeTouch, out int index) => GetTouchWithID(id, _swipeTouches, out swipeTouch, out index);
    private bool GetTouchWithID(int id, List<TouchData> touches, out TouchData touche, out int index)
    {
        touche = null;
        index = -1;

        for (int i = 0; i < touches.Count; i++)
        {
            var curTouche = touches[i];
            if (curTouche.ID == id)
            {
                touche = curTouche;
                index = i;
                return true;
            }
        }

        return false;
    }
    private bool GetPinchTouches(out Touch one, out Touch two)
    {
        one = new Touch();
        two = new Touch();

        bool foundOne = false;
        bool foundTwo = false;

        if (_pinchTouch != null)
        {
            foreach (var t in Input.touches)
            {
                if (_pinchTouch.IDOne == t.fingerId)
                {
                    one = t;
                    foundOne = true;
                }
                if (_pinchTouch.IDTwo == t.fingerId)
                {
                    two = t;
                    foundTwo = true;
                }
            }
        }

        return foundOne && foundTwo;
    }

    public static float DPIMagnitude(Vector2 one, Vector2 two) => (one - two).magnitude / Screen.dpi;

    [Serializable]
    public class TouchData
    {
        public int ID { get; set; }
        public Vector2 StartPos { get; set; }
        public float StartTime { get; set; }

        public TouchData(Touch touch)
        {
            ID = touch.fingerId;
            StartPos = touch.position;
            StartTime = Time.realtimeSinceStartup;
        }
        public TouchData(int id, Vector2 startPos)
        {
            ID = id;
            StartPos = startPos;
            StartTime = Time.realtimeSinceStartup;
        }
    }

    public class PinchData
    {
        public int IDOne { get; set; }
        public int IDTwo { get; set; }
        public float PrevFrameSQRMagnitude { get; set; }

        public bool UpdatedOnThisFrame { get; set; }

        public bool IsInvolved(int id) => IDOne == id || IDTwo == id;

        public PinchData(int iDOne, int iDTwo, float prevFrameSQRDistance)
        {
            IDOne = iDOne;
            IDTwo = iDTwo;
            PrevFrameSQRMagnitude = prevFrameSQRDistance;
            UpdatedOnThisFrame = false;
        }
    }

    public struct ClickInfo
    {
        public Vector2 StartPos { get; set; }
        public Vector2 EndPos { get; set; }
    }
    public struct SwipeInfo
    {
        public int ID { get; set; }
        public Vector2 StartPosition { get; set; }
        public Vector2 FinalPosition { get; set; }
        public Vector2 DeltaPosition { get; set; }
        public bool IsFinished { get; set; }
        public Vector2 DeltaDirectionNormalized => DeltaPosition.normalized;
        public float DeltaMagnitude => DeltaPosition.magnitude;
    }
    public struct PinchInfo
    {
        public float Delta { get; set; }
    }
}