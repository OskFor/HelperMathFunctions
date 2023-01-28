namespace HelperMathFunctions
{
    public class Vector
    {
        private float _x, _y, _z;
        public Vector()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }
        public Vector(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public Vector(Vector v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }

        public void SetVec(Vector v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }
        public void SetVec(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var v3 = new Vector((v1._x + v2._x), (v1._y + v2._y), (v1._z + v2._z));
            return v3;
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            var v3 = new Vector((v1._x - v2._x), (v1._y - v2._y), (v1._z - v2._z));
            return v3;
        }
        public static float operator *(Vector v1, Vector v2)
        {
            var f = v1._x * v2._x + v1._y * v2._y + v1._z * v2._z;
            return f;
        }
    }
}
