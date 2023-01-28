namespace HelperMathFunctions
{
    class Matrix
    {
        public float _x1, _x2, _x3, _y1, _y2, _y3, _z1, _z2, _z3;
        public Matrix(float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3)
        {
            _x1 = x1;
            _x2 = x2;
            _x3 = x3;
            _y1 = y1;
            _y2 = y2;
            _y3 = y3;
            _z1 = z1;
            _z2 = z2;
            _z3 = z3;
        }
        public Matrix()
        { }
        public Vector GetFirstColumn
        {
            get { return new Vector(_x1, _y1, _z1); }
        }
        public Matrix Inverse()
        {
            var d = 1 / Det();
            var inv = new Matrix(d * (_y2 * _z3 - _y3 * _z2), d * (_x3 * _z2 - _x2 * _z3), d * (_x2 * _y3 - _x3 * _y2),
                                    d * (_y3 * _z1 - _y1 * _z3), d * (_x1 * _z3 - _x3 * _z1), d * (_x3 * _y1 - _x1 * _y3),
                                    d * (_y1 * _z2 - _y2 * _z1), d * (_x2 * _z1 - _x1 * _z2), d * (_x1 * _y2 - _x2 * _y1));
            return inv;
        }
        public float Det()
        {
            float det = (_x1 * _y2 * _z3) + (_x2 * _y3 * _z1) + (_x3 * _y1 * _z2)
                            - (_x3 * _y2 * _z1) - (_x2 * _y1 * _z3) - (_x1 * _y3 * _z2);
            return det;
        }

        public static Vector operator *(Vector v, Matrix m)
        {
            var res = new Vector(m._x1 * v.X + m._y1 * v.Y + m._z1 * v.Z,
                                    m._x2 * v.X + m._y2 * v.Y + m._z2 * v.Z,
                                    m._x3 * v.X + m._y3 * v.Y + m._z3 * v.Z);
            return res;
        }

        public static void printMatrix(Matrix m)
        {
            Console.WriteLine("___________________________________________");
            Console.WriteLine(m._x1 + "\t" + m._x2 + "\t" + m._x3);
            Console.WriteLine(m._y1 + "\t" + m._y2 + "\t" + m._y3);
            Console.WriteLine(m._z1 + "\t" + m._z2 + "\t" + m._z3);
            Console.WriteLine("___________________________________________");
        }

    }
}
