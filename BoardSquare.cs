using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SplashKitSDK;

namespace Chess
{
    public class BoardSquare
    {
        public int SquareNumber { get; set; }
        public int PieceOnSquare { get; set; }
        public Rectangle Bounds { get; set; }
        public Color BackgroundColor { get; set; }
        public Bitmap? PieceImage { get; set; }
        
        public BoardSquare(int squareNumber, double x, double y, double size)
        {
            SquareNumber = squareNumber;
            PieceOnSquare = 0;
            Bounds = new Rectangle() { X = x, Y = y, Width = size, Height = size };
            BackgroundColor = Color.White;
            PieceImage = null;
        }
        
        public bool ContainsPoint(Point2D point)
        {
            return point.X >= Bounds.X && point.X <= Bounds.X + Bounds.Width &&
                   point.Y >= Bounds.Y && point.Y <= Bounds.Y + Bounds.Height;
        }
        
        public Point2D Center
        {
            get
            {
                return new Point2D()
                {
                    X = Bounds.X + Bounds.Width / 2,
                    Y = Bounds.Y + Bounds.Height / 2
                };
            }
        }
        
        public void SetPieceImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                PieceImage = SplashKit.LoadBitmap(Path.GetFileNameWithoutExtension(imagePath), imagePath);
            }
            else
            {
                PieceImage = null;
            }
        }
    }
}
