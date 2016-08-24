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


#ifndef STRUCTS_H
#define STRUCTS_H

#include <vector>
#include "constants.h"


//==============================================================================


struct Key {
    
    std::vector<std::vector<int>> state;
    
};


//==============================================================================


struct KeyHash {
    
    std::size_t operator()(const Key & k) const {
        
        size_t h = 0;
        
        for (unsigned int i = 0; i < GRID_DIM; i++)
            for (unsigned int j = 0; j < GRID_DIM; j++)
                h += size_t(k.state[i][j] * int(i + j));
        
        return h;
        
    }
    
};


//==============================================================================


struct KeyEqual {
    
    bool operator()(const Key & lhs, const Key & rhs) const {
        
        int count = 0;
        
        for (unsigned int i = 0; i < GRID_DIM; i++)
            for (unsigned int j = 0; j < GRID_DIM; j++)
                if (lhs.state[i][j] == rhs.state[i][j])
                    count++;
        
        return (count == (GRID_DIM * GRID_DIM));
        
    }
    
};


//==============================================================================


struct state_node {
    
    std::vector<std::vector<int>> state;
    int cost;
    
};


//==============================================================================


struct compare {
    
    bool operator()(const state_node & l, const state_node & r) {
        
        return (l.cost > r.cost);
        
    }
    
};


//==============================================================================


#endif 