// Adam Dernis © 2021

using ClusterNet.Kernels;
using ColorExtractor.ColorSpaces;
using ColorExtractor.Shapes;
using ComputeSharp;

namespace ClusterNet.GPU.MeanShift
{
    /// <summary>
    /// A ComputerShader generated shader for shifting points in a MeanShift operation.
    /// </summary>
    /// <remarks>
    /// This Shader is hard coded for Vector3s, Euclidean distances, and GuassianKernels.
    /// This is kind of just play time for until ComputerShader supports generics.
    /// </remarks>
    [AutoConstructor]
    internal readonly partial struct PointShiftShader : IComputeShader
    {
        /// <summary>
        /// The cluster being shifted towards the points.
        /// </summary>
        public readonly RGBColor _cluster;

        /// <summary>
        /// The original points to shift around.
        /// </summary>
        public readonly ReadOnlyBuffer<RGBColor> _pointBuffer;

        /// <summary>
        /// A buffer to write the resulting weighted distances from points to the cluster.
        /// </summary>
        public readonly ReadWriteBuffer<double> _weights;

        /// <summary>
        /// The Kernel to weight use when weighting distances.
        /// </summary>
        public readonly GaussianKernel _kernel;

        /// <inheritdoc/>
        public void Execute()
        {
            int offset = ThreadIds.X;
            RGBColor point = _pointBuffer[offset];

            RGBShape shape = default;
            double weight = shape.FindDistanceSquared(point, _cluster);

            weight = _kernel.WeightDistance(weight);
            _weights[offset] = weight;
        }
    }
}
