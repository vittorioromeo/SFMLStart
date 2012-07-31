#region
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

#endregion

namespace SFMLStart
{
    public class Camera
    {
        private readonly RenderWindow _renderWindow;

        public Camera(GameWindow mGameWindow, int mWidth, int mHeight)
        {
            Debug.Assert(mGameWindow != null);

            _renderWindow = mGameWindow.RenderWindow;
            View = new View(new FloatRect(0, 0, mWidth, mHeight));
        }

        internal View View { get; private set; }
        public Vector2f MousePosition { get { return _renderWindow.ConvertCoords(new Vector2i(Mouse.GetPosition(_renderWindow).X, Mouse.GetPosition(_renderWindow).Y), View); } }
        public Vector2f ConvertCoords(float mX, float mY) { return _renderWindow.ConvertCoords(new Vector2i((int) mX, (int) mY), View); }

        public void Resize(float mXOffset, float mYOffset, float mWidth, float mHeight)
        {
            Debug.Assert(mWidth > 0 && mHeight > 0);

            View = new View(new FloatRect(0, 0, mWidth, mHeight))
                   {
                       Viewport = new FloatRect(mXOffset/_renderWindow.Size.X,
                                                mYOffset/_renderWindow.Size.Y,
                                                mWidth/_renderWindow.Size.X,
                                                mHeight/_renderWindow.Size.Y)
                   };
        }

        public bool IsInView(Vector2f mPosition) { return mPosition.X <= View.Center.X + View.Size.X && (mPosition.X >= View.Center.X - View.Size.X && (mPosition.Y <= View.Center.Y + View.Size.Y && mPosition.Y >= View.Center.Y - View.Size.Y)); }
        public void Move(Vector2f mVector) { View.Move(mVector); }
        public void Zoom(float mZoomFactor) { View.Zoom(mZoomFactor); }
        public void CenterOn(Vector2f mPosition) { View.Center = mPosition; }
    }
}