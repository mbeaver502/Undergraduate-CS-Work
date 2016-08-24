/*
    Michael Beaver
    CS 470, Spring 2016
    Program 2
    23 March 2016
 
    Resources:
        http://en.cppreference.com/w/cpp/container/priority_queue
        http://cboard.cprogramming.com/cplusplus-programming/78627-stl-min-priority-queue.html#post556126
        http://en.cppreference.com/w/cpp/container/unordered_map/unordered_map
        http://www.cplusplus.com/reference/vector/vector/
 
    Notes:
        Be sure to include -std=c++11 in compiler flags.
 
    Description:
        This program implements the A* search algorithm to solve the classic 8-puzzle.
        This particular implementation uses a 2D vector to represent the puzzle
        board internally:
                +----+----+----+
                | 00 | 01 | 02 |
                +----+----+----+        We can use these coordinates to
                | 10 | 11 | 12 |            calculate the Manhattan Distance
                +----+----+----+            within the heuristic function.
                | 20 | 21 | 22 |
                +----+----+----+
        In principle this implementation may be extended to larger puzzles
        with some (minor) modifications.
*/


#ifndef CONSTANTS_H
#define CONSTANTS_H

#include <vector>
#include <iostream>


//==============================================================================


const   std::string                     INPUT_FILE  = "data.txt";


//==============================================================================


const   unsigned int                    GRID_DIM    = 3;

const   std::vector<std::vector<int>>   GOAL_STATE  = {
                                                        {1, 2, 3},
                                                        {4, 0, 5},
                                                        {6, 7, 8}
                                                      };


//==============================================================================


const   int                             MOVE_LEFT   = -1;
const   int                             MOVE_RIGHT  = 1;
const   int                             MOVE_UP     = -1;
const   int                             MOVE_DOWN   = 1;
const   int                             MOVE_NONE   = 0;
const   unsigned int                    MOVE_COST   = 1;


//==============================================================================


#endif