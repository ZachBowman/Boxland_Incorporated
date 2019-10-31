using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public partial class Boxland : Game
    {
    void load_map ()
      {
      int mx, my, mz;
      //int light_number = 0;
      //float map_alpha = 0f;
      //bool light_found;
      //int radius;
      //Stream filestream;
      //string levelPath = string.Format ("maps/{0}-{1}.txt", player_world, player_level);
      //string levelPath = "0.txt";
      //string gridspace;
      //int random = 0;

      redraw_floor_buffer = true;

      if (randomized_map == true)
        {
        game_state = Game_State.creation;
    //    create_map ();
        }

      else if (randomized_map == false)
        {
        textmap = new List<List<string>> ();
        for (int layer = 0; layer < 3; layer += 1) textmap.Add (new List<string> ());

        map.background = test_background4;

        // RICHARD'S HOUSE
        if (player_level == 0)
          {
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   CP  CP  CP  CP  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  .   ");
          textmap[0].Add (".   CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  CP  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add ("DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  ");
          textmap[1].Add ("DT  .   .   .   .   DT  .   .   .   .   .   .   .   .   .   DT  .   .   DT  ");
          textmap[1].Add ("DT  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add ("DT  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add ("DT  .   .   .   .   DT  .   .   .   .   .   .   .   .   .   DT  .   c   DT  ");
          textmap[1].Add ("DT  .   .   .   .   DT  DT  DT  DT  DT  DT  DT  .   .   DT  DT  DT  DT  DT  ");
          textmap[1].Add ("DT  .   .   .   .   DT  pl1 c   co  .   .   rd  .   .   .   DT  .   .   DT  ");
          textmap[1].Add ("DT  .   .   .   .   .   .   .   pn  .   .   .   .   .   .   DT  .   .   DT  ");
          textmap[1].Add ("DT  .   .   .   .   .   .   .   .   tv  .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add ("DT  .   .   .   .   DT  .   p1  .   .   .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add ("BW  BW  BW  BW  BW  BW  BW  G1  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ly  .   ly  .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          ambient_light = new Color (96, 96, 64);
          map.background = test_background5;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;
          }

        // RICHARD'S STREET
        if (player_level == 1)
          {
          ambient_light = new Color (112, 112, 112);
          map.background = test_background5;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  AS  AS  AS  AS  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  AS  AS  AS  AS  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  AS  AS  AS  AS  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  AS  AS  AS  AS  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  AS  AS  AS  AS  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  GR  GR  GR  GR  GR  GR  GR  GR  GR  AS  AS  AS  AS  GR  GR  GR  GR  ");
          textmap[0].Add ("AS  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  ");

          textmap[1].Add (".   .   .   .   BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   BW  BW  BW  G0  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   p0  .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   pn  .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   G2  .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ly  .   ly  .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // LOBBY
        if (player_level == 2)
          {
          textmap[0].Add (".   .   .   .   CE  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   CE  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   PSY CE  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   CE  CE  .   .   .   .   .   .   .   .   .   .   CE  .   .   ");
          textmap[0].Add (".   .   .   CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  ");
          textmap[0].Add (".   .   .   CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  .   ");
          textmap[0].Add (".   .   .   CE  CE  CE  CE  CE  CE  TN  TN  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   ");
          textmap[0].Add (".   TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  TN  .   .   .   .   .   .   ");
          textmap[0].Add (".   TN  TN  TN  TN  TN  TN  TN  TN  TK  TK  TK  .   .   .   .   .   .   ");
          textmap[0].Add (".   TN  TN  TN  TN  TN  TN  TN  TN  TK  TK  TK  .   .   .   .   .   .   ");
          textmap[0].Add (".   TN  TN  TN  TN  TN  TN  TN  TN  TK  TK  TK  .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   TN  TN  TN  TN  TN  TK  TK  TK  .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   SW  .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   SW  SW  SW  SW  SW  SW  SW  SW  SW  SW  .   .   .   .   .   ");

          textmap[1].Add (".   .   .   DM  G3  DM  .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   DM  DM  DYV DM  .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   DM  .   p3  DM  .   .   .   .   .   .   .   .   .   g4  .   .   ");
          textmap[1].Add (".   .   DM  .   .   DM  DM  DM  DM  DM  DM  DM  DM  DM  DM  K1  DM  DM  ");
          textmap[1].Add (".   .   DM  .   .   .   b   .   P1g .   Vy  .   .   .   .   p4  .   DM  ");
          textmap[1].Add (".   .   DM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   DM  ");
          textmap[1].Add (".   .   DM  DM  DM  DM  DM  DM  DM  DM  .   DM  DM  DM  DM  DM  DM  DM  ");
          textmap[1].Add (".   .   DT  DT  DT  DT  DT  DT  DT  .   .   DT  .   .   .   .   .   .   ");
          textmap[1].Add ("DT  DT  DT  DT  DT  DT  DT  .   DT  .   .   DT  .   .   .   .   .   .   ");
          textmap[1].Add ("DT  .   .   DT  Vr  Ph1 DT  .   DT  .   DT  DT  .   .   .   .   .   .   ");
          textmap[1].Add ("DT  .   .   Dmv .   .   DT  DT  DT  b   .   DT  .   .   .   .   .   .   ");
          textmap[1].Add ("DT  DT  DT  DT  .   .   .   sc  .   .   .   DT  DT  DT  .   .   .   .   ");
          textmap[1].Add ("DT  .   .   DT  .   .   D1  .   .   .   .   .   .   DT  .   .   .   .   ");
          textmap[1].Add ("DT  .   .   Dmv .   .   .   .   .   .   .   .   .   DT  .   .   .   .   ");
          textmap[1].Add ("DT  DT  DT  DT  .   .   .   .   .   .   .   .   BR  DT  .   .   .   .   ");
          textmap[1].Add ("DT  P1m .   Tvr .   .   .   .   BR  .   .   .   BR  .   .   .   .   .   ");
          textmap[1].Add ("DT  C3e .   .   .   .   .   B1w BR  .   .   .   BR  .   .   .   .   .   ");
          textmap[1].Add ("DT  .   T3w C3n .   .   .   .   BR  .   .   b   BR  .   .   .   .   .   ");
          textmap[1].Add ("DT  DT  DT  DT  P1m .   pn  .   BR  .   b   b   BR  .   .   .   .   .   ");
          textmap[1].Add ("BR  BR  BR  BR  BR  BR  Dwh BR  BR  BR  BR  BR  BR  .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   00  00  00  00  00  00  00  00  00  00  00  00  00  ");
          textmap[2].Add (".   .   00  .   .   .   .   .   .   .   .   .   .   .   .   .   .   00  ");
          textmap[2].Add (".   .   00  .   lw  .   .   lw  .   .   lw  .   .   lw  .   .   lw  00  ");
          textmap[2].Add (".   .   00  00  00  00  00  00  00  .   .   00  00  00  00  00  00  00  ");
          textmap[2].Add (".   .   .   .   .   .   .   .   00  .   .   00  .   .   .   .   .   .   ");
          textmap[2].Add ("00  00  00  00  00  00  00  .   00  .   .   00  .   .   .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   00  .   00  .   .   00  .   .   .   .   .   .   ");
          textmap[2].Add ("00  .   lW  .   .   .   00  00  00  .   .   00  .   .   .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   .   ly  .   .   .   00  00  00  .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   .   .   .   .   .   .   .   00  .   .   .   .   ");
          textmap[2].Add ("00  .   lW  .   .   .   .   .   .   .   .   .   .   00  .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   .   .   .   .   .   .   00  00  .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   ly  .   .   .   .   .   00  .   .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   .   .   .   .   .   .   00  .   .   .   .   .   ");
          textmap[2].Add ("00  .   .   .   .   .   .   .   .   .   .   .   00  .   .   .   .   .   ");
          textmap[2].Add ("00  00  00  00  .   .   ly  .   .   .   .   .   00  .   .   .   .   .   ");
          textmap[2].Add ("00  00  00  00  00  00  00  00  00  00  00  00  00  .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          //map.tile_width = textmap[0][0].Length / 4;
          //map.tile_length = textmap.Count;
          //map.tile_height = 3;
          ambient_light = new Color (64, 64, 48);
          map.background = brush_control.tile[(int) Texture_Type.grass].texture;
          //map.background = test_background1;
          map.bg_scroll = false;
          random_retard1 = 0;
          random_retard2 = 0;
          }

        // machine room
        if (player_level == 3)
          {
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 1;
          random_retard2 = 0;

          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CP  CP  CP  CP  CP  .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CP  CP  CP  CP  CP  .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CP  CP  CP  CP  CP  .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CP  CP  CP  CP  CP  .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CP  CP  CP  CP  CP  .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   TK  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   TK  TK  ZR  ZR  ZR  TK  TK  TK  TK  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   FG  FG  FG  FG  FG  TK  TK  TK  TK  TK  TK  .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  .   .   ");
          textmap[0].Add (".   .   FG  FG  FG  FG  FG  TK  TK  TK  TK  TK  TK  .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  .   .   ");
          textmap[0].Add (".   .   FG  FG  FG  FG  FG  TK  TK  TK  TK  TK  TK  .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  .   .   ");
          textmap[0].Add (".   .   FG  FG  FG  FG  FG  TK  TK  TK  TK  TK  .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  psg .   ");
          textmap[0].Add (".   FG  FG  FG  FG  FG  FG  FG  FG  FG  FG  FG  .   FG  FG  .   .   FG  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   FG  .   .   FG  .   .   FG  .   .   .   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  .   .   ");
          textmap[0].Add (".   FG  .   .   FG  .   .   FG  .   .   .   .   .   .   .   .   .   .   .   .   TK  TK  .   .   .   TK  TK  .   .   .   ");
          textmap[0].Add (".   FG  .   .   FG  .   .   FG  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   FG  FG  FG  FG  FG  FG  FG  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   FG  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   FG  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  DY  DY  DY  DY  DY  DY  .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  fc  fc  .   .   .   DY  .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   .   k   .   .   DY  .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   t2  .   .   .   DY  .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   .   .   .   .   DY  .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  c   .   .   .   pl1 DY  .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  MB  MB  MB  MB  MB  DRV MB  MB  MB  MB  .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  MB  b   .   .   .   .   .   .   b   b   MB  MB  ");
          textmap[1].Add (".   .   .   .   .   .   .   MM  MM  MM  MM  MM  MM  MM  .   .   .   MB  .   .   .   .   .   .   .   .   .   b   b   MB  ");
          textmap[1].Add (".   MM  MM  MM  MM  MM  MM  MM  c   vm  .   .   .   MM  .   .   .   MB  b   .   .   WE  .   .   .   WE  .   .   .   MB  ");
          textmap[1].Add (".   MM  c   .   .   .   .   MM  .   .   .   .   .   MM  .   .   .   MB  MB  .   .   .   b   .   .   .   .   .   MB  MB  ");
          textmap[1].Add (".   MM  .   BM  00  00  .   MM  .   .   .   .   .   MM  .   .   .   .   MB  b   .   .   .   //  .   .   b   .   MB  .   ");
          textmap[1].Add (".   MM  .   00  00  00  .   MM  .   .   .   .   .   MM  .   .   .   .   MB  .   .   .   .   .   m   .   .   b   MB  MB  ");
          textmap[1].Add ("MM  MM  .   00  00  00  .   MM  MM  MM  DGV MM  MM  .   .   .   MM  MM  MB  .   .   WE  .   .   .   WE  .   m   .   MB  ");
          textmap[1].Add ("MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   b   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add ("MM  .   .   .   .   .   .   .   MM  MM  MM  .   .   MM  .   .   .   MM  MB  .   .   .   .   .   m   .   b   .   MB  MB  ");
          textmap[1].Add ("MM  .   .   .   .   .   .   .   MM  .   .   .   .   .   .   .   .   .   MB  MB  .   .   MB  MB  MB  c   b   MB  MB  .   ");
          textmap[1].Add ("MM  .   .   .   .   .   .   .   MM  .   .   .   .   .   .   .   .   .   .   MB  MB  MB  MB  .   MB  MB  MB  MB  .   .   ");
          textmap[1].Add ("MM  .   .   .   pn  .   .   .   MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add ("MM  MM  MM  MM  p2  MM  MM  MM  MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   MM  G2  MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ly  .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   lw  .   .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   lR  .   .   .   lb  .   lb  .   lb  .   .   .   lb  .   lb  .   lw  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // METAL zone MAIN
        if (player_level == 4)
          {
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   TK  .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   TK  .   .   .   .   TK  CE  CE  CE  TK  .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   TK  TK  TK  .   .   .   TK  CE  CE  CE  TK  .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   TK  TK  TK  .   .   .   TK  CE  CE  CE  TK  .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   TK  TK  TK  TK  TK  FG  TK  CE  CE  CE  TK  FG  TK  TK  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   TK  TK  TK  TK  TK  FG  TK  CE  CE  CE  TK  FG  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add (".   .   .   TK  TK  TK  TK  TK  FG  TK  CE  CE  CE  TK  FG  TK  TK  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  CE  CE  CE  TK  TK  TK  TK  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  CE  CE  CE  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  CE  CE  CE  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   TK  TK  TK  FG  TK  CE  CE  CE  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   TK  TK  TK  FG  TK  CE  CE  CE  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   TK  TK  TK  FG  TK  CE  CE  CE  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  CE  CE  CE  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TN  TN  TN  TN  TN  TN  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   TK  .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add (".   .   .   .   .   .   .   .   MB  MB  MB  K10 MB  MB  MB  .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   MB  pl1 .   .   .   pl1 MB  .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   MB  MB  G7  MB  MB  .   MB  .   .   .   .   .   MB  .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   MB  .   p7  .   MB  .   MB  .   .   .   .   .   MB  .   .   .   .   .   .   .   ");
          textmap[1].Add (".   .   MB  .   .   .   MB  MB  MB  .   .   .   .   .   MB  MB  MB  MB  MB  MB  MB  MB  ");
          textmap[1].Add (".   .   MB  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add (".   .   MB  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   p6  g6  ");
          textmap[1].Add (".   .   MB  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add (".   .   MB  MB  MB  MB  MB  MB  MB  .   .   .   .   .   DY  DY  DY  .   .   DY  DY  DY  ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DY  .   .   .   .   .   .   vm  .   .   .   .   .   DY  ");
          textmap[1].Add (".   .   .   .   DY  DY  DY  DY  DY  .   .   .   .   .   .   .   .   .   .   .   .   DY  ");
          textmap[1].Add (".   .   .   .   DY  .   .   .   .   .   .   .   .   .   DY  .   .   t1  t1  .   .   DY  ");
          textmap[1].Add (".   .   .   .   DY  .   pn  .   .   .   .   .   .   .   DY  .   .   .   .   .   .   DY  ");
          textmap[1].Add (".   .   .   .   DY  .   p2  .   .   .   .   .   .   .   DY  .   .   t2  .   .   .   DY  ");
          textmap[1].Add (".   .   .   .   DY  DY  G2  DY  DY  .   .   .   .   .   .   .   .   .   .   .   .   DY  ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DY  .   .   .   .   .   .   .   .   .   .   .   .   DY  ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DY  .   .   p5  .   .   DY  DY  DY  DY  DY  DY  DY  DY  ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DY  DY  DY  G5  DY  DY  DY  .   .   .   .   .   .   .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   lw  .   .   .   lw  .   .   .   lw  .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // metal zone 1
        if (player_level == 5)
          {
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("TK  TK  TK  TK  TB  ");
          textmap[0].Add ("TK  TK  TK  TB  TK  ");
          textmap[0].Add ("TK  TK  TM  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TN  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TN  ");

          textmap[1].Add ("BE  BE  G4  BE  BE  ");
          textmap[1].Add ("BE  .   p4  .   BE  ");
          textmap[1].Add ("BE  .   pn  .   BE  ");
          textmap[1].Add ("BE  .   .   .   BE  ");
          textmap[1].Add ("BE  BE  BE  BE  BE  ");

          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");

          // map width = 5 tiles = 480 px
          // map height = 5 tiles = 480 px
          // screen width = 1280 px
          // screen height = 768 px
          // scroll x = 306
          // scroll y = 0

          // target = 308 x 144?
          }

        /*
        // metal zone 1
        if (player_level == 5)
          {
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 4;
          random_retard2 = 0;

          textmap[0].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .     TK .  .  .  .  .  .  .  .  ");
          textmap[0].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  TK TK TK TK TK .  .  .  .  .  ");
          textmap[0].Add(".  .  .  .  .  TK TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  TK TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN TN .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK .. .. ");
          textmap[0].Add(".  .  .  .  .  TK FG FG FG FG FG FG FG TN TN TN TN TN TN TN TN TN TN TN TN TN .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK .. .. ");
          textmap[0].Add(".  .  .  .  .  TK FG FG FG FG FG FG FG TN TN TN TN TN TN TN TN TN TN TN TN TN .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK sg .. ");
          textmap[0].Add(".  .  .  .  .  TK FG FG FG FG FG FG FG TN TN TN TN TN TN TN TN TN TN TN TN TN .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK TK .. ");
          textmap[0].Add(".  .  .  .  .  TK FG FG FG FG FG FG FG TN TN TN TN TN TN TN TN TN TN TN TN TN .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK TK .. ");
          textmap[0].Add(".  .  FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK TK .. ");
          textmap[0].Add(".  .  FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG FG .. FG FG FG .. FG .. TK TK TK TK TK TK TK TK TK .. ");
          textmap[0].Add(".  .  FG FG FG FG FG FG FG FG FG FG .  .  .  .  .  .  .  .  .  FG FG FG FG FG .  .  .. .. .. .. .. .. .. .. TK TK TK TK TK TK TK TK TK .. ");
          textmap[0].Add(".  .  .  .  .  .  .  FG .  .  .  FG .  .  .  .  .  .  .  .  .  CE CE CE CE CE .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  FG .  .  .  FG .  .  .  .  .  .  .  .  .  CE CE CE CE CE .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  FG .  .  .  FG .  .  .  .  .  .  .  .  .  CE CE CE CE CE .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  FG FG FG FG FG .  .  .  .  .  .  .  .  .  CE CE CE CE CE .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");

          textmap[1].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .. .. .. .. .. .. WR WR G3 WR WR WR WP .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY .  .  .. .. .. .. .. .. WR // p3 .. .. .  WP .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  WY .  .  .  .  .  .  .  m  .  .  .  .  fc WY WY .  .  .  .  .  WY .  .  .. .. .. .. .. .. WR .. .. WP WP WP WP WP WP WP .. ");
          textmap[1].Add(".  .  .  .  .  WY k  .  .  c  .  .  sm WG t1 .  .  .  .  b  .  .  .  .  .  .  WY .  .  .. .. .. .. .. .. WR .. .. WP .. .. .. .. .. WP .. ");
          textmap[1].Add(".  .  .  .  .  WG WG WG WG WG WG WG WG WG .  .  .  .  .  .  .  .  .  r1 .  .  WY .  .  .. .. .. .. .. .. WR .. .. .. .. .. WP WP b  WP WP ");
          textmap[1].Add(".  .  .  .  .  WG WG c  .  .  .  .  WG WG .  .  .  .  .  WY WY .  .  m  .  .  WY .  .  .. .. .. .. .. .. WR h  // WP .. b  WP b  .. .. WP ");
          textmap[1].Add(".  .  .  .  .  WG WG .  BM 00 00 .  WG WG .  .  .  .  .  WY WY h  .  b  .  .  WY .  .  .. .. .. .. .. .. WP WP WP WP b  .. WP .  // m  WP ");
          textmap[1].Add(".  .  .  .  .  WG WG .  00 00 00 .  WG WG WG WG b  WY WY WY WY WY WY .  WY WY WY WG WG .. .. .. .. .. .. WP .. .. r2 b  b  WP .  .. .. WP ");
          textmap[1].Add(".  MM MM MM WG WG WG .  00 00 00 .  WG WG WG WG .  WG WG WG WG WG WG .  WG WG WG WG WG WG WG .. WG .. WG WP .. .. .. .. b  .  .  .. .. WP ");
          textmap[1].Add(".  MM c  .  .  pn p1 .  .  .  .  .  .  b  .  .  .  .  b  c  .  .  .  .  .  // .  // .  .. .. .. .. .. .. .. .. .. .. .. .. WP .  .. .. WP ");
          textmap[1].Add(".  MM .  .  .  .  .  .  .  .  .  .  WG WG WG WG WG WG WG WG WG WG WG DG WG WG WG WG .  .. .. WG .. .. .. WP WP .. .. .. .. WP c  // h  WP ");
          textmap[1].Add(".  MM MM EN MM WG WG .  .  .  .  .  WG .  .  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .. .. .. .. .. .. WP WP WP WP WP WP WP WP WP WP WP ");
          textmap[1].Add(".  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  .  WG .  .  c  .  .  WG .  .  .  .  .  .  .  WG .  .  k  .  .  WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  .  WG WG WG WG WG WG WG .  .  .  .  .  .  .  WG WG WG WG WG WG WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");

          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WR WR WR WR WR WR .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lY .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lw .. lw .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. lw .. lw .. lw .. .. .. lw .. lw .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lw .. lw .. lw .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. lw .. lw .. lw .. lR .. lw .. lB .. lB .. lB .. lB .. lB .. lB .. lB .. lB .. .. .. .. .. .. .. lw .. lw .. lw .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. lw .. lw .. lw .. .. .. .. .. .. .. .. .. .. .. lw .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lw .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ..  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          }
        */

        // metal zone 2
        else if (player_level == 6)
          {
          ambient_light = new Color (64, 64, 64);
          map.background = test_background5;
          map.bg_scroll = true;
          random_retard1 = 6;
          random_retard2 = 0;// 2;

          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   PSY CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   CE  CE  CE  CE  CE  CE  PSG .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  CE  CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  CE  CE  CE  CE  CE  CE  CE  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  CE  CE  CE  CE  CE  CE  CE  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   TK  WR  WR  TK  TK  TK  TK  TK  CE  CE  CE  CE  CE  CE  CE  TK  TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  WR  PSB TK  TK  CE  CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   TK  FM  FM  FM  FM  FM  TK  WR  TK  .   .   CE  CE  CE  CE  CE  CE  CE  .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  TK  TK  TK  .   .   .   .   CE  CE  CE  .   .   .   .   .   .   .   ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  TK  TK  TK  .   .   .   .   TK  TK  TK  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  TK  TK  TK  .   .   .   .   TK  TK  TK  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   TK  FM  FM  FM  FM  FM  TK  TK  TK  .   .   .   .   PSR TK  TK  .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   TK  TK  TK  TK  TK  TK  TK  TK  TK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  DY  DY  DY  DY  DY  DY  DY  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   c   .   .   .   fc  DY  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  //  t2  .   .   b   .   DY  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   //  //  .   .   .   DY  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   .   .   .   .   .   DY  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DP  DP  DP  DGV DP  DP  DP  DP  DP  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DP  DP  DP  DP  DP  DP  DP  DP  DP  .   .   .   t2  .   b   .   DP  .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DP  .   k   .   DP  .   .   .   .   .   //  .   //  .   .   .   DP  BR  BR  BR  BR  ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DP  .   .   .   .   .   .   b   .   .   .   .   .   .   .   .   DP  .   .   .   BR  ");
          textmap[1].Add (".   .   .   .   .   .   .   .   DP  //  .   h   DP  DP  DP  DP  DP  .   .   .   m   .   .   .   DRH //  k   //  BR  ");
          textmap[1].Add (".   .   .   .   .   BR  BR  BR  BR  DBV BR  BR  BR  BR  BR  BR  DP  t1  //  .   .   .   .   .   DP  .   .   .   BR  ");
          textmap[1].Add (".   .   .   .   .   BR  b   .   .   .   //  .   .   BR  .   BR  DP  .   m   .   .   .   .   .   DP  BR  BR  BR  BR  ");
          textmap[1].Add ("BR  BR  BR  BR  BR  BR  //  .   .   .   .   .   .   BR  .   BR  DP  t2  .   .   .   .   .   pl1 DP  .   .   .   .   ");
          textmap[1].Add ("BR  .   .   b   b   BR  .   .   .   .   .   .   .   .   .   BR  DP  DP  DP  DP  DYV DP  DP  DP  DP  .   .   .   .   ");
          textmap[1].Add ("g4  p4  pn  .   b   .   .   .   .   .   //  .   .   .   //  BR  .   .   DY  b   //  .   DY  .   .   .   .   .   .   ");
          textmap[1].Add ("BR  .   .   .   .   BR  .   .   .   .   .   .   .   .   .   BR  .   .   DY  .   sy  .   DY  .   .   .   .   .   .   ");
          textmap[1].Add ("BR  BR  BR  BR  BR  BR  .   .   .   .   .   .   .   //  b   BR  .   .   DY  .   //  .   DY  .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   BR  c   .   .   .   //  .   .   b   b   BR  .   .   DY  DY  DY  DY  DY  .   .   .   .   .   .   ");
          textmap[1].Add (".   .   .   .   .   BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lg  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   lF  .   .   lw  .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lw  .   lw  .   lR  .   lR  .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   lw  .   .   .   lF  .   lF  .   .   .   lw  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   lw  .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lY  .   lY  .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   lw  .   .   lw  .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   lw  .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lY  .   lY  .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // metal zone 3
        else if (player_level == 7)
          {
          ambient_light = new Color (64, 64, 64);
          map.background = test_background4;
          map.bg_scroll = true;
          random_retard1 = 2;
          random_retard2 = 0;

          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   ZR  ZR  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  .   ");
          textmap[0].Add (".   ZR  ZR  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  .   ");
          textmap[0].Add (".   ZR  ZR  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  .   ");
          textmap[0].Add (".   TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  .   ");
          textmap[0].Add (".   TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  .   ");
          textmap[0].Add (".   TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  TB  .   ");
          textmap[0].Add (".   .   TB  TB  TB  TB  .   .   .   .   TB  TB  TB  TB  .   ");
          textmap[0].Add (".   .   TB  TB  TB  TB  .   .   .   .   .   .   TB  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add ("BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  .   .   .   .   .   ");
          textmap[1].Add ("BR  .   .   .   //  .   .   .   .   BE  BE  BE  BE  BE  BE  ");
          textmap[1].Add ("BR  .   .   .   .   .   .   .   .   .   .   .   b   .   BE  ");
          textmap[1].Add ("BR  .   .   BR  .   BE  .   b   .   BE  .   BE  .   .   BE  ");
          textmap[1].Add ("BR  BR  BR  BR  .   //  .   BE  BE  .   .   .   .   //  BE  ");
          textmap[1].Add ("BE  //  .   .   BE  .   BE  .   b   .   .   BE  BE  BE  BE  ");
          textmap[1].Add ("BE  .   .   .   .   .   .   .   .   .   b   .   b   .   BE  ");
          textmap[1].Add ("BE  BE  .   b   BE  DRV BE  BE  BE  BE  .   pn  p4  .   BE  ");
          textmap[1].Add (".   BE  .   .   BE  k   BE  .   .   BE  BE  BE  G4  BE  BE  ");
          textmap[1].Add (".   BE  BE  BE  BE  BE  BE  .   .   .   .   .   .   .   .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   lR  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   lR  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   lR  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // ice zone main

        // hot zone main

        // hot zone 1
        else if (player_level == 8)
          {
          ambient_light = new Color (64, 64, 64);
          map.background = test_background2;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 2;

          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   FM  FM  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   FM  FM  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   FM  FM  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   FM  FM  .   .   .   .   .   .   .   .   PSR .   .   ");
          textmap[0].Add (".   FM  FM  FM  .   .   .   .   .   FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ZY  .   ");
          textmap[0].Add (".   FM  FM  FM  FG  FG  FG  FG  FG  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ZY  .   ");
          textmap[0].Add (".   FM  FM  FM  .   .   .   .   .   FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ZY  .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   FG  .   FG  .   .   .   .   .   .   .   .   FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   FG  FG  FG  FG  FG  FG  FG  FG  FG  FG  FG  FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   FG  FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   FM  FM  FM  .   .   .   .   .   .   .   FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   FM  FM  FM  .   .   .   .   .   .   .   FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   FM  FM  FM  .   .   .   .   .   .   .   FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   FG  .   .   .   .   .   .   .   .   FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   FG  .   .   .   .   .   .   .   FG  FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   FG  FG  FG  FG  FG  FG  FG  FG  FG  FG  .   .   ");
          textmap[0].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  MK  MK  MK  .   ");
          textmap[1].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   .   XY  .   ");
          textmap[1].Add (".   .   .   .   MK  MK  MK  MK  MK  .   .   .   .   .   .   .   .   .   .   MK  .   .   MK  .   ");
          textmap[1].Add (".   .   .   .   MK  .   .   .   MK  .   MK  MK  MK  MK  .   .   .   .   .   MK  .   .   MK  .   ");
          textmap[1].Add ("MK  MK  MK  MK  MK  .   .   .   MK  MK  MK  .   .   MK  MK  MK  MK  MK  MK  MK  MK  .   WY  WY  ");
          textmap[1].Add ("MK  .   .   .   MK  .   .   .   MK  .   .   .   m   .   .   MK  .   //  .   .   .   .   .   WY  ");
          textmap[1].Add ("MK  .   pn  .   .   .   .   .   .   .   .   .   m   m   .   DRH .   .   .   .   .   //  .   WY  ");
          textmap[1].Add ("MK  .   .   .   MK  .   .   .   MK  .   .   .   .   .   .   MK  .   //  .   .   .   .   .   WY  ");
          textmap[1].Add ("MK  MK  MK  MK  MK  .   .   .   MK  MK  .   MK  .   MK  MK  MK  MK  MK  MK  MK  MK  .   WY  WY  ");
          textmap[1].Add (".   .   MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   MK  .   .   .   .   .   .   .   .   .   c   //  .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   MK  MK  .   .   .   .   .   .   .   c   h   c   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   .   MK  MK  .   .   .   .   .   .   .   c   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   .   .   MK  MK  .   .   .   .   .   MK  DY  MK  .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   .   .   .   MK  MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   .   .   .   .   MK  MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add (".   .   .   .   .   .   .   MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  .   ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lY  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   lR  .   lR  .   .   .   .   .   lR  .   lR  .   .   .   .   .   lY  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   lR  .   lR  .   .   .   .   .   lR  .   lR  .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   lR  .   lR  .   lR  .   lR  .   lR  .   lR  .   lR  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lR  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   lR  .   lR  .   .   .   .   .   .   .   lR  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   lR  .   lR  .   .   .   .   .   .   .   lR  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   lR  .   lR  .   lR  .   lR  .   lR  .   lR  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        else if (player_level == 9)
          {
          ambient_light = new Color (64, 64, 64);
          map.background = test_background4;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  ");
          textmap[0].Add ("CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  ");
          textmap[0].Add ("CE  CE  CE  TK  TK  TK  TK  TK  CE  CE  CE  ");
          textmap[0].Add ("CE  CE  TK  TK  TK  TK  TK  ZY  TK  CE  CE  ");
          textmap[0].Add ("CE  CE  TK  TK  TK  TK  TK  ZY  TK  CE  CE  ");
          textmap[0].Add ("CE  CE  TK  TK  TK  TK  TK  ZY  TK  CE  CE  ");
          textmap[0].Add ("CE  CE  CE  TK  TK  TK  TK  TK  CE  CE  CE  ");
          textmap[0].Add ("CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  ");
          textmap[0].Add ("CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  CE  ");

          textmap[1].Add ("MB  MB  MB  MB  MB  XY  MB  MB  MB  MB  MB  ");
          textmap[1].Add ("MB  .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add ("MB  pn  .   .   .   .   .   DY  .   .   MB  ");
          textmap[1].Add ("MB  .    .  b   .   MB  .   .   DY  .   MB  ");
          textmap[1].Add ("MB  .   MB  b   MB  .   .   .   DY  .   MB  ");
          textmap[1].Add ("MB  .   .   .   .   .   .   .   DY  .   MB  ");
          textmap[1].Add ("MB  .   .   MB  b   MB  .   DY  .   .   MB  ");
          textmap[1].Add ("MB  .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add ("MB  MB  MB  MB  MB  MB  MB  MB  MB  MB  MB  ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   lw  .   lw  .   lw  .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   lw  .   lw  .   lw  .   lY  .   lw  .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   lw  .   lw  .   lw  .   lY  .   lw  .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   lw  .   lw  .   lw  .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   ");
          }

        // test map - environment
        else if (player_level == 10)
          {
          ambient_light = new Color (64, 64, 64);
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  PSY FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  TK  TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  PSG TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  TK  TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  PSR TK  TK  TK  TK  TK  TK  Cns Cnw FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  TK  TK  TK  TK  TK  TK  TK  Cne Cnn FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  TK  TK  TK  TK  TK  TK  TK  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");

          textmap[1].Add ("MK  MK  MK  MK  MK  MK  G0  MK  DY  DY  K1  DY  K10 DY  MK  ");
          textmap[1].Add ("MK  r1  DYH .   vm  .   .   ID  .   k   .   k   .   k   MK  ");
          textmap[1].Add ("MK  DYV MK  .   .   .   .   .   .   m   .   k   .   k   MK  ");
          textmap[1].Add ("MK  .   DGH .   .   .   .   .   .   .   i   .   .   k   MK  ");
          textmap[1].Add ("MK  DGV MK  .   .   pn  .   MK  .   i   i   .   .   k   MK  ");
          textmap[1].Add ("MK  .   DRH .   .   .   .   .   .   .   .   .   .   k   MK  ");
          textmap[1].Add ("MK  DRV MK  .   .   .   .   .   b   .   .   .   .   k   MK  ");
          textmap[1].Add ("MK  .   .   .   .   pl1 .   .   .   .   .   .   .   k   MK  ");
          textmap[1].Add ("MK  .   .   .   .   .   .   .   .   .   .   .   .   .   MK  ");
          textmap[1].Add ("MK  k   h   c   .   .   .   sp  sf  si  se  sm  .   .   MK  ");
          textmap[1].Add ("MK  e   `   .   .   .   .   .   .   .   .   .   .   .   MK  ");
          textmap[1].Add ("MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   lR  .   .   lB  .   .   lG  .   .   lw  .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   lw  .   .   lY  .   .   lF  .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // test map - AI
        else if (player_level == 11)
          {
          ambient_light = new Color (72, 72, 72);
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  ZG  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  ");

          textmap[1].Add ("MM  MM  MM  MM  MM  MM  MM  MM  MM  ");
          textmap[1].Add ("MM  .   .   .   MM  r2  .   .   MM  ");
          textmap[1].Add ("MM  .   MM  .   MM  MM  MM  .   MM  ");
          textmap[1].Add ("MM  .   MM  .   .   .   MM  .   MM  ");
          textmap[1].Add ("MM  .   MM  .   MM  .   MM  r1  MM  ");
          textmap[1].Add ("MM  .   .   .   MM  .   MM  .   MM  ");
          textmap[1].Add ("MM  pn  b   .   MM  .   .   .   MM  ");
          textmap[1].Add ("MM  p0  .   .   MM  .   MM  .   MM  ");
          textmap[1].Add ("MM  MM  MM  MM  MM  MM  MM  MM  MM  ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   lw  .   lw  .   lw  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   lw  .   lw  .   lw  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   lw  .   lw  .   lw  .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   ");
          }

        // test map - rube goldberg
        else if (player_level == 12)
          {
          ambient_light = new Color (80, 80, 80);
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  Cn  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  Cs  TK  Ce  Cn  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  Cs  TK  Cn  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  Cn  Cw  Cw  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");
          textmap[0].Add ("TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  TK  ");

          textmap[1].Add ("BR  BR  BR  BR  BR  BR  XR  BR  BR  BR  BR  BR  BR  BR  ");
          textmap[1].Add ("BR  .   .   .   .   pn  p0  .   .   .   .   .   .   BR  ");
          textmap[1].Add ("BR  .   WE  WE  WE  .   WE  WE  .   WE  WE  WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  .   .   bb  .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  .   wi  lhg .   .   WE  .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  .   wi  .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  WE  wi  WE  .   WE  .   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  .   DGH .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  WE  WE  .   .   WE  i   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  .   .   .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  .   .   .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add ("BR  .   WE  WE  WE  WE  WE  WE  WE  WE  WE  WE  .   BR  ");
          textmap[1].Add ("BR  .   .   .   .   .   .   .   .   .   .   .   .   BR  ");
          textmap[1].Add ("BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  ");

          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   lW  .   lW  .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   lW  .   lW  .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   lW  .   lW  .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // test map - web page
        else if (player_level == 13)
          {
          ambient_light = new Color (96, 96, 96);
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add ("..  ..  ..  ..  ..  ..  ..  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  log00  00  00  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  00  00  00  00  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("CE  CE  CE  CE  TK  TK  TK  TK  CE  CE  CE  CE  FM  ");
          textmap[0].Add ("..  ..  ..  ..  ..  ..  ..  ..  FM  FM  FM  FM  FM  ");

          textmap[1].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[1].Add ("DP  DP  DP  DP  DP  DP  DP  ..  DP  DP  DP  DP  ..  ");
          textmap[1].Add ("DP  tp  ..  b   p0  pn  ..  b   ..  m   vm  DP  ..  ");
          textmap[1].Add ("DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
          textmap[1].Add ("DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  b   DP  ..  ");
          textmap[1].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");

          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  lw  ..  lw  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  lw  ..  lw  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          textmap[2].Add ("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          }

        // test map - rube goldberg in feature order
        // 1 - pressure switch opens door
        // 2 - box behind door travels down conveyor belt
        // 3 - box is torched by incinerator
        // 4 - box sets second box on fire
        else if (player_level == 14)
          {
          ambient_light = new Color (96, 96, 96);

          textmap[0].Add (".   .   .   .   .   ");
          textmap[0].Add (".   TK  Cns TK  .   ");
          textmap[0].Add (".   TK  Cns TK  .   ");
          textmap[0].Add (".   TK  Cns TK  .   ");
          textmap[0].Add (".   TK  Cns PSG .   ");
          textmap[0].Add (".   TK  TK  TK  .   ");
          textmap[0].Add (".   TK  TK  TK  .   ");
          textmap[0].Add (".   TK  TK  TK  .   ");
          textmap[0].Add (".   .   .   .   .   ");

          textmap[1].Add ("BY  BY  BY  BY  BY  ");
          textmap[1].Add ("BY  .   b   pn  BY  ");
          textmap[1].Add ("BY  .   .   .   BY  ");
          textmap[1].Add ("BY  .   DGV .   BY  ");
          textmap[1].Add ("BY  .   .   .   BY  ");
          textmap[1].Add ("BY  .   .   .   BY  ");
          textmap[1].Add ("BY  .   b   .   BY  ");
          textmap[1].Add ("BY  .   .   .   BY  ");
          textmap[1].Add ("BY  BY  BY  BY  BY  ");

          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          textmap[2].Add (".   .   .   .   .   ");
          }

        // box shadow debug map
        else if (player_level == 15)
          {
          ambient_light = new Color (96, 96, 96);

          textmap[0].Add ("TK  .   .   ");
          textmap[0].Add (".   Cs  Cw  ");
          textmap[0].Add (".   Ce  Cn  ");

          textmap[1].Add ("pn  .   .   ");
          textmap[1].Add (".   b   .   ");
          textmap[1].Add (".   .   .   ");

          textmap[2].Add (".   .   .   ");
          textmap[2].Add (".   .   .   ");
          textmap[2].Add (".   .   .   ");
          }

        total_levels = 15;

        map.tile_width = textmap[0][0].Length / 4;
        map.tile_length = textmap[0].Count;
        map.tile_height = textmap.Count;

        map.char_width = map.tile_width * 4;

        for (mz = 0; mz < map.tile_height; mz += 1)
          for (my = 0; my < map.tile_length; my += 1)
            for (mx = 0; mx < map.tile_width; mx += 1)
              matrixmap[mx, my, mz] = textmap[mz][my].Substring (mx * 4, 3);
        }

      construct_world ();

      fader.reset ();
      fadein ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void construct_world ()  // build world from matrix or reset current map
      {
      string gridspace;
      int mx, my, mz;
      int x, y, z;
      //int light_number = 0;
      //float light_alpha = 0f;
      //Color light_color;
      //string gate;
      int b, f, f2, f3;//, r;
      bool electric_north, electric_south, electric_east, electric_west;
      int spot;
      //bool found;
      int tile, tile_front;

      // clear level data
      brush_control.brush = new List<Brush> ();
      fixture_control.fixture = new List<Fixture> ();
      object_control.obj = new List<Object> ();
      light = new List<Light> ();
      //total_draw_slots = 1;
      total_character_spots = 0;

      while (character_control.character.Count > 1)
        {
        if (character_control.character[0].type == Name.RICHARD) character_control.character.RemoveAt (1);
        else character_control.character.RemoveAt (0);
        }

      if (character_control.character.Count == 1 && character_control.character[0].type == Name.RICHARD) PLAYER = 0;

      // map reader
      x = 0; y = map.tile_length * tilesize; z = 0;

      for (mz = 0; mz < map.tile_height; mz += 1)
        {
        for (my = 0; my < map.tile_length; my += 1)
          {
          for (mx = 0; mx < map.tile_width; mx += 1)
            {
            gridspace = matrixmap[mx, my, mz];

            if (gridspace == "// ")
              {
              random_character_spot[total_character_spots].x = x;
              random_character_spot[total_character_spots].y = y;
              random_character_spot[total_character_spots].z = z;
              random_character_spot[total_character_spots].used = false;
              total_character_spots += 1;
              }
            else if (gridspace == "00 ") add_brush ((int) Texture_Type.invisible, (int) Texture_Type.invisible, x, y, z);
            else if (gridspace == "bb ") add_brush ((int) Texture_Type.box_banded_test, (int) Texture_Type.box_banded_test, x, y, z);
            else if (gridspace == "BM ") add_brush ((int) Texture_Type.big_machine_test, (int) Texture_Type.big_machine_test, x, y, z);
            else if (gridspace == "DRV") add_brush ((int) Texture_Type.door_red_v_top_closed_test, (int) Texture_Type.door_red_v_front_closed_test, x, y, z);
            else if (gridspace == "DRH") add_brush ((int) Texture_Type.door_red_h_top_closed_test, (int) Texture_Type.door_red_h_front_closed_test, x, y, z);
            else if (gridspace == "DYV") add_brush ((int) Texture_Type.door_yellow_v_top_closed_test, (int) Texture_Type.door_yellow_v_front_closed_test, x, y, z);
            else if (gridspace == "DYH") add_brush ((int) Texture_Type.door_yellow_h_top_closed_test, (int) Texture_Type.door_yellow_h_front_closed_test, x, y, z);
            else if (gridspace == "DGV") add_brush ((int) Texture_Type.door_green_v_top_closed_test, (int) Texture_Type.door_green_v_front_closed_test, x, y, z);
            else if (gridspace == "DGH") add_brush ((int) Texture_Type.door_green_h_top_closed_test, (int) Texture_Type.door_green_h_front_closed_test, x, y, z);
            else if (gridspace == "DBV") add_brush ((int) Texture_Type.door_blue_v_top_closed_test, (int) Texture_Type.door_blue_v_front_closed_test, x, y, z);
            else if (gridspace == "DBH") add_brush ((int) Texture_Type.door_blue_h_top_closed_test, (int) Texture_Type.door_blue_h_front_closed_test, x, y, z);
            else if (gridspace == "DP ") add_brush ((int) Texture_Type.drywall_purple_top_test, (int) Texture_Type.drywall_purple_front_test, x, y, z);
            else if (gridspace == "DT ") add_brush ((int) Texture_Type.drywall_tan_top_test, (int) Texture_Type.drywall_tan_front_test, x, y, z);
            else if (gridspace == "DY ") add_brush ((int) Texture_Type.drywall_yellow_top_test, (int) Texture_Type.drywall_yellow_front_test, x, y, z);
            else if (gridspace == "EN ") add_brush ((int) Texture_Type.gateway_v_top_test, (int) Texture_Type.gateway_v_front_closed_test, x, y, z);
            else if (gridspace == "En ") add_brush ((int) Texture_Type.gateway_h_top_test, (int) Texture_Type.gateway_h_front_test, x, y, z);
            else if ((gridspace[0] == 'G' || gridspace[0] == 'g')  // gateway to new map
                     && (gridspace[1] == '0' || gridspace[1] == '1' || gridspace[1] == '2' || gridspace[1] == '3' || gridspace[1] == '4'
                     || gridspace[1] == '5' || gridspace[1] == '6' || gridspace[1] == '7' || gridspace[1] == '8' || gridspace[1] == '9'))
              {
              if (gridspace[0] == 'G') add_brush ((int) Texture_Type.gateway_v_top_test, (int) Texture_Type.gateway_v_front_open_test, x, y, z);
              else if (gridspace[0] == 'g') add_brush ((int) Texture_Type.gateway_h_top_test, (int) Texture_Type.gateway_h_front_test, x, y, z);

              brush_control.brush[brush_control.brush.Count - 1].gateway = Convert.ToInt16 (gridspace.Substring (1, 2));
              }
            else if (gridspace == "FG ") add_brush ((int) Texture_Type.floor_grate_test, (int) Texture_Type.floor_grate_test, x, y, z);
            else if (gridspace == "IU ") add_brush ((int) Texture_Type.incinerator_test_up, (int) Texture_Type.metal_black_test, x, y, z);
            else if (gridspace == "ID ") add_brush ((int) Texture_Type.incinerator_test_down, (int) Texture_Type.incinerator_test_down_front, x, y, z);
            else if (gridspace == "IL ") add_brush ((int) Texture_Type.incinerator_test_left, (int) Texture_Type.metal_black_test, x, y, z);
            else if (gridspace == "IR ") add_brush ((int) Texture_Type.incinerator_test_right, (int) Texture_Type.metal_black_test, x, y, z);
            else if (gridspace == "log") add_brush ((int) Texture_Type.floor_logo_test, (int) Texture_Type.floor_logo_test, x, y, z);
            else if (gridspace == "m  ") add_brush ((int) Texture_Type.box_metal_test, (int) Texture_Type.box_metal_test, x, y, z);
            else if (gridspace == "MB ") add_brush ((int) Texture_Type.metal_blue_top_test, (int) Texture_Type.metal_blue_front_test, x, y, z);
            else if (gridspace == "MM ") add_brush ((int) Texture_Type.metal_mint_top_test, (int) Texture_Type.metal_mint_front_test, x, y, z);
            else if (gridspace == "PSB") add_brush ((int) Texture_Type.switch_blue_test, (int) Texture_Type.switch_blue_test, x, y, z);
            else if (gridspace == "PSG") add_brush ((int) Texture_Type.switch_green_test, (int) Texture_Type.switch_green_test, x, y, z);
            else if (gridspace == "PSR") add_brush ((int) Texture_Type.switch_red_test, (int) Texture_Type.switch_red_test, x, y, z);
            else if (gridspace == "PSY") add_brush ((int) Texture_Type.switch_yellow_test, (int) Texture_Type.switch_yellow_test, x, y, z);
            else if (gridspace == "XR ") add_brush ((int) Texture_Type.exit_red_v_top_closed_test, (int) Texture_Type.exit_red_v_front_closed_test, x, y, z);  // red vertical exit
            else if (gridspace == "Xr ") add_brush ((int) Texture_Type.exit_red_h_top_closed_test, (int) Texture_Type.exit_red_h_front_closed_test, x, y, z);  // red horizontal exit

            else if (gridspace == "B1w") add_fixture (Fixture_Type.bench1_west_test, x, y, z);
            else if (gridspace == "C1s") add_fixture (Fixture_Type.chair1_south_test, x, y, z);
            else if (gridspace == "C3e") add_fixture (Fixture_Type.chair3_east_test, x, y, z);
            else if (gridspace == "C3n") add_fixture (Fixture_Type.chair3_north_test, x, y, z);
            else if (gridspace == "Cnn") add_fixture (Fixture_Type.conveyor_north_test, x, y, z);
            else if (gridspace == "Cns") add_fixture (Fixture_Type.conveyor_south_test, x, y, z);
            else if (gridspace == "Cne") add_fixture (Fixture_Type.conveyor_east_test, x, y, z);
            else if (gridspace == "Cnw") add_fixture (Fixture_Type.conveyor_west_test, x, y, z);
            else if (gridspace == "Cos") add_fixture (Fixture_Type.couch_south_test, x, y, z);
            else if (gridspace == "D1 ") add_fixture (Fixture_Type.desk1_test, x, y, z);
            else if (gridspace == "Dmh") add_fixture (Fixture_Type.door_mint_horizontal_test, x, y, z);
            else if (gridspace == "Dmv") add_fixture (Fixture_Type.door_mint_vertical_test, x, y, z);
            else if (gridspace == "Dwh") add_fixture (Fixture_Type.door_white_horizontal_test, x, y, z);
            else if (gridspace == "Fc ") add_fixture (Fixture_Type.filing_test, x, y, z);
            else if (gridspace == "Lhg") add_fixture (Fixture_Type.laser_horizontal_green_test, x, y, z);
            else if (gridspace == "K1 ") add_fixture (Fixture_Type.keydoor_1_test, x, y, z);
            else if (gridspace == "K10") add_fixture (Fixture_Type.keydoor_10_test, x, y, z);
            else if (gridspace == "P1g") add_fixture (Fixture_Type.plant1_green_test, x, y, z);
            else if (gridspace == "P1m") add_fixture (Fixture_Type.plant1_mint_test, x, y, z);
            else if (gridspace == "Ph1") add_fixture (Fixture_Type.phone1_test, x, y, z);
            else if (gridspace == "Tb1") add_fixture (Fixture_Type.table1_test, x, y, z);
            else if (gridspace == "Tb2") add_fixture (Fixture_Type.table2_test, x, y, z);
            else if (gridspace == "T3m") add_fixture (Fixture_Type.table3_mint_test, x, y, z);
            else if (gridspace == "T3w") add_fixture (Fixture_Type.table3_white_test, x, y, z);
            else if (gridspace == "Tv ") add_fixture (Fixture_Type.tv1_test, x, y, z);
            else if (gridspace == "Tvr") add_fixture (Fixture_Type.tv2_test, x, y, z);
            else if (gridspace == "Vr ") add_fixture (Fixture_Type.vending_red_test, x, y, z);
            else if (gridspace == "Vy ") add_fixture (Fixture_Type.vending_yellow_test, x, y, z);
            else if (gridspace == "Wi ") add_fixture (Fixture_Type.wires, x, y, z);

            else if (gridspace == "c  ") add_object ((int) Object_Type.coin, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "e  ") add_object ((int) Object_Type.energy, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "h  ") add_object ((int) Object_Type.health, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "k  ") add_object ((int) Object_Type.key, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));

            if (gridspace == "lW ") add_light (Color.White, x + (tilesize / 2), y + (tilesize / 2), z, 2f, 1f, SOLID);
            else if (gridspace == "lw ") add_light (Color.White, x + (tilesize / 2), y + (tilesize / 2), z, 3f, .6f, SOLID);
            else if (gridspace == "lY ") add_light (new Color (255, 255, 64), x + (tilesize / 2), y + (tilesize / 2), z, 2f, 1f, SOLID);
            else if (gridspace == "ly ") add_light (new Color (255, 255, 128), x + (tilesize / 2), y + (tilesize / 2), z, 3f, .7f, SOLID);
            else if (gridspace == "lB ") add_light (Color.Blue, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
            else if (gridspace == "lb ") add_light (new Color (128, 128, 255), x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
            else if (gridspace == "lF ") add_light (Color.Purple, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
            else if (gridspace == "lG ") add_light (Color.Green, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
            else if (gridspace == "lR ") add_light (Color.Red, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);

            if (gridspace != null && gridspace[0] == 'p' && gridspace.Length > 1)
              {
              int level_tag;
              if (gridspace[1] != 'n' && int.TryParse (gridspace.Substring (1, 2), out level_tag) && player_last_level == level_tag)
                {
                player_start_location (x, y, z);
                }
              else if (gridspace[1] == 'n' && player_last_level == -1) player_start_location (x, y, z);
              }
            else if (gridspace == "r1 " && toggle_enemies == true) add_character (Name.RETARD, x + (tilesize / 2), y + (tilesize / 2), z);
            else if (gridspace == "r2 " && toggle_enemies == true) add_character (Name.THROWING_RETARD, x + (tilesize / 2), y + (tilesize / 2), z);
            else if (gridspace == "rd " && toggle_enemies == true) add_character (Name.RICHARDS_DAD, x + (tilesize / 2), y + (tilesize / 2), z);
            else if (gridspace == "s  ") add_object ((int) Object_Type.scrap_metal, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sc ")
              {
              add_character (Name.SECRETARY, x + (tilesize / 2), y + (tilesize / 2), z);
              }
            else if (gridspace == "se ") add_object ((int) Object_Type.shirt_teal, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sf ") add_object ((int) Object_Type.shirt_red, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "si ") add_object ((int) Object_Type.shirt_white, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sm ") add_object ((int) Object_Type.shirt_fushia, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sy ") add_object ((int) Object_Type.shirt_yellow, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));

            else
              {
              tile = brush_control.find_tile_from_symbol (gridspace);
              if (tile > -1)
                {
                if (brush_control.tile[tile].name.Substring (brush_control.tile[tile].name.Length - 3) == "top") tile_front = tile + 1;
                else tile_front = tile;

                add_brush (tile, tile_front, x, y, z);
                }
              }

            x += tilesize;
            }
          x = 0;
          y -= tilesize;
          }
        x = 0;
        y = map.tile_length * tilesize;
        z += tilesize;
        }

      map.pixel_width = tilesize * map.tile_width;
      map.pixel_length = tilesize * map.tile_length;
      map.pixel_height = tilesize * map.tile_height;

      for (b = 0; b < brush_control.brush.Count; b += 1)
        {
        // add background textures to brushes that need them
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_yellow_test ||
            brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_red_test ||
            brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_green_test ||
            brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_blue_test)
          {
          int clip = brush_control.brush_north_of_brush (brush_control.brush[b], false, false);
          if (clip == -1) clip = brush_control.brush_west_of_brush (brush_control.brush[b], false, false);
          if (clip == -1) clip = brush_control.brush_east_of_brush (brush_control.brush[b], false, false);
          if (clip == -1) clip = brush_control.brush_south_of_brush (brush_control.brush[b], false, false);
          if (clip > -1) brush_control.brush[b].background_texture = brush_control.brush[clip].top_texture_number;
          else brush_control.brush[b].background_texture = (int) Texture_Type.tile_black;
          }

        // find any shadows being cast on the top of the brush and save them
        brush_control.calculate_top_shadows (brush_control.brush[b]);

        // outline each group of same-texture brushes in cartoony black
        brush_control.calculate_outlines (brush_control.brush[b]);
        }

      // reassign generic wires and pipes with facing directions based on surroundings
      for (f = 0; f < fixture_control.fixture.Count; f += 1)
        {
        if (fixture_control.fixture[f].type == Fixture_Type.wires)
          {
          b = brush_control.point_in_brush (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_north = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_north = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_north = true;
          else electric_north = false;

          b = brush_control.point_in_brush (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y - (tilesize / 2), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y - (tilesize / 2), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y - (tilesize / 2), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_south = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_south = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_south = true;
          else electric_south = false;

          b = brush_control.point_in_brush (fixture_control.fixture[f].x - (tilesize / 2), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x - (tilesize / 2), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x - (tilesize / 2), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_west = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_west = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_west = true;
          else electric_west = false;

          b = brush_control.point_in_brush (fixture_control.fixture[f].x + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_east = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_east = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_east = true;
          else electric_east = false;

          if (electric_north == true && electric_south == true) fixture_control.fixture[f].type = Fixture_Type.wires_vertical_test;
          else if (electric_south == true && electric_east == true) fixture_control.fixture[f].type = Fixture_Type.wires_southeast_test;
          else if (electric_north == true) fixture_control.fixture[f].type = Fixture_Type.wires_vertical_test;
          else if (electric_south == true) fixture_control.fixture[f].type = Fixture_Type.wires_vertical_test;
          else fixture_control.fixture[f].type = Fixture_Type.wires_horizontal_test;
          }
        }

      //Add_Stickers ();z

      screen.scroll_x = 0;
      screen.scroll_y = 0;

      // reset player stats
      for (int c = 0; c < character_control.character.Count; c += 1)
        {
        if (character_control.character[c].type == Name.RICHARD) PLAYER = c;
        }
      if (PLAYER == -1) add_player ();
      character_control.character[PLAYER].health = 100;
      character_control.character[PLAYER].action = Action.none;
      character_control.character[PLAYER].on_fire = 0;

      // random enemy placement
      if (total_character_spots >= random_retard1 + random_retard2)
        {
        spot = -1;
        for (int guy = 0; guy < random_retard1; guy += 1)
          {
          while (spot == -1 || random_character_spot[spot].used == true) spot = rnd.Next (0, total_character_spots);
          add_character (Name.RETARD, random_character_spot[spot].x + (tilesize / 2), random_character_spot[spot].y + (tilesize / 2), random_character_spot[spot].z);
          random_character_spot[spot].used = true;
          }
        spot = -1;
        for (int guy = 0; guy < random_retard2; guy += 1)
          {
          while (spot == -1 || random_character_spot[spot].used == true) spot = rnd.Next (0, total_character_spots);
          add_character (Name.THROWING_RETARD, random_character_spot[spot].x + (tilesize / 2), random_character_spot[spot].y + (tilesize / 2), random_character_spot[spot].z);
          random_character_spot[spot].used = true;
          }
        }

      floor_buffer = new RenderTarget2D (GraphicsDevice, map.pixel_width, map.pixel_length);
      }
    }
  }
