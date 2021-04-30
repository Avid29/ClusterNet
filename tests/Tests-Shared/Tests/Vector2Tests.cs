using System.Numerics;

namespace Tests_Shared.Tests
{
    public static class Vector2Tests
    {
        public static Test<Vector2> Vector2Test1 = new Test<Vector2>(
            new Vector2[]
            {
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 0),
                new Vector2(7, 5),
                new Vector2(5, 7),
                new Vector2(6, 6),
            }, 5);
    }
}
