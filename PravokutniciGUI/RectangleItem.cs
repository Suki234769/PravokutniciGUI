using System.Drawing;

namespace PravokutniciGUI
{
    // UTORAK – model za spremanje pravokutnika u JSON
    public class RectangleItem
    {
        // Pozicija i veličina pravokutnika
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Boja spremljena kao ARGB integer
        public int ColorArgb { get; set; }

        // Pomoćna metoda – pretvori u Rectangle
        public Rectangle ToRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        // Pomoćna metoda – pretvori ARGB u Color
        public Color ToColor()
        {
            return Color.FromArgb(ColorArgb);
        }

        // Statička metoda za kreiranje iz postojećih podataka
        public static RectangleItem From(Rectangle rect, Color color)
        {
            return new RectangleItem
            {
                X = rect.X,
                Y = rect.Y,
                Width = rect.Width,
                Height = rect.Height,
                ColorArgb = color.ToArgb()
            };
        }
    }
}
