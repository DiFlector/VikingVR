using UnityEngine;

public class randomDots : MonoBehaviour
{
    public int n;
    private GameObject circle;
    private GameObject rect;
    private GameObject dotSample;
    private GameObject rectCorner;
    private float rad;
    private Vector3 rectScale;
    private Vector3[] rectCoords;
    private Vector3 currentPos;
    private Vector3 rectPos;
    private float timer;
    private bool change;


    void Start()
    {
        dotSample = gameObject.transform.GetChild(1).gameObject;
        circle = gameObject.transform.GetChild(0).gameObject;
        rect = gameObject.transform.GetChild(2).gameObject;
        if (n == 0)
        {
            n = 1;
        }
        currentPos = circle.transform.position;
        rad = circle.transform.localScale.x / 2;
        rectPos = rect.transform.position;
        rectScale = rect.transform.lossyScale;
        rectCoords = new Vector3[2] {
            rectPos + new Vector3(rectScale.x / 2, 0, rectScale.y / 2),
            rectPos + new Vector3(rectScale.x / -2, 0, rectScale.y / -2),
        };
        for (int i = 0; i < n; i++)
        {
            Instantiate(dotSample, calculatePosition(currentPos, rad, rectCoords), circle.transform.rotation, circle.transform);
        }
        rectCorner = Instantiate(dotSample, rectCoords[0], rect.transform.rotation, rect.transform);
        rectCorner.transform.localScale = Vector3.one / 20;
        rectCorner = Instantiate(dotSample, rectCoords[1], rect.transform.rotation, rect.transform);
        rectCorner.transform.localScale = Vector3.one / 10;
        timer = Time.time;
        change = false;
    }

