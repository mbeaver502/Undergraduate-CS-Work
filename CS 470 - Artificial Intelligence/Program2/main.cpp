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


#include <iostream>
#include <fstream>
#include <vector>
#include <queue>
#include <unordered_map>
#include <stdlib.h>

#include "constants.h"
#include "structs.h"

using namespace std;


//==============================================================================


void    initStart       (state_node & n);
bool    isGoal          (const vector<vector<int>> & n);
void    printState      (const vector<vector<int>> & n);
int     heuristic       (const vector<vector<int>> & n);

void    findCoord       (const vector<vector<int>> & state,
                        const int n,
                        unsigned int & x,
                        unsigned int & y);

void    genSuccessors   (const state_node & cState,
                        vector<state_node> * successors,
                        unordered_map<Key, int, KeyHash, KeyEqual> * expList);

void    moveBlank       (vector<vector<int>> & nState,
                        const int blankX,
                        const int blankY,
                        const int deltaX,
                        const int deltaY);

void    addSuccessor    (const int cost,
                         const vector<vector<int>> & nState,
                         vector<state_node> * successors,
                         unordered_map<Key, int, KeyHash, KeyEqual> * expList);


//==============================================================================


/*
    Function: main
    Input: N/A
    Output: Series of expanded nodes in the search path printed to console
    Return: Exit code as integer
    Purpose: This is the main function that implements the A* algorithm.
*/
int main() {
    
    unordered_map<Key, int, KeyHash, KeyEqual> expList;
    priority_queue<state_node, vector<state_node>, compare> queue;
    vector<state_node> successors;
    state_node currentNode;
    bool foundGoal = false;
    Key key;
    
    
    // Initialize start node and priority queue
    initStart(currentNode);
    queue.push(currentNode);
    cout << "Start:" << endl;
   
    
    // Attempt to find the goal using A* search
    while (!queue.empty() && !foundGoal) {
        
        currentNode = queue.top();
        queue.pop();
        
        if (isGoal(currentNode.state))
            foundGoal = true;
        
        else {
            
            // Expand the current state and generate successors
            key.state = currentNode.state;
            if (expList.find(key) == expList.end()) {
                
                cout << "Expanding... " << endl;
                printState(currentNode.state);
                expList.emplace(make_pair(key, NULL));

                genSuccessors(currentNode, &successors, &expList);
                for (int i = 0; i < successors.size(); i++)
                    queue.push(successors[i]);
                successors.clear();

            }
            
        }
        
    }
    
    
    if (foundGoal) {
        
        cout << "Goal:" << endl;
        printState(currentNode.state);
        
    }
    
    else
        cout << "If I had hands, I would throw them up in defeat." << endl;

    
    return 0;
    
}


//==============================================================================


/*
    Function: initStart
    Input: n is a state_node struct object
    Output: n is modified by reference
    Return: N/A (void)
    Purpose: Initialize the start node to default values, and then attempt to
        read in the user-defined start state.
*/
void initStart(state_node & n) {
 
    ifstream inFile;
    
    n.state = {{0, 0, 0}, {0, 0, 0}, {0, 0, 0}};
    n.cost = 0;
    
    
    inFile.open(INPUT_FILE);
    if (inFile.is_open()) {
        
        for (unsigned int i = 0; i < GRID_DIM; i++)
            for (unsigned int j = 0; j < GRID_DIM; j++)
                inFile >> n.state[i][j];
        
    }
    inFile.close();
    
}


//==============================================================================


/*
    Function: isGoal
    Input: n is the 2D vector representing the current state node in the search
    Output: N/A
    Return: true if goal, false otherwise
    Purpose: This is the goal test that compares any given 2D vector against the
        predefined GOAL_STATE.
*/
bool isGoal(const vector<vector<int>> & n) {
    
    int count = 0;
    
    for (unsigned int i = 0; i < GRID_DIM; i++)
        for (unsigned int j = 0; j < GRID_DIM; j++)
            if (n[i][j] == GOAL_STATE[i][j])
                count++;

    return (count == (GRID_DIM * GRID_DIM));
    
}


