using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace SepAxisThm
{

    class Object3D
    {
        List<Vector<float>> vertices;
        List<Vector<float>> normals;
        public Object3D()
        {
            vertices = new();
        }

        public void AddVertex(float x, float y, float z)
        {
            var vertex = Vector<float>.Build.DenseOfArray(new float[] { x, y, z });
            vertices.Add(vertex);
        } 

        public static float ProjectionDistancePointToLine(Vector<float> point, Vector<float> direction)
        {
            return (float)(point.DotProduct(direction) / direction.L2Norm());
        }
    }

    internal class Program
    {
        static int Main(string[] args)
        {
            Object3D triangle1 = new Object3D();
            triangle1.AddVertex(1,1,0);
            triangle1.AddVertex(1,2,0);
            triangle1.AddVertex(2,1,0);
            Object3D triangle2 = new Object3D();
            triangle2.AddVertex(3,1,0);
            triangle2.AddVertex(3,2,0);
            triangle2.AddVertex(3,1,0);



            return 0;
        }
    }
}