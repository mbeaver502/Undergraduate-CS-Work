///*
//    Michael Beaver
//    CS 470, Spring 2016
//    Program 2
//    23 March 2016
// 
//    Resources:
//        http://en.cppreference.com/w/cpp/container/priority_queue
//        http://cboard.cprogramming.com/cplusplus-programming/78627-stl-min-priority-queue.html#post556126
//        http://en.cppreference.com/w/cpp/container/unordered_map/unordered_map
//        http://www.cplusplus.com/reference/vector/vector/
//
//    Notes:
//        Be sure to include -std=c++11 in compiler flags.
// 
//    Description:
//        Description goes here.
//    
//        We can treat the puzzle as a table and represent it internally as a 2D vector:
//            +----+----+----+
//            | 00 | 01 | 02 |
//            +----+----+----+        We can use these coordinates to
//            | 10 | 11 | 12 |            calculate the Manhattan Distance.
//            +----+----+----+
//            | 20 | 21 | 22 |
//            +----+----+----+
// 
//*/
//
//#include <iostream>
//#include <fstream>
//#include <vector>
//#include <queue>
//#include <unordered_map>
//#include <stdlib.h>
//
//#include "constants.h"
//#include "structs.h"
//
//using namespace std;
//
////==============================================================================
//
//void    getStart    (vector<vector<int>> & n);
//bool    isGoal      (const vector<vector<int>> & n);
//void    printState  (const vector<vector<int>> & n);
//int     heuristic   (const vector<vector<int>> & n);
//void    findCoord   (const vector<vector<int>> & state,
//                        const int n,
//                            unsigned int & x,
//                                unsigned int & y);
//void    genSuc      (const state_node & cState,
//                        vector<state_node> & successors,
//                            const unordered_map<Key, int, KeyHash, KeyEqual> * expList);
//
////==============================================================================
//
///*
//    Function:
//    Input:
//    Output:
//    Returns:
//*/
//int main() {
//    
//    vector<vector<int>> state_n = {{0, 0 , 0}, {0, 0, 0}, {0, 0, 0}};
//    unordered_map<Key, int, KeyHash, KeyEqual> expList;
//    priority_queue<state_node, vector<state_node>, compare> queue;
//    vector<state_node> successors;
//    Key key;
//    state_node sn;
//    bool foundGoal = false;
//    
//    
//    // Initial setup
//    getStart(state_n);
//    sn.state = state_n;
//    sn.cost = 0;
//    queue.push(sn);
//    cout << "Start:" << endl;
//   
//    
//    // Attempt to find the goal using A*
//    while (!queue.empty() && !foundGoal) {
//        
//        sn = queue.top();
//        queue.pop();
//        
//        if (isGoal(sn.state))
//            foundGoal = true;
//        
//        else {
//            
//            key.state = sn.state;
//            if (expList.find(key) == expList.end()) {
//                
//                expList.emplace(make_pair(key, 0));
//                
//                cout << "Expanding... " << endl;
//                printState(sn.state);
//                genSuc(sn, successors, &expList);
//                
//                for (int i = 0; i < successors.size(); i++)
//                    queue.push(successors[i]);
//                successors.clear();
//
//            }
//            
//        }
//        
//    }
//    
//    if (foundGoal) {
//        
//        cout << "Goal:" << endl;
//        printState(sn.state);
//        
//    }
//    
//    
//    else
//        cout << "Failed to find the goal!" << endl;
//
//    return 0;
//    
//}
//
////==============================================================================
//
///*
//    Function:
//    Input:
//    Output:
//    Returns:
//*/
//void getStart(vector<vector<int>> & n) {
//    
//    ifstream inFile;
//    
//    inFile.open(INPUT_FILE);
//
//    if (inFile.is_open()) {
//        
//        for (unsigned int i = 0; i < GRID_DIM; i++)
//            for (unsigned int j = 0; j < GRID_DIM; j++)
//                inFile >> n[i][j];
//
//    }
//    
//    inFile.close();
//    
//}
//
////==============================================================================
//
///*
//    Function:
//    Input:
//    Output:
//    Returns:
//*/
//bool isGoal(const vector<vector<int>> & n) {
//    
//    int count = 0;
//    
//    for (unsigned int i = 0; i < GRID_DIM; i++) {
//        
//        for (unsigned int j = 0; j < GRID_DIM; j++)
//            if (n[i][j] == GOAL_STATE[i][j])
//                count++;
//        
//    }
//            
//    return (count == (GRID_DIM * GRID_DIM));
//    
//}
//
////==============================================================================
//
///*
//    Function:
//    Input:
//    Output:
//    Returns:
//*/
//void printState(const vector<vector<int>> & n) {
//    
//    for (unsigned int i = 0; i < GRID_DIM; i++) {
//        
//        cout << "+---+---+---+" << endl;
//        cout << "|";
//        
//        for (unsigned int j = 0; j < GRID_DIM; j++) {
//            
//            cout << " ";
//            
//            if (n[i][j] != 0)
//                cout << n[i][j];
//            else
//                cout << " ";
//            
//            cout << " " << "|";
//            
//        }
//        
//        cout << endl;
//        
//    }
//    
//    cout << "+---+---+---+" << endl << endl;
//    
//}
//
////==============================================================================
//
///*
//    Function:
//    Input:
//    Output:
//    Returns:
//*/
//int heuristic(const vector<vector<int>> & n) {
//    
//    int h = 0;
//    unsigned int x, y;
//    
//    for (unsigned int i = 0; i < GRID_DIM; i++) {
//        
//        for (unsigned int j = 0; j < GRID_DIM; j++) {
//            
//            // I call this the "Janky Manhattan Distance"
//            if (n[i][j] != GOAL_STATE[i][j] && n[i][j] != 0) {
//                
//                findCoord(GOAL_STATE, n[i][j], x, y);
//                h += abs(int(i - x)) + abs(int(j - y));
//                
//            }
//    
//        }
//    }
//    
//    return h;
//    
//}
//
////==============================================================================
//
///*
// Function:
// Input:
// Output:
// Returns:
// */
//void findCoord(const vector<vector<int>> & state, const int n,
//               unsigned int & x, unsigned int & y) {
//    
//    for (unsigned int i = 0; i < GRID_DIM; i++) {
//        
//        for (unsigned int j = 0; j < GRID_DIM; j++) {
//            
//            if (state[i][j] == n) {
//                
//                x = i;
//                y = j;
//                
//                return;
//                
//            }
//        }
//    }
//    
//}
//
////==============================================================================
//
///*
//    Function:
//    Input:
//    Output:
//    Returns:
//*/
//void genSuc(const state_node & cState, vector<state_node> & successors,
//            const unordered_map<Key, int, KeyHash, KeyEqual> * expList) {
//
//    vector<vector<int>> nState;
//    unsigned int blankX, blankY;
//    unsigned int tempX, tempY, tempN;
//    state_node newNode;
//    Key key;
//    
//    blankX = blankY = tempX = tempY = tempN = 0;
//    findCoord(cState.state, 0, blankX, blankY);
//
//    // Left top corner (x = 0, y = 0)
//    if (blankX == 0 && blankY == 0) {
//        
//        // Move blank down one slot
//        nState = cState.state;
//        tempX = blankX + 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        
//        // Move blank right one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY + 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
//
//    // Middle top center (x = 0, y = 1)
//    if (blankX == 0 && blankY == 1) {
//        
//        // Move blank left one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY - 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        
//        
//        // Move blank right one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY + 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        
//        // Move blank down one slot
//        nState = cState.state;
//        tempX = blankX + 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
//
//    // Right top corner (x = 0, y = 2 [i.e., GRID_DIM - 1])
//    if (blankX == 0 && blankY == 2) {
//        
//        // Move blank left one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY - 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        
//        // Move blank down one slot
//        nState = cState.state;
//        tempX = blankX + 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
//
//    // Right middle center (x = 1, y = 2)
//    if (blankX == 1 && blankY == 2) {
//        
//        // Move blank left one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY - 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        
//        // Move blank up one slot
//        nState = cState.state;
//        tempX = blankX - 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        
//        // Move blank down one slot
//        nState = cState.state;
//        tempX = blankX + 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
//
//    // Right bottom corner (x = 2 [i.e., GRID_DIM - 1], y = 2)
//    if (blankX == 2 && blankY == 2) {
//        
//        // Move blank up one slot
//        nState = cState.state;
//        tempX = blankX - 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank left one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY - 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//    }
//
//    // Middle bottom center (x = 2, y = 1)
//    if (blankX == 2 && blankY == 1) {
//        
//        // Move blank up one slot
//        nState = cState.state;
//        tempX = blankX - 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank left one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY - 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank right one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY + 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
//
//    // Left bottom corner (x = 2, y = 0)
//    if (blankX == 2 && blankY == 0) {
//        
//        // Move blank up one slot
//        nState = cState.state;
//        tempX = blankX - 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank right one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY + 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
// 
//    // Left middle center (x = 1, y = 0)
//    if (blankX == 1 && blankY == 0) {
//        
//        // Move blank up one slot
//        nState = cState.state;
//        tempX = blankX - 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank right one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY + 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank down one slot
//        nState = cState.state;
//        tempX = blankX + 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//    }
//
//    // Interior (x = 1, y = 1)
//    if (blankX == 1 && blankY == 1) {
//        
//        // Move blank left one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY - 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank up one slot
//        nState = cState.state;
//        tempX = blankX - 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank right one slot
//        nState = cState.state;
//        tempX = blankX + 0;
//        tempY = blankY + 1;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//        // Move blank down one slot
//        nState = cState.state;
//        tempX = blankX + 1;
//        tempY = blankY + 0;
//        tempN = nState[tempX][tempY];
//        nState[tempX][tempY] = 0;
//        nState[blankX][blankY] = tempN;
//        
//        // If not in the expanded list, add the candidate node to the list of successors
//        key.state = nState;
//        if (expList->find(key) == expList->end()) {
//            
//            newNode.state = nState;
//            newNode.cost = cState.cost + 1 + heuristic(nState);
//            successors.push_back(newNode);
//            
//        }
//        
//    }
//
//}
//
//void moveBlank(vector<vector<int>> & nState, const int blankX,
//               const int blankY, const int deltaX, const int deltaY) {
//    
//    unsigned int tempX, tempY, tempN;
//    
//    tempX = blankX + deltaX;
//    tempY = blankY + deltaY;
//    tempN = nState[tempX][tempY];
//    nState[tempX][tempY] = 0;
//    nState[blankX][blankY] = tempN;
//    
//}
