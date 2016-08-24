/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#ifndef MINBINARYHEAP_H
#define MINBINARYHEAP_H

#include "node.h"


class minBinaryHeap {

	private:
			
		vector<Node *> heap;
		int numItems;

		int getLeftChild(int index) const;
		int getRightChild(int index) const;
		int getParent(int index) const;

		void percolateDown(int index);


	public:

		// Constructors and Destructors
		minBinaryHeap();
		minBinaryHeap(int size);
		minBinaryHeap(const minBinaryHeap & h);
		~minBinaryHeap();
		minBinaryHeap & operator =(const minBinaryHeap & h);

		// Accessors and Mutators
		void insert(Node * iNode);		
		Node * popMin();

		Node * getMin() const;
		const vector<Node *> & getHeap() const;
		int getNumItems() const;
		bool isEmpty() const;

		void print() const;

};


#endif