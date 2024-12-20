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
  public class QuaternionFTest
  {
    [Test]
    public void Zero()
    {
      QuaternionF zero = QuaternionF.Zero;
      Assert.AreEqual(zero.W, 0.0f);
      Assert.AreEqual(zero.X, 0.0f);
      Assert.AreEqual(zero.Y, 0.0f);
      Assert.AreEqual(zero.Z, 0.0f);
    }


    [Test]
    public void Identity()
    {
      QuaternionF identity = QuaternionF.Identity;
      Assert.AreEqual(identity.W, 1.0f);
      Assert.AreEqual(identity.X, 0.0f);
      Assert.AreEqual(identity.Y, 0.0f);
      Assert.AreEqual(identity.Z, 0.0f);

      Vector3F v = new Vector3F(2.0f, 2.0f, 2.0f);
      Vector3F rotated = identity.ToRotationMatrix33() * v;
      Assert.IsTrue(v == rotated);
    }


    [Test]
    public void Constructor()
    {
      QuaternionF q = new QuaternionF(1, 2, 3, 4);
      Assert.AreEqual(1.0f, q.W);
      Assert.AreEqual(2.0f, q.X);
      Assert.AreEqual(3.0f, q.Y);
      Assert.AreEqual(4.0f, q.Z);

      q = new QuaternionF(new float[] { 1, 2, 3, 4 });
      Assert.AreEqual(1.0f, q.W);
      Assert.AreEqual(2.0f, q.X);
      Assert.AreEqual(3.0f, q.Y);
      Assert.AreEqual(4.0f, q.Z);

      q = new QuaternionF(new List<float>(new float[] { 1, 2, 3, 4 }));
      Assert.AreEqual(1.0f, q.W);
      Assert.AreEqual(2.0f, q.X);
      Assert.AreEqual(3.0f, q.Y);
      Assert.AreEqual(4.0f, q.Z);

      // From matrix
      q = QuaternionF.CreateRotation(Matrix33F.Identity);
      Assert.AreEqual(QuaternionF.Identity, q);

      q = new QuaternionF(0.123f, new Vector3F(1.0f, 2.0f, 3.0f));
      Assert.AreEqual(0.123f, q.W);
      Assert.AreEqual(1.0f, q.X);
      Assert.AreEqual(2.0f, q.Y);
      Assert.AreEqual(3.0f, q.Z);
    }


    [Test]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void ConstructorException1()
    {
      new QuaternionF(new float[] { });
    }


    [Test]
    [ExpectedException(typeof(NullReferenceException))]
    public void ConstructorException2()
    {
      new QuaternionF(null);
    }


    [Test]
    public void QuaternionFromMatrix33()
    {
      Vector3F v = Vector3F.One;
      Matrix33F m = Matrix33F.Identity;
      QuaternionF q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.CreateRotation(Vector3F.UnitX, 0.3f);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.CreateRotation(Vector3F.UnitY, 1.0f);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.CreateRotation(Vector3F.UnitZ, 4.0f);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.Identity;
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.CreateRotation(-Vector3F.UnitX, 1.3f);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.CreateRotation(-Vector3F.UnitY, -1.4f);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = Matrix33F.CreateRotation(-Vector3F.UnitZ, -0.1f);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = new Matrix33F(0,  0, 1,
                        0, -1, 0,
                        1,  0, 0);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));

      m = new Matrix33F(-1, 0,  0,
                         0, 1,  0,
                         0, 0, -1);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(m * v, q.Rotate(v)));
    }


    [Test]
    public void Properties()
    {
      QuaternionF q = new QuaternionF(0.123f, 1.0f, 2.0f, 3.0f);
      Assert.AreEqual(0.123f, q.W);
      Assert.AreEqual(1.0f, q.X);
      Assert.AreEqual(2.0f, q.Y);
      Assert.AreEqual(3.0f, q.Z);

      q.W = 1.0f;
      q.X = 2.0f;
      q.Y = 3.0f;
      q.Z = 4.0f;
      Assert.AreEqual(1.0f, q.W);
      Assert.AreEqual(2.0f, q.X);
      Assert.AreEqual(3.0f, q.Y);
      Assert.AreEqual(4.0f, q.Z);

      q.V = new Vector3F(-1.0f, -2.0f, -3.0f);
      Assert.AreEqual(-1.0f, q.X);
      Assert.AreEqual(-2.0f, q.Y);
      Assert.AreEqual(-3.0f, q.Z);
      Assert.AreEqual(new Vector3F(-1.0f, -2.0f, -3.0f), q.V);
    }


    [Test]
    public void Angle()
    {
      Vector3F axis = new Vector3F(1.0f, 2.0f, 3.0f);
      QuaternionF q = QuaternionF.CreateRotation(axis, 0.4f);
      Assert.IsTrue(Numeric.AreEqual(0.4f, q.Angle));
      q.Angle = 0.9f;
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q, QuaternionF.CreateRotation(axis, 0.9f)));

      Assert.AreEqual(0, new QuaternionF(1.000001f, 0, 0, 0).Angle);
    }


    [Test]
    public void Axis()
    {
      Vector3F axis = new Vector3F(1.0f, 2.0f, 3.0f);
      float angle = 0.2f;      

      QuaternionF q = QuaternionF.CreateRotation(axis, angle);
      Assert.IsTrue(Numeric.AreEqual(angle, q.Angle));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(axis.Normalized, q.Axis));
      axis = new Vector3F(1.0f, 1.0f, 1.0f);
      q.Axis = axis;
      Assert.IsTrue(Numeric.AreEqual(angle, q.Angle));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(axis.Normalized, q.Axis));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(Matrix33F.CreateRotation(axis, angle) * Vector3F.One, q.Rotate(Vector3F.One)));

      Assert.AreEqual(Vector3F.Zero, QuaternionF.Identity.Axis);
      q.Axis = Vector3F.Zero;
      Assert.AreEqual(QuaternionF.Identity, q);
    }


    [Test]
    public void Equality()
    {
      QuaternionF q1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF copyOfQ1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q2 = new QuaternionF(-1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q3 = new QuaternionF(1.0f, -2.0f, 3.0f, 4.0f);
      QuaternionF q4 = new QuaternionF(1.0f, 2.0f, -3.0f, 4.0f);
      QuaternionF q5 = new QuaternionF(1.0f, 2.0f, 3.0f, -4.0f);
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
      float originalEpsilon = Numeric.EpsilonF;
      Numeric.EpsilonF = 1e-8f;

      QuaternionF q1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q2 = new QuaternionF(1.000001f, 2.000001f, 3.000001f, 4.000001f);
      QuaternionF q3 = new QuaternionF(1.00000001f, 2.00000001f, 3.00000001f, 4.00000001f);

      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q1, q1));
      Assert.IsFalse(QuaternionF.AreNumericallyEqual(q1, q2));
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q1, q3));

      Numeric.EpsilonF = originalEpsilon;
    }


    [Test]
    public void AreEqualWithEpsilon()
    {
      float epsilon = 0.001f;
      QuaternionF q1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q2 = new QuaternionF(1.002f, 2.002f, 3.002f, 4.002f);
      QuaternionF q3 = new QuaternionF(1.0001f, 2.0001f, 3.0001f, 4.0001f);

      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q1, q1, epsilon));
      Assert.IsFalse(QuaternionF.AreNumericallyEqual(q1, q2, epsilon));
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q1, q3, epsilon));
    }


    [Test]
    public void TestEquals()
    {
      QuaternionF q1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q2 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q3 = new QuaternionF(1.0f, 2.0f, 3.0f, 0.0f);
      Assert.IsTrue(q1.Equals(q2));
      Assert.IsFalse(q1.Equals(q3));
    }


    [Test]
    public void TestEquals2()
    {
      QuaternionF q = QuaternionF.Identity;
      Assert.IsFalse(q.Equals(q.ToString()));
    }


    [Test]
    public void HashCode()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      Assert.AreNotEqual(QuaternionF.Zero.GetHashCode(), q.GetHashCode());
    }


    [Test]
    public void TestToString()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      string s = q.ToString();
      Assert.IsNotNull(s);
      Assert.Greater(s.Length, 0);
    }


    [Test]
    public void Length()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      float length = (float)Math.Sqrt(1 + 4 + 9 + 16);
      Assert.AreEqual(length, q.Modulus);
    }


    [Test]
    public void LengthSquared()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      float lengthSquared = 1 + 4 + 9 + 16;
      Assert.AreEqual(lengthSquared, q.Norm);
    }


    [Test]
    public void Conjugated()
    {
      QuaternionF q = new QuaternionF(1, 2, 3, 4);
      QuaternionF conjugate = q.Conjugated;
      Assert.AreEqual(1.0f, conjugate.W);
      Assert.AreEqual(-2.0f, conjugate.X);
      Assert.AreEqual(-3.0f, conjugate.Y);
      Assert.AreEqual(-4.0f, conjugate.Z);
    }


    [Test]
    public void Conjugate()
    {
      QuaternionF q = new QuaternionF(1, 2, 3, 4);
      q.Conjugate();
      Assert.AreEqual(1.0f, q.W);
      Assert.AreEqual(-2.0f, q.X);
      Assert.AreEqual(-3.0f, q.Y);
      Assert.AreEqual(-4.0f, q.Z);
    }


    [Test]
    public void Inverse()
    {
      QuaternionF identity = QuaternionF.Identity;
      QuaternionF inverseIdentity = identity.Inverse;
      Assert.AreEqual(inverseIdentity, identity);

      float angle = 0.4f;
      Vector3F axis = new Vector3F(1.0f, 1.0f, 1.0f);
      axis.Normalize();
      QuaternionF q = QuaternionF.CreateRotation(axis, angle);
      QuaternionF inverse = q.Inverse;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(-axis, inverse.Axis));

      q = new QuaternionF(1, 2, 3, 4);
      inverse = q.Inverse;
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(QuaternionF.Identity, inverse * q));
    }


    [Test]
    public void Invert()
    {
      QuaternionF inverseIdentity = QuaternionF.Identity;
      inverseIdentity.Invert();
      Assert.AreEqual(QuaternionF.Identity, inverseIdentity);

      float angle = 0.4f;
      Vector3F axis = new Vector3F(1.0f, 1.0f, 1.0f);
      axis.Normalize();
      QuaternionF q = QuaternionF.CreateRotation(axis, angle);
      q.Invert();
      Assert.IsTrue(Vector3F.AreNumericallyEqual(-axis, q.Axis));

      q = new QuaternionF(1, 2, 3, 4);
      QuaternionF inverse = q;
      inverse.Invert();
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(QuaternionF.Identity, inverse * q));
    }


    [Test]
    [ExpectedException(typeof(MathematicsException))]
    public void InvertException()
    {
      QuaternionF inverseOfZero = QuaternionF.Zero;
      inverseOfZero.Invert();
    }


    [Test]
    public void IndexerRead()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      Assert.AreEqual(1.0f, q[0]);
      Assert.AreEqual(2.0f, q[1]);
      Assert.AreEqual(3.0f, q[2]);
      Assert.AreEqual(4.0f, q[3]);
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerReadException()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      float d = q[-1];
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerReadException2()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      float d = q[4];
    }


    [Test]
    public void IndexerWrite()
    {
      QuaternionF q = QuaternionF.Zero;
      q[0] = 1.0f;
      q[1] = 2.0f;
      q[2] = 3.0f;
      q[3] = 4.0f;
      Assert.AreEqual(1.0f, q.W);
      Assert.AreEqual(2.0f, q.X);
      Assert.AreEqual(3.0f, q.Y);
      Assert.AreEqual(4.0f, q.Z);
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerWriteException()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      q[-1] = 0.0f;
    }


    [Test]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerWriteException2()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      q[4] = 0.0f;
    }


    [Test]
    public void Normalize()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      Assert.AreNotEqual(1.0f, q.Modulus);
      Assert.IsFalse(q.IsNumericallyNormalized);
      q.Normalize();
      Assert.IsTrue(Numeric.AreEqual(1.0f, q.Modulus));
      Assert.IsTrue(q.IsNumericallyNormalized);
    }


    [Test]
    [ExpectedException(typeof(DivideByZeroException))]
    public void NormalizeException()
    {
      QuaternionF q = QuaternionF.Zero;
      q.Normalize();
    }


    [Test]
    public void Normalized()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      Assert.AreNotEqual(1.0f, q.Modulus);
      Assert.IsFalse(q.IsNumericallyNormalized);
      QuaternionF normalized = q.Normalized;
      Assert.AreEqual(new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f), q);
      Assert.IsTrue(Numeric.AreEqual(1.0f, normalized.Modulus));
      Assert.IsTrue(normalized.IsNumericallyNormalized);
    }


    [Test]
    [ExpectedException(typeof(DivideByZeroException))]
    public void NormalizedException()
    {
      QuaternionF q = QuaternionF.Zero;
      q = q.Normalized;
    }


    [Test]
    public void TryNormalize()
    {
      QuaternionF q = QuaternionF.Zero;
      bool normalized = q.TryNormalize();
      Assert.IsFalse(normalized);

      q = new QuaternionF(1, 2, 3, 4);
      normalized = q.TryNormalize();
      Assert.IsTrue(normalized);
      Assert.AreEqual(new QuaternionF(1, 2, 3, 4).Normalized, q);

      q = new QuaternionF(0, -1, 0, 0);
      normalized = q.TryNormalize();
      Assert.IsTrue(normalized);
      Assert.AreEqual(new QuaternionF(0, -1, 0, 0).Normalized, q);
    }


    [Test]
    public void ExplicitDoubleCast()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      float[] values = (float[])q;
      Assert.AreEqual(4, values.Length);
      Assert.AreEqual(1.0f, values[0]);
      Assert.AreEqual(2.0f, values[1]);
      Assert.AreEqual(3.0f, values[2]);
      Assert.AreEqual(4.0f, values[3]);
    }


    [Test]
    public void ExplicitDoubleCast2()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      float[] values = q.ToArray();
      Assert.AreEqual(4, values.Length);
      Assert.AreEqual(1.0f, values[0]);
      Assert.AreEqual(2.0f, values[1]);
      Assert.AreEqual(3.0f, values[2]);
      Assert.AreEqual(4.0f, values[3]);
    }


    [Test]
    public void ExplicitListCast()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      List<float> values = (List<float>)q;
      Assert.AreEqual(4, values.Count);
      Assert.AreEqual(1.0f, values[0]);
      Assert.AreEqual(2.0f, values[1]);
      Assert.AreEqual(3.0f, values[2]);
      Assert.AreEqual(4.0f, values[3]);
    }


    [Test]
    public void ExplicitListCast2()
    {
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      List<float> values = q.ToList();
      Assert.AreEqual(4, values.Count);
      Assert.AreEqual(1.0f, values[0]);
      Assert.AreEqual(2.0f, values[1]);
      Assert.AreEqual(3.0f, values[2]);
      Assert.AreEqual(4.0f, values[3]);
    }


    [Test]
    public void ImplicitQuaternionDCast()
    {
      QuaternionF qF = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionD qD = qF;
      Assert.AreEqual(1.0f, (float)qD.W);
      Assert.AreEqual(2.0f, (float)qD.X);
      Assert.AreEqual(3.0f, (float)qD.Y);
      Assert.AreEqual(4.0f, (float)qD.Z);
    }


    [Test]
    public void ToQuaternionF()
    {
      QuaternionF qF = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionD qD = qF.ToQuaternionD();
      Assert.AreEqual(1.0f, (float)qD.W);
      Assert.AreEqual(2.0f, (float)qD.X);
      Assert.AreEqual(3.0f, (float)qD.Y);
      Assert.AreEqual(4.0f, (float)qD.Z);
    }


    [Test]
    public void ExplicitFromXnaCast()
    {
      Quaternion xna = new Quaternion(6, 7, 8, 9);
      QuaternionF v = (QuaternionF)xna;

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void FromXna()
    {
      Quaternion xna = new Quaternion(6, 7, 8, 9);
      QuaternionF v = QuaternionF.FromXna(xna);

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void ExplicitToXnaCast()
    {
      QuaternionF v = new QuaternionF(6, 7, 8, 9);
      Quaternion xna = (Quaternion)v;

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void ToXna()
    {
      QuaternionF v = new QuaternionF(6, 7, 8, 9);
      Quaternion xna = v.ToXna();

      Assert.AreEqual(xna.X, v.X);
      Assert.AreEqual(xna.Y, v.Y);
      Assert.AreEqual(xna.Z, v.Z);
      Assert.AreEqual(xna.W, v.W);
    }


    [Test]
    public void DotProduct()
    {
      QuaternionF q1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q2 = new QuaternionF(5.0f, 6.0f, 7.0f, 8.0f);
      float dotProduct = QuaternionF.Dot(q1, q2);
      Assert.AreEqual(70, dotProduct);
    }


    [Test]
    public void Rotate()
    {
      float angle = 0.4f;
      Vector3F axis = new Vector3F(1.0f, 2.0f, 3.0f);
      QuaternionF q = QuaternionF.CreateRotation(axis, angle);
      Matrix33F m33 = Matrix33F.CreateRotation(axis, angle);
      Vector3F v = new Vector3F(0.3f, -2.4f, 5.6f);
      Vector3F result1 = q.Rotate(v);
      Vector3F result2 = m33 * v;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void RotationMatrix33()
    {
      float angle = -1.6f;
      Vector3F axis = new Vector3F(1.0f, 2.0f, -3.0f);
      QuaternionF q = QuaternionF.CreateRotation(axis, angle);
      Matrix33F m33 = Matrix33F.CreateRotation(axis, angle);
      Vector3F v = new Vector3F(0.3f, -2.4f, 5.6f);
      Vector3F result1 = q.ToRotationMatrix33() * v;
      Vector3F result2 = m33 * v;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void RotationMatrix44()
    {
      float angle = -1.6f;
      Vector3F axis = new Vector3F(1.0f, 2.0f, -3.0f);
      QuaternionF q = QuaternionF.CreateRotation(axis, angle);
      Matrix44F m44 = Matrix44F.CreateRotation(axis, angle);
      Assert.IsTrue(Matrix44F.AreNumericallyEqual(q.ToRotationMatrix44(), m44));
    }


    [Test]
    public void MultiplyScalarOperator()
    {
      float s = 123.456f;
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF expectedResult = new QuaternionF(s * 1.0f, s * 2.0f, s * 3.0f, s * 4.0f);
      QuaternionF result1 = s * q;
      QuaternionF result2 = q * s;
      Assert.AreEqual(expectedResult, result1);
      Assert.AreEqual(expectedResult, result2);
    }


    [Test]
    public void MultiplyScalar()
    {
      float s = 123.456f;
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF expectedResult = new QuaternionF(s * 1.0f, s * 2.0f, s * 3.0f, s * 4.0f);
      QuaternionF result = QuaternionF.Multiply(s, q);
      Assert.AreEqual(expectedResult, result);
    }


    [Test]
    public void MultiplyOperator()
    {
      float angle1 = 0.4f;
      Vector3F axis1 = new Vector3F(1.0f, 2.0f, 3.0f);
      QuaternionF q1 = QuaternionF.CreateRotation(axis1, angle1);
      Matrix33F m1 = Matrix33F.CreateRotation(axis1, angle1);

      float angle2 = -1.6f;
      Vector3F axis2 = new Vector3F(1.0f, -2.0f, -3.5f);
      QuaternionF q2 = QuaternionF.CreateRotation(axis2, angle2);
      Matrix33F m2 = Matrix33F.CreateRotation(axis2, angle2);

      Vector3F v = new Vector3F(0.3f, -2.4f, 5.6f);
      Vector3F result1 = (q2 * q1).Rotate(v);
      Vector3F result2 = m2 * m1 * v;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void Multiply()
    {
      float angle1 = 0.4f;
      Vector3F axis1 = new Vector3F(1.0f, 2.0f, 3.0f);
      QuaternionF q1 = QuaternionF.CreateRotation(axis1, angle1);
      Matrix33F m1 = Matrix33F.CreateRotation(axis1, angle1);

      float angle2 = -1.6f;
      Vector3F axis2 = new Vector3F(1.0f, -2.0f, -3.5f);
      QuaternionF q2 = QuaternionF.CreateRotation(axis2, angle2);
      Matrix33F m2 = Matrix33F.CreateRotation(axis2, angle2);

      Vector3F v = new Vector3F(0.3f, -2.4f, 5.6f);
      Vector3F result1 = QuaternionF.Multiply(q2, q1).Rotate(v);
      Vector3F result2 = m2 * m1 * v;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void DivisionScalarOperator()
    {
      float s = 123.456f;
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF expectedResult = new QuaternionF(1.0f / s, 2.0f / s, 3.0f / s, 4.0f / s);
      QuaternionF result = q / s;
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(expectedResult, result));
    }


    [Test]
    public void DivisionScalar()
    {
      float s = 123.456f;
      QuaternionF q = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF expectedResult = new QuaternionF(1.0f / s, 2.0f / s, 3.0f / s, 4.0f / s);
      QuaternionF result = QuaternionF.Divide(q, s);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(expectedResult, result));
    }


    [Test]
    public void DivisionOperator()
    {
      float angle1 = 0.4f;
      Vector3F axis1 = new Vector3F(1.0f, 2.0f, 3.0f);
      QuaternionF q1 = QuaternionF.CreateRotation(axis1, angle1);
      Matrix33F m1 = Matrix33F.CreateRotation(axis1, angle1);

      float angle2 = -1.6f;
      Vector3F axis2 = new Vector3F(1.0f, -2.0f, -3.5f);
      QuaternionF q2 = QuaternionF.CreateRotation(axis2, angle2);
      Matrix33F m2 = Matrix33F.CreateRotation(axis2, angle2);

      Vector3F v = new Vector3F(0.3f, -2.4f, 5.6f);
      Vector3F result1 = (q2 / q1).Rotate(v);
      Vector3F result2 = m2 * m1.Inverse * v;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void Division()
    {
      float angle1 = 0.4f;
      Vector3F axis1 = new Vector3F(1.0f, 2.0f, 3.0f);
      QuaternionF q1 = QuaternionF.CreateRotation(axis1, angle1);
      Matrix33F m1 = Matrix33F.CreateRotation(axis1, angle1);

      float angle2 = -1.6f;
      Vector3F axis2 = new Vector3F(1.0f, -2.0f, -3.5f);
      QuaternionF q2 = QuaternionF.CreateRotation(axis2, angle2);
      Matrix33F m2 = Matrix33F.CreateRotation(axis2, angle2);

      Vector3F v = new Vector3F(0.3f, -2.4f, 5.6f);
      Vector3F result1 = QuaternionF.Divide(q2, q1).Rotate(v);
      Vector3F result2 = m2 * m1.Inverse * v;
      Assert.IsTrue(Vector3F.AreNumericallyEqual(result1, result2));
    }


    [Test]
    public void CreateRotation()
    {
      QuaternionF q;

      // From matrix vs. from angle/axis
      Matrix33F m = Matrix33F.CreateRotation(Vector3F.UnitX, (float)Math.PI / 4);
      q = QuaternionF.CreateRotation(m);
      QuaternionF q2 = QuaternionF.CreateRotation(Vector3F.UnitX, (float)Math.PI / 4);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q2, q));
      m = Matrix33F.CreateRotation(Vector3F.UnitY, (float)Math.PI / 4);
      q = QuaternionF.CreateRotation(m);
      q2 = QuaternionF.CreateRotation(Vector3F.UnitY, (float)Math.PI / 4);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q2, q));
      m = Matrix33F.CreateRotation(Vector3F.UnitZ, (float)Math.PI / 4);
      q = QuaternionF.CreateRotation(m);
      q2 = QuaternionF.CreateRotation(Vector3F.UnitZ, (float)Math.PI / 4);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q2, q));

      // From vector-vector
      Vector3F start, end;
      start = Vector3F.UnitX;
      end = Vector3F.UnitY;
      q = QuaternionF.CreateRotation(start, end);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = Vector3F.UnitY;
      end = Vector3F.UnitZ;
      q = QuaternionF.CreateRotation(start, end);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = Vector3F.UnitZ;
      end = Vector3F.UnitX;
      q = QuaternionF.CreateRotation(start, end);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = new Vector3F(1, 1, 1);
      end = new Vector3F(1, 1, 1);
      q = QuaternionF.CreateRotation(start, end);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = new Vector3F(1.0f, 1.0f, 1.0f);
      end = new Vector3F(-1.0f, -1.0f, -1.0f);
      q = QuaternionF.CreateRotation(start, end);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      start = new Vector3F(-1.0f, 2.0f, 1.0f);
      end = new Vector3F(-2.0f, -1.0f, -1.0f);
      q = QuaternionF.CreateRotation(start, end);
      Assert.IsTrue(Vector3F.AreNumericallyEqual(end, q.ToRotationMatrix33() * start));

      float degree45 = MathHelper.ToRadians(45);
      q = QuaternionF.CreateRotation(degree45, Vector3F.UnitZ, degree45, Vector3F.UnitY, degree45, Vector3F.UnitX, false);
      QuaternionF expected = QuaternionF.CreateRotation(Vector3F.UnitZ, degree45) * QuaternionF.CreateRotation(Vector3F.UnitY, degree45)
                             * QuaternionF.CreateRotation(Vector3F.UnitX, degree45);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(expected, q));

      q = QuaternionF.CreateRotation(degree45, Vector3F.UnitZ, degree45, Vector3F.UnitY, degree45, Vector3F.UnitX, true);
      expected = QuaternionF.CreateRotation(Vector3F.UnitX, degree45) * QuaternionF.CreateRotation(Vector3F.UnitY, degree45)
                 * QuaternionF.CreateRotation(Vector3F.UnitZ, degree45);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(expected, q));
    }


    [Test]
    public void FromMatrixWithZeroTrace()
    {
      QuaternionF q;
      Matrix33F m = new Matrix33F(0, 1, 0,
                                  0, 0, 1,
                                  1, 0, 0);
      q = QuaternionF.CreateRotation(m);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(new QuaternionF(-0.5f, 0.5f, 0.5f, 0.5f), q));
    }


    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateRotationException1()
    {
      QuaternionF.CreateRotation(Vector3F.Zero, Vector3F.UnitX);
    }


    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateRotationException2()
    {
      QuaternionF.CreateRotation(Vector3F.UnitY, Vector3F.Zero);
    }


    [Test]
    public void CreateRotationX()
    {
      float angle = 0.3f;
      QuaternionF q = QuaternionF.CreateRotation(Vector3F.UnitX, angle);
      QuaternionF qx = QuaternionF.CreateRotationX(angle);
      Assert.AreEqual(q, qx);
    }


    [Test]
    public void CreateRotationY()
    {
      float angle = 0.3f;
      QuaternionF q = QuaternionF.CreateRotation(Vector3F.UnitY, angle);
      QuaternionF qy = QuaternionF.CreateRotationY(angle);
      Assert.AreEqual(q, qy);
    }


    [Test]
    public void CreateRotationZ()
    {
      float angle = 0.3f;
      QuaternionF q = QuaternionF.CreateRotation(Vector3F.UnitZ, angle);
      QuaternionF qz = QuaternionF.CreateRotationZ(angle);
      Assert.AreEqual(q, qz);
    }


    [Test]
    public void Exp()
    {
      float θ = -0.3f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF(0.0f, θ * v);
      QuaternionF exp = QuaternionF.Exp(q);
      Assert.IsTrue(Numeric.AreEqual((float)Math.Cos(θ), exp.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual((float)Math.Sin(θ) * v, exp.V));
    }


    [Test]
    public void Exp2()
    {
      float θ = -0.3f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF(0.0f, θ * v);
      q.Exp();
      Assert.IsTrue(Numeric.AreEqual((float)Math.Cos(θ), q.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual((float)Math.Sin(θ) * v, q.V));
    }


    [Test]
    public void Exp3()
    {
      float θ = 0.0f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF(0.0f, θ * v);
      QuaternionF exp = QuaternionF.Exp(q);
      Assert.IsTrue(Numeric.AreEqual(1, exp.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(Vector3F.Zero, exp.V));
    }


    [Test]
    public void Exp4()
    {
      float θ = 0.0f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF(0.0f, θ * v);
      q.Exp();
      Assert.IsTrue(Numeric.AreEqual(1, q.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(Vector3F.Zero, q.V));
    }


    [Test]
    public void Ln()
    {
      float θ = 0.3f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();
      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);

      QuaternionF ln = QuaternionF.Ln(q);
      Assert.IsTrue(Numeric.AreEqual(0.0f, ln.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(θ * v, ln.V));
    }


    [Test]
    public void Ln2()
    {
      float θ = 0.0f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();
      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);

      QuaternionF ln = QuaternionF.Ln(q);
      Assert.IsTrue(Numeric.AreEqual(0.0f, ln.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(θ * v, ln.V));
    }


    [Test]
    public void Ln3()
    {
      float θ = 0.3f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();
      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);

      q.Ln();
      Assert.IsTrue(Numeric.AreEqual(0.0f, q.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(θ * v, q.V));
    }


    [Test]
    public void Ln4()
    {
      float θ = 0.0f;
      Vector3F v = new Vector3F(1.0f, 2.0f, 3.0f);
      v.Normalize();
      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);

      q.Ln();
      Assert.IsTrue(Numeric.AreEqual(0.0f, q.W));
      Assert.IsTrue(Vector3F.AreNumericallyEqual(θ * v, q.V));
    }


    [Test]
    [ExpectedException(typeof(MathematicsException))]
    public void LnException()
    {
      QuaternionF q = new QuaternionF(1.5f, 0.0f, 0.0f, 0.0f);
      QuaternionF.Ln(q);
    }


    [Test]
    public void AdditionOperator()
    {
      QuaternionF a = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF b = new QuaternionF(2.0f, 3.0f, 4.0f, 5.0f);
      QuaternionF c = a + b;
      Assert.AreEqual(new QuaternionF(3.0f, 5.0f, 7.0f, 9.0f), c);
    }


    [Test]
    public void Addition()
    {
      QuaternionF a = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF b = new QuaternionF(2.0f, 3.0f, 4.0f, 5.0f);
      QuaternionF c = QuaternionF.Add(a, b);
      Assert.AreEqual(new QuaternionF(3.0f, 5.0f, 7.0f, 9.0f), c);
    }


    [Test]
    public void SubtractionOperator()
    {
      QuaternionF a = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF b = new QuaternionF(10.0f, -10.0f, 0.5f, 2.5f);
      QuaternionF c = a - b;
      Assert.AreEqual(new QuaternionF(-9.0f, 12.0f, 2.5f, 1.5f), c);
    }


    [Test]
    public void Subtraction()
    {
      QuaternionF a = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF b = new QuaternionF(10.0f, -10.0f, 0.5f, 2.5f);
      QuaternionF c = QuaternionF.Subtract(a, b);
      Assert.AreEqual(new QuaternionF(-9.0f, 12.0f, 2.5f, 1.5f), c);
    }


    [Test]
    public void NegationOperator()
    {
      QuaternionF a = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      Assert.AreEqual(new QuaternionF(-1.0f, -2.0f, -3.0f, -4.0f), -a);
    }


    [Test]
    public void Negation()
    {
      QuaternionF a = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      Assert.AreEqual(new QuaternionF(-1.0f, -2.0f, -3.0f, -4.0f), QuaternionF.Negate(a));
    }


    [Test]
    public void Power()
    {
      const float θ = 0.4f;
      const float t = -1.2f;
      Vector3F v = new Vector3F(2.3f, 1.0f, -2.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);
      QuaternionF power = QuaternionF.Power(q, t);
      QuaternionF expected = new QuaternionF((float)Math.Cos(t * θ), (float)Math.Sin(t * θ) * v);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(expected, power));
    }


    [Test]
    public void Power2()
    {
      const float θ = 0.4f;
      const float t = -1.2f;
      Vector3F v = new Vector3F(2.3f, 1.0f, -2.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);
      q.Power(t);
      QuaternionF expected = new QuaternionF((float)Math.Cos(t * θ), (float)Math.Sin(t * θ) * v);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(expected, q));
    }


    [Test]
    public void Power3()
    {
      const float θ = 0.4f;
      Vector3F v = new Vector3F(2.3f, 1.0f, -2.0f);
      v.Normalize();

      QuaternionF q = new QuaternionF((float)Math.Cos(θ), (float)Math.Sin(θ) * v);
      QuaternionF q2 = q;
      q2.Power(2);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q * q, q2));
      QuaternionF q3 = q;
      q3.Power(3);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q * q * q, q3));

      q2 = q;
      q2.Power(-2);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q.Inverse * q.Inverse, q2));

      q3 = q;
      q3.Power(-3);
      Assert.IsTrue(QuaternionF.AreNumericallyEqual(q.Inverse * q.Inverse * q.Inverse, q3));
    }


    [Test]
    public void GetAngleTest()
    {
      QuaternionF qIdentity = QuaternionF.Identity;
      QuaternionF q03 = QuaternionF.CreateRotation(Vector3F.UnitX, 0.3f);
      QuaternionF q03Plus11 = QuaternionF.CreateRotation(new Vector3F(1, 0.2f, -3), 1.1f) * q03;
      QuaternionF q0 = QuaternionF.CreateRotation(Vector3F.UnitX, 0.0f);
      QuaternionF qPi = QuaternionF.CreateRotation(Vector3F.UnitX, ConstantsF.Pi);
      QuaternionF q2Pi = QuaternionF.CreateRotation(Vector3F.UnitX, ConstantsF.TwoPi);

      Assert.IsTrue(Numeric.AreEqual(0.0f, QuaternionF.GetAngle(qIdentity, qIdentity)));
      Assert.IsTrue(Numeric.AreEqual(0.3f, QuaternionF.GetAngle(qIdentity, q03)));
      Assert.IsTrue(Numeric.AreEqual(0.3f, QuaternionF.GetAngle(qIdentity, -q03))); // Remember: q and -q represent the same orientation.
      Assert.IsTrue(Numeric.AreEqual(1.1f, QuaternionF.GetAngle(q03, q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(1.1f, QuaternionF.GetAngle(-q03, q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(1.1f, QuaternionF.GetAngle(q03, -q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(1.1f, QuaternionF.GetAngle(-q03, -q03Plus11)));
      Assert.IsTrue(Numeric.AreEqual(0.0f, QuaternionF.GetAngle(qIdentity, q0)));
      Assert.IsTrue(Numeric.AreEqual(0.0f, QuaternionF.GetAngle(qIdentity, q2Pi)));
      Assert.IsTrue(Numeric.AreEqual(0.0f, QuaternionF.GetAngle(q0, q2Pi)));
      Assert.IsTrue(Numeric.AreEqual(0.3f, QuaternionF.GetAngle(q03, q0)));
      Assert.IsTrue(Numeric.AreEqual(ConstantsF.Pi, QuaternionF.GetAngle(q0, qPi)));
      Assert.IsTrue(Numeric.AreEqual(ConstantsF.Pi, QuaternionF.GetAngle(q2Pi, qPi)));
    }


    [Test]
    public void IsNaN()
    {
      const int numberOfRows = 4;
      Assert.IsFalse(new QuaternionF().IsNaN);

      for (int i = 0; i < numberOfRows; i++)
      {
        QuaternionF v = new QuaternionF();
        v[i] = float.NaN;
        Assert.IsTrue(v.IsNaN);
      }
    }


    [Test]
    public void SerializationXml()
    {
      QuaternionF q1 = new QuaternionF(1.0f, 2.0f, 3.0f, 4.0f);
      QuaternionF q2;
      string fileName = "SerializationQuaternionF.xml";

      if (File.Exists(fileName))
        File.Delete(fileName);

      XmlSerializer serializer = new XmlSerializer(typeof(QuaternionF));
      StreamWriter writer = new StreamWriter(fileName);
      serializer.Serialize(writer, q1);
      writer.Close();

      serializer = new XmlSerializer(typeof(QuaternionF));
      FileStream fileStream = new FileStream(fileName, FileMode.Open);
      q2 = (QuaternionF)serializer.Deserialize(fileStream);
      Assert.AreEqual(q1, q2);
    }



    [Test]
    public void SerializationXml2()
    {
      QuaternionF q1 = new QuaternionF(0.1f, -0.2f, 6, 40);
      QuaternionF q2;

      string fileName = "SerializationQuaternionF_DataContractSerializer.xml";

      if (File.Exists(fileName))
        File.Delete(fileName);

      var serializer = new DataContractSerializer(typeof(QuaternionF));
      using (var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
      using (var writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
        serializer.WriteObject(writer, q1);

      using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
      using (var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas()))
        q2 = (QuaternionF)serializer.ReadObject(reader);

      Assert.AreEqual(q1, q2);
    }


    [Test]
    public void SerializationJson()
    {
      QuaternionF q1 = new QuaternionF(0.1f, -0.2f, 6, 40);
      QuaternionF q2;

      string fileName = "SerializationQuaternionF.json";

      if (File.Exists(fileName))
        File.Delete(fileName);

      var serializer = new DataContractJsonSerializer(typeof(QuaternionF));
      using (var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
        serializer.WriteObject(stream, q1);

      using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        q2 = (QuaternionF)serializer.ReadObject(stream);

      Assert.AreEqual(q1, q2);
    }


    [Test]
    public void Parse()
    {
      QuaternionF vector = QuaternionF.Parse("(0.0123; (9.876; 0.0; -2.3))", CultureInfo.InvariantCulture);
      Assert.AreEqual(0.0123f, vector.W);
      Assert.AreEqual(9.876f, vector.X);
      Assert.AreEqual(0.0f, vector.Y);
      Assert.AreEqual(-2.3f, vector.Z);

      vector = QuaternionF.Parse("(   0.0123   ;  ( 9;  0.1 ; -2.3 ) ) ", CultureInfo.InvariantCulture);
      Assert.AreEqual(0.0123f, vector.W);
      Assert.AreEqual(9f, vector.X);
      Assert.AreEqual(0.1f, vector.Y);
      Assert.AreEqual(-2.3f, vector.Z);
    }


    [Test]
    [ExpectedException(typeof(FormatException))]
    public void ParseException()
    {
      QuaternionF vector = QuaternionF.Parse("(0.0123; 9.876; 4.1; -9.0)");
    }


    [Test]
    public void ToStringAndParse()
    {
      QuaternionF quaternion = new QuaternionF(0.0123f, 9.876f, 0.0f, -2.3f);
      string s = quaternion.ToString();
      QuaternionF parsedQuaternion = QuaternionF.Parse(s);
      Assert.AreEqual(quaternion, parsedQuaternion);
    }
  }
}
