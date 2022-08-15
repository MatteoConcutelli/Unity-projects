using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct block_type
{
    public byte top;
    public byte bot;
    public byte left;
    public byte right;
    public byte front;
    public byte back;
    public byte ID;
}

public class Blocks
{
    public block_type Air_block;
    public block_type Grass_block;
    public block_type Stone_block;
    public block_type Wood_block;
    public block_type Mattoni_block;

    public Blocks() {

        Air_block.top = 0;
        Air_block.bot = 0;
        Air_block.left = 0;
        Air_block.right = 0;
        Air_block.front = 0;
        Air_block.back = 0;
        Air_block.ID = 0;

        Grass_block.top = 0;
        Grass_block.bot = 2;
        Grass_block.left = 3;
        Grass_block.right = 3;
        Grass_block.front = 3;
        Grass_block.back = 3;
        Grass_block.ID = 1;

        Stone_block.top = 1;
        Stone_block.bot = 1;
        Stone_block.left = 1;
        Stone_block.right = 1;
        Stone_block.front = 1;
        Stone_block.back = 1;
        Stone_block.ID = 2;

        Wood_block.top = 4;
        Wood_block.bot = 4;
        Wood_block.left = 4;
        Wood_block.right = 4;
        Wood_block.front = 4;
        Wood_block.back = 4;
        Wood_block.ID = 3;

        Mattoni_block.top = 7;
        Mattoni_block.bot = 7;
        Mattoni_block.left = 7;
        Mattoni_block.right = 7;
        Mattoni_block.front = 7;
        Mattoni_block.back = 7;
        Mattoni_block.ID = 4;

    }
}
