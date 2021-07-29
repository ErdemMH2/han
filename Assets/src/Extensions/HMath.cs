using UnityEngine;

﻿
namespace HAN.Lib.Extension.Math
{
    public static class HMath
    {
        /// Float epsilon value
        public static float EpsilonFloat { get { return System.Single.Epsilon; } }

        /// Uses EpsilonFloat to compare two floats.
        public static bool IsAlmostEqual( float a, float b ) {  return System.Math.Abs( a - b ) < EpsilonFloat; }

        /// Uses epsilon to compare two floats.
        public static bool IsAlmostEqual( float a, float b, float epsilon ) { return System.Math.Abs( a - b ) < epsilon; }


        /// Sums Vectors. Vector 2 to Vector 3
        public static Vector3 AddVector( Vector3 a, Vector3 b ) {  return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }

        /// Sums Vectors. Vector 2 to Vector 2
        public static Vector2 AddVector( Vector2 a, Vector2 b ) {  return new Vector2(a.x + b.x, a.y + b.y); }

        /// Sums Vectors. Vector 3 to Vector 2
        public static Vector2 AddVector( Vector3 a, Vector2 b ) {  return new Vector2(a.x + b.x, a.y + b.y); }

        public static Vector3 WorldToScreenPosition( Vector3 a_worldPos ) { return Camera.main.WorldToScreenPoint( a_worldPos ); }

        public static Vector3 WorldToScreenPosition( System.Numerics.Vector3 a_worldPos ) 
        {
            Vector3 worldPos = new Vector3( a_worldPos.X, a_worldPos.Y, a_worldPos.Z );
            return Camera.main.WorldToScreenPoint( worldPos ); 
        }

        public static Vector3 ConvertToUnityVector(System.Numerics.Vector3 a_vector) { return new Vector3(a_vector.X, a_vector.Y, a_vector.Z); }
    }
}