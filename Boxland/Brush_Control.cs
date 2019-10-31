// Boxland Incorporated
// Brush Control Class
// 2011-2018 Nightmare Games
// Zach Bowman

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  // brushes are the cubes that compose the game world
  public class Brush_Control : Game
    {
    public class Tile
      {
      public Texture2D texture;
      public string name;
      public string map_symbol;
      public string set;

      //void ConvertToPremultipliedAlpha (Color? colorKey)
      //  {
      //  Color[] data = new Color[texture.Width * texture.Height];
      //  texture.GetData<Color> (data, 0, data.Length);

      //  if (colorKey.HasValue)
      //    {
      //    for (int i = 0; i < data.Length; i += 1)
      //      {
      //      if (data[i] == colorKey) data[i] = Color.Transparent;
      //      else data[i] = new Color (new Vector4 (data[i].ToVector3 () * (data[i].A / 255f), (data[i].A / 255f)));
      //      }
      //    }
      //  else
      //    {
      //    for (int i = 0; i < data.Length; i += 1)
      //      data[i] = new Color (new Vector4 (data[i].ToVector3 () * (data[i].A / 255f), (data[i].A / 255f)));
      //    }
      //  texture.SetData<Color> (data, 0, data.Length);
      //  }
      }

    ////////////////////////////////////////////////////////////////////////////////

    public List <Brush> brush = new List<Brush> ();
    public const int max_brushes = 625;
    public Tile[] tile = new Tile[99];
    int tilesize;
    List<int> tile_lookup = new List<int> ();
    
    public Brush_Control (int new_tilesize)
      {
      tilesize = new_tilesize;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public int find_tile_from_symbol (string symbol)
      {
      for (int t = 0; t < tile.Length; t += 1)
        {
        if (tile[t].map_symbol == symbol) return t;
        }
      return -1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // create new brush
    public void add (int top_texture_number, int front_texture_number, int x, int y, int z, int width, int length, int height)
      {
      int q;
      int top_offset_x = 0;
      int top_offset_y = 0;
      int front_offset_x;
      int front_offset_y;
      Brush b = new Brush ();

      // defaults        
      b.x = x;
      b.y = y;
      b.z = z;
      b.dx = x;
      b.dy = y;
      b.dz = z;
      b.width = width;
      b.length = length;
      b.height = height;
      b.moveable = false;
      b.weight = 100;
      b.ext_x_velocity = 0;
      b.ext_y_velocity = 0;
      b.ext_z_velocity = 0;
      b.moving_north = false;
      b.moving_south = false;
      b.moving_west = false;
      b.moving_east = false;
      b.top_sticker = -1;
      b.front_sticker = -1;
      b.gateway = -1;     // does not load a new level on collision
      b.temperature = 70;
      b.transparent = false;
      b.electric = false;

      // texture on top of brush
      b.top_texture_number = top_texture_number;

      // top texture offsets in texture sheet
      if (top_texture_number != (int) Texture_Type.invisible)
        {
        for (q = 0; q < x; q += tilesize)
          {
          top_offset_x += tilesize;
          if (top_offset_x + tilesize > tile[top_texture_number].texture.Width) top_offset_x = 0;
          }
        top_offset_y = tile[top_texture_number].texture.Height - tilesize;
        for (q = 0; q < y; q += tilesize)
          {
          top_offset_y -= tilesize;
          if (top_offset_y < 0) top_offset_y = tile[top_texture_number].texture.Height - tilesize;
          }
        }
      b.top_texture_offset_x = top_offset_x;
      b.top_texture_offset_y = top_offset_y;

      // texture on front of brush
      b.front_texture_number = front_texture_number;

      // front texture offsets in texture sheet
      front_offset_x = top_offset_x;
      front_offset_y = top_offset_y + length;
      if (front_texture_number != (int) Texture_Type.invisible)
        {
        for (q = 0; q < x; q += tilesize)
          {
          front_offset_x += tilesize;
          if (front_offset_x + tilesize > tile[front_texture_number].texture.Width) front_offset_x = 0;
          }
        front_offset_y = tile[front_texture_number].texture.Height - tilesize;
        for (q = 0; q < z; q += tilesize)
          {
          front_offset_y -= tilesize;
          if (front_offset_y < 0) front_offset_y = tile[front_texture_number].texture.Height - tilesize;
          }
        front_offset_y += tilesize;
        if (front_offset_y + tilesize > tile[front_texture_number].texture.Height) front_offset_y = 0;
        }

      b.front_texture_offset_x = front_offset_x;
      b.front_texture_offset_y = front_offset_y;

      // brush traits based on texture
      if (top_texture_number == (int) Texture_Type.invisible) b.transparent = true;
      else if (top_texture_number == (int) Texture_Type.box_banded_test) b.moveable = true;
      else if (top_texture_number == (int) Texture_Type.box_ice_test)
        {
        b.moveable = true;
        b.transparent = true;
        }
      else if (top_texture_number == (int) Texture_Type.box_metal_test) b.moveable = true;
      else if (top_texture_number == (int) Texture_Type.box_wood) b.moveable = true;
      else if (top_texture_number == (int) Texture_Type.door_red_v_top_closed_test)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) Texture_Type.door_yellow_v_top_closed_test)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) Texture_Type.door_green_v_top_closed_test)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) Texture_Type.door_blue_v_top_closed_test)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) Texture_Type.door_red_h_top_closed_test) b.electric = true;
      else if (top_texture_number == (int) Texture_Type.door_yellow_h_top_closed_test) b.electric = true;
      else if (top_texture_number == (int) Texture_Type.door_green_h_top_closed_test) b.electric = true;
      else if (top_texture_number == (int) Texture_Type.door_blue_h_top_closed_test) b.electric = true;
      else if (top_texture_number == (int) Texture_Type.floor_grate_test) b.transparent = true;
      else if (top_texture_number == (int) Texture_Type.gateway_v_top_test) b.transparent = true;
      else if (top_texture_number == (int) Texture_Type.gateway_h_top_test) b.transparent = true;

      brush.Add (b);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // check if point collides with a brush
    public int point_in_brush (int x, int y, int z, bool solid_only, bool invisible_counts)
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

    // check if two brushes are colliding
    public int brush_in_brush (int b1)
      {
      int b2 = 0;
      int clip = -1;

      while (clip == -1 && b2 < brush.Count)
        {
        if (b1 != b2 && brush[b2].solid &&
            brush[b1].x + brush[b1].width > brush[b2].x && brush[b1].x < brush[b2].x + brush[b2].width &&
            brush[b1].y + brush[b1].length > brush[b2].y && brush[b1].y < brush[b2].y + brush[b2].length &&
            brush[b1].z + brush[b1].height > brush[b2].z && brush[b1].z < brush[b2].z + brush[b2].height)
          {
          clip = b2;
          }
        b2 += 1;
        }
      return clip;
      }

    ///////////////////////////////////////////////////////////////////////////////

    // check if anothong brush is in a specific grid location relative to this brush
    public int brush_around_brush (int b, int x_grid, int y_grid, int z_grid)
      {
      int x = brush[b].x + (tilesize / 2) + (tilesize * x_grid);
      int y = brush[b].y + (tilesize / 2) + (tilesize * y_grid);
      int z = brush[b].z + (tilesize / 2) + (tilesize * z_grid);

      return point_in_brush (x, y, z, true, true);
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_north_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x + (tilesize / 2), Convert.ToInt16 (check_brush.y + (tilesize * 1.5)), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_south_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x + (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_east_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (Convert.ToInt16 (check_brush.x + (tilesize * 1.5)), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_west_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x - (tilesize / 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_northwest_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x - (tilesize / 2), Convert.ToInt16 (check_brush.y + (tilesize * 1.5)), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_northeast_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (Convert.ToInt16 (check_brush.x + tilesize * 1.5), Convert.ToInt16 (check_brush.y + (tilesize * 1.5)), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_southwest_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x - (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_southeast_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (Convert.ToInt16 (check_brush.x + tilesize * 1.5), check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x + (tilesize / 2), Convert.ToInt16 (check_brush.y + (tilesize / 2)), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_north_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x + (tilesize / 2), Convert.ToInt32 (check_brush.y + (tilesize * 1.5)), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_south_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x + (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_east_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (Convert.ToInt16 (check_brush.x + (tilesize * 1.5)), check_brush.y + (tilesize / 2), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_west_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x - (tilesize / 2), check_brush.y + (tilesize / 2), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_northeast_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (Convert.ToInt16 (check_brush.x + (tilesize * 1.5)), Convert.ToInt16 (check_brush.y + (tilesize * 1.5)), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_northwest_of_brush (Brush check_brush, bool moving_counts, bool transparent_counts)
      {
      int clip = point_in_brush (check_brush.x - (tilesize / 2), Convert.ToInt16 (check_brush.y + (tilesize * 1.5)), check_brush.z + tilesize + (tilesize / 2), true, true);
      if (clip > -1 && brush[clip].moving && !moving_counts) return -1;
      else if (clip > -1 && brush[clip].transparent && !transparent_counts) return -1;
      else return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_below_south_of_brush (Brush check_brush)
      {
      return point_in_brush (check_brush.x + (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z - (tilesize / 2), true, true);
      }

    ////////////////////////////////////////////////////////////////////////////////

    // check if this brush is colliding with a fixture
    public int brush_in_fixture (Brush b, List<Fixture> fixture, bool solid_only)
      {
      int f = 0;
      int clip = -1;

      while (clip == -1 && f < fixture.Count)
        {
        if (b.x + b.width > fixture[f].x && b.x < fixture[f].x + fixture[f].width
            && b.y + b.length > fixture[f].y && b.y < fixture[f].y + fixture[f].length
            && b.z + b.height > fixture[f].z && b.z < fixture[f].z + fixture[f].height - 1)
          {
          if (solid_only == true && fixture[f].solid == false)
            {
            clip = -1;
            if (fixture[f].type == Fixture_Type.laser_horizontal_green_test) fixture[f].powered = true;
            }
          else clip = f;
          }
        f += 1;
        }

      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // decide which walls are casting shadows on this floor
    public void calculate_top_shadows (Brush check_brush)
      {
      check_brush.top_shadow_north = false;
      check_brush.top_shadow_south = false;
      check_brush.top_shadow_east = false;
      check_brush.top_shadow_west = false;
      check_brush.top_shadow_northeast = false;
      check_brush.top_shadow_northwest = false;

      if (brush_above_brush (check_brush, false, false) > -1) return;
      else
        {
        if (brush_above_north_of_brush (check_brush, false, false) > -1) check_brush.top_shadow_north = true;
        if (brush_above_south_of_brush (check_brush, false, false) > -1) check_brush.top_shadow_south = true;
        if (brush_above_east_of_brush (check_brush, false, false) > -1) check_brush.top_shadow_east = true;
        if (brush_above_west_of_brush (check_brush, false, false) > -1) check_brush.top_shadow_west = true;
        if (brush_above_northeast_of_brush (check_brush, false, false) > -1
            && brush_above_north_of_brush (check_brush, false, false) == -1
            && brush_above_east_of_brush (check_brush, false, false) == -1) check_brush.top_shadow_northeast = true;
        if (brush_above_northwest_of_brush (check_brush, false, false) > -1
            && brush_above_north_of_brush (check_brush, false, false) == -1
            && brush_above_west_of_brush (check_brush, false, false) == -1) check_brush.top_shadow_northwest = true;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    // decide which sides of this wall are an edge side
    public void calculate_outlines (Brush check_brush)
      {
      int brush_north, brush_south, brush_east, brush_west;
      int brush_northwest, brush_northeast, brush_southwest, brush_southeast;

      check_brush.top_top_outline = false;
      check_brush.top_bottom_outline = false;
      check_brush.top_left_outline = false;
      check_brush.top_right_outline = false;
      check_brush.top_top_left_outline = false;
      check_brush.top_top_right_outline = false;
      check_brush.top_bottom_left_outline = false;
      check_brush.top_bottom_right_outline = false;
      check_brush.front_top_outline = false;
      check_brush.front_bottom_outline = false;
      check_brush.front_left_outline = false;
      check_brush.front_right_outline = false;
      check_brush.front_top_left_outline = false;
      check_brush.front_top_right_outline = false;
      check_brush.front_bottom_left_outline = false;
      check_brush.front_bottom_right_outline = false;
      
      if (check_brush.transparent) return;
      else if (check_brush.top_texture_number == (int) Texture_Type.box_wood) return;
      
      if (brush_above_brush (check_brush, false, false) == -1 && check_brush.z > 0)  // only draw top outlines if the top's visible
        {
        brush_north = brush_north_of_brush (check_brush, false, false);
        brush_south = brush_south_of_brush (check_brush, false, false);
        brush_east = brush_east_of_brush (check_brush, false, false);
        brush_west = brush_west_of_brush (check_brush, false, false);
        brush_northeast = brush_northeast_of_brush (check_brush, false, false);
        brush_northwest = brush_northwest_of_brush (check_brush, false, false);
        brush_southeast = brush_southeast_of_brush (check_brush, false, false);
        brush_southwest = brush_southwest_of_brush (check_brush, false, false);

        if (brush_north == -1) check_brush.top_top_outline = true;
        //else if (brush[brush_north].top_texture_number != check_brush.top_texture_number && check_brush.z > 0) check_brush.top_top_outline = true;
        else if (brush[brush_north].top_texture_number != check_brush.top_texture_number) check_brush.top_top_outline = true;

        if (brush_west == -1) check_brush.top_left_outline = true;
        else if (brush[brush_west].top_texture_number != check_brush.top_texture_number) check_brush.top_left_outline = true;

        if (brush_east == -1) check_brush.top_right_outline = true;
        else if (brush[brush_east].top_texture_number != check_brush.top_texture_number) check_brush.top_right_outline = true;

        if (brush_south == -1) check_brush.top_bottom_outline = true;
        else if (brush[brush_south].top_texture_number != check_brush.top_texture_number) check_brush.top_bottom_outline = true;

        if (!check_brush.top_top_outline && !check_brush.top_left_outline
          && (brush_northwest == -1 || brush[brush_northwest].top_texture_number != check_brush.top_texture_number))
          check_brush.top_top_left_outline = true;

        if (!check_brush.top_top_outline && !check_brush.top_right_outline
          && (brush_northeast == -1 || brush[brush_northeast].top_texture_number != check_brush.top_texture_number))
          check_brush.top_top_right_outline = true;

        if (!check_brush.top_bottom_outline && !check_brush.top_left_outline
          && (brush_southwest == -1 || brush[brush_southwest].top_texture_number != check_brush.top_texture_number))
          check_brush.top_bottom_left_outline = true;

        if (!check_brush.top_bottom_outline && !check_brush.top_right_outline
          && (brush_southeast == -1 || brush[brush_southeast].top_texture_number != check_brush.top_texture_number))
          check_brush.top_bottom_right_outline = true;
        }
      if (brush_south_of_brush (check_brush, false, false) == -1)  // only draw front outlines if the front's visible
        {
        if (!check_brush.top_bottom_outline) check_brush.front_top_outline = true;
        check_brush.front_bottom_outline = true;

        brush_west = brush_west_of_brush (check_brush, false, false);
        if (brush_west == -1) check_brush.front_left_outline = true;
        else if (brush[brush_west].front_texture_number != check_brush.front_texture_number) check_brush.front_left_outline = true;

        brush_east = brush_east_of_brush (check_brush, false, false);
        if (brush_east == -1) check_brush.front_right_outline = true;
        else if (brush[brush_east].front_texture_number != check_brush.front_texture_number) check_brush.front_right_outline = true;
        }
      }
    }
  }
