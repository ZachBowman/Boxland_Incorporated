// Shapes
// Zach Bowman
// 2010 Nightmare Games

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Boxland
  {
  public class Shapes
    {

    ////////////////////////////////////////////////////////////////////////////////

    public void line (SpriteBatch spriteBatch, int x1, int y1, int x2, int y2, Texture2D pixel, float opacity)
      {
      Vector2 v;
      double dir_radians;
      double xmove, ymove, double_x, double_y;
      double distance, x_distance, y_distance;
      //float temp_fade;

      distance = Convert.ToInt16(Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1))));
      x_distance = x2 - x1;
      y_distance = y2 - y1;

      if (x_distance > 0 && y_distance >= 0) dir_radians = Math.Atan(y_distance / x_distance);
      else if (x_distance > 0 && y_distance < 0) dir_radians = Math.Atan(y_distance / x_distance) + (2 * Math.PI);
      else if (x_distance < 0) dir_radians = Math.Atan(y_distance / x_distance) + Math.PI;
      else if (x_distance == 0 && y_distance > 0) dir_radians = MathHelper.ToRadians(90);//Math.PI / 2;
      else if (x_distance == 0 && y_distance < 0) dir_radians = MathHelper.ToRadians(270);//-1 * Math.PI / 2;
      else dir_radians = 0;  // x_distance = 0, y_distance = 0

      double_x = x1;
      double_y = y1;
      xmove = 1 * Math.Cos(dir_radians);
      ymove = 1 * Math.Sin(dir_radians);

      for (int d = 0; d < distance; d++)
        {
        double_x += xmove;
        double_y += ymove;
        v.X = Convert.ToSingle(double_x);
        v.Y = Convert.ToSingle(double_y);
        //temp_fade = Convert.ToSingle (opacity / 255);
        spriteBatch.Draw (pixel, v, Color.White * opacity);
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void rectangle (SpriteBatch spriteBatch, int x1, int y1, int x2, int y2, Texture2D pixel, float opacity)
      {
      int xa, ya, xb, yb, xc, yc, xd, yd;

      xa = Convert.ToInt16(x1);
      ya = Convert.ToInt16(y1);
      xb = Convert.ToInt16(x2);
      yb = Convert.ToInt16(y1);
      xc = Convert.ToInt16(x2);
      yc = Convert.ToInt16(y2);
      xd = Convert.ToInt16(x1);
      yd = Convert.ToInt16(y2);

      line (spriteBatch, xa, ya, xb, yb, pixel, opacity);
      line (spriteBatch, xb, yb, xc, yc, pixel, opacity);
      line (spriteBatch, xc, yc, xd, yd, pixel, opacity);
      line (spriteBatch, xd, yd, xa, ya, pixel, opacity);
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void rectangle_filled (SpriteBatch spriteBatch, int x1, int y1, int x2, int y2, Texture2D pixel, float opacity)
      {
      Rectangle r;

      r.X = x1;
      r.Y = y1;
      r.Width = x2 - x1;
      r.Height = y2 - y1;

      spriteBatch.Draw (pixel, r, Color.White * opacity);
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void test_pattern (SpriteBatch spriteBatch, Texture2D pixel, float opacity)
      {
      line (spriteBatch, 400, 400, 200, 200, pixel, opacity);
      line (spriteBatch, 400, 400, 300, 200, pixel, opacity);
      line (spriteBatch, 400, 400, 400, 200, pixel, opacity);
      line (spriteBatch, 400, 400, 500, 200, pixel, opacity);
      line (spriteBatch, 400, 400, 600, 200, pixel, opacity);
      line (spriteBatch, 400, 400, 600, 300, pixel, opacity);
      line (spriteBatch, 400, 400, 600, 400, pixel, opacity);
      line (spriteBatch, 400, 400, 600, 500, pixel, opacity);
      line (spriteBatch, 400, 400, 600, 600, pixel, opacity);
      line (spriteBatch, 400, 400, 500, 600, pixel, opacity);
      line (spriteBatch, 400, 400, 400, 600, pixel, opacity);
      line (spriteBatch, 400, 400, 300, 600, pixel, opacity);
      line (spriteBatch, 400, 400, 200, 600, pixel, opacity);
      line (spriteBatch, 400, 400, 200, 500, pixel, opacity);
      line (spriteBatch, 400, 400, 200, 400, pixel, opacity);
      line (spriteBatch, 400, 400, 200, 300, pixel, opacity);
      }

    ////////////////////////////////////////////////////////////////////////////////

    }
  }
