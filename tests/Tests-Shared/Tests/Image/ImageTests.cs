namespace Tests.Tests.Image
{
    public static class ImageTests
    {
        public static ImageTest Califorinaction =
            new ImageTest(Images.Califorinaction, 1920, .15, 3);

        public static ImageTest Contra =
            new ImageTest(Images.Contra, 1920, .15, 3);

        public static ImageTest EveryThingAllInTime =
            new ImageTest(Images.EveryThingAllInTime, 1920, .15, 3);

        public static ImageTest IsThisIt =
            new ImageTest(Images.IsThisIt, 1920, .05, 3);

        public static ImageTest Minecraft =
            new ImageTest(Images.Minecraft, 1920, .15, 2);

        public static ImageTest Nevermind =
            new ImageTest(Images.Nevermind, 1920, .15, 2);

        public static ImageTest Revolver =
            new ImageTest(Images.Revolver, 1920, .15, 2);

        public static ImageTest Time =
            new ImageTest(Images.Time, 1920, .15, 2);

        public static ImageTest[] All = new ImageTest[]
        {
            Califorinaction,
            Contra,
            EveryThingAllInTime,
            IsThisIt,
            Minecraft,
            Nevermind,
            Revolver,
            Time,
        };
    }
}
