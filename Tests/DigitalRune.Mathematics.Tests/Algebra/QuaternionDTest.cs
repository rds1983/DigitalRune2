using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using NUnit.Framework;


namespace DigitalRune.Mathematics.Algebra.Tests
{
  [TestFixture]
  public class QuaternionDTest
  {
    [Test]
    public void Zero()
    {
      QuaternionD zero = QuaternionD.Zero;
      Assert.AreEqual(zero.W, 0.0);
      Assert.AreEqual(zero.X, 0.0);
      Assert.AreEqual(zero.Y, 0.0);
      Assert.AreEqual(zero.Z, 0.0);
    }


    [Test]
    public void Identity()
    {
      QuaternionD identity = QuaternionD.Identity;
      Assert.AreEqual(identity.W, 1.0);
      Assert.AreEqual(identity.X, 0.0);
      Assert.AreEqual(identity.Y, 0.0);
      Assert.AreEqual(identity.Z, 0.0);

      Vector3D v = new Vector3D(2.0, 2.0, 2.0);
      Vector3D rotated = identity.ToRotationMatrix33() * v;
      Assert.IsTrue(v == rotated);
    }


    [Test]
    public void Constructor()
    {
      QuaternionD q = new QuaternionD(1, 2, 3, 4);
      Assert.AreEqual(1.0, q.W);
      Assert.AreEqual(2.0, q.X);
      Assert.AreEqual(3.0, q.Y);
      Assert.AreEqual(4.0, q.Z);

      q = new QuaternionD(new double[] { 1, 2, 3, 4 });
      Assert.AreEqual(1.0, q.W);
      Assert.AreEqual(2.0, q.X);
      Assert.AreEqual(3.0, q.Y);
      Assert.AreEqual(4.0, q.Z);

      q = new QuaternionD(new List<double>(new double[] { 1, 2, 3, 4 }));
      Assert.AreEqual(1.0, q.W);
      Assert.AreEqual(2.0, q.X);
      Assert.AreEqual(3.0, q.Y);
      Assert.AreEqual(4.0, q.Z);

      // From matrix
      q = QuaternionD.CreateRotation(Matrix33D.Identity);
      Assert.AreEqual(QuaternionD.Identity, q);

      q = new QuaternionD(0.123, new Vector3D(1.0, 2.0, 3.0));
      Assert.AreEqual(0.123, q.W);
      Assert.AreEqual(1.0, q.X);
      Assert.AreEqual(2.0, q.Y);
      Assert.AreEqual(3.0, q.Z);
    }


    [Test]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void ConstructorException1()
    {
      new QuaternionD(new double[] { });
    }


    [Test]
    [ExpectedException(typeof(NullReferenceException))]
    public void ConstructorException2()
    {
      new QuaternionD(null);
    }


    [Test]
    public void QuaternionDFromMatrix33()
    {
      Vector3D v = Vector3D.One;
      Matrix33D m = Matrix33D.Identity;
      QuaternionD q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.CreateRotation(Vector3D.UnitX, 0.3);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.CreateRotation(Vector3D.UnitY, 1.0);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.CreateRotation(Vector3D.UnitZ, 4.0);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.Identity;
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.CreateRotation(-Vector3D.UnitX, 1.3);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.CreateRotation(-Vector3D.UnitY, -1.4);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33D.CreateRotation(-Vector3D.UnitZ, -0.1);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = new Matrix33D(0, 0, 1,
                        0, -1, 0,
                        1, 0, 0);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = new Matrix33D(-1, 0, 0,
                         0, 1, 0,
                         0, 0, -1);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(m * v, q.Rotate(v)));
    }


    [Test]
    public void Properties()
    {
      QuaternionD q = new QuaternionD(0.123, 1.0, 2.0, 3.0);
      Assert.AreEqual(0.123, q.W);
      Assert.AreEqual(1.0, q.X);
      Assert.AreEqual(2.0, q.Y);
      Assert.AreEqual(3.0, q.Z);

      q.W = 1.0;
      q.X = 2.0;
      q.Y = 3.0;
      q.Z = 4.0;
      Assert.AreEqual(1.0, q.W);
      Assert.AreEqual(2.0, q.X);
      Assert.AreEqual(3.0, q.Y);
      Assert.AreEqual(4.0, q.Z);

      q.V = new Vector3D(-1.0, -2.0, -3.0);
      Assert.AreEqual(-1.0, q.X);
      Assert.AreEqual(-2.0, q.Y);
      Assert.AreEqual(-3.0, q.Z);
      Assert.AreEqual(new Vector3D(-1.0, -2.0, -3.0), q.V);
    }


