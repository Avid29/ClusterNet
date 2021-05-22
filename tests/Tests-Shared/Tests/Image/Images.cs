namespace Tests.Tests.Image
{
    public static class Images
    {
        public const string Califorinaction_Name = "Red Hot Chili Peppers - Califorinaction";
        private const string Califorinaction_Url = "https://th.bing.com/th/id/OIP.F21MZzafrllhDLK-SZ4jhQHaHW?pid=Api&rs=1";

        public const string Contra_Name = "Vampie Weekend - Contra";
        private const string Contra_Url = "https://th.bing.com/th/id/OIP.RgT_6lZp_G-peHs97hqMNAHaHa?pid=Api&rs=1";

        public const string EveryThingAllInTime_Name = "Band of Horses - Everything All The Time";
        public const string EveryThingAllInTime_Url = "https://fanart.tv/fanart/music/07b6020a-c539-4d68-aeef-f159f3befc76/albumcover/everything-all-the-time-52c5e32f129ab.jpg";

        public const string IsThisIt_Name = "The Strokes - Is This It";
        private const string IsThisIt_Url = "https://media.pitchfork.com/photos/5929a58b13d1975652138f9b/1:1/w_600/c1b895b7.jpg";

        public const string Minecraft_Name = "C418 - Minecraft";
        private const string Minecraft_Url = "https://f4.bcbits.com/img/a3390257927_10.jpg";

        public const string Nevermind_Name = "Nirvana - Nevermind";
        private const string Nevermind_Url = "https://th.bing.com/th/id/OIP.eJ971LxYKRGqfkPXbzRkGAHaHa?pid=Api&rs=1";

        public const string Revolver_Name = "The Beatles - Revolver";
        private const string Revolver_Url = "https://neonmoderntimes.files.wordpress.com/2014/07/beatles-revolver-cover-art.jpg";

        public const string Time_Name = "Pink Floyd - Time";
        private const string Time_Url = "https://s-media-cache-ak0.pinimg.com/736x/70/7c/98/707c98df5d2cffde6d4f755e3008771b.jpg";

        public static Image Califorinaction = new Image(Califorinaction_Name, Califorinaction_Url);
        public static Image Contra = new Image(Contra_Name, Contra_Url);
        public static Image EveryThingAllInTime = new Image(EveryThingAllInTime_Name, EveryThingAllInTime_Url);
        public static Image IsThisIt = new Image(IsThisIt_Name, IsThisIt_Url);
        public static Image Minecraft = new Image(Minecraft_Name, Minecraft_Url);
        public static Image Nevermind = new Image(Nevermind_Name, Nevermind_Url);
        public static Image Revolver = new Image(Revolver_Name, Revolver_Url);
        public static Image Time = new Image(Time_Name, Time_Url);

        public static Image[] All = 
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
