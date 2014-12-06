// Particle Engine for Boxland
// Nightmare Games

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Boxland.Particle_Engine
  {
  public class Particle_Engine
    {
    private class Particle_Effect
      {
      private struct Particle
        {
        }
      }
    }
  }

namespace Boxland
  {
  public class Particle_Effect
    {
    Random rnd = new Random();

    //int box_x1, box_x2, box_y1, box_y2;  // bounding box of effect area
    //int screen_width, screen_height;

    public bool active = false;   // are any particles still visible

    public struct Particle        // one individual particle
      {
      public int x, y, z;         // x, y, z world location
      public double dx, dy, dz;   // fractional world location
      public float degrees;       // direction travelling on x-y plane
      public double velocity;
      public double z_velocity;   // upward-downward momentum
      public double acceleration;
      public float fade;          // alpha transparency
      public float fade_speed;
      public double gravity;
      public double gravity_accelerated;
      public Texture2D sprite;
      public bool alive;
      public double scale;         // size ratio compared to original sprite (1 = same)
      public double scale_change;  // amount to change scale by each frame
      public string source_type;   // character, fixture, brush
      public int source;
      public string type;          // light, fire, freeze, smoke
      }

    public const int Max_Particles = 100;
    public Particle[] particle = new Particle[Max_Particles];

    // create
    public void create (Texture2D sprite, int amount, int xorigin, int yorigin, int zorigin,
                 double degrees, double spread, double avg_velocity, double velocity_range, double avg_z_velocity, double z_velocity_range,
                 double acceleration, float fade, float fade_speed, double gravity, double scale, double scale_change,
                 string type, string source_type, int source)
      {
      int temp;

      active = true;

      //screen_width  = screen_w;
      //screen_height = screen_h;

      int p = 0;
      while (p < Max_Particles)
        {
        if (p < amount)
          {
          particle[p].sprite = sprite;
          particle[p].x = xorigin;
          particle[p].y = yorigin;
          particle[p].z = zorigin;
          particle[p].dx = xorigin;
          particle[p].dy = yorigin;
          particle[p].dz = zorigin;
          temp = rnd.Next (Convert.ToInt32 ((degrees * 100) - (spread * 100 / 2)), Convert.ToInt32 ((degrees * 100) + (spread * 100 / 2)));
          particle[p].degrees = Convert.ToSingle (Convert.ToDouble (temp / 100));
          //particle[p].degrees = particle[p].degrees * -1;
          if (particle[p].degrees >= 360) particle[p].degrees -= 360;
          if (particle[p].degrees < 0)    particle[p].degrees += 360;
          if (velocity_range == 0) particle[p].velocity = avg_velocity;
          else
            {
            temp = rnd.Next(Convert.ToInt16((avg_velocity * 100) - (velocity_range * 100 / 2)), Convert.ToInt16((avg_velocity * 100) + ((velocity_range * 100) / 2)));
            particle[p].velocity = Convert.ToDouble(temp) / 100;
            }
          if (particle[p].velocity < 0) particle[p].velocity = 0;

          if (z_velocity_range == 0) particle[p].z_velocity = avg_z_velocity;
          else
            {
            temp = rnd.Next (Convert.ToInt16 ((avg_z_velocity * 100) - (z_velocity_range * 100 / 2)), Convert.ToInt16 ((avg_z_velocity * 100) + ((z_velocity_range * 100) / 2)));
            particle[p].z_velocity = Convert.ToDouble (temp) / 100;
            }
          if (particle[p].z_velocity < 0) particle[p].z_velocity = 0;

          particle[p].acceleration = acceleration;
          particle[p].fade = fade;
          particle[p].fade_speed = fade_speed;
          particle[p].alive = true;
          particle[p].gravity = gravity;
          particle[p].gravity_accelerated = 0;
          particle[p].scale = scale;
          particle[p].scale_change = scale_change;
          particle[p].type = type;
          particle[p].source_type = source_type;
          particle[p].source = source;
          }
        else particle[p].alive = false;
        p += 1;
        }
      }

    // update
    public void update ()
      {
      double xmove, ymove;

      active = false;

      for (int p = 0; p < Max_Particles; p += 1)
        {
        if (particle[p].alive == true)
          {
          active = true;

          // move
          xmove = particle[p].velocity * Math.Cos(MathHelper.ToRadians(particle[p].degrees));
          //ymove = -1 * particle[p].velocity * Math.Sin (MathHelper.ToRadians (particle[p].degrees));
          ymove = particle[p].velocity * Math.Sin (MathHelper.ToRadians (particle[p].degrees));
          particle[p].dx += xmove;
          particle[p].dy += ymove;
          particle[p].dz += particle[p].z_velocity;
          //     if (particle[p].pos_double_x < 0)             particle[p].alive = false;
          //else if (particle[p].pos_double_x > screen_width)  particle[p].alive = false;
          //else if (particle[p].pos_double_y < 0)             particle[p].alive = false;
          //else if (particle[p].pos_double_y > screen_height) particle[p].alive = false;
        
          // accelerate / decelerate
          particle[p].velocity += particle[p].acceleration;
          if (particle[p].velocity < 0) particle[p].velocity = 0;

          // gravity
          particle[p].gravity_accelerated += particle[p].gravity;
          particle[p].dz += particle[p].gravity_accelerated;

          // fade (transparency)
          particle[p].fade += particle[p].fade_speed;
          if (particle[p].fade <= 0) particle[p].alive = false;

          // scale
          particle[p].scale += particle[p].scale_change;
          }
        }
      }

    // draw
    public void draw (SpriteBatch spriteBatch, int screen_width, int screen_height, int scroll_x, int scroll_y)
      {
      //Vector2 v_draw;
      Rectangle r_draw;

      for (int p = 0; p < Max_Particles; p += 1)
        {
        if (particle[p].alive == true)
          {
          particle[p].x = Convert.ToInt32 (particle[p].dx);
          particle[p].y = Convert.ToInt32 (particle[p].dy);
          particle[p].z = Convert.ToInt32 (particle[p].dz);
          r_draw.Width = Convert.ToInt32 (particle[p].sprite.Width * particle[p].scale);
          r_draw.Height = Convert.ToInt32 (particle[p].sprite.Height * particle[p].scale);
          r_draw.X = particle[p].x - (r_draw.Width / 2) + scroll_x;
          r_draw.Y = (screen_height - particle[p].y) - (r_draw.Height / 2) - (particle[p].z / 2) + scroll_y;
          spriteBatch.Draw (particle[p].sprite, r_draw, Color.White * particle[p].fade);
          }
        }
      }
    }
  }
