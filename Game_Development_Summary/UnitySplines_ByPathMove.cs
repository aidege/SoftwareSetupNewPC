
//"com.unity.splines": "2.6.1"

using UnityEngine;
using UnityEngine.Splines;

[DisallowMultipleComponent]
public class Test : MonoBehaviour
{
    public SplineAnimate m_SplineAnimate;

    public float m_Speed = 0.01f;
    public int m_Direction = 1; // 1 for forward, -1 for backward
    public float m_Progress = 0.0f; // Tracks position on the spline

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Direction = -m_Direction;
            ApplyForwardAxis();
        }

        m_Progress += m_Direction * m_Speed * Time.deltaTime;
        if (m_Progress > 1.0f)
        {
            m_Progress = 0.0f;  // Loop back to the start if moving forward
        }
        else if (m_Progress < 0.0f)
        {
            m_Progress = 1.0f;  // Loop back to the end if moving backward
        }

        // Update the position along the spline
        m_SplineAnimate.NormalizedTime = m_Progress;

    }

    private void ApplyForwardAxis()
    {
        if (m_SplineAnimate == null) return;

        // Forward
        m_SplineAnimate.ObjectForwardAxis = (m_Direction >= 0)
            ? SplineComponent.AlignAxis.ZAxis
            : SplineComponent.AlignAxis.NegativeZAxis;
    }
}
