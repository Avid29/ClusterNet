using ClusterNet.Helpers;
using ClusterNet.Kernels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Shapes;

namespace Tests.Common.Helpers.ConnectedComponents
{
    [TestClass]
    public class ConnectedComponents
    {
        [TestMethod]
        public void ConnectedComponentsTest1()
        {
            GaussianKernel kernel = new GaussianKernel(.1);
            (double, int)[] components =
            {
                (0, 1),
                (1, 1),
                (-1, 1),
                (2, 1),
                (-2, 1),
            };

            var results = ClusterNet.Helpers.ConnectedComponents.ConnectComponents<double, DoubleShape, GaussianKernel, (double, double)>(components, kernel);

            // Should not effect collection
            Assert.AreEqual(components.Length, results.Length);
            for (int i = 0; i < components.Length; i++)
            {
                Assert.AreEqual(components[i], results[i]);
            }
        }

        [TestMethod]
        public void ConnectedComponentsTest2()
        {
            GaussianKernel kernel = new GaussianKernel(.1);
            (double, int)[] components =
            {
                (0, 1),
                (.05, 1),
                (-.05, 1),
                (.1, 1),
                (-.1, 1),
            };

            (double, int)[] expected =
            {
                (0, 5)
            };

            var results = ClusterNet.Helpers.ConnectedComponents.ConnectComponents<double, DoubleShape, GaussianKernel, (double, double)>(components, kernel);

            // Compare results to expected
            Assert.AreEqual(expected.Length, results.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], results[i]);
            }
        }

        [TestMethod]
        public void ConnectedComponentsTest3()
        {
            GaussianKernel kernel = new GaussianKernel(.1);
            (double, int)[] components =
            {
                (0, 1),
                (.05, 3),
                (.1, 1),
                (3, 1),
                (3.1, 1),
            };

            (double, int)[] expected =
            {
                (.05, 5),
                (3, 1),
                (3.1, 1),
            };

            var results = ClusterNet.Helpers.ConnectedComponents.ConnectComponents<double, DoubleShape, GaussianKernel, (double, double)>(components, kernel);

            // Compare results to expected
            Assert.AreEqual(expected.Length, results.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], results[i]);
            }
        }
    }
}
