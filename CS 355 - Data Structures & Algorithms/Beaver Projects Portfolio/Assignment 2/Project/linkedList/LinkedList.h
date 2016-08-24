/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 2
	Description: This program demonstrates possible usage of a programmer-defined singly LinkedList class.  
				 The LinkedList class has the ability to insert a Node at the head, insert a Node at the end,
				 remove the beginning Node, remove the last Node, remove all Nodes, and print itself.  A 
				 series of 17 cases of varying complexity test the accuracy of the LinkedList's implementation.
				 The LinkedList is cleared before executing each user-selected test case.
    Due Date: August 30, 2012
*/



#ifndef LINKEDLIST_H
#define LINKEDLIST_H

#include <iostream>



//----------------------------------------------------------------------------------------
// NODE
//----------------------------------------------------------------------------------------

class Node {

	private:
	
		int data;    // Integer data value
		Node *next;  // Pointer to the next Node in the list -- either an address or NULL (empty list)


	public:

		// Constructors
		Node();					//    Default constructor -- 0 for data, NULL next pointer
		Node(int e);			// Overloaded constructor -- data only, NULL next pointer
		Node(int e, Node *n);	// Overloaded constructor -- data and next pointer (*n)


		// Destructor
		~Node();


	friend class LinkedList;


};



//----------------------------------------------------------------------------------------
// LINKEDLIST
//----------------------------------------------------------------------------------------

class LinkedList {

	private:

		Node *head;  // Beginning of the list
		int size;	 // The number of Nodes in the list


	public:

		// Constructor
		LinkedList();   // Default -- empty list


		// Destructor
		~LinkedList(); 


		// Member methods
		bool insertFront(int e);
		bool insertBack(int e);
		
		bool removeFront();
		bool removeBack();

		int getSize() const;
		void clearList();

		void print(std::ostream *os) const;


		// Friend functions
		friend std::ostream& operator <<(std::ostream& os, const LinkedList& list);		


};




#endif