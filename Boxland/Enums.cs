using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxland
  {
  enum Game_State
    {
    title,
    game,
    creation
    }

  public enum Name
    {
    // NONE = -1;
    RICHARD,
    RICHARDS_DAD,
    RETARD,
    SECRETARY,
    THROWING_RETARD
    }

  public enum Action
    {
    attack1,
    attack2,
    attack3,
    attack4,
    attack_before,
    attack_after,
    death,
    flamethrower,
    freeze_ray,
    grab,
    jump,
    jump_kick,
    knocked_out,
    none,
    punch,
    push,
    run,
    skid,
    slide,
    superpunch,
    hurt1,
    hurt2,
    walk
    }

  public enum Fade_State
    {
    none,
    fading_in,
    fading_out
    }

  public enum Texture_Type
    {
    invisible,  // 0
    asphalt_test,
    box_ice_test,
    box_metal_test,
    box_wood,
    brick_blue_test,
    brick_grey_test,
    brick_red,
    brick_white_test,
    brick_yellow_test,
    carpet_grey_test,
    carpet_purple_test,
    door_red_v_top_closed_test,
    door_red_v_front_closed_test,
    door_red_v_top_open_test,
    door_red_v_front_open_test,
    door_red_h_top_closed_test,
    door_red_h_front_closed_test,
    door_red_h_top_open_test,
    door_red_h_front_open_test,
    door_green_v_top_closed_test,
    door_green_v_front_closed_test,
    door_green_v_top_open_test,
    door_green_v_front_open_test,
    door_green_h_top_closed_test,
    door_green_h_front_closed_test,
    door_green_h_top_open_test,
    door_green_h_front_open_test,
    door_yellow_v_top_closed_test,
    door_yellow_v_front_closed_test,
    door_yellow_v_top_open_test,
    door_yellow_v_front_open_test,
    door_yellow_h_top_closed_test,
    door_yellow_h_front_closed_test,
    door_yellow_h_top_open_test,
    door_yellow_h_front_open_test,
    door_blue_v_top_closed_test,
    door_blue_v_front_closed_test,
    door_blue_v_top_open_test,
    door_blue_v_front_open_test,
    door_blue_h_top_closed_test,
    door_blue_h_front_closed_test,
    door_blue_h_top_open_test,
    door_blue_h_front_open_test,
    drywall_mint_top_test,
    drywall_mint_front_test,
    drywall_purple_top_test,
    drywall_purple_front_test,
    drywall_tan_top_test,
    drywall_tan_front_test,
    drywall_yellow_top_test,
    drywall_yellow_front_test,
    exit_red_v_top_closed_test,
    exit_red_v_top_open_test,
    exit_red_v_front_closed_test,
    exit_red_h_top_closed_test,
    exit_red_v_front_open_test,
    exit_red_h_top_open_test,
    exit_red_h_front_closed_test,
    exit_red_h_front_open_test,
    floor_grate_test,
    floor_metal_test,
    floor_zone_yellow_test,
    floor_zone_red_test,
    floor_zone_green_test,
    gateway_v_top_test,
    gateway_v_front_closed_test,
    gateway_v_front_open_test,
    gateway_h_top_test,
    gateway_h_front_test,
    grass,
    metal_black_test,
    metal_blue_top_test,
    metal_blue_front_test,
    metal_mint_top_test,
    metal_mint_front_test,
    sidewalk_test,
    switch_green_test,
    switch_green_down_test,
    switch_blue_test,
    switch_blue_down_test,
    switch_red_test,
    switch_red_down_test,
    switch_yellow_test,
    switch_yellow_down_test,
    texture_highlight_red,
    tile_black,
    tile_blue_test,
    tile_brown,
    tile_mint,
    incinerator_test_up,
    incinerator_test_down,
    incinerator_test_down_front,
    incinerator_test_left,
    incinerator_test_right,
    /////////////////////////////////////
    single_piece,                      // everything below here is a single piece that does not tile
    /////////////////////////////////////
    box_banded_test,
    //warning_sign_test1,
    //warning_sign_test2,
    //warning_sign_test3,
    //warning_sign_test4,
    big_machine_test,
    floor_logo_test
    }

  public enum Fixture_Type
    {
    bench1_west_test,
    chair1_south_test,
    chair3_east_test,
    chair3_north_test,
    conveyor_north_test,
    conveyor_south_test,
    conveyor_east_test,
    conveyor_west_test,
    couch_south_test,
    desk1_test,
    door_mint_horizontal_test,
    door_mint_vertical_test,
    door_white_horizontal_test,
    filing_test,
    keydoor_1_test,
    keydoor_10_test,
    laser_fixture_horizontal_test,
    laser_horizontal_green_test,
    phone1_test,
    plant1_green_test,
    plant1_mint_test,
    table1_test,
    table2_test,
    table3_mint_test,
    table3_white_test,
    tv1_test,
    tv2_test,
    vending_red_test,
    vending_yellow_test,
    wires,
    wires_horizontal_test,
    wires_vertical_test,
    wires_southeast_test
    }

  public enum Fixture_Animation
    {
    idle,
    door_opening,
    door_closing
    }

  public enum Object_Type
    {
    none,
    // do not move or change shirt values (used for richard skins)
    shirt_yellow,  // 1
    shirt_white,   // 2
    shirt_red,     // 3
    shirt_teal,    // 4
    shirt_fushia,  // 5
                   /////////////////////////////////////////////////////////////
    health,
    hotdog,
    rock,
    //rock_brown,
    //rock_red,
    //ROCK_white,
    key,
    coin,
    scrap_metal,
    energy
    }

  public enum Door
    {
    none,
    pressure,
    swing,
    slide
    }

  public enum Light_Color
    {
    blue,
    blue_pale,
    dark,
    fushia,
    green,
    red,
    white,
    yellow,
    yellow_pale
    }

  enum Mouse_Button
    {
    none,
    left,
    right,
    middle
    }

  enum Controller_Button
    {
    none,
    A,
    B,
    X,
    Y,
    L,
    R,
    left_trigger,
    right_trigger,
    up,
    down,
    left,
    right,
    select,
    start,
    center
    }

  enum Movement
    {
    keyboard,
    mouse,
    controller
    }

  enum Key_Action
    {
    up,
    down,
    left,
    right,
    jump,
    attack,
    grab,
    run,
    special,
    cycle,
    shirt1,
    shirt2,
    shirt3,
    shirt4,
    shirt5,
    shirt6,
    menu,
    interact
    }

  enum Preset
    {
    keyboard1,
    keyboard2,
    mouse,
    controller1,
    controller2
    }

  public enum Click
    {
    none,
    new_game,
    exit
    }
  }
