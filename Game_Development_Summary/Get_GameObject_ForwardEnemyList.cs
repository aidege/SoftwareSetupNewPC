using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private float start_angel_1 = -80f;
    [SerializeField] private float start_angel_2 = 80f;
    [SerializeField] private float distance = 5f; // Distance in front of the object to place the lines

    [SerializeField] private LineRenderer m_debug_attack_range_1 = null;
    [SerializeField] private LineRenderer m_debug_attack_range_2 = null;

    void Start()
    {
        ShowDebugObj();
    }

    private void OnEnable()
    {
        ShowDebugObj();
    }

    private void ShowDebugObj()
    {
        // Initialize the LineRenderers if they are not set in the inspector
        if (m_debug_attack_range_1 == null)
        {
            m_debug_attack_range_1 = CreateLineRenderer(Color.red);
        }

        if (m_debug_attack_range_2 == null)
        {
            m_debug_attack_range_2 = CreateLineRenderer(Color.yellow);
        }

        // Set the positions for both attack ranges
        UpdateLinePos();
    }

    private void UpdateLinePos()
    {
        if (m_debug_attack_range_1 == null)
        {
            return;
        }

        if (m_debug_attack_range_2 == null)
        {
            return;
        }

        UpdateLineRendererPositions(m_debug_attack_range_1, start_angel_1);
        UpdateLineRendererPositions(m_debug_attack_range_2, start_angel_2);
    }

    private LineRenderer CreateLineRenderer(Color lineColor)
    {
        GameObject lineObj = new GameObject("DebugAttackRange");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2; // Only two points for the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        return lineRenderer;
    }

    private void UpdateLineRendererPositions(LineRenderer lineRenderer, float angle)
    {
        // Get the forward direction of the object
        Vector3 forward = transform.forward;

        // Calculate the position for the start and end of the line based on the angle
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Quaternion.Euler(0, angle, 0) * forward * distance);

        // Set the positions for the line renderer
        lineRenderer.SetPosition(0, startPos); 
        lineRenderer.SetPosition(1, endPos); 
    }

    void Update()
    {
        UpdateLinePos();
        On_Attack_Click();
    }
    
    public List<GameObject> m_SearchTarget = new List<GameObject>();
    public LayerMask layer_select;
    private static readonly Collider[] hitResults = new Collider[32];
    
    /// <summary>
    /// 球形范围检测：一次性收集多个敌人
    /// </summary>
    public List<GameObject> GetObjList(float search_range)
    {
        List<GameObject> lst = new List<GameObject>();
        
        // 一旦有敌人进入，就进行一次范围检测
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, search_range,
            hitResults, layer_select);
        
        for (int i = 0; i < hitCount; i++)
        {
            var collider = hitResults[i];
            if (collider != null)
            {
                lst.Add(collider.gameObject);
            }
        }
        
        return lst;
    }
    
    public List<GameObject> m_select_target_forward = new List<GameObject>();
    private void On_Attack_Click()
    {
        m_SearchTarget = GetObjList(6);
        //
        m_select_target_forward = Get_ForwardEnemyList(m_SearchTarget,start_angel_1,start_angel_2);
    }

    public List<GameObject> Get_ForwardEnemyList(List<GameObject> all_enemy_lst,float start_angel,float end_angel)
    {
        List<GameObject> lst = new List<GameObject>();

        foreach (var enemy in all_enemy_lst)
        {
            // Get the direction from the player to the enemy
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

            // Calculate the angle between the player's forward direction and the direction to the enemy
            float angle = Vector3.Angle(transform.forward, directionToEnemy);

            // Check if the angle is within the specified range
            if (angle >= start_angel && angle <= end_angel)
            {
                lst.Add(enemy);
            }
        }

        return lst;
    }


}
