namespace Tests
{
    public static class DoubleTests
    {
        public static Test<double> DoubleTest1 = new Test<double>(
            "Double test 1",
            new double[]
            {
                0,
                1,
                8,
                10,
                12,
                22,
                24,
            }, 5, 3);
    }
}
