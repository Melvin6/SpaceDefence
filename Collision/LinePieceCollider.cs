using System;
using SpaceDefence.Collision;
using Microsoft.Xna.Framework;

namespace SpaceDefence
{

    public class LinePieceCollider : Collider, IEquatable<LinePieceCollider>
    {

        public Vector2 Start;
        public Vector2 End;

        /// <summary>
        /// The length of the LinePiece, changing the length moves the end vector to adjust the length.
        /// </summary>
        public float Length 
        { 
            get { 
                return (End - Start).Length(); 
            } 
            set {
                End = Start + GetDirection() * value; 
            }
        }

        /// <summary>
        /// The A component from the standard line formula Ax + By + C = 0
        /// </summary>
        public float StandardA
        {
            get
            {
                // TODO: Implement
                return 0;
            }
        }

        /// <summary>
        /// The B component from the standard line formula Ax + By + C = 0
        /// </summary>
        public float StandardB
        {
            get
            {
                // TODO: Implement
                return 0;
            }
        }

        /// <summary>
        /// The C component from the standard line formula Ax + By + C = 0
        /// </summary>
        public float StandardC
        {
            get
            {
                // TODO: Implement
                return 0;
            }
        }

        public LinePieceCollider(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }
        
        public LinePieceCollider(Vector2 start, Vector2 direction, float length)
        {
            Start = start;
            End = start + direction * length;
        }

        /// <summary>
        /// Should return the angle between a given direction and the up vector.
        /// </summary>
        /// <param name="direction">The Vector2 pointing out from (0,0) to calculate the angle to.</param>
        /// <returns> The angle in radians between the the up vector and the direction to the cursor.</returns>
        public static float GetAngle(Vector2 direction)
        {
            // TODO: Implement
            return MathF.Atan2(direction.X, -direction.Y);
        }


        /// <summary>
        /// Calculates the normalized vector pointing from point1 to point2
        /// </summary>
        /// <returns> A Vector2 containing the direction from point1 to point2. </returns>
        public static Vector2 GetDirection(Vector2 point1, Vector2 point2)
        {
            // TODO Implement, currently pointing up.
            Vector2 direction = point2 - point1;
            if (direction.LengthSquared() > 0)
                direction = Vector2.Normalize(direction);

            return direction;    

            // return -Vector2.UnitY;
        }


        /// <summary>
        /// Gets whether or not the Line intersects another Line
        /// </summary>
        /// <param name="other">The Line to check for intersection</param>
        /// <returns>true there is any overlap between the Circle and the Line.</returns>
        public override bool Intersects(LinePieceCollider other)
        {
            // TODO Implement.
            return false;
        }


        /// <summary>
        /// Gets whether or not the line intersects a Circle.
        /// </summary>
        /// <param name="other">The Circle to check for intersection.</param>
        /// <returns>true there is any overlap between the two Circles.</returns>
        public override bool Intersects(CircleCollider other)
        {
            // TODO Implement hint, you can use the NearestPointOnLine function defined below.
            Vector2 nearestPoint = NearestPointOnLine(other.Center);
            float distance = Vector2.DistanceSquared(nearestPoint, other.Center);

            return distance <= other.Radius * other.Radius;
        }

