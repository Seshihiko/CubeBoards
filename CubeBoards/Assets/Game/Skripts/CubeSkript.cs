using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeSkript : MonoBehaviour
{
    [SerializeField] private GameObject ParticleWhite;
    
    public int index; // индекс куба
    public bool path; //Включает следование по пути

    private int SolidLayer = 1;

    private List<Vector3> pathToTarget = new List<Vector3>(); //путь к цели
    private List<Point> checkedPoint = new List<Point>();
    private List<Point> waitingPoint = new List<Point>();

    public void particleTrue()
    {
        ParticleWhite.SetActive(true);
    }

    public void particleFalse()
    {
        ParticleWhite.SetActive(false);
    }

    public void GetTarget(Transform _object) //Установка цели
    {
        GetPath(_object.position);
    }

    private int CountPoints;//Количество точек в масиве пути
    private void Update()
    {
        if(path && pathToTarget.Count > 0) //Следование по пути
        {
            transform.position = Vector3.MoveTowards(transform.position, pathToTarget[CountPoints], 3 * Time.deltaTime);

            if (transform.position == pathToTarget[CountPoints] && transform.position != pathToTarget[0]) --CountPoints;
            else if (transform.position == pathToTarget[0]) 
            {
                path = false;
                GameController.instance.Combination();
            }
        }
    }

    private List<Vector3> GetPath(Vector3 target)
    {
        pathToTarget = new List<Vector3>();
        checkedPoint = new List<Point>();
        waitingPoint = new List<Point>();

        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPosition = new Vector3(target.x, target.y, target.z);

        if (startPosition == targetPosition) return pathToTarget; 

        Point startPoint = new Point(0, startPosition, targetPosition, null);
        checkedPoint.Add(startPoint);
        waitingPoint.AddRange(GetNeighbourNodes(startPoint));

        while (waitingPoint.Count > 0)
        {
            Point pointToCheck = waitingPoint.Where(x => x.F == waitingPoint.Min(y => y.F)).FirstOrDefault();

            if (pointToCheck.Position == targetPosition)
            {
                return CalculatePathFromNode(pointToCheck);
            }

            Collider[] walkable = Physics.OverlapSphere(pointToCheck.Position, 0.1f, SolidLayer);

            if (walkable.Length > 0) //если точка стена то добавляется в проверенные точки 
            {
                waitingPoint.Remove(pointToCheck);
                checkedPoint.Add(pointToCheck);
            }
            else if (walkable.Length == 0) //если точка не стена то проверяются её соседи
            {
                waitingPoint.Remove(pointToCheck);

                if (!checkedPoint.Where(x => x.Position == pointToCheck.Position).Any())//проверка что бы не было уже имеющихся точек
                {
                    checkedPoint.Add(pointToCheck);
                    waitingPoint.AddRange(GetNeighbourNodes(pointToCheck));
                }
            }
        }

        return pathToTarget;
    }

    private List<Vector3> CalculatePathFromNode(Point point) //Просчет пути
    {
        var path = new List<Vector3>();
        Point currentPoint = point;

        while (currentPoint.PreviousNode != null)
        {
            path.Add(new Vector3(currentPoint.Position.x, currentPoint.Position.y, currentPoint.Position.z));
            currentPoint = currentPoint.PreviousNode;
        }

        pathToTarget = path;
        CountPoints = pathToTarget.Count - 1;
        this.path = true;
        return path;
    }

    private List<Point> GetNeighbourNodes(Point point)//Возвращает соседей
    {
        var Neighbours = new List<Point>();

        Neighbours.Add(new Point(point.G + 1.1f ,
            new Vector3( point.Position.x - 1.1f, point.Position.y, point.Position.z),
            point.TargetPosition,
            point));
        Neighbours.Add(new Point(point.G + 1.1f ,
            new Vector3(point.Position.x + 1.1f , point.Position.y, point.Position.z),
            point.TargetPosition,
            point));
        Neighbours.Add(new Point(point.G + 1.1f ,
            new Vector3(point.Position.x, point.Position.y, point.Position.z - 1.1f),
            point.TargetPosition,
            point));
        Neighbours.Add(new Point(point.G + 1.1f , 
            new Vector3(point.Position.x, point.Position.y, point.Position.z + 1.1f),
            point.TargetPosition,
            point));
        return Neighbours;
    }

    public void OnDrawGizmos()//Показывает путь в редакторе сцены 
    {
        foreach (var item in checkedPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(item.Position.x, 0, item.Position.z), 0.1f);
        }
        if (pathToTarget != null)
            foreach (var item in pathToTarget)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(item.x, 0, item.z), 0.2f);
            }
    }

}

public class Point
{
    public Vector3 Position;
    public Vector3 TargetPosition;
    public Point PreviousNode; //Предыдущая нода
    public float F; // F=G+H формула расчета цены 
    public float G; // расстояние от старта до ноды
    public float H; // расстояние от ноды до цели

    public Point(float g, Vector3 nodePosition, Vector3 targetPosition, Point previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.z - Position.z);
        F = G + H;
    }
}