    [Test]
    public void Angle()
    {
      Vector3D axis = new Vector3D(1.0, 2.0, 3.0);
      QuaternionD q = QuaternionD.CreateRotation(axis, 0.4);
      Assert.IsTrue(Numeric.AreEqual(0.4, q.Angle));
      q.Angle = 0.9;
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q, QuaternionD.CreateRotation(axis, 0.9)));

      Assert.AreEqual(0, new QuaternionD(1.00000001f, 0, 0, 0).Angle);
    }


    [Test]
    public void Axis()
    {
      Vector3D axis = new Vector3D(1.0, 2.0, 3.0);
      double angle = 0.2;

      QuaternionD q = QuaternionD.CreateRotation(axis, angle);
      Assert.IsTrue(Numeric.AreEqual(angle, q.Angle));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(axis.Normalized, q.Axis));
      axis = new Vector3D(1.0, 1.0, 1.0);
      q.Axis = axis;
      Assert.IsTrue(Numeric.AreEqual(angle, q.Angle));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(axis.Normalized, q.Axis));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(Matrix33D.CreateRotation(axis, angle) * Vector3D.One, q.Rotate(Vector3D.One)));

      Assert.AreEqual(Vector3D.Zero, QuaternionD.Identity.Axis);
      q.Axis = Vector3D.Zero;
      Assert.AreEqual(QuaternionD.Identity, q);
    }


    [Test]
    public void Equality()
    {
      QuaternionD q1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD copyOfQ1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q2 = new QuaternionD(-1.0, 2.0, 3.0, 4.0);
      QuaternionD q3 = new QuaternionD(1.0, -2.0, 3.0, 4.0);
      QuaternionD q4 = new QuaternionD(1.0, 2.0, -3.0, 4.0);
      QuaternionD q5 = new QuaternionD(1.0, 2.0, 3.0, -4.0);
      Assert.IsTrue(q1 == copyOfQ1);
      Assert.IsFalse(q1 == q2);
      Assert.IsFalse(q1 == q3);
      Assert.IsFalse(q1 == q4);
      Assert.IsFalse(q1 == q5);

      Assert.IsFalse(q1 != copyOfQ1);
      Assert.IsTrue(q1 != q2);
      Assert.IsTrue(q1 != q3);
      Assert.IsTrue(q1 != q4);
      Assert.IsTrue(q1 != q5);
    }


    [Test]
    public void AreEqual()
    {
      double originalEpsilon = Numeric.EpsilonD;
      Numeric.EpsilonD = 1e-8;

      QuaternionD q1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q2 = new QuaternionD(1.000001, 2.000001, 3.000001, 4.000001);
      QuaternionD q3 = new QuaternionD(1.00000001, 2.00000001, 3.00000001, 4.00000001);

      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q1, q1));
      Assert.IsFalse(QuaternionD.AreNumericallyEqual(q1, q2));
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q1, q3));

      Numeric.EpsilonD = originalEpsilon;
    }


    [Test]
    public void AreEqualWithEpsilon()
    {
      double epsilon = 0.001;
      QuaternionD q1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q2 = new QuaternionD(1.002, 2.002, 3.002, 4.002);
      QuaternionD q3 = new QuaternionD(1.0001, 2.0001, 3.0001, 4.0001);

      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q1, q1, epsilon));
      Assert.IsFalse(QuaternionD.AreNumericallyEqual(q1, q2, epsilon));
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q1, q3, epsilon));
    }


    [Test]
    public void TestEquals()
    {
      QuaternionD q1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q2 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q3 = new QuaternionD(1.0, 2.0, 3.0, 0.0);
      Assert.IsTrue(q1.Equals(q2));
      Assert.IsFalse(q1.Equals(q3));
    }


    [Test]
    public void TestEquals2()
    {
      QuaternionD q = QuaternionD.Identity;
      Assert.IsFalse(q.Equals(q.ToString()));
    }


    [Test]
    public void HashCode()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      Assert.AreNotEqual(QuaternionD.Zero.GetHashCode(), q.GetHashCode());
    }


    [Test]
    public void TestToString()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      string s = q.ToString();
      Assert.IsNotNull(s);
      Assert.Greater(s.Length, 0);
    }


    [Test]
    public void Length()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      double length = (double)Math.Sqrt(1 + 4 + 9 + 16);
      Assert.AreEqual(length, q.Modulus);
    }


    [Test]
    public void LengthSquared()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      double lengthSquared = 1 + 4 + 9 + 16;
      Assert.AreEqual(lengthSquared, q.Norm);
    }


    [Test]
    public void Conjugated()
    {
      QuaternionD q = new QuaternionD(1, 2, 3, 4);
      QuaternionD conjugate = q.Conjugated;
      Assert.AreEqual(1.0, conjugate.W);
      Assert.AreEqual(-2.0, conjugate.X);
      Assert.AreEqual(-3.0, conjugate.Y);
      Assert.AreEqual(-4.0, conjugate.Z);
    }


    [Test]
    public void Conjugate()
    {
      QuaternionD q = new QuaternionD(1, 2, 3, 4);
      q.Conjugate();
      Assert.AreEqual(1.0, q.W);
      Assert.AreEqual(-2.0, q.X);
      Assert.AreEqual(-3.0, q.Y);
      Assert.AreEqual(-4.0, q.Z);
    }


    [Test]
    public void Inverse()
    {
      QuaternionD identity = QuaternionD.Identity;
      QuaternionD inverseIdentity = identity.Inverse;
      Assert.AreEqual(inverseIdentity, identity);

      double angle = 0.4;
      Vector3D axis = new Vector3D(1.0, 1.0, 1.0);
      axis.Normalize();
      QuaternionD q = QuaternionD.CreateRotation(axis, angle);
      QuaternionD inverse = q.Inverse;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(-axis, inverse.Axis));

      q = new QuaternionD(1, 2, 3, 4);
      inverse = q.Inverse;
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(QuaternionD.Identity, inverse * q));
    }


    [Test]
    public void Invert()
    {
      QuaternionD inverseIdentity = QuaternionD.Identity;
      inverseIdentity.Invert();
      Assert.AreEqual(QuaternionD.Identity, inverseIdentity);

      double angle = 0.4;
      Vector3D axis = new Vector3D(1.0, 1.0, 1.0);
      axis.Normalize();
      QuaternionD q = QuaternionD.CreateRotation(axis, angle);
      q.Invert();
      Assert.IsTrue(Vector3D.AreNumericallyEqual(-axis, q.Axis));

      q = new QuaternionD(1, 2, 3, 4);
      QuaternionD inverse = q;
      inverse.Invert();
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(QuaternionD.Identity, inverse * q));
    }


    [Test]
    [ExpectedException(typeof(MathematicsException))]
    public void InvertException()
    {
      QuaternionD inverseOfZero = QuaternionD.Zero;
      inverseOfZero.Invert();
    }


    [Test]
    public void IndexerRead()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      Assert.AreEqual(1.0, q[0]);
      Assert.AreEqual(2.0, q[1]);
      Assert.AreEqual(3.0, q[2]);
      Assert.AreEqual(4.0, q[3]);
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerReadException()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      double d = q[-1];
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerReadException2()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      double d = q[4];
    }


    [Test]
    public void IndexerWrite()
    {
      QuaternionD q = QuaternionD.Zero;
      q[0] = 1.0;
      q[1] = 2.0;
      q[2] = 3.0;
      q[3] = 4.0;
      Assert.AreEqual(1.0, q.W);
      Assert.AreEqual(2.0, q.X);
      Assert.AreEqual(3.0, q.Y);
      Assert.AreEqual(4.0, q.Z);
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerWriteException()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      q[-1] = 0.0;
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerWriteException2()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      q[4] = 0.0;
    }


    [Test]
    public void Normalize()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      Assert.AreNotEqual(1.0, q.Modulus);
      Assert.IsFalse(q.IsNumericallyNormalized);
      q.Normalize();
      Assert.IsTrue(Numeric.AreEqual(1.0, q.Modulus));
      Assert.IsTrue(q.IsNumericallyNormalized);
    }


    [Test]
    [ExpectedException(typeof(DivideByZeroException))]
    public void NormalizeException()
    {
      QuaternionD q = QuaternionD.Zero;
      q.Normalize();
    }


    [Test]
    public void Normalized()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      Assert.AreNotEqual(1.0, q.Modulus);
      Assert.IsFalse(q.IsNumericallyNormalized);
      QuaternionD normalized = q.Normalized;
      Assert.AreEqual(new QuaternionD(1.0, 2.0, 3.0, 4.0), q);
      Assert.IsTrue(Numeric.AreEqual(1.0, normalized.Modulus));
      Assert.IsTrue(normalized.IsNumericallyNormalized);
    }


    [Test]
    [ExpectedException(typeof(DivideByZeroException))]
    public void NormalizedException()
    {
      QuaternionD q = QuaternionD.Zero;
      q = q.Normalized;
    }


    [Test]
    public void TryNormalize()
    {
      QuaternionD q = QuaternionD.Zero;
      bool normalized = q.TryNormalize();
      Assert.IsFalse(normalized);

      q = new QuaternionD(1, 2, 3, 4);
      normalized = q.TryNormalize();
      Assert.IsTrue(normalized);
      Assert.AreEqual(new QuaternionD(1, 2, 3, 4).Normalized, q);

      q = new QuaternionD(0, -1, 0, 0);
      normalized = q.TryNormalize();
      Assert.IsTrue(normalized);
      Assert.AreEqual(new QuaternionD(0, -1, 0, 0).Normalized, q);
    }


    [Test]
    public void ExplicitDoubleCast()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      double[] values = (double[])q;
      Assert.AreEqual(4, values.Length);
      Assert.AreEqual(1.0, values[0]);
      Assert.AreEqual(2.0, values[1]);
      Assert.AreEqual(3.0, values[2]);
      Assert.AreEqual(4.0, values[3]);
    }


    [Test]
    public void ExplicitDoubleCast2()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      double[] values = q.ToArray();
      Assert.AreEqual(4, values.Length);
      Assert.AreEqual(1.0, values[0]);
      Assert.AreEqual(2.0, values[1]);
      Assert.AreEqual(3.0, values[2]);
      Assert.AreEqual(4.0, values[3]);
    }


    [Test]
    public void ExplicitListCast()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      List<double> values = (List<double>)q;
      Assert.AreEqual(4, values.Count);
      Assert.AreEqual(1.0, values[0]);
      Assert.AreEqual(2.0, values[1]);
      Assert.AreEqual(3.0, values[2]);
      Assert.AreEqual(4.0, values[3]);
    }


    [Test]
    public void ExplicitListCast2()
    {
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      List<double> values = q.ToList();
      Assert.AreEqual(4, values.Count);
      Assert.AreEqual(1.0, values[0]);
      Assert.AreEqual(2.0, values[1]);
      Assert.AreEqual(3.0, values[2]);
      Assert.AreEqual(4.0, values[3]);
    }


    [Test]
    public void ExplicitCastToQuaternionF()
    {
      double x = 23.4;
      double y = -11.0;
      double z = 0.0;
      double w = 0.3;
      double[] elementsD = new[] { w, x, y, z };
      float[] elementsF = new[] { (float)w, (float)x, (float)y, (float)z };
      QuaternionD vectorD = new QuaternionD(elementsD);
      QuaternionF vectorF = (QuaternionF)vectorD;
      Assert.AreEqual(new QuaternionF(elementsF), vectorF);
    }


    [Test]
    public void ToVector4F()
    {
      double x = 23.4;
      double y = -11.0;
      double z = 0.0;
      double w = 0.3;
      double[] elementsD = new[] { w, x, y, z };
      float[] elementsF = new[] { (float)w, (float)x, (float)y, (float)z };
      QuaternionD vectorD = new QuaternionD(elementsD);
      QuaternionF vectorF = vectorD.ToQuaternionF();
      Assert.AreEqual(new QuaternionF(elementsF), vectorF);
    }


    [Test]
    public void ExplicitFromXnaCast()
    {
      Quaternion xna = new Quaternion(6, 7, 8, 9);
      QuaternionD v = (QuaternionD)xna;

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void FromXna()
    {
      Quaternion xna = new Quaternion(6, 7, 8, 9);
      QuaternionD v = QuaternionD.FromXna(xna);

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void ExplicitToXnaCast()
    {
      QuaternionD v = new QuaternionD(6, 7, 8, 9);
      Quaternion xna = (Quaternion)v;

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void ToXna()
    {
      QuaternionD v = new QuaternionD(6, 7, 8, 9);
      Quaternion xna = v.ToXna();

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void DotProduct()
    {
      QuaternionD q1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q2 = new QuaternionD(5.0, 6.0, 7.0, 8.0);
      double dotProduct = QuaternionD.Dot(q1, q2);
      Assert.AreEqual(70, dotProduct);
    }


    [Test]
    public void Rotate()
    {
      double angle = 0.4;
      Vector3D axis = new Vector3D(1.0, 2.0, 3.0);
      QuaternionD q = QuaternionD.CreateRotation(axis, angle);
      Matrix33D m33 = Matrix33D.CreateRotation(axis, angle);
      Vector3D v = new Vector3D(0.3, -2.4, 5.6);
      Vector3D result1 = q.Rotate(v);
      Vector3D result2 = m33 * v;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void RotationMatrix33()
    {
      double angle = -1.6;
      Vector3D axis = new Vector3D(1.0, 2.0, -3.0);
      QuaternionD q = QuaternionD.CreateRotation(axis, angle);
      Matrix33D m33 = Matrix33D.CreateRotation(axis, angle);
      Vector3D v = new Vector3D(0.3, -2.4, 5.6);
      Vector3D result1 = q.ToRotationMatrix33() * v;
      Vector3D result2 = m33 * v;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void RotationMatrix44()
    {
      double angle = -1.6;
      Vector3D axis = new Vector3D(1.0, 2.0, -3.0);
      QuaternionD q = QuaternionD.CreateRotation(axis, angle);
      Matrix44D m44 = Matrix44D.CreateRotation(axis, angle);
      Assert.IsTrue(Matrix44D.AreNumericallyEqual(q.ToRotationMatrix44(), m44));
    }


    [Test]
    public void MultiplyScalarOperator()
    {
      double s = 123.456;
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD expectedResult = new QuaternionD(s * 1.0, s * 2.0, s * 3.0, s * 4.0);
      QuaternionD result1 = s * q;
      QuaternionD result2 = q * s;
      Assert.AreEqual(expectedResult, result1);
      Assert.AreEqual(expectedResult, result2);
    }


    [Test]
    public void MultiplyScalar()
    {
      double s = 123.456;
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD expectedResult = new QuaternionD(s * 1.0, s * 2.0, s * 3.0, s * 4.0);
      QuaternionD result = QuaternionD.Multiply(s, q);
      Assert.AreEqual(expectedResult, result);
    }


    [Test]
    public void MultiplyOperator()
    {
      double angle1 = 0.4;
      Vector3D axis1 = new Vector3D(1.0, 2.0, 3.0);
      QuaternionD q1 = QuaternionD.CreateRotation(axis1, angle1);
      Matrix33D m1 = Matrix33D.CreateRotation(axis1, angle1);

      double angle2 = -1.6;
      Vector3D axis2 = new Vector3D(1.0, -2.0, -3.5);
      QuaternionD q2 = QuaternionD.CreateRotation(axis2, angle2);
      Matrix33D m2 = Matrix33D.CreateRotation(axis2, angle2);

      Vector3D v = new Vector3D(0.3, -2.4, 5.6);
      Vector3D result1 = (q2 * q1).Rotate(v);
      Vector3D result2 = m2 * m1 * v;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void Multiply()
    {
      double angle1 = 0.4;
      Vector3D axis1 = new Vector3D(1.0, 2.0, 3.0);
      QuaternionD q1 = QuaternionD.CreateRotation(axis1, angle1);
      Matrix33D m1 = Matrix33D.CreateRotation(axis1, angle1);

      double angle2 = -1.6;
      Vector3D axis2 = new Vector3D(1.0, -2.0, -3.5);
      QuaternionD q2 = QuaternionD.CreateRotation(axis2, angle2);
      Matrix33D m2 = Matrix33D.CreateRotation(axis2, angle2);

      Vector3D v = new Vector3D(0.3, -2.4, 5.6);
      Vector3D result1 = QuaternionD.Multiply(q2, q1).Rotate(v);
      Vector3D result2 = m2 * m1 * v;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void DivisionScalarOperator()
    {
      double s = 123.456;
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD expectedResult = new QuaternionD(1.0 / s, 2.0 / s, 3.0 / s, 4.0 / s);
      QuaternionD result = q / s;
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(expectedResult, result));
    }


    [Test]
    public void DivisionScalar()
    {
      double s = 123.456;
      QuaternionD q = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD expectedResult = new QuaternionD(1.0 / s, 2.0 / s, 3.0 / s, 4.0 / s);
      QuaternionD result = QuaternionD.Divide(q, s);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(expectedResult, result));
    }


    [Test]
    public void DivisionOperator()
    {
      double angle1 = 0.4;
      Vector3D axis1 = new Vector3D(1.0, 2.0, 3.0);
      QuaternionD q1 = QuaternionD.CreateRotation(axis1, angle1);
      Matrix33D m1 = Matrix33D.CreateRotation(axis1, angle1);

      double angle2 = -1.6;
      Vector3D axis2 = new Vector3D(1.0, -2.0, -3.5);
      QuaternionD q2 = QuaternionD.CreateRotation(axis2, angle2);
      Matrix33D m2 = Matrix33D.CreateRotation(axis2, angle2);

      Vector3D v = new Vector3D(0.3, -2.4, 5.6);
      Vector3D result1 = (q2 / q1).Rotate(v);
      Vector3D result2 = m2 * m1.Inverse * v;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void Division()
    {
      double angle1 = 0.4;
      Vector3D axis1 = new Vector3D(1.0, 2.0, 3.0);
      QuaternionD q1 = QuaternionD.CreateRotation(axis1, angle1);
      Matrix33D m1 = Matrix33D.CreateRotation(axis1, angle1);

      double angle2 = -1.6;
      Vector3D axis2 = new Vector3D(1.0, -2.0, -3.5);
      QuaternionD q2 = QuaternionD.CreateRotation(axis2, angle2);
      Matrix33D m2 = Matrix33D.CreateRotation(axis2, angle2);

      Vector3D v = new Vector3D(0.3, -2.4, 5.6);
      Vector3D result1 = QuaternionD.Divide(q2, q1).Rotate(v);
      Vector3D result2 = m2 * m1.Inverse * v;
      Assert.IsTrue(Vector3D.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void CreateRotation()
    {
      QuaternionD q;

      // From matrix vs. from angle/axis
      Matrix33D m = Matrix33D.CreateRotation(Vector3D.UnitX, (double)Math.PI / 4);
      q = QuaternionD.CreateRotation(m);
      QuaternionD q2 = QuaternionD.CreateRotation(Vector3D.UnitX, (double)Math.PI / 4);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q2, q));
      m = Matrix33D.CreateRotation(Vector3D.UnitY, (double)Math.PI / 4);
      q = QuaternionD.CreateRotation(m);
      q2 = QuaternionD.CreateRotation(Vector3D.UnitY, (double)Math.PI / 4);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q2, q));
      m = Matrix33D.CreateRotation(Vector3D.UnitZ, (double)Math.PI / 4);
      q = QuaternionD.CreateRotation(m);
      q2 = QuaternionD.CreateRotation(Vector3D.UnitZ, (double)Math.PI / 4);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q2, q));

      // From vector-vector
      Vector3D start, end;
      start = Vector3D.UnitX;
      end = Vector3D.UnitY;
      q = QuaternionD.CreateRotation(start, end);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = Vector3D.UnitY;
      end = Vector3D.UnitZ;
      q = QuaternionD.CreateRotation(start, end);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = Vector3D.UnitZ;
      end = Vector3D.UnitX;
      q = QuaternionD.CreateRotation(start, end);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = new Vector3D(1, 1, 1);
      end = new Vector3D(1, 1, 1);
      q = QuaternionD.CreateRotation(start, end);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = new Vector3D(1.0, 1.0, 1.0);
      end = new Vector3D(-1.0, -1.0, -1.0);
      q = QuaternionD.CreateRotation(start, end);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = new Vector3D(-1.0, 2.0, 1.0);
      end = new Vector3D(-2.0, -1.0, -1.0);
      q = QuaternionD.CreateRotation(start, end);
      Assert.IsTrue(Vector3D.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      double degree45 = MathHelper.ToRadians(45);
      q = QuaternionD.CreateRotation(degree45, Vector3D.UnitZ, degree45, Vector3D.UnitY, degree45, Vector3D.UnitX, false);
      QuaternionD expected = QuaternionD.CreateRotation(Vector3D.UnitZ, degree45) * QuaternionD.CreateRotation(Vector3D.UnitY, degree45)
                             * QuaternionD.CreateRotation(Vector3D.UnitX, degree45);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(expected, q));

      q = QuaternionD.CreateRotation(degree45, Vector3D.UnitZ, degree45, Vector3D.UnitY, degree45, Vector3D.UnitX, true);
      expected = QuaternionD.CreateRotation(Vector3D.UnitX, degree45) * QuaternionD.CreateRotation(Vector3D.UnitY, degree45)
                 * QuaternionD.CreateRotation(Vector3D.UnitZ, degree45);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(expected, q));
    }


    [Test]
    public void FromMatrixWithZeroTrace()
    {
      QuaternionD q;
      Matrix33D m = new Matrix33D(0, 1, 0,
                                  0, 0, 1,
                                  1, 0, 0);
      q = QuaternionD.CreateRotation(m);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(new QuaternionD(-0.5, 0.5, 0.5, 0.5), q));
    }


    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateRotationException1()
    {
      QuaternionD.CreateRotation(Vector3D.Zero, Vector3D.UnitX);
    }


    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateRotationException2()
    {
      QuaternionD.CreateRotation(Vector3D.UnitY, Vector3D.Zero);
    }


    [Test]
    public void CreateRotationX()
    {
      double angle = 0.3;
      QuaternionD q = QuaternionD.CreateRotation(Vector3D.UnitX, angle);
      QuaternionD qx = QuaternionD.CreateRotationX(angle);
      Assert.AreEqual(q, qx);
    }


    [Test]
    public void CreateRotationY()
    {
      double angle = 0.3;
      QuaternionD q = QuaternionD.CreateRotation(Vector3D.UnitY, angle);
      QuaternionD qy = QuaternionD.CreateRotationY(angle);
      Assert.AreEqual(q, qy);
    }


    [Test]
    public void CreateRotationZ()
    {
      double angle = 0.3;
      QuaternionD q = QuaternionD.CreateRotation(Vector3D.UnitZ, angle);
      QuaternionD qz = QuaternionD.CreateRotationZ(angle);
      Assert.AreEqual(q, qz);
    }


    [Test]
    public void Exp()
    {
      double θ = -0.3;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();

      QuaternionD q = new QuaternionD(0.0, θ * v);
      QuaternionD exp = QuaternionD.Exp(q);
      Assert.IsTrue(Numeric.AreEqual((double)Math.Cos(θ), exp.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual((double)Math.Sin(θ) * v, exp.V));
    }


    [Test]
    public void Exp2()
    {
      double θ = -0.3;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();

      QuaternionD q = new QuaternionD(0.0, θ * v);
      q.Exp();
      Assert.IsTrue(Numeric.AreEqual((double)Math.Cos(θ), q.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual((double)Math.Sin(θ) * v, q.V));
    }


    [Test]
    public void Exp3()
    {
      double θ = 0.0;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();

      QuaternionD q = new QuaternionD(0.0, θ * v);
      QuaternionD exp = QuaternionD.Exp(q);
      Assert.IsTrue(Numeric.AreEqual(1, exp.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(Vector3D.Zero, exp.V));
    }


    [Test]
    public void Exp4()
    {
      double θ = 0.0;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();

      QuaternionD q = new QuaternionD(0.0, θ * v);
      q.Exp();
      Assert.IsTrue(Numeric.AreEqual(1, q.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(Vector3D.Zero, q.V));
    }


    [Test]
    public void Ln()
    {
      double θ = 0.3;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();
      QuaternionD q = new QuaternionD((double)Math.Cos(θ), (double)Math.Sin(θ) * v);

      QuaternionD ln = QuaternionD.Ln(q);
      Assert.IsTrue(Numeric.AreEqual(0.0, ln.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(θ * v, ln.V));
    }


    [Test]
    public void Ln2()
    {
      double θ = 0.0;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();
      QuaternionD q = new QuaternionD((double)Math.Cos(θ), (double)Math.Sin(θ) * v);

      QuaternionD ln = QuaternionD.Ln(q);
      Assert.IsTrue(Numeric.AreEqual(0.0, ln.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(θ * v, ln.V));
    }


    [Test]
    public void Ln3()
    {
      double θ = 0.3;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();
      QuaternionD q = new QuaternionD((double)Math.Cos(θ), (double)Math.Sin(θ) * v);

      q.Ln();
      Assert.IsTrue(Numeric.AreEqual(0.0, q.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(θ * v, q.V));
    }


    [Test]
    public void Ln4()
    {
      double θ = 0.0;
      Vector3D v = new Vector3D(1.0, 2.0, 3.0);
      v.Normalize();
      QuaternionD q = new QuaternionD((double)Math.Cos(θ), (double)Math.Sin(θ) * v);

      q.Ln();
      Assert.IsTrue(Numeric.AreEqual(0.0, q.W));
      Assert.IsTrue(Vector3D.AreNumericallyEqual(θ * v, q.V));
    }


    [Test]
    [ExpectedException(typeof(MathematicsException))]
    public void LnException()
    {
      QuaternionD q = new QuaternionD(1.5, 0.0, 0.0, 0.0);
      QuaternionD.Ln(q);
    }


    [Test]
    public void AdditionOperator()
    {
      QuaternionD a = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD b = new QuaternionD(2.0, 3.0, 4.0, 5.0);
      QuaternionD c = a + b;
      Assert.AreEqual(new QuaternionD(3.0, 5.0, 7.0, 9.0), c);
    }


    [Test]
    public void Addition()
    {
      QuaternionD a = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD b = new QuaternionD(2.0, 3.0, 4.0, 5.0);
      QuaternionD c = QuaternionD.Add(a, b);
      Assert.AreEqual(new QuaternionD(3.0, 5.0, 7.0, 9.0), c);
    }


    [Test]
    public void SubtractionOperator()
    {
      QuaternionD a = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD b = new QuaternionD(10.0, -10.0, 0.5, 2.5);
      QuaternionD c = a - b;
      Assert.AreEqual(new QuaternionD(-9.0, 12.0, 2.5, 1.5), c);
    }


    [Test]
    public void Subtraction()
    {
      QuaternionD a = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD b = new QuaternionD(10.0, -10.0, 0.5, 2.5);
      QuaternionD c = QuaternionD.Subtract(a, b);
      Assert.AreEqual(new QuaternionD(-9.0, 12.0, 2.5, 1.5), c);
    }


    [Test]
    public void NegationOperator()
    {
      QuaternionD a = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      Assert.AreEqual(new QuaternionD(-1.0, -2.0, -3.0, -4.0), -a);
    }


    [Test]
    public void Negation()
    {
      QuaternionD a = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      Assert.AreEqual(new QuaternionD(-1.0, -2.0, -3.0, -4.0), QuaternionD.Negate(a));
    }


    [Test]
    public void Power()
    {
      const double θ = 0.4;
      const double t = -1.2;
      Vector3D v = new Vector3D(2.3, 1.0, -2.0);
      v.Normalize();

      QuaternionD q = new QuaternionD((double)Math.Cos(θ), (double)Math.Sin(θ) * v);
      QuaternionD power = QuaternionD.Power(q, t);
      QuaternionD expected = new QuaternionD((double)Math.Cos(t * θ), (double)Math.Sin(t * θ) * v);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(expected, power));
    }


    [Test]
    public void Power2()
    {
      const double θ = 0.4;
      const double t = -1.2;
      Vector3D v = new Vector3D(2.3, 1.0, -2.0);
      v.Normalize();

      QuaternionD q = new QuaternionD(Math.Cos(θ), Math.Sin(θ) * v);
      q.Power(t);
      QuaternionD expected = new QuaternionD(Math.Cos(t * θ), Math.Sin(t * θ) * v);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(expected, q));
    }


    [Test]
    public void Power3()
    {
      const double θ = 0.4;
      Vector3D v = new Vector3D(2.3, 1.0, -2.0);
      v.Normalize();

      QuaternionD q = new QuaternionD(Math.Cos(θ), Math.Sin(θ) * v);
      QuaternionD q2 = q;
      q2.Power(2);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q * q, q2));
      QuaternionD q3 = q;
      q3.Power(3);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q * q * q, q3));

      q2 = q;
      q2.Power(-2);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q.Inverse * q.Inverse, q2));

      q3 = q;
      q3.Power(-3);
      Assert.IsTrue(QuaternionD.AreNumericallyEqual(q.Inverse * q.Inverse * q.Inverse, q3));
    }


    [Test]
    public void GetAngleTest()
    {
      QuaternionD qIdentity = QuaternionD.Identity;
      QuaternionD q03 = QuaternionD.CreateRotation(Vector3D.UnitX, 0.3);
      QuaternionD q03Plus11 = QuaternionD.CreateRotation(new Vector3D(1, 0.2, -3), 1.1) * q03;
      QuaternionD q0 = QuaternionD.CreateRotation(Vector3D.UnitX, 0.0);
      QuaternionD qPi = QuaternionD.CreateRotation(Vector3D.UnitX, ConstantsD.Pi);
      QuaternionD q2Pi = QuaternionD.CreateRotation(Vector3D.UnitX, ConstantsD.TwoPi);

      Assert.IsTrue(Numeric.AreEqual(0.0, QuaternionD.GetAngle(qIdentity, qIdentity)));
      Assert.IsTrue(Numeric.AreEqual(0.3, QuaternionD.GetAngle(qIdentity, q03)));
      Assert.IsTrue(Numeric.AreEqual(0.3, QuaternionD.GetAngle(qIdentity, -q03))); // Remember: q and -q represent the same orientation.
      Assert.IsTrue(Numeric.AreEqual(1.1, QuaternionD.GetAngle(q03, q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(1.1, QuaternionD.GetAngle(-q03, q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(1.1, QuaternionD.GetAngle(q03, -q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(1.1, QuaternionD.GetAngle(-q03, -q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(0.0, QuaternionD.GetAngle(qIdentity, q0)));
      Assert.IsTrue(Numeric.AreEqual(0.0, QuaternionD.GetAngle(qIdentity, q2Pi)));
      Assert.IsTrue(Numeric.AreEqual(0.0, QuaternionD.GetAngle(q0, q2Pi)));
      Assert.IsTrue(Numeric.AreEqual(0.3, QuaternionD.GetAngle(q03, q0)));
      Assert.IsTrue(Numeric.AreEqual(ConstantsD.Pi, QuaternionD.GetAngle(q0, qPi)));
      Assert.IsTrue(Numeric.AreEqual(ConstantsD.Pi, QuaternionD.GetAngle(q2Pi, qPi)));
    }


    [Test]
    public void IsNaN()
    {
      const int numberOfRows = 4;
      Assert.IsFalse(new QuaternionD().IsNaN);

      for (int i = 0; i < numberOfRows; i++)
      {
        QuaternionD v = new QuaternionD();
        v[i] = double.NaN;
        Assert.IsTrue(v.IsNaN);
      }
    }


    [Test]
    public void SerializationXml()
    {
      QuaternionD q1 = new QuaternionD(1.0, 2.0, 3.0, 4.0);
      QuaternionD q2;
      string fileName = "SerializationQuaternionD.xml";

      if (File.Exists(fileName))
        File.Delete(fileName);

      XmlSerializer serializer = new XmlSerializer(typeof(QuaternionD));
      StreamWriter writer = new StreamWriter(fileName);
      serializer.Serialize(writer, q1);
      writer.Close();

      serializer = new XmlSerializer(typeof(QuaternionD));
      FileStream fileStream = new FileStream(fileName, FileMode.Open);
      q2 = (QuaternionD)serializer.Deserialize(fileStream);
      Assert.AreEqual(q1, q2);
    }



    [Test]
    public void SerializationXml2()
    {
      QuaternionD q1 = new QuaternionD(0.1, -0.2, 6, 40);
      QuaternionD q2;

      string fileName = "SerializationQuaternionD_DataContractSerializer.xml";

      if (File.Exists(fileName))
        File.Delete(fileName);

      var serializer = new DataContractSerializer(typeof(QuaternionD));
      using (var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
      using (var writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
        serializer.WriteObject(writer, q1);

      using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
      using (var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
        q2 = (QuaternionD)serializer.ReadObject(reader);

      Assert.AreEqual(q1, q2);
    }


    [Test]
    public void SerializationJson()
    {
      QuaternionD q1 = new QuaternionD(0.1, -0.2, 6, 40);
      QuaternionD q2;

      string fileName = "SerializationQuaternionD.json";

      if (File.Exists(fileName))
        File.Delete(fileName);

      var serializer = new DataContractJsonSerializer(typeof(QuaternionD));
      using (var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
        serializer.WriteObject(stream, q1);

      using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        q2 = (QuaternionD)serializer.ReadObject(stream);

      Assert.AreEqual(q1, q2);
    }


    [Test]
    public void Parse()
    {
      QuaternionD vector = QuaternionD.Parse("(0.0123; (9.876; 0.0; -2.3))", CultureInfo.InvariantCulture);
      Assert.AreEqual(0.0123, vector.W);
      Assert.AreEqual(9.876, vector.X);
      Assert.AreEqual(0.0, vector.Y);
      Assert.AreEqual(-2.3, vector.Z);

      vector = QuaternionD.Parse("(   0.0123   ;  ( 9;  0.1 ; -2.3 ) ) ", CultureInfo.InvariantCulture);
      Assert.AreEqual(0.0123, vector.W);
      Assert.AreEqual(9, vector.X);
      Assert.AreEqual(0.1, vector.Y);
      Assert.AreEqual(-2.3, vector.Z);
    }


    [Test]
    [ExpectedException(typeof(FormatException))]
    public void ParseException()
    {
      QuaternionD vector = QuaternionD.Parse("(0.0123; 9.876; 4.1; -9.0)");
    }


    [Test]
    public void ToStringAndParse()
    {
      QuaternionD quaternion = new QuaternionD(0.0123, 9.876, 0.0, -2.3);
      string s = quaternion.ToString();
      QuaternionD parsedQuaternion = QuaternionD.Parse(s);
      Assert.AreEqual(quaternion, parsedQuaternion);
    }
  }
}
