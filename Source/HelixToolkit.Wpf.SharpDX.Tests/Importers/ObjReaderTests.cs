﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjReaderTests.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Test for pull request #52.
//   Fix ArgumentOutOfRangeException in MeshBuilder.ComputeNormals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using NUnit.Framework;

namespace HelixToolkit.Wpf.SharpDX.Tests.Importers
{
    [TestFixture]
    class ObjReaderTests
    {
        private ObjReader _objReader;

        [SetUp]
        public void SetUp()
        {
            _objReader = new ObjReader();
        }

        /// <summary>
        /// Test for pull request #52.
        /// Fix ArgumentOutOfRangeException in MeshBuilder.ComputeNormals.
        /// </summary>
        [Test]
        public void LoadModelWithoutNormals()
        {
            var reader = new ObjReader();
            var objects = reader.Read(@"Models\obj\cornell_box.obj");
            
            Assert.IsNotNull(objects);
            Assert.AreEqual(9, objects.Count);

            var floorGeometry = objects[0].Geometry as MeshGeometry3D;

            Assert.IsNotNull(floorGeometry);
            Assert.AreEqual(4, floorGeometry.Positions.Count);
            Assert.AreEqual(4, floorGeometry.Normals.Count);
        }

        [Test]
        public void CanParseFaceWithRelativeIndices() 
        {
            var model = _objReader.Read(@"Models\obj\face_relative_vertices.obj");

            Assert.AreEqual(1, model.Count);
            model[0].AssertHasVertices(new[] { -1d, 0d, 1d }, new[] { 1d, 0d, 1d }, new[] { -1d, 0d, -1d });
        }

        [Test]
        public void CanParseFaceWithAbsoluteNormals() 
        {
            var model = _objReader.Read(@"Models\obj\simple_triangle_with_normals.obj");

            Assert.AreEqual(1, model.Count);
            model[0].AssertHasNormals(new[] { 0d, 1d, 0d }, new[] { 0d, 1d, 0d }, new[] { 0d, 1d, 0d });
        }

        [Test]
        public void CanParseFaceWithRelativeNormals() 
        {
            var model = _objReader.Read(@"Models\obj\face_relative_vertex_normals.obj");

            Assert.AreEqual(1, model.Count);
            model[0].AssertHasNormals(new[] { 0d, 1d, 0d }, new[] { 0d, 1d, 0d }, new[] { 0d, 1d, 0d });
        }
    }

    public static class TestExtensions 
    {
        public static void AssertHasVertices(this Object3D model, params double[][] vertices) 
        {
            var geometry = (MeshGeometry3D)model.Geometry;
            Assert.AreEqual(vertices.Length, geometry.Positions.Count, "Expected to find {0} vertices in model", vertices.Length);
            foreach (var vertex in vertices)
                Assert.IsTrue(geometry.Positions.Contains(vertex), "Expected geometry to contain vertex [{0},{1},{2}]", vertex[0], vertex[1], vertex[2]);
        }

        public static void AssertHasNormals(this Object3D model, params double[][] normals) 
        {
            var geometry = (MeshGeometry3D)model.Geometry;
            Assert.AreEqual(normals.Length, geometry.Normals.Count, "Expected to find {0} normals in model", normals.Length);
            foreach (var normal in normals)
                Assert.IsTrue(geometry.Normals.Contains(normal), "Expected geometry to contain normal [{0},{1},{2}]", normal[0], normal[1], normal[2]);
        }

        public static bool Contains(this Core.Vector3Collection vectors, double[] expectedVertex) 
        {
            return vectors.Any(vector => Math.Abs((float)expectedVertex[0] - vector.X) < float.Epsilon &&
                                         Math.Abs((float)expectedVertex[1] - vector.Y) < float.Epsilon &&
                                         Math.Abs((float)expectedVertex[2] - vector.Z) < float.Epsilon);
        }
     }		     
}
