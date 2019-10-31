using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Object_Control : Game
    {
    public const int max_object_list = 16;
    public const int max_object_skins = 4;
    public const int max_objects = 50;
    public List<Object> obj = new List<Object> ();
    public Texture2D[,] object_sprite = new Texture2D[max_object_list, max_object_skins];

    ////////////////////////////////////////////////////////////////////////////////

    //public void load_sprites (GraphicsDevice GraphicsDevice)
    //  {
      //object_sprite[(int) Object_Type.shirt_yellow, 0] = Content.Load<Texture2D> ("");
      //object_sprite[(int) Object_Type.shirt_yellow, 1] = Content.Load<Texture2D> ("");
      //object_sprite[(int) Object_Type.shirt_red, 0] = Content.Load<Texture2D> ("");
      //object_sprite[(int) Object_Type.shirt_white, 0] = Content.Load<Texture2D> ("");//objects\\shirt_ice0_ink.png
      //object_sprite[(int) Object_Type.shirt_white, 1] = Content.Load<Texture2D> ("");//objects\\shirt_ice1_ink.png
      //object_sprite[(int) Object_Type.shirt_fushia, 0] = Content.Load<Texture2D> ("");//objects\\shirt_magnet0_ink.png
      //object_sprite[(int) Object_Type.shirt_fushia, 1] = Content.Load<Texture2D> ("");//objects\\shirt_magnet1_ink.png
      //object_sprite[(int) Object_Type.shirt_teal, 0] = Content.Load<Texture2D> ("");//objects\\shirt_electric.png
      //object_sprite[(int) Object_Type.rock, 0] = Content.Load<Texture2D> ("");//objects\\food_hotdog_ink.png
      //object_sprite[(int) Object_Type.rock, 1] = Content.Load<Texture2D> ("");//objects\\food_hamburger_ink.png
      //object_sprite[(int) Object_Type.rock, 2] = Content.Load<Texture2D> ("");//objects\\food_pizza_ink.png
      //object_sprite[(int) Object_Type.rock, 3] = Content.Load<Texture2D> ("");//objects\\rock_white_test.png
      //object_sprite[(int) Object_Type.key, 0] = Content.Load<Texture2D> ("");//objects\\key1_ink.png
      //object_sprite[(int) Object_Type.health, 0] = Content.Load<Texture2D> ("");//objects\\health0_ink.png
      //object_sprite[(int) Object_Type.health, 1] = Content.Load<Texture2D> ("");//objects\\health1_ink.png
      //object_sprite[(int) Object_Type.health, 2] = Content.Load<Texture2D> ("");//objects\\health2_ink.png
      //object_sprite[(int) Object_Type.health, 3] = Content.Load<Texture2D> ("");//objects\\health3_ink.png
      //object_sprite[(int) Object_Type.coin, 0] = Content.Load<Texture2D> ("");//objects\\coin0_ink.png
      //object_sprite[(int) Object_Type.scrap_metal, 0] = Content.Load<Texture2D> ("");//objects\\scrap_metal_ink.png
      //object_sprite[(int) Object_Type.energy, 0] = Content.Load<Texture2D> ("");//objects\\energy0_ink.png
      //}

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

        if (object_type == (int) Object_Type.health)
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

    }
  }
