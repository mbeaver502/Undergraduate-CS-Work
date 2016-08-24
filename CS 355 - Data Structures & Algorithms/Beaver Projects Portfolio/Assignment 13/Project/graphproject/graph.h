/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#ifndef GRAPH_H
#define GRAPH_H

#include <queue>
#include <string>
#include <fstream>
#include "node.h"


class Graph {

	private:
			
		vector<Node *> nodeList;
		Node * arr;

		string nodeType;
		string edgeType;
		string weightUnits;

		Node * findNode(int n, string name) const;


	public:
			
		// Constructors and Destructor
		Graph();
		Graph(const Graph & g);
		~Graph();
		Graph & operator =(const Graph & g);


		// Accessors and Mutators
		void readFromFile(string & input);
		void saveToFile(string & output) const;

		void displayGraph() const;
		void displayNodes() const;

		void addNewNode(Node * n);
		Node * findNode(string n) const;
		bool isNodeIsolated(Node * n) const;

		void dijkstraHeap(Node * first, Node * last) const;

		void setNodeType(string type);
		void setEdgeType(string type);
		void setWeightUnits(string units);

		string getNodeType() const;
		string getEdgeType() const;
		string getWeightUnits() const;
		Node* getArr() const;
		const vector<Node *> & getNodeList() const;

		void resetGraph();
		void clearGraph();


};


#endif