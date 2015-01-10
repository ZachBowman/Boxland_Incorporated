using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Object_Control
    {
    public const int max_object_list = 16;
    public const int max_object_skins = 4;
    public const int max_objects = 50;
    public List<Object> obj = new List<Object> ();
    public Texture2D[,] object_sprite = new Texture2D[max_object_list, max_object_skins];

    public enum O
      {
      NONE,
      // do not move or change shirt values (used for richard skins)
      SHIRT_YELLOW,     // 1
      SHIRT_RED,      // 2
      SHIRT_WHITE,       // 3
      SHIRT_PURPLE,    // 4
      SHIRT_BLUE,  // 5
      /////////////////////////////////////////////////////////////
      HEALTH,
      FOOD_HOTDOG,
      ROCK,
      ROCK_BROWN,
      ROCK_RED,
      ROCK_WHITE,
      KEYCARD,
      COIN,
      SCRAP_METAL,
      ENERGY
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void load_sprites (GraphicsDevice GraphicsDevice)
      {
      object_sprite[(int) O.SHIRT_YELLOW, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_power0_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_YELLOW, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_power1_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_RED, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_fire0_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_WHITE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_ice0_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_WHITE, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_ice1_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_PURPLE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_magnet0_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_PURPLE, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_magnet1_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SHIRT_BLUE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_electric.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.ROCK, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\food_hotdog_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.ROCK_BROWN, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\food_hamburger_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.ROCK_RED, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\food_pizza_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.ROCK_WHITE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\rock_white_test.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.KEYCARD, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\key1_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.HEALTH, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health0_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.HEALTH, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health1_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.HEALTH, 2] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health2_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.HEALTH, 3] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health3_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.COIN, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\coin0_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.SCRAP_METAL, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\scrap_metal_ink.png", FileMode.Open, FileAccess.Read));
      object_sprite[(int) O.ENERGY, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\energy0_ink.png", FileMode.Open, FileAccess.Read));

      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_YELLOW, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_YELLOW, 1], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_RED, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_WHITE, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_WHITE, 1], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_PURPLE, 0], new Color (0, 255, 0, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_PURPLE, 1], new Color (0, 255, 0, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SHIRT_BLUE, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.ROCK, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.ROCK_BROWN, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.ROCK_RED, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.ROCK_WHITE, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.KEYCARD, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.HEALTH, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.HEALTH, 1], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.HEALTH, 2], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.HEALTH, 3], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.COIN, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.SCRAP_METAL, 0], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (object_sprite[(int) O.ENERGY, 0], new Color (255, 0, 255, 255));
      }

    ////////////////////////////////////////////////////////////////////////////////

    void ConvertToPremultipliedAlpha (Texture2D texture, Color? colorKey)
      {
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color> (data, 0, data.Length);
      if (colorKey.HasValue)
        {
        for (int i = 0; i < data.Length; i += 1)
          {
          if (data[i] == colorKey)
            {
            data[i] = Color.Transparent;
            }
          else
            {
            data[i] = new Color (new Vector4 (data[i].ToVector3 () * (data[i].A / 255f), (data[i].A / 255f)));
            }
          }
        }
      else
        {
        for (int i = 0; i < data.Length; i += 1)
          {
          data[i] = new Color (new Vector4 (data[i].ToVector3 () * (data[i].A / 255f), (data[i].A / 255f)));
          }
        }
      texture.SetData<Color> (data, 0, data.Length);
      }

    ////////////////////////////////////////////////////////////////////////////////

    public int add (int object_type, int x, int y, int z)
      {
      Random rnd = new Random ();
      //int o = -1;
      //int o2;

      //if (total_objects < 0) total_objects = 0;

      //if (total_objects >= max_objects)
      //{
      //o2 = 0;
      //while (o2 < max_objects && obj[o2].essential == true) o2 += 1;
      //if (o2 < max_objects && obj[o2].essential == false) destroy_object (o2);
      //destroy_object (0);//total_objects - 1);
      //destroy_object (total_objects);
      //}

      //else
      if (obj.Count < max_objects)
        {
        Object o = new Object ();

        o.type = object_type;
        o.dx = x;
        o.dy = y;
        o.dz = z;
        o.x = x;
        o.y = y;
        o.z = z;

        // set defaults
        o.width = object_sprite[o.type, 0].Width;
        o.length = object_sprite[o.type, 0].Width;
        o.height = object_sprite[o.type, 0].Height;
        o.dir = MathHelper.ToRadians (270);
        o.velocity = 0;
        o.source = -1;
        o.essential = false;
        o.destroyed = false;
        o.skin = 0;

        if (object_type == (int) Object_Control.O.HEALTH)
          {
          o.skin = rnd.Next (0, 4);
          }

        //total_objects += 1;
        obj.Add (o);
        //total_draw_slots += 1;
        }

      //return o;
      return obj.Count - 1;
      }

    ////////////////////////////////////////////////////////////

    // needs access to fixture_control, brush_control
    
    //public void draw (int o)
    //  {
    //  //o = draw_list[l].id;
    //  int tx, ty, tz;
    //  bool endloop, endloop2;

    //  // draw floor shadow
    //  tx = obj[o].x;
    //  ty = obj[o].y;
    //  tz = obj[o].z;

    //  endloop = false;
    //  while (endloop == false && tz >= 0)
    //    {
    //    endloop2 = false;
    //    b = 0;
    //    f = 0;
    //    while (endloop2 == false && b < brush.Count)
    //      {
    //      if (tx >= brush[b].x && tx <= brush[b].x + brush[b].width &&
    //          ty >= brush[b].y && ty <= brush[b].y + brush[b].length &&
    //          tz == brush[b].z + brush[b].height) { endloop2 = true; endloop = true; }
    //      b += 1;
    //      }
    //    while (endloop2 == false && f < fixture_control.fixture.Count)//total_fixtures)
    //      {
    //      if (tx >= fixture_control.fixture[f].x && tx <= fixture_control.fixture[f].x + fixture_control.fixture[f].width &&
    //          ty >= fixture_control.fixture[f].y && ty <= fixture_control.fixture[f].y + fixture_control.fixture[f].length &&
    //          tz == fixture_control.fixture[f].z + fixture_control.fixture[f].height) { endloop2 = true; endloop = true; }
    //      f += 1;
    //      }
    //    tz -= 1;
    //    }

    //  distance = object_control.obj[o].z - tz;
    //  temp_fade = Convert.ToSingle (.3f - (distance / 300f));
    //  if (temp_fade < .1f) temp_fade = .1f;
    //  shadow_scale = 1.0 + (distance / 128.0);

    //  r_source.X = 0;
    //  r_source.Y = 0;
    //  r_source.Width = object_control.object_sprite[object_control.obj[o].type, 0].Width;
    //  r_source.Height = object_control.object_sprite[object_control.obj[o].type, 0].Height;
    //  r_draw.X = tx + screen.scroll_x;
    //  r_draw.Y = (screen.height - ty) - (tz / 2) + screen.scroll_y - 3;
    //  r_draw.Width = Convert.ToInt32 (object_control.object_sprite[object_control.obj[o].type, 0].Width * shadow_scale);
    //  r_draw.Height = Convert.ToInt32 (object_control.object_sprite[object_control.obj[o].type, 0].Height / 4 * shadow_scale);
    //  v_origin.X = object_control.object_sprite[object_control.obj[o].type, 0].Width / 2;
    //  v_origin.Y = 0;
    //  spriteBatch.Draw (object_control.object_sprite[object_control.obj[o].type, object_control.obj[o].skin], r_draw, r_source, Color.Black * temp_fade, MathHelper.ToRadians (0), v_origin, SpriteEffects.FlipVertically, 0);

    //  //      // draw object
    //  v_draw.X = object_control.obj[o].x - (object_control.obj[o].width / 2) + screen.scroll_x;
    //  v_draw.Y = (screen.height - object_control.obj[o].y) - object_control.obj[o].height - (object_control.obj[o].z / 2) + screen.scroll_y;
    //  spriteBatch.Draw (object_control.object_sprite[object_control.obj[o].type, object_control.obj[o].skin], v_draw, Color.White);

    //  if (debug == true)
    //    {
    //    // draw object id over object (remove later)
    //    v_draw2.X = v_draw.X + (object_control.object_sprite[object_control.obj[o].type, 0].Width / 2) - 5;
    //    v_draw2.Y = v_draw.Y - 15;
    //    spriteBatch.DrawString (debug_font, Convert.ToString (o), v_draw2, Color.White);
    //    }
    //  }
    }
  }
