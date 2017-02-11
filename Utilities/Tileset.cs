#region
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFMLStart.Vectors;

#endregion

namespace SFMLStart.Utilities
{
    public class Tileset
    {
        public Tileset(int mTileWidth, int mTileHeight, int mSeparation)
        {
            Labels = new Dictionary<string, SSVector2I>();

            TileWidth = mTileWidth;
            TileHeight = mTileHeight;
            Separation = mSeparation;
        }

        public IntRect this[int mX, int mY] { get { return GetTextureRect(mX, mY); } }
        public IntRect this[string mLabel] { get { return GetTextureRect(mLabel); } }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int Separation { get; set; }

        public Dictionary<string, SSVector2I> Labels { get; set; }

        public Sprite GetSprite(int mX, int mY, Texture mTexture)
        {
            return new Sprite(mTexture)
                   {
                       TextureRect = GetTextureRect(mX, mY),
                       Origin = new Vector2f(GetTextureRect(mX, mY).Width/2f, GetTextureRect(mX, mY).Height/2f)
                   };
        }
        public Sprite GetSprite(string mLabel, Texture mTexture)
        {
            return new Sprite(mTexture)
                   {
                       TextureRect = GetTextureRect(mLabel),
                       Origin = new Vector2f(GetTextureRect(mLabel).Width/2f, GetTextureRect(mLabel).Height/2f)
                   };
        }

        public IntRect GetTextureRect(int mX, int mY) { return new IntRect(mX*TileWidth + Separation*mX, mY*TileHeight + Separation*mY, TileWidth, TileHeight); }
        public IntRect GetTextureRect(string mLabel)
        {
#if CHECK_ASSETS
            if(mLabel == null || !Labels.ContainsKey(mLabel))
            {
                Utils.Log(string.Format("Tileset: missing label {0}", mLabel), "Asset error");
                return new IntRect(0,0,16,16);
            }
            #endif

            var x = Labels[mLabel].X;
            var y = Labels[mLabel].Y;
            return new IntRect(x*TileWidth + Separation*x, y*TileHeight + Separation*y, TileWidth, TileHeight);
        }

        public void SetLabel(string mLabel, int mIndexX, int mIndexY) { Labels.Add(mLabel, new SSVector2I(mIndexX, mIndexY)); }
    }
}