//==============================================================================


/*
    Function: printState
    Input: n is any given 2D vector representing a state node in the search
    Output: Tabular visual representation of the state node printed to console
    Return: N/A (void)
    Purpose: This function prints the tabular visual representation of the given
        state node (i.e., 2D vector n).
*/
void printState(const vector<vector<int>> & n) {
    
    for (unsigned int i = 0; i < GRID_DIM; i++) {
        
        cout << "+---+---+---+" << endl;
        cout << "|";
        
        for (unsigned int j = 0; j < GRID_DIM; j++) {
            
            cout << " ";
            
            if (n[i][j] != 0)
                cout << n[i][j];
            else
                cout << " ";
            
            cout << " " << "|";
            
        }
        
        cout << endl;
        
    }
    
    cout << "+---+---+---+" << endl << endl;
    
}


//==============================================================================


/*
    Function: heuristic
    Input: n is any given 2D vector representing a state node in the search
    Output: N/A
    Return: The Manhattan Distance for the given state node
    Purpose: This function will calculate the heuristic value for the given
        state node. This implementation is perhaps best described as the 
        "Janky Manhattan Distance" approach.
*/
int heuristic(const vector<vector<int>> & n) {
    
    int h = 0;
    unsigned int x, y;
    
    
    // Accumulate Manhattan Distances for each tile in the puzzle
    for (unsigned int i = 0; i < GRID_DIM; i++) {
        
        for (unsigned int j = 0; j < GRID_DIM; j++) {
            
            if (n[i][j] != GOAL_STATE[i][j] && n[i][j] != 0) {
                
                findCoord(GOAL_STATE, n[i][j], x, y);
                h += abs(int(i - x)) + abs(int(j - y));
                
            }
    
        }
    }
    
    return h;
    
}


//==============================================================================


/*
    Function: findCoord
    Input: state is the 2D vector representing a state node in the search; n is
        the integer value to be found; x and y are the (x,y) coordinate pair in
        the table (puzzle board, see above)
    Output: The values in x and y are updated to hold the coordinate pair (x,y)
        that contains the requested integer (n)
    Return: N/A (void)
    Purpose: This function will attempt to locate an integer n within a given
        2D vector state. The location, when found, is "returned" by updating
        the reference parameters x and y.
*/
void findCoord(const vector<vector<int>> & state, const int n,
               unsigned int & x, unsigned int & y) {
    
    for (unsigned int i = 0; i < GRID_DIM; i++) {
        
        for (unsigned int j = 0; j < GRID_DIM; j++) {
            
            if (state[i][j] == n) {
                
                x = i;
                y = j;
                
                return;
                
            }
            
        }
        
    }
    
}


//==============================================================================