        /// <summary>
        /// Gets whether or not the Line intersects the Rectangle.
        /// </summary>
        /// <param name="other">The Rectangle to check for intersection.</param>
        /// <returns>true there is any overlap between the Circle and the Rectangle.</returns>
        public override bool Intersects(RectangleCollider other)
        {
            // TODO Implement
            if (!this.GetBoundingBox().Intersects(other.shape))
                return false;
            Vector2 topLeft = new Vector2(other.shape.Left, other.shape.Top);
            Vector2 topRight = new Vector2(other.shape.Right, other.shape.Top);
            Vector2 bottomLeft = new Vector2(other.shape.Left, other.shape.Bottom);
            Vector2 bottomRight = new Vector2(other.shape.Right, other.shape.Bottom);

            LinePieceCollider[] edges = new LinePieceCollider[]
            {
                new LinePieceCollider(topLeft, topRight),
                new LinePieceCollider(topRight, bottomRight),
                new LinePieceCollider(bottomRight, bottomLeft),
                new LinePieceCollider(bottomLeft, topLeft)
            };

            // Check if any edge intersects with this line
            foreach (var edge in edges)
            {
                if (this.GetIntersection(edge) != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates the intersection point between 2 lines.
        /// </summary>
        /// <param name="Other">The line to intersect with</param>
        /// <returns>A Vector2 with the point of intersection.</returns>
        public Vector2 GetIntersection(LinePieceCollider Other)
        {
            // TODO Implement
             float d = (this.End.X - this.Start.X) * (Other.End.Y - Other.Start.Y) - (this.End.Y - this.Start.Y) * (Other.End.X - Other.Start.X);
    
            if (d == 0) 
                return Vector2.Zero;

            float u = ((Other.Start.X - this.Start.X) * (Other.End.Y - Other.Start.Y) - (Other.Start.Y - this.Start.Y) * (Other.End.X - Other.Start.X)) / d;
                
            float v = ((Other.Start.X - this.Start.X) * (this.End.Y - this.Start.Y) - (Other.Start.Y - this.Start.Y) * (this.End.X - this.Start.X)) / d;

            if (u >= 0 && u <= 1 && v >= 0 && v <= 1)
            {
                return new Vector2(
                    this.Start.X + u * (this.End.X - this.Start.X),
                    this.Start.Y + u * (this.End.Y - this.Start.Y)
                );
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Finds the nearest point on a line to a given vector, taking into account if the line is .
        /// </summary>
        /// <param name="other">The Vector you want to find the nearest point to.</param>
        /// <returns>The nearest point on the line.</returns>
        public Vector2 NearestPointOnLine(Vector2 other)
        {
            // TODO Implement
            Vector2 direction = End - Start;
            Vector2 AO = other - Start;

            float t = Vector2.Dot(AO, direction) / Vector2.Dot(direction, direction);
            t = Math.Clamp(t, 0f, 1f);
            Vector2 nearestPoint = Start + t * direction;

            return nearestPoint;
        }

        /// <summary>
        /// Returns the enclosing Axis Aligned Bounding Box containing the control points for the line.
        /// As an unbound line has infinite length, the returned bounding box assumes the line to be bound.
        /// </summary>
        /// <returns></returns>
        public override Rectangle GetBoundingBox()
        {
            Point topLeft = new Point((int)Math.Min(Start.X, End.X), (int)Math.Min(Start.Y, End.Y));
            Point size = new Point((int)Math.Max(Start.X, End.X), (int)Math.Max(Start.Y, End.X)) - topLeft;
            return new Rectangle(topLeft,size);
        }


        /// <summary>
        /// Gets whether or not the provided coordinates lie on the line.
        /// </summary>
        /// <param name="coordinates">The coordinates to check.</param>
        /// <returns>true if the coordinates are within the circle.</returns>
        public override bool Contains(Vector2 coordinates)
        {
            // TODO: Implement

            return false;
        }

        public bool Equals(LinePieceCollider other)
        {
            return other.Start == this.Start && other.End == this.End;
        }

        /// <summary>
        /// Calculates the normalized vector pointing from point1 to point2
        /// </summary>
        /// <returns> A Vector2 containing the direction from point1 to point2. </returns>
        public static Vector2 GetDirection(Point point1, Point point2)
        {
            return GetDirection(point1.ToVector2(), point2.ToVector2());
        }


        /// <summary>
        /// Calculates the normalized vector pointing from point1 to point2
        /// </summary>
        /// <returns> A Vector2 containing the direction from point1 to point2. </returns>
        public Vector2 GetDirection()
        {
            return GetDirection(Start, End);
        }


        /// <summary>
        /// Should return the angle between a given direction and the up vector.
        /// </summary>
        /// <param name="direction">The Vector2 pointing out from (0,0) to calculate the angle to.</param>
        /// <returns> The angle in radians between the the up vector and the direction to the cursor.</returns>
        public float GetAngle()
        {
            return GetAngle(GetDirection());
        }
    }
}
