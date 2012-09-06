#region
using System;
using System.Collections.Generic;

#endregion

namespace SFMLStart.Data
{
    public static class Settings
    {
        #region Nested type: Assets
        public static class Assets
        {
            public static string Regex = @"(\s)|(//.*)|(\r\n)";
            public static char SeparatorGroup = ';';
            public static char SeparatorItem = ',';
            public static bool LoadImages = true;
            public static bool LoadTextures = true;
            public static bool LoadTilesets = true;
            public static bool LoadAnimations = true;
            public static bool LoadSounds = true;
            public static bool LoadMusic = true;
            public static List<string> ImagesExtensions = new List<string> {".png", ".jpg", ".bmp", ".jpeg"};
            public static List<string> TilesetExtensions = new List<string> {".tileset"};
            public static List<string> AnimationExtensions = new List<string> {".animation"};
            public static List<string> SoundsExtensions = new List<string> {".wav", ".ogg"};
            public static List<string> MusicExtensions = new List<string> {".wav", ".ogg"};
        }
        #endregion

        #region Nested type: Framerate
        public static class Framerate
        {
            public static int Limit = 60;
            public static bool IsLimited;
        }
        #endregion

        #region Nested type: Frametime
        public static class Frametime
        {
            public static float StaticValue = 1;
            public static bool IsStatic = false;
        }
        #endregion

        #region Nested type: Logging
        public static class Logging
        {
            public static bool IsEnabled = true;
        }
        #endregion

        #region Nested type: Paths
        public static class Paths
        {
            public static string Data = Environment.CurrentDirectory + @"/Data/";
            public static string Images = Environment.CurrentDirectory + @"/Data/Images/";
            public static string Tilesets = Environment.CurrentDirectory + @"/Data/Tilesets/";
            public static string Animations = Environment.CurrentDirectory + @"/Data/Animations/";
            public static string Sounds = Environment.CurrentDirectory + @"/Data/Sounds/";
            public static string Music = Environment.CurrentDirectory + @"/Data/Music/";
        }
        #endregion
    }
}