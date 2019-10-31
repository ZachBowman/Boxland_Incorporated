// Boxland Incorporated
// Text Control for Sprite Fonts
// 2011-2018 Nightmare Games
// Zach Bowman

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {

  ////////////////////////////////////////////////////////////////////////////////

  public partial class Boxland : Game
    {
    public struct Letter
      {
      public char letter;
      public string file;
      public Texture2D sprite;
      }

    public void add_letter (char new_letter, string new_file)
      {
      Letter letter = new Letter ();
      letter.letter = new_letter;
      letter.sprite = Content.Load<Texture2D> (font_path + new_file);

      font.Add (letter);
      }

    ////////////////////////////////////////////////////////////////////////////////

    public class Text_Box
      {
      public Rectangle box = new Rectangle ();
      public string text;
      public float scale;
      public Color color;
      public Click click;

      public Text_Box (string text2, int x2, int y2, int width2, int height2, float scale2, Color color2, Click click2)
        {
        text = text2.ToLower ();
        box.X = x2;
        box.Y = y2;
        box.Width = width2;
        box.Height = height2;
        scale = scale2;
        color = color2;
        click = click2;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    const string font_path = "images\\menu\\font\\";
    List <Letter> font = new List <Letter> ();
    public List<Text_Box> text_boxes = new List<Text_Box> ();

    public void load_letter_sprites ()
      {
      add_letter ('a', "a");
      add_letter ('b', "b");
      add_letter ('c', "c");
      add_letter ('d', "d");
      add_letter ('e', "e");
      add_letter ('f', "f");
      add_letter ('g', "g");
      add_letter ('h', "h");
      add_letter ('i', "i");
      add_letter ('j', "j");
      add_letter ('k', "k");
      add_letter ('l', "l");
      add_letter ('m', "m");
      add_letter ('n', "n");
      add_letter ('o', "o");
      add_letter ('p', "p");
      add_letter ('q', "q");
      add_letter ('r', "r");
      add_letter ('s', "s");
      add_letter ('t', "t");
      add_letter ('u', "u");
      add_letter ('v', "v");
      add_letter ('w', "w");
      add_letter ('x', "x");
      add_letter ('y', "y");
      add_letter ('z', "z");
      add_letter ('.', "period");
      add_letter (',', "comma");
      add_letter ('!', "exclamation");
      add_letter ('?', "question");
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void draw_text_boxes ()
      {
      foreach (Text_Box box in text_boxes)
        {
        write (box);
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    // draw the text using the sprite font
    public void write (Text_Box box)
      {

      Rectangle r_draw;
      Vector2 size = new Vector2 { X = 0, Y = 0 };
      Letter letter;// = font[0];

      r_draw.X = box.box.X;
      r_draw.Y = box.box.Y;

      for (int c = 0; c < box.text.Length; c += 1)
        {
        letter = font.Find (l => l.letter == box.text[c]);

        if (letter.sprite != null)
          {
          r_draw.Width = Convert.ToInt32 (letter.sprite.Width * box.scale);
          r_draw.Height = Convert.ToInt32 (letter.sprite.Height * box.scale);

          if (box.box.Width > 0 && box.box.Height > 0 && r_draw.X + r_draw.Width > box.box.X + box.box.Width)
            {
            r_draw.X = box.box.X;
            r_draw.Y += r_draw.Height + Convert.ToInt32 (3 * box.scale);
            if (r_draw.Y + r_draw.Height > box.box.Y + box.box.Height) break;
            }

          spriteBatch.Draw (letter.sprite, r_draw, box.color);

          if (r_draw.X + r_draw.Width - box.box.X > size.X) size.X = r_draw.X + r_draw.Width - box.box.X;
          if (r_draw.Y + r_draw.Height - box.box.Y > size.Y) size.Y = r_draw.Y + r_draw.Height - box.box.Y;

          r_draw.X += r_draw.Width + Convert.ToInt32 (3 * box.scale);
          }
        else r_draw.X += Convert.ToInt32 (12 * box.scale);
        }

      // if box has no max size, use the final size for clickable bounding box
      if (box.box.Width == 0) box.box.Width = Convert.ToInt32 (size.X);
      if (box.box.Height == 0) box.box.Height = Convert.ToInt32 (size.Y);
      }

    ////////////////////////////////////////////////////////////////////////////////

    }
  }
