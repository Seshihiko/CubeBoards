using UnityEngine;

public class CameraSkript : MonoBehaviour
{
    private Camera m_Camera;
    private RaycastHit p_hit;
    private PointController point;
    private CubeSkript cube;
    void Start()
    {
        m_Camera = Camera.main;
    }

    void Update()
    {
        Ray p_ray = m_Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(p_ray, out p_hit) && p_hit.collider != null)
        {
            if (p_hit.collider.tag == "Cube") 
            {
                var _cube = p_hit.collider.gameObject.GetComponent<CubeSkript>();

                if (Input.GetMouseButtonDown(0)) //¬ыбирает кубик на который нажал игрок
                {
                    if(cube != null) cube.particleFalse();
                    cube = _cube;
                    cube.particleTrue();
                }
            }
            else if (p_hit.collider.tag == "Point")
            {
                point = p_hit.collider.gameObject.GetComponent<PointController>();
                point.ActiveParticle();

                if (Input.GetMouseButtonDown(0) && cube != null && !cube.path) //”станавливает кубику нужную цель 
                {
                    point.cubeIndex = cube.index;
                    cube.GetTarget(point.transform);
                }
            }
            else if (point != null)
            {
                point.NotActiveParticle();
                point = null;
            }
        }
    }
}
