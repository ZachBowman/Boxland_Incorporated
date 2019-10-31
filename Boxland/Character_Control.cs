// Boxland Incorporated
// Character Control Class
// 2011-2018 Nightmare Games
// Zach Bowman

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Character_Control : Game
    {
    public const int max_character_list = 5;
    public const int max_character_skins = 7;
    public double reach_distance;  // minimum distance to reach another object/character

    public List<Character> character = new List<Character> ();
    public Texture2D[,] character_sprite = new Texture2D[max_character_list, max_character_skins];

    public const int pow_sprite_width = 80;
    public const int pow_sprite_height = 80;

    public Character_Control (int tilesize)
      {
      reach_distance = tilesize;// *.9;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // give sprite transparent background
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

    // create new character
    public void add (Name type, int x, int y, int z)
      {
      Character c = new Character ();

      c.type = type;
      c.sprite = (int) type;
      c.dx = x;
      c.dy = y;
      c.dz = z;
      c.defaults ();

      character.Add (c);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // are these characters within arm's length
    public bool reach_character (int c1, int c2, List<Brush> brush)
      {
      bool reach = false;
      double x_distance, y_distance, z_distance, h_distance;
      double arm_x, arm_y, arm_z;

      x_distance = Math.Abs (character[c1].dx - character[c2].dx);
      y_distance = Math.Abs (character[c1].dy - character[c2].dy);
      z_distance = Math.Abs (character[c1].dz - character[c2].dz);
      h_distance = Math.Sqrt ((x_distance * x_distance) + (y_distance * y_distance));

      // if he's close enough to hit him
      if (h_distance <= reach_distance && z_distance <= reach_distance * 2)
        {
        reach = true;

        arm_x = character[c1].x;
        arm_y = character[c1].y;
        arm_z = character[c1].z;

        // if there are no walls between them
        for (int d = 0; d < h_distance; d += 1)
          {
          arm_x += 1 * Math.Cos (Convert.ToInt32 (character[c1].dir));
          if (point_in_brush (brush, Convert.ToInt16 (arm_x), Convert.ToInt16 (arm_y), Convert.ToInt16 (arm_z), true, false) >= 0) reach = false;

          arm_y += 1 * Math.Sin (character[c1].dir);
          if (point_in_brush (brush, Convert.ToInt16 (arm_x), Convert.ToInt16 (arm_y), Convert.ToInt16 (arm_z), true, false) >= 0) reach = false;
          }
        }

      return reach;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // can character 1 see character 2
    public bool sees_character (int c1, int c2, List<Brush> brush)
      {
      bool sees_character = true;
      double eye_x = character[c1].x;
      double eye_y = character[c1].y;
      double eye_z = character[c1].z + character[c1].height;
      double eye_dir = get_direction (character[c1].x, character[c1].y, character[c2].x, character[c2].y);
      double distance = distance2d (eye_x, eye_y, character[c2].x, character[c2].y);
      bool endloop = false;

      while (endloop == false)
        {
        eye_x += 4 * Math.Cos (eye_dir);
        eye_y += 4 * Math.Sin (eye_dir);
        if (point_in_brush (brush, Convert.ToInt32 (eye_x), Convert.ToInt32 (eye_y), Convert.ToInt32 (eye_z), true, true) >= 0) { sees_character = false; endloop = true; }
        if (distance2d (character[c1].x, character[c1].y, eye_x, eye_y) >= distance) { sees_character = true; endloop = true; }
        }

      return sees_character;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // is this point colliding with a brush
    int point_in_brush (List<Brush> brush, int x, int y, int z, bool solid_only, bool invisible_counts)
      {
      int b = 0;
      int clip = -1;

      while (clip == -1 && b < brush.Count)
        {
        if (x >= brush[b].x && x <= brush[b].x + brush[b].width &&
            y >= brush[b].y && y <= brush[b].y + brush[b].length &&
            z >= brush[b].z && z <= brush[b].z + brush[b].height)
          {
          clip = b;
          if (solid_only == true && !brush[b].solid) clip = -1;
          if (!invisible_counts && brush[b].top_texture_number == (int) Texture_Type.invisible) clip = -1;
          break;  // stop looking
          }
        b += 1;
        }
      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    double distance2d (double x1, double y1, double x2, double y2)
      {
      double h, x, y;

      x = x2 - x1;
      y = y2 - y1;
      h = Math.Sqrt ((x * x) + (y * y));

      if (h < 0) h = h * -1;  // absolute value

      return h;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // is this character facing toward this object
    public bool character_facing_object (Character c, Object o)
      {
      double radians_needed = get_direction (c.dx, c.dy, o.dx, o.dy);

      if (radians_needed + MathHelper.ToRadians (45) > MathHelper.ToRadians (360)
          && c.dir + MathHelper.ToRadians (360) <= radians_needed + MathHelper.ToRadians (45)
          && c.dir + MathHelper.ToRadians (360) >= radians_needed - MathHelper.ToRadians (45))
        return true;

      else if (radians_needed - MathHelper.ToRadians (45) < MathHelper.ToRadians (0)
          && c.dir <= radians_needed + MathHelper.ToRadians (45)
          && c.dir - MathHelper.ToRadians (360) >= radians_needed - MathHelper.ToRadians (45))
        return true;

      else if (c.dir <= radians_needed + MathHelper.ToRadians (45)
          && c.dir >= radians_needed - MathHelper.ToRadians (45))
        return true;

      else return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    double get_direction (double x1, double y1, double x2, double y2)
      {
      double dir_radians;
      double x_distance, y_distance;

      x_distance = x2 - x1;
      y_distance = y2 - y1;

      // get radians of direction
      if (x_distance > 0 && y_distance >= 0) dir_radians = Math.Atan (y_distance / x_distance);
      else if (x_distance > 0 && y_distance < 0) dir_radians = Math.Atan (y_distance / x_distance) + (2 * Math.PI);
      else if (x_distance < 0) dir_radians = Math.Atan (y_distance / x_distance) + Math.PI;
      else if (x_distance == 0 && y_distance > 0) dir_radians = MathHelper.ToRadians (90);
      else if (x_distance == 0 && y_distance < 0) dir_radians = MathHelper.ToRadians (270);
      else dir_radians = 0;  // x_distance = 0, y_distance = 0

      return dir_radians;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public bool active (int c)
      {
      if (c > -1 && c < character.Count && character[c].action != Action.knocked_out) return true;
      else return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void blink (int c)
      {
      Random rnd = new Random ();

      if (rnd.Next (0, 50) == 0)
        {
        character[c].blinking = true;
        character[c].anim_frame_sequence = 0;
        character[c].max_anim_frame_sequence = 1;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    // create an impact sprite effect for melee combat
    public void pow (int c, bool low, bool behind)
      {
      Random rnd = new Random ();

      character[c].pow1.opacity = .75f;
      character[c].pow1.x = -1 * (pow_sprite_width / 2);
      character[c].pow1.y = 0 - character[c].sprite_height;
      character[c].pow1.behind = behind;
      character[c].pow1.color = rnd.Next (0, 3);
      character[c].pow1.shape = rnd.Next (0, 5);
      }
    }
  }
