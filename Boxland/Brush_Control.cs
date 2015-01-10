using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Brush_Control
    {
    public List<Brush> brush = new List<Brush> ();
    public const int max_brushes = 625;

    public const int max_textures = 96;
    public Texture2D[] texture = new Texture2D[max_textures];

    public const string Texture_Path = @"c:\boxland_images\";

    int tilesize;

    // TEXTURES
    public enum T
      {
      //ZERO,
      INVISIBLE_WALL,
      ASPHALT_TEST,
      BOX_ICE_TEST,
      BOX_METAL_TEST,
      BOX_WOOD_TEST,
      BRICK_GREY_TEST,
      BRICK_RED_TEST,
      BRICK_WHITE_TEST,
      CARPET_GREY_TEST,
      CARPET_PURPLE_TEST,
      DOOR_RED_V_TOP_CLOSED_TEST,
      DOOR_RED_V_TOP_OPEN_TEST,
      DOOR_RED_V_FRONT_CLOSED_TEST,
      DOOR_RED_V_FRONT_OPEN_TEST,
      DOOR_RED_H_TOP_CLOSED_TEST,
      DOOR_RED_H_TOP_OPEN_TEST,
      DOOR_RED_H_FRONT_CLOSED_TEST,
      DOOR_RED_H_FRONT_OPEN_TEST,
      DOOR_GREEN_V_TOP_CLOSED_TEST,
      DOOR_GREEN_V_TOP_OPEN_TEST,
      DOOR_GREEN_V_FRONT_CLOSED_TEST,
      DOOR_GREEN_V_FRONT_OPEN_TEST,
      DOOR_GREEN_H_TOP_CLOSED_TEST,
      DOOR_GREEN_H_TOP_OPEN_TEST,
      DOOR_GREEN_H_FRONT_CLOSED_TEST,
      DOOR_GREEN_H_FRONT_OPEN_TEST,
      DOOR_YELLOW_V_TOP_CLOSED_TEST,
      DOOR_YELLOW_V_TOP_OPEN_TEST,
      DOOR_YELLOW_V_FRONT_CLOSED_TEST,
      DOOR_YELLOW_V_FRONT_OPEN_TEST,
      DOOR_YELLOW_H_TOP_CLOSED_TEST,
      DOOR_YELLOW_H_TOP_OPEN_TEST,
      DOOR_YELLOW_H_FRONT_CLOSED_TEST,
      DOOR_YELLOW_H_FRONT_OPEN_TEST,
      DOOR_BLUE_V_TOP_CLOSED_TEST,
      DOOR_BLUE_V_TOP_OPEN_TEST,
      DOOR_BLUE_V_FRONT_CLOSED_TEST,
      DOOR_BLUE_V_FRONT_OPEN_TEST,
      DOOR_BLUE_H_TOP_CLOSED_TEST,
      DOOR_BLUE_H_TOP_OPEN_TEST,
      DOOR_BLUE_H_FRONT_CLOSED_TEST,
      DOOR_BLUE_H_FRONT_OPEN_TEST,
      DRYWALL_MINT_FRONT_TEST,
      DRYWALL_MINT_TOP_TEST,
      DRYWALL_PURPLE_FRONT_TEST,
      DRYWALL_PURPLE_TOP_TEST,
      DRYWALL_TAN_TOP_TEST,
      DRYWALL_TAN_FRONT_TEST,
      DRYWALL_YELLOW_FRONT_TEST,
      DRYWALL_YELLOW_TOP_TEST,
      EXIT_RED_V_TOP_CLOSED_TEST,
      EXIT_RED_V_TOP_OPEN_TEST,
      EXIT_RED_V_FRONT_CLOSED_TEST,
      EXIT_RED_V_FRONT_OPEN_TEST,
      EXIT_RED_H_TOP_CLOSED_TEST,
      EXIT_RED_H_TOP_OPEN_TEST,
      EXIT_RED_H_FRONT_CLOSED_TEST,
      EXIT_RED_H_FRONT_OPEN_TEST,
      FLOOR_GRATE_TEST,
      FLOOR_METAL_TEST,
      FLOOR_ZONE_YELLOW_TEST,
      FLOOR_ZONE_RED_TEST,
      FLOOR_ZONE_GREEN_TEST,
      GATEWAY_V_TOP_TEST,
      GATEWAY_V_FRONT_CLOSED_TEST,
      GATEWAY_V_FRONT_OPEN_TEST,
      GATEWAY_H_TOP_TEST,
      GATEWAY_H_FRONT_TEST,
      GRASS,
      METAL_BLACK_TEST,
      METAL_BLUE_FRONT_TEST,
      METAL_BLUE_TOP_TEST,
      METAL_MINT_FRONT_TEST,
      METAL_MINT_TOP_TEST,
      SIDEWALK_TEST,
      SWITCH_GREEN_TEST,
      SWITCH_GREEN_DOWN_TEST,
      SWITCH_BLUE_TEST,
      SWITCH_BLUE_DOWN_TEST,
      SWITCH_RED_TEST,
      SWITCH_RED_DOWN_TEST,
      SWITCH_YELLOW_TEST,
      SWITCH_YELLOW_DOWN_TEST,
      TEXTURE_HIGHLIGHT_RED,
      TILE_BLACK_TEST,
      TILE_BLUE_TEST,
      TILE_BROWN_TEST,
      INCINERATOR_TEST_UP,
      INCINERATOR_TEST_DOWN,
      INCINERATOR_TEST_DOWN_FRONT,
      INCINERATOR_TEST_LEFT,
      INCINERATOR_TEST_RIGHT,
      /////////////////////////////////////
      SINGLE_PIECE,                      // everything below here is a single piece that does not tile
      /////////////////////////////////////
      BOX_BANDED_TEST,
      //WARNING_SIGN_TEST1,
      //WARNING_SIGN_TEST2,
      //WARNING_SIGN_TEST3,
      //WARNING_SIGN_TEST4,
      BIG_MACHINE_TEST,
      FLOOR_LOGO_TEST
      }
    
    public Brush_Control (int new_tilesize)
      {
      tilesize = new_tilesize;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void load_textures (GraphicsDevice GraphicsDevice)
      {
      texture[(int) T.ASPHALT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\asphalt_test.png", FileMode.Open, FileAccess.Read));

      texture[(int) Brush_Control.T.BOX_ICE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\box_ice4.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (texture[(int) Brush_Control.T.BOX_ICE_TEST], new Color (255, 0, 255, 255));
      
      texture[(int) T.BOX_METAL_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\box_metal_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.BOX_WOOD_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\box_wood_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.BRICK_GREY_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\brick_grey_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.BRICK_RED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\brick_red.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.BRICK_WHITE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\brick_white_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.CARPET_GREY_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\carpet_grey_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.CARPET_PURPLE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\carpet_purple_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_RED_V_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_vertical_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_RED_V_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_vertical_top_open.png", FileMode.Open, FileAccess.Read));
      
      texture[(int) T.DOOR_RED_V_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_vertical_front_closed.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_RED_V_FRONT_CLOSED_TEST], new Color (255, 0, 255, 255));
      
      texture[(int) T.DOOR_RED_V_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_vertical_front_open.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_RED_V_FRONT_OPEN_TEST], new Color (255, 0, 255, 255));
      
      texture[(int) T.DOOR_RED_H_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_horizontal_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_RED_H_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_horizontal_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_RED_H_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_horizontal_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_RED_H_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_red_horizontal_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_YELLOW_V_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_vertical_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_YELLOW_V_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_vertical_top_open.png", FileMode.Open, FileAccess.Read));
      
      texture[(int) T.DOOR_YELLOW_V_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_vertical_front_closed.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_YELLOW_V_FRONT_CLOSED_TEST], new Color (255, 0, 255, 255));
      
      texture[(int) T.DOOR_YELLOW_V_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_vertical_front_open.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_YELLOW_V_FRONT_OPEN_TEST], new Color (255, 0, 255, 255));
      
      texture[(int) T.DOOR_YELLOW_H_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_horizontal_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_YELLOW_H_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_horizontal_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_YELLOW_H_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_horizontal_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_YELLOW_H_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_yellow_horizontal_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_GREEN_V_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_vertical_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_GREEN_V_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_vertical_top_open.png", FileMode.Open, FileAccess.Read));
      
      texture[(int) T.DOOR_GREEN_V_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_vertical_front_closed.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_GREEN_V_FRONT_CLOSED_TEST], new Color (255, 0, 255, 255));
      
      texture[(int) T.DOOR_GREEN_V_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_vertical_front_open.png", FileMode.Open, FileAccess.Read));
     
      texture[(int) T.DOOR_GREEN_H_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_horizontal_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_GREEN_H_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_horizontal_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_GREEN_H_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_horizontal_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_GREEN_H_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_green_horizontal_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_V_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_vertical_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_V_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_vertical_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_V_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_vertical_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_V_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_vertical_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_H_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_horizontal_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_H_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_horizontal_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_H_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_horizontal_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DOOR_BLUE_H_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\door_test_blue_horizontal_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_MINT_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_mint_top_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_MINT_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_mint_front_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_PURPLE_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_purple_top_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_PURPLE_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_purple_front_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_TAN_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_tan_top_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_TAN_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_tan_front_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_YELLOW_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_yellow_front_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.DRYWALL_YELLOW_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\drywall_yellow_top_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_V_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_vertical_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_V_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_vertical_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_V_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_vertical_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_V_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_vertical_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_H_TOP_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_horizontal_top_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_H_TOP_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_horizontal_top_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_H_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_horizontal_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.EXIT_RED_H_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\exit_test_red_horizontal_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.FLOOR_GRATE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\floor_grate_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.FLOOR_LOGO_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\floor_test_logo.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.FLOOR_METAL_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\floor_metal_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.FLOOR_ZONE_GREEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\loading_zone_green_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.FLOOR_ZONE_RED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\loading_zone_red_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.FLOOR_ZONE_YELLOW_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\loading_zone_yellow_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.GATEWAY_V_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\gateway_test_vertical_top.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.GATEWAY_V_FRONT_CLOSED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\gateway_test_vertical_front_closed.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.GATEWAY_V_FRONT_OPEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\gateway_test_vertical_front_open.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.GATEWAY_H_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\gateway_test_horizontal_top.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.GATEWAY_H_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\gateway_test_horizontal_front.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.GRASS] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\grass.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.METAL_BLUE_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\metal_blue_front_test2.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.METAL_BLUE_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\metal_blue_top_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.METAL_MINT_FRONT_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\metal_mint_front_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.METAL_MINT_TOP_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\metal_mint_top_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SIDEWALK_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\sidewalk_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_GREEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_green.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_GREEN_DOWN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_green_down.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_RED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_red.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_RED_DOWN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_red_down.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_BLUE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_blue.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_BLUE_DOWN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_blue_down.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_YELLOW_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_yellow.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.SWITCH_YELLOW_DOWN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\switch_test_yellow_down.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.TEXTURE_HIGHLIGHT_RED] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\texture_highlight_red.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.TILE_BLACK_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\tile_black_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.TILE_BLUE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\tile_blue_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.TILE_BROWN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\tile_brown_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.METAL_BLACK_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\metal_black_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.INCINERATOR_TEST_UP] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\incinerator_test_up.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.INCINERATOR_TEST_DOWN] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\incinerator_test_down.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.INCINERATOR_TEST_DOWN_FRONT] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\incinerator_test_down_front.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.INCINERATOR_TEST_LEFT] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\incinerator_test_left.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.INCINERATOR_TEST_RIGHT] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\incinerator_test_right.png", FileMode.Open, FileAccess.Read));
      //texture[(int) T.WARNING_SIGN_TEST1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\warning_sign1.png", FileMode.Open, FileAccess.Read));
      //texture[(int) T.WARNING_SIGN_TEST2] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\warning_sign2.png", FileMode.Open, FileAccess.Read));
      //texture[(int) T.WARNING_SIGN_TEST3] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\warning_sign3.png", FileMode.Open, FileAccess.Read));
      //texture[(int) T.WARNING_SIGN_TEST4] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\warning_sign4.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.BIG_MACHINE_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\big_machine_test.png", FileMode.Open, FileAccess.Read));
      texture[(int) T.BOX_BANDED_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Texture_Path + "textures\\box_banded_test.png", FileMode.Open, FileAccess.Read));

      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_GREEN_V_FRONT_OPEN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_BLUE_V_FRONT_CLOSED_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.DOOR_BLUE_V_FRONT_OPEN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.EXIT_RED_V_TOP_CLOSED_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.EXIT_RED_V_TOP_OPEN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.EXIT_RED_V_FRONT_CLOSED_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.EXIT_RED_V_FRONT_OPEN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.FLOOR_GRATE_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.GATEWAY_V_FRONT_CLOSED_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.GATEWAY_V_FRONT_OPEN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.SWITCH_YELLOW_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.SWITCH_YELLOW_DOWN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (texture[(int) T.BOX_BANDED_TEST], new Color (255, 0, 255, 255));
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

    public void add (int top_texture_number, int front_texture_number, int x, int y, int z, int width, int length, int height)
      {
      int q;
      int top_offset_x = 0;
      int top_offset_y = 0;
      int front_offset_x;
      int front_offset_y;
      //bool tall_brush = false;
      Brush b = new Brush ();

      // check for identical brush underneath this one (combine into one tall block)
      if (z - (height / 2) >= 0)
        {
        //        b_clip = point_in_brush (x + (width / 2), y + (length / 2), z - (height / 2), false);
        //        if (b_clip > -1 && brush[b_clip].front_texture_number == front_texture_number)
        //          {
        //          brush[b_clip].height += height;
        //          tall_brush = true;
        //          }
        }

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
      if (top_texture_number != (int) T.INVISIBLE_WALL)
        {
        for (q = 0; q < x; q += tilesize)
          {
          top_offset_x += tilesize;
          if (top_offset_x + tilesize > texture[top_texture_number].Width) top_offset_x = 0;
          }
        top_offset_y = texture[top_texture_number].Height - tilesize;
        for (q = 0; q < y; q += tilesize)
          {
          top_offset_y -= tilesize;
          if (top_offset_y < 0) top_offset_y = texture[top_texture_number].Height - tilesize;
          }
        }
      b.top_texture_offset_x = top_offset_x;
      b.top_texture_offset_y = top_offset_y;

      // texture on front of brush
      b.front_texture_number = front_texture_number;

      // front texture offsets in texture sheet
      front_offset_x = top_offset_x;
      front_offset_y = top_offset_y + length;
      if (front_texture_number != (int) T.INVISIBLE_WALL)
        {
        for (q = 0; q < x; q += tilesize)
          {
          front_offset_x += tilesize;
          if (front_offset_x + tilesize > texture[front_texture_number].Width) front_offset_x = 0;
          }
        front_offset_y = texture[front_texture_number].Height - tilesize;
        for (q = 0; q < z; q += tilesize)
          {
          front_offset_y -= tilesize;
          if (front_offset_y < 0) front_offset_y = texture[front_texture_number].Height - tilesize;
          }
        front_offset_y += tilesize;
        if (front_offset_y + tilesize > texture[front_texture_number].Height) front_offset_y = 0;
        }

      b.front_texture_offset_x = front_offset_x;
      b.front_texture_offset_y = front_offset_y;

      // brush traits based on texture
      if (top_texture_number == (int) T.INVISIBLE_WALL) b.transparent = true;
      else if (top_texture_number == (int) T.BOX_BANDED_TEST) b.moveable = true;
      else if (top_texture_number == (int) T.BOX_ICE_TEST)
        {
        b.moveable = true;
        b.transparent = true;
        }
      else if (top_texture_number == (int) T.BOX_METAL_TEST) b.moveable = true;
      else if (top_texture_number == (int) T.BOX_WOOD_TEST) b.moveable = true;
      else if (top_texture_number == (int) T.DOOR_RED_V_TOP_CLOSED_TEST)
        {
        b.transparent = true;
        b.electric = true;
        //b.door = Brush.Door.red;
        }
      else if (top_texture_number == (int) T.DOOR_YELLOW_V_TOP_CLOSED_TEST)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) T.DOOR_GREEN_V_TOP_CLOSED_TEST)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) T.DOOR_BLUE_V_TOP_CLOSED_TEST)
        {
        b.transparent = true;
        b.electric = true;
        }
      else if (top_texture_number == (int) T.DOOR_RED_H_TOP_CLOSED_TEST) b.electric = true;
      else if (top_texture_number == (int) T.DOOR_YELLOW_H_TOP_CLOSED_TEST) b.electric = true;
      else if (top_texture_number == (int) T.DOOR_GREEN_H_TOP_CLOSED_TEST) b.electric = true;
      else if (top_texture_number == (int) T.DOOR_BLUE_H_TOP_CLOSED_TEST) b.electric = true;
      else if (top_texture_number == (int) T.FLOOR_GRATE_TEST) b.transparent = true;
      else if (top_texture_number == (int) T.GATEWAY_V_TOP_TEST) b.transparent = true;
      else if (top_texture_number == (int) T.GATEWAY_H_TOP_TEST) b.transparent = true;

      brush.Add (b);
      }

    ////////////////////////////////////////////////////////////////////////////////

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
          if (!invisible_counts && brush[b].top_texture_number == (int) T.INVISIBLE_WALL) clip = -1;
          break;  // stop looking
          }
        b += 1;
        }
      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

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

    public int brush_around_brush (int b, int x_grid, int y_grid, int z_grid)
      {
      int x = brush[b].x + (tilesize / 2) + (tilesize * x_grid);
      int y = brush[b].y + (tilesize / 2) + (tilesize * y_grid);
      int z = brush[b].z + (tilesize / 2) + (tilesize * z_grid);

      return point_in_brush (x, y, z, true, true);
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_north_of_brush (Brush check_brush)
      {
      return point_in_brush (check_brush.x + (tilesize / 2), Convert.ToInt32 (check_brush.y + (tilesize * 1.5)), check_brush.z + (tilesize / 2), true, true);
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_south_of_brush (Brush check_brush)
      {
      return point_in_brush (check_brush.x + (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_east_of_brush (Brush check_brush)
      {
      return point_in_brush (Convert.ToInt32 (check_brush.x + (tilesize * 1.5)), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_west_of_brush (Brush check_brush)
      {
      return point_in_brush (check_brush.x - (tilesize / 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 2), true, true);
      }


    //////////////////////////////////////////////////////////////////////////////////

    public int brush_above_brush (Brush check_brush)
      {
      return point_in_brush (check_brush.x + (tilesize / 2), Convert.ToInt32 (check_brush.y + (tilesize / 2)), check_brush.z + tilesize + (tilesize / 2), true, true);
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int brush_below_south_of_brush (Brush check_brush)
      {
      return point_in_brush (check_brush.x + (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z - (tilesize / 2), true, true);
      }


    ////////////////////////////////////////////////////////////////////////////////

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
            if (fixture[f].type == (int) Fixture_Control.F.LASER_HORIZONTAL_GREEN_TEST) fixture[f].powered = true;
            }
          else clip = f;
          }
        f += 1;
        }

      return clip;
      }
    }
  }
