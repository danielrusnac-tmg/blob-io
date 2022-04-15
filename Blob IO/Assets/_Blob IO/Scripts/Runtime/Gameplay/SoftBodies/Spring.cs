namespace BlobIO.Gameplay.SoftBodies
{
    public struct Spring
    {
        public int PointIndexA;
        public int PointIndexB;
        
        public Spring(int pointIndexA, int pointIndexB)
        {
            PointIndexA = pointIndexA;
            PointIndexB = pointIndexB;
        }
    }
}