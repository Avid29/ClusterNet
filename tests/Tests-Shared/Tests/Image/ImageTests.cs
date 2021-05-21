namespace Tests.Tests.Image
{
    public static class ImageTests
    {
        public static ImageTest ImageTest_IsThisIt =
            new ImageTest("Is This It - The Strokes", "https://media.pitchfork.com/photos/5929a58b13d1975652138f9b/1:1/w_600/c1b895b7.jpg", 1920, .15, 3);
        public static ImageTest ImageTest_Minecraft =
            new ImageTest("Minecraft - C418", "https://f4.bcbits.com/img/a3390257927_10.jpg", 1920, .15, 2);

        public static ImageTest[] All_ImageTests = new ImageTest[]
        {
            ImageTest_IsThisIt,
            ImageTest_Minecraft,
        };
    }
}
