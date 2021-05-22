using Tests.Tests.Gradient.Shape;

namespace Tests.Tests.Gradient
{
    public class GradientTest<T, TShape> : Test<T>
        where T : unmanaged
        where TShape : struct, IGradient<T>
    {
        private DimensionSpecs[] _specs;

        public GradientTest(string name, DimensionSpecs[] specs, double bandwidth, int clusters) :
            base(name, null, bandwidth, clusters)
        {
            _specs = specs;
        }

        public override T[] Input => GradientGenerator.Generate<T, TShape>(_specs);
    }
}
