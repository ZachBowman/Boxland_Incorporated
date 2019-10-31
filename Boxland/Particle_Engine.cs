// Boxland Incorporated
// Particle Engine
// 2011-2018 Nightmare Games
// Zach Bowman

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Boxland
  {
  public class Particle_Engine
    {
    Random rnd = new Random();
    public bool active = false;  // are any particles still visible
    double parallax;  // used for perspective drawing offsets

    public Particle_Engine (double new_parallax)
      {
      parallax = new_parallax;
      }

    // one individual particle
    public class Particle
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
      public double gravitational_acceleration;
      public Texture2D sprite;
      public double h_scale;       // size ratio compared to original sprite (1 = same)
      public double v_scale;
      public double scale_change;  // amount to change scale by each frame
      public string source_type;   // character, fixture, brush
      public int source;
      public string type;          // light, fire, freeze, smoke
      }

    public const int max_particles = 2000; //750;
    List<Particle> particle = new List<Particle> ();

    // create new particle effect
    // particles needs separate horizontal and vertical scales to make flames look right.
    public void create (Texture2D sprite, int amount, int xorigin, int yorigin, int zorigin,
                 double degrees, double spread, double avg_velocity, double velocity_range, double avg_z_velocity, double z_velocity_range,
                 double acceleration, float fade, float fade_speed, double gravity, double h_scale, double v_scale, double scale_change,
                 string type, string source_type, int source)
      {
      int temp;
      active = true;
      int p = 0;

      while (particle.Count < max_particles && p < amount)
        {
        Particle new_particle = new Particle();

        new_particle.sprite = sprite;
        new_particle.x = xorigin;
        new_particle.y = yorigin;
        new_particle.z = zorigin;
        new_particle.dx = xorigin;
        new_particle.dy = yorigin;
        new_particle.dz = zorigin;
        temp = rnd.Next (Convert.ToInt32 ((degrees * 100) - (spread * 100 / 2)), Convert.ToInt32 ((degrees * 100) + (spread * 100 / 2)));
        new_particle.degrees = Convert.ToSingle (Convert.ToDouble (temp / 100));
        if (new_particle.degrees >= 360) new_particle.degrees -= 360;
        if (new_particle.degrees < 0)    new_particle.degrees += 360;
        if (velocity_range == 0) new_particle.velocity = avg_velocity;
        else
          {
          temp = rnd.Next(Convert.ToInt16((avg_velocity * 100) - (velocity_range * 100 / 2)), Convert.ToInt16((avg_velocity * 100) + ((velocity_range * 100) / 2)));
          new_particle.velocity = Convert.ToDouble(temp) / 100;
          }
        if (new_particle.velocity < 0) new_particle.velocity = 0;

        if (z_velocity_range == 0) new_particle.z_velocity = avg_z_velocity;
        else
          {
          temp = rnd.Next (Convert.ToInt16 ((avg_z_velocity * 100) - (z_velocity_range * 100 / 2)), Convert.ToInt16 ((avg_z_velocity * 100) + ((z_velocity_range * 100) / 2)));
          new_particle.z_velocity = Convert.ToDouble (temp) / 100;
          }
        if (new_particle.z_velocity < 0) new_particle.z_velocity = 0;

        new_particle.acceleration = acceleration;
        new_particle.fade = fade;
        new_particle.fade_speed = fade_speed;
        new_particle.gravity = gravity;
        new_particle.gravitational_acceleration = 0;
        new_particle.h_scale = h_scale;
        new_particle.v_scale = v_scale;
        new_particle.scale_change = scale_change;
        new_particle.type = type;
        new_particle.source_type = source_type;
        new_particle.source = source;

        particle.Add (new_particle);
        p += 1;
        }
      }

    // update
    public void update ()
      {
      double xmove, ymove;

      int p = 0;
      while (p < particle.Count)
        {
        // move
        xmove = particle[p].velocity * Math.Cos(MathHelper.ToRadians(particle[p].degrees));
        ymove = particle[p].velocity * Math.Sin (MathHelper.ToRadians (particle[p].degrees));
        particle[p].dx += xmove;
        particle[p].dy += ymove;
        particle[p].dz += particle[p].z_velocity;
        
        // accelerate / decelerate
        particle[p].velocity += particle[p].acceleration;
        if (particle[p].velocity < 0) particle[p].velocity = 0;

        // gravity
        particle[p].gravitational_acceleration += particle[p].gravity;
        particle[p].dz += particle[p].gravitational_acceleration;

        // scale
        particle[p].h_scale += particle[p].scale_change;
        particle[p].v_scale += particle[p].scale_change;

        // fade (transparency)
        particle[p].fade += particle[p].fade_speed;
        if (particle[p].fade <= 0) particle.RemoveAt (p);
        else p += 1;
        }
      }

    // draw
    public void draw (SpriteBatch spriteBatch, int screen_width, int screen_height, int scroll_x, int scroll_y)
      {
      Rectangle r_draw;

      for (int p = 0; p < particle.Count; p += 1)
        {
        particle[p].x = Convert.ToInt32 (particle[p].dx);
        particle[p].y = Convert.ToInt32 (particle[p].dy);
        particle[p].z = Convert.ToInt32 (particle[p].dz);
        r_draw.Width = Convert.ToInt32 (particle[p].sprite.Width * particle[p].h_scale);
        r_draw.Height = Convert.ToInt32 (particle[p].sprite.Height * particle[p].v_scale);
        r_draw.X = particle[p].x - (r_draw.Width / 2) + scroll_x;
        r_draw.Y = Convert.ToInt32((screen_height - particle[p].y) - (r_draw.Height * parallax) - (particle[p].z * parallax) + scroll_y);
        spriteBatch.Draw (particle[p].sprite, r_draw, Color.White * particle[p].fade);
        }
      }
    }
  }
