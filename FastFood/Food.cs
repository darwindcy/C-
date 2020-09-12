using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Fastfood
{
    class Food
    {
        public int X { get; set; }
        public int Y { get; set; }

        public String name { get; set; } 
        public Food(int x, int y)
        {
            X = x;
            Y = y;

            Random rnd = new Random();
            string[] foods = { "pizza", "chicken", "fries", "hamburger", "icecream", "sandwich", "pickle"};

            name = foods[rnd.Next(foods.Length)].ToLower();
        }
        public Food(int x, int y, String s)
        {
            X = x;
            Y = y;
            name = s.ToLower();
        }

        public void Draw(System.Drawing.Graphics screen)
        {
            switch (name)
            {
                case "pizza":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Pizza), X, Y);
                    break;

                case "chicken":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Chicken), X, Y);
                    break;

                case "fries":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Fries), X, Y);
                    break;

                case "hamburger":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Hamburger), X, Y);
                    break;

                case "icecream":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.icecream), X, Y);
                    break;

                case "sandwich":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Sandwich), X, Y);
                    break;

                case "pickle":
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Pickle), X, Y);
                    break;
                default:
                    screen.DrawImage(Resource1.GetBitmap(Resource1.BitmapResources.Mouthopen), X, Y);
                    break;
            }
        }
    }
}