    void Update()
    {
        if (currentPos != circle.transform.position || rad != circle.transform.lossyScale.x / 2 || rectPos != rect.transform.position || rectScale != rect.transform.lossyScale)
        {
            change = true;
        }
        if (change && Time.time - timer > 1)
        {
            currentPos = circle.transform.position;
            rad = circle.transform.localScale.x / 2;
            rectPos = rect.transform.position;
            rectScale = rect.transform.lossyScale;
            rectCoords = new Vector3[2] {
                rectPos + new Vector3(rectScale.x / 2, 0, rectScale.y / 2),
                rectPos + new Vector3(rectScale.x / -2, 0, rectScale.y / -2),
            };
            for (int i = 0; i < circle.transform.childCount; i++)
            {
                Destroy(circle.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < n; i++)
            {
                Instantiate(dotSample, calculatePosition(currentPos, rad, rectCoords), circle.transform.rotation, circle.transform);
            }
            timer = Time.time;
            change = false;
        }
    }

    private Vector3 calculatePosition(Vector3 center, float radius, Vector3[] zoneCorners)
    {
        float distance;
        float angle;
        float firstAngle;
        float lastAngle;

        if (zoneCorners.Length < 2)
        {
            //Debug.Log("zone incorrect: length");
            return center;
        }
        if (zoneCorners[0].Equals(new Vector3()) || zoneCorners[1].Equals(new Vector3()))
        {
            //Debug.Log("zone incorrect: values");
            return center;
        }
        if (center.x < zoneCorners[0].x && center.x > zoneCorners[1].x && center.z < zoneCorners[0].z && center.z > zoneCorners[1].z)
        {
            //Debug.Log("in zone");
            angle = Random.Range(0, Mathf.PI * 2);
            if (center.x + radius < zoneCorners[0].x && center.x - radius > zoneCorners[1].x && center.z + radius < zoneCorners[0].z && center.z - radius > zoneCorners[1].z)
            {
                //Debug.Log("circle in zone");
                distance = Mathf.Sqrt(Random.Range(0, radius * radius));
            }
            else
            {
                //Debug.Log("circle not in zone");
                Vector3[] points = calculateCrossing(center, radius, angle, zoneCorners);
                //Debug.DrawLine(center, points[0], Color.green, 2);
                if (points[0].Equals(new Vector3()))
                {
                    //Debug.Log("invalid crossing");
                    distance = 0;
                }
                else
                {
                    //Debug.Log("crossing ok");
                    distance = Mathf.Sqrt(Random.Range(0, Mathf.Min(radius * radius, Mathf.Pow(Vector3.Distance(center, points[0]), 2))));
                }
            }
        }
        else
        {
            //Debug.Log("out of zone");
            if (-(zoneCorners[0] - center).z == 0)
            {
                firstAngle = Mathf.PI / 2;
                if ((zoneCorners[0] - center).x < 0)
                {
                    firstAngle += Mathf.PI;
                }
            }
            else
            {
                firstAngle = Mathf.Atan((zoneCorners[0] - center).x / -(zoneCorners[0] - center).z);
                if (Mathf.Acos((zoneCorners[0] - center).x / -(zoneCorners[0] - center).z) > Mathf.PI / 2)
                {
                    firstAngle += Mathf.PI;
                }
            }

            if (-(zoneCorners[1] - center).z == 0)
            {
                lastAngle = Mathf.PI / 2;
                if ((zoneCorners[1] - center).x < 0)
                {
                    lastAngle += Mathf.PI;
                }
            }
            else
            {
                lastAngle = Mathf.Atan((zoneCorners[1] - center).x / -(zoneCorners[1] - center).z);
                if (Mathf.Acos((zoneCorners[1] - center).x / -(zoneCorners[1] - center).z) > Mathf.PI / 2)
                {
                    lastAngle += Mathf.PI;
                }
            }

            angle = Random.Range(Mathf.Min(firstAngle, lastAngle), Mathf.Max(firstAngle, lastAngle));
            Vector3[] points = calculateCrossing(center, radius, angle, zoneCorners);

            if (points[0].Equals(new Vector3()))
            {
                //Debug.Log("no crossing");
                distance = Mathf.Sqrt(Random.Range(0, radius * radius));
            }
            else if (points[1].Equals(new Vector3()))
            {
                //Debug.Log("one crossing");
                distance = Mathf.Sqrt(Random.Range(0, Mathf.Min(radius, Mathf.Pow(Vector3.Distance(center, points[0]), 2))));
            }
            else
            {
                //Debug.Log("two crossings");
                distance = Mathf.Sqrt(Random.Range(Mathf.Min(radius, Mathf.Pow(Vector3.Distance(center, points[0]), 2)), Mathf.Min(radius, Mathf.Pow(Vector3.Distance(center, points[0]), 2))));
            }
        }
        return center + (new Vector3(Mathf.Sin(angle), 0.01f, Mathf.Cos(angle))).normalized * distance;
    }

    private Vector3[] calculateCrossing(Vector3 center, float radius, float angle, Vector3[] corners)
    {
        bool firstOpposite;
        bool lastOpposite;
        Vector3 delta;
        float sin;
        float sin1;
        Vector3[] ans = new Vector3[] { new Vector3(), new Vector3() };
        corners = new Vector3[] { corners[0], new Vector3(corners[0].x, 0, corners[1].z), corners[1], new Vector3(corners[1].x, 0, corners[0].z) };
        foreach (var corner in corners)
        {
            //Debug.DrawLine(center, corner, Color.yellow, 5);
        }
        for (int i = 0; i < 4; i++)
        {
            firstOpposite = anglesOpposite(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius, corners[i] - center, corners[(i + 1) % 4] - center);
            lastOpposite = anglesOpposite(corners[i] - corners[(i + 1) % 4], center - corners[(i + 1) % 4], center + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius - corners[(i + 1) % 4]);
            //Debug.Log($"{firstOpposite}, {lastOpposite}");
            if (firstOpposite && lastOpposite) 
            { 
                //Debug.Log("opposite");
                firstOpposite = anglesOpposite(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius, corners[i] - center, corners[(i + 1) % 4] - center);
                lastOpposite = anglesOpposite(corners[i] - corners[(i + 1) % 4], center - corners[(i + 1) % 4], center + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius - corners[(i + 1) % 4]);
                ////Debug.DrawRay(corners[(i + 1) % 4], corners[i] - corners[(i + 1) % 4], Color.blue, 15);
                ////Debug.DrawRay(corners[(i + 1) % 4], center - corners[(i + 1) % 4], Color.cyan, 15);
                ////Debug.DrawRay(corners[(i + 1) % 4], center + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius - corners[(i + 1) % 4], Color.magenta, 15);
                //Debug.DrawRay(center, new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius, Color.blue, 2);
                //Debug.DrawRay(center, corners[i] - center, Color.cyan, 2);
                //Debug.DrawRay(center, corners[(i + 1) % 4] - center, Color.magenta, 2);
                delta = corners[(i + 1) % 4] - corners[i];
                sin1 = - relativeSin(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)), corners[i] - center);
                sin = Mathf.Min(Mathf.Abs(relativeSin(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius, corners[i] - corners[(i + 1) % 4])), Mathf.Abs(relativeSin(corners[i] - corners[(i + 1) % 4], new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius)));
                //Debug.Log($"sin1: {sin1}, sin2: {sin2}");
                Vector3 crossing = corners[i] + delta.normalized * (corners[i] - center).magnitude * sin1 / sin; //corners[i].z + (corners[(i + 1) % 4].z - corners[i].z) * Vector3.Distance(center, corners[i]) * relativeSin(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)), corners[i] - center) / (Vector3.Distance(center, corners[(i + 1) % 4]) * relativeSin(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)), corners[(i + 1) % 4] - center))
                if (ans[0].Equals(new Vector3()))
                {
                    ans[0] = crossing;
                }
                else
                {
                    ans[1] = crossing;
                }
            }
        }
        return ans;
    }

    private bool anglesOpposite(Vector3 axis, Vector3 direction1, Vector3 direction2)
    {
        Vector2 axis_sin_cos = calculateSinAndCos(axis);
        Vector2 dir1_sin_cos = calculateSinAndCos(direction1);
        Vector2 dir2_sin_cos = calculateSinAndCos(direction2);
        //Debug.Log($"{axis_sin_cos} :::: {dir1_sin_cos} :::: {dir2_sin_cos}");
        return ((dir1_sin_cos.x * axis_sin_cos.y - dir1_sin_cos.y * axis_sin_cos.x) * (dir2_sin_cos.x * axis_sin_cos.y - dir2_sin_cos.y * axis_sin_cos.x)) <= 0;
    }

    private float relativeSin(Vector3 from, Vector3 to)
    {
        Vector2 from_sin_cos = calculateSinAndCos(from);
        Vector2 to_sin_cos = calculateSinAndCos(to);
        return -(from_sin_cos.x * to_sin_cos.y - from_sin_cos.y * to_sin_cos.x);
    }

    private Vector2 calculateSinAndCos(Vector3 dir)
    {
        if (dir.x == 0)
        {
            return new Vector2(0, 1);
        }
        else if (dir.z == 0)
        {
            return new Vector2(1, 0);
        }
        else
        {
            float sin = Mathf.Sqrt(1 / (1 + 1 / Mathf.Pow(dir.x / dir.z, 2)));
            if (dir.x < 0)
            {
                sin = -sin;
            }
            float cos = Mathf.Sqrt(1 - sin * sin);
            if (sin < 0 ^ (dir.x / dir.z) < 0)
            {
                cos = -cos;
            }
            return new Vector2(sin, cos);
        }
    }
}