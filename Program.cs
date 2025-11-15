using System;
using MathNet.Spatial.Euclidean;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Providers.LinearAlgebra;

namespace SepAxisThm
{

    class Object3D
    {
        List<Vector3D> vertices;
        List<ValueTuple<int, int, int>> faces;
        List<Vector3D> normals;
        public Object3D()
        {
            vertices = new();
            faces = new();
            normals = new();
        }

        public int AddVertex(float x, float y, float z)
        {
            //var vertex = Vector3D.Build.DenseOfArray(new float[] { x, y, z });
            vertices.Add(new Vector3D(x, y, z));
            return vertices.Count;
        }

        public int MakeFace(int a, int b, int c)
        {
            faces.Add((a, b, c));
            normals.Add((vertices[a] - vertices[b]).CrossProduct(vertices[a] - vertices[c]).Normalize().ToVector3D());
            return faces.Count;
        }


        public static float ProjectionDistancePointToLine(Vector<float> point, Vector<float> direction)
        {
            return (float)(point.DotProduct(direction) / direction.L2Norm());
        }
    }

    class Object2D
    {
        public List<Vector2D> vertices;
        public List<ValueTuple<int, int>> lines;
        public List<Vector2D> linenormals;

        public Object2D()
        {
            vertices = new();
            lines = new();
            linenormals = new();
        }

        public int AddVertex(float x, float y)
        {
            vertices.Add(new Vector2D(x, y));
            return vertices.Count - 1;
        }

        public int MakeLine(int a, int b)
        {
            lines.Add((a, b));
            var dirvec = vertices[b] - vertices[a];
            //Console.WriteLine(dirvec);
            //dirvec.Orthogonal.Normalize();
            linenormals.Add(dirvec.Orthogonal.Normalize());
            return lines.Count - 1;
        }

        public bool IsConvex()
        {
            return true;
        }

        public List<Object2D> ToTriangleStrip()
        {
            return null;
        }
    }

    internal class Program
    {
        static int Main(string[] args)
        {
            // Object2D triangle1 = new Object2D();
            // triangle1.AddVertex(0, 3);
            // triangle1.AddVertex(1, 3);
            // triangle1.AddVertex(0, 2);
            // /*triangle1.AddVertex(1, 1);
            // triangle1.AddVertex(1, 2);
            // triangle1.AddVertex(4, 1);*/
            // triangle1.MakeLine(0, 1);
            // triangle1.MakeLine(1, 2);
            // triangle1.MakeLine(2, 0);
            // Object2D triangle2 = new Object2D();
            // /*triangle2.AddVertex(3, 1);
            // triangle2.AddVertex(3, 2);
            // triangle2.AddVertex(4, 1);*/
            // triangle2.AddVertex(3, 0);
            // triangle2.AddVertex(3, 1);
            // triangle2.AddVertex(2, 0);
            // triangle2.MakeLine(0, 1);
            // triangle2.MakeLine(1, 2);
            // triangle2.MakeLine(2, 0);

            Object2D triangle1 = new Object2D();
            triangle1.AddVertex(0, 0);
            triangle1.AddVertex(2, 0);
            triangle1.AddVertex(1, 2);
            triangle1.MakeLine(0, 1);
            triangle1.MakeLine(1, 2);
            triangle1.MakeLine(2, 0);

            Object2D triangle2 = new Object2D();
            triangle2.AddVertex(1, 1);   // inside the first triangle
            triangle2.AddVertex(3, 1);
            triangle2.AddVertex(2, 3);
            triangle2.MakeLine(0, 1);
            triangle2.MakeLine(1, 2);
            triangle2.MakeLine(2, 0);


            //Console.WriteLine($"{triangle1.linenormals[1]}");

            var allnormals = new List<Vector2D>();
            allnormals.AddRange(triangle1.linenormals);
            allnormals.AddRange(triangle2.linenormals);
            //deduplication if needed...

            float obj1_max, obj1_min, obj2_max, obj2_min;
            obj1_max = -999;
            obj1_min = 999;
            obj2_max = -999;
            obj2_min = 999;

            Vector2D sepaxisT = new(0, 0);
            float mindist = 999;

            foreach (var candline in allnormals)
            {
                obj1_max = -999;
                obj1_min = 999;
                obj2_max = -999;
                obj2_min = 999;
                System.Console.WriteLine($"Began check for {candline}");
                foreach (var v in triangle1.vertices)
                {
                    float d = (float)v.DotProduct(candline);
                    System.Console.WriteLine(d);
                    if (obj1_max < d) obj1_max = d;
                    if (d < obj1_min) obj1_min = d;
                }
                System.Console.WriteLine($"Max {obj1_max} Min {obj1_min}");
                foreach (var v in triangle2.vertices)
                {
                    float d = (float)v.DotProduct(candline);
                    System.Console.WriteLine(d);
                    if (obj2_max < d) obj2_max = d;
                    if (d < obj2_min) obj2_min = d;
                }
                System.Console.WriteLine($"Max {obj2_max} Min {obj2_min}");
                /*
                bool ovcheck = obj1_max < obj2_min || obj2_max < obj1_min;
                float ovdist = 0;
                if (obj2_max < obj1_max && obj1_min < obj2_min)
                {
                    ovdist = obj1_max - obj1_min;
                    Console.WriteLine($"Overlap(Fully Inside) on: {candline} Distance {obj1_max - obj1_min}");
                }
                else if (obj1_max < obj2_max && obj2_min < obj1_min)
                {
                    ovdist = obj2_max - obj2_min;
                    Console.WriteLine($"Overlap(Fully Inside) on: {candline} Distance {obj2_max - obj2_min}");
                }
                else if (!ovcheck && obj2_min < obj1_max)
                {
                    ovdist = obj1_max - obj2_min;
                    Console.WriteLine($"Overlap on: {candline} Distance {obj1_max - obj2_min}");
                }
                else if (!ovcheck && obj1_min < obj2_max)
                {
                    ovdist = obj2_max - obj1_min;
                    Console.WriteLine($"Overlap on: {candline} Distance {obj2_max - obj1_min}");
                }

                if (ovdist != 0 && ovdist < mindist)
                {
                    mindist = ovdist;
                    sepaxis = candline;
                }*/
                float overlap = Math.Min(obj1_max, obj2_max) - Math.Max(obj1_min, obj2_min); //GPT code - why does it work?
                if(overlap>0)
                {
                    System.Console.WriteLine($"Overlap on axis {candline}, Distance {overlap}");
                }
                if(overlap<mindist)
                {
                    mindist=overlap;
                    sepaxisT=candline;
                }
            }
            if (sepaxisT.Length > 0)
                System.Console.WriteLine($"Seperation Axis Normal {sepaxisT} with distance {mindist}");

            return 0;
        }
    }
}