/*
    Function: genSuccessors
    Input: cState is the current state_node within the search; successors is a
        pointer to a vector of state_node, which will contain the list of 
        successors; expList is a pointer to the expanded list
    Output: successors is updated to contain a list of successor states
    Return: N/A (void)
    Purpose: This function generates the successor states of a given state node
        within the search (cState). The function operates by considering the
        location of the blank tile (0). This function probably could be condensed
        by using some clever logic. I prefer this version, though, as its logic
        is intuitive, easy to follow, and fairly simple to extend.
*/
void genSuccessors(const state_node & cState, vector<state_node> * successors,
                   unordered_map<Key, int, KeyHash, KeyEqual> * expList) {

    vector<vector<int>> nState;
    unsigned int blankX, blankY;
    unsigned int tempX, tempY, tempN;
    state_node newNode;
    Key key;
    
    
    blankX = blankY = tempX = tempY = tempN = 0;
    findCoord(cState.state, 0, blankX, blankY);

    
    // Left top corner (x = 0, y = 0)
    if (blankX == 0 && blankY == 0) {
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_DOWN, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_RIGHT);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Middle top center (x = 0, y = 1)
    if (blankX == 0 && blankY == 1) {
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_LEFT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_RIGHT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_DOWN, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Right top corner (x = 0, y = 2 [i.e., GRID_DIM - 1])
    if (blankX == 0 && blankY == GRID_DIM - 1) {
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_LEFT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_DOWN, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Right middle center (x = 1, y = 2)
    if (blankX == 1 && blankY == GRID_DIM - 1) {
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_LEFT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_UP, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_DOWN, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Right bottom corner (x = 2 [i.e., GRID_DIM - 1], y = 2)
    if (blankX == GRID_DIM - 1 && blankY == GRID_DIM - 1) {
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_UP, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_LEFT);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Middle bottom center (x = 2, y = 1)
    if (blankX == GRID_DIM - 1 && blankY == 1) {

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_UP, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_LEFT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_RIGHT);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Left bottom corner (x = 2, y = 0)
    if (blankX == GRID_DIM - 1 && blankY == 0) {
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_UP, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_RIGHT);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }
 
    
    // Left middle center (x = 1, y = 0)
    if (blankX == 1 && blankY == 0) {

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_UP, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_RIGHT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_DOWN, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

    
    // Interior (x = 1, y = 1)
    if (blankX == 1 && blankY == 1) {

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_LEFT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_UP, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_NONE, MOVE_RIGHT);
        addSuccessor(cState.cost, nState, successors, expList);

        nState = cState.state;
        moveBlank(nState, blankX, blankY, MOVE_DOWN, MOVE_NONE);
        addSuccessor(cState.cost, nState, successors, expList);
        
    }

}


//==============================================================================


/*
    Function: moveBlank
    Input: nState is a reference to a 2D vector; blankX and blankY are the
        coordinates of the blank within nState; deltaX and deltaY are the
        differentials by which to move the blank within nState
    Output: nState is updated with the blank moved
    Return: N/A (void)
    Purpose: This function will "move" the blank tile (0) within the given state.
        Modify the delta constants (MOVE_UP, MOVE_DOWN, MOVE_LEFT, and
        MOVE_RIGHT) if you want the blank to move different amounts. Since the 
        blank can only move up, down, left, and right, we could take in
        one delta constant (MOVE_UP, MOVE_DOWN, MOVE_LEFT, or MOVE_RIGHT) and
        operate based on that one argument. The version here is more flexible, I
        believe, since it allows the user to specify movement in any direction.
        Extensive modifications or moves may affect MOVE_COST, however (see below).
*/
void moveBlank(vector<vector<int>> & nState, const int blankX,
               const int blankY, const int deltaX, const int deltaY) {
    
    int tempX, tempY, tempN;
    
    tempX = blankX + deltaX;
    tempY = blankY + deltaY;
    tempN = nState[tempX][tempY];
    nState[tempX][tempY] = 0;
    nState[blankX][blankY] = tempN;
    
}


//==============================================================================


/*
    Function: addSuccessor
    Input: cost is the cost of the current state; nState is a 2D vector representing
        the next state in the search (i.e., successor); successors is a pointer to
        a vector of state_node containing a list of successor nodes; expList is a
        pointer to the expaned list
    Output: The vector successors is updated to contain valid candidate successor states
    Return: N/A (void)
    Purpose: This function determines whether a state is in the expanded list. If
        the state is a valid candidate (i.e., not in the expanded list), then it
        is added to the list of successors. Update the constant MOVE_COST if you
        want the state transitions to have a different (constant) cost. 
        The default is MOVE_COST = 1. In hindsight, a good portion of this could
        be achieved in the main() function, but this also works.
*/
void addSuccessor(const int cost, const vector<vector<int>> & nState,
                  vector<state_node> * successors,
                  unordered_map<Key, int, KeyHash, KeyEqual> * expList) {
    
    state_node newNode;
    Key key;
    
    key.state = nState;
    if (expList->find(key) == expList->end()) {
        
        newNode.state = nState;
        newNode.cost = cost + heuristic(nState) + MOVE_COST;
        successors->push_back(newNode);
        
    }
    
}

