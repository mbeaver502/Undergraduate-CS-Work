/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#ifndef NODE_H
#define NODE_H

#include <iostream>
#include <string>
#include <vector>
#include <climits>

using namespace std;


// Forward declaration
class Edge;


//-------------------------------------------------------------
// NODE
//-------------------------------------------------------------

class Node {

	private:

		string name;          // Node name
		vector<Edge> adjList; // Adjacency List
		bool visited;	      // Was Node visited in Dijkstra's Algorithm?
		int totalWeight;
		Node * changer;


	public: 
		
		// Constructors and Destructor
		Node();
		Node(string n);
		Node(const Node & n);
		~Node();
		Node & operator=(const Node & n);

		// Accessors and Mutators
		string getName() const;
		bool hasBeenVisited() const;
		bool hasAdjacentNodes() const;
		int getNumAdjacentNodes() const;
		const vector<Edge> & getAdjList() const;

		void addAdjacentNode(Node * adj, unsigned int edgeWeight);
		void displayAdjList() const;

		void setName(string n);
		void setVisitStatus(bool flag);
		void setTotalWeight(unsigned int w);
		void setChanger(Node * n);
		void setVisited();

		int getTotalWeight() const;
		Node * getChanger() const;
		bool isNodeInAdjList(Node * n) const;
		unsigned int getAdjNodeEdgeWeight(Node * adjNode) const;

};



//-------------------------------------------------------------
// EDGE
//-------------------------------------------------------------

class Edge {

	private:

		Node * srcNode;      // The source Node
		Node * dstNode;      // The adjacent Node
		unsigned int weight; // The weight (cost) of the Edge


	public:

		// Constructors and Destructor
		Edge();
		Edge(Node * first, Node * second, unsigned int w);
		Edge(const Edge & e);
		~Edge();
		Edge & operator=(const Edge & e);

		// Accessors and Mutators
		Node * getSrc() const;
		Node * getDst() const;
		unsigned int getWeight() const;

		void setSrcNode(Node * src, bool deleteNode);
		void setDstNode(Node * dst, bool deleteNode);
		void setWeight(unsigned int w);


};




#endif