/*
	Names: Drew Aaron, Michael Beaver, Andrew Hamilton, and Elizabeth McClellan
	CS355 Fall 2012
	Assignment: Assignment 6, Homework 4 -- Group 2
	Description: This program implements and tests an integer-based Binary Search Tree.  The BST
				 has the ability to insert values, remove values, copy and assign itself to other
				 BSTs, print itself in three ways (Pre, Post, and In order), and manipulate the
				 position of the cursor.
    Due Date: October 9, 2012
*/


#include <iostream>
using namespace std;


class BNode {

	private:
	
		// Data members
		int data;
		BNode * left;
		BNode * right;


		// Constructors
		BNode();
		BNode(int e);
		BNode(int e, BNode * l, BNode * r);	
	

		friend class BST;


	public:

		// Accessor
		int GetData()const;


};


class BST {

	private:
	
		// Data members
		BNode * root;
		BNode * cursor;


		// Helper routines
		void PrintInHelp(ostream & os, BNode * n) const;
		void PrintPreHelp(ostream & os, BNode * n) const;
		void PrintPostHelp(ostream & os, BNode * n) const; 

		void GoToNextHelp(int e, BNode * n, bool & flag);
		void GoToPrevHelp(int e, BNode * n, bool & flag);

		void ClearHelp(BNode * n);
		void CopyHelp(BNode * src, BNode *& ths, BNode * srcCursor);

		BNode * findParent(int e) const;


	public:

		// Constructors and Destructor
		BST();					// Default constructor
		BST(const BST & b);		// Copy constructor
		~BST();					// Destructor


		// Operators
		BST & operator =(const BST & b);


		// Accessors and Mutators
		bool Insert(int e);		// Cursor at insertion point
		bool Remove(int e);		// Cursor at parent of removed item or at root

		void PrintPre(ostream & os) const;
		void PrintPost(ostream & os) const;
		void PrintIn(ostream & os) const;

		BNode * Find(int e);	// Cursor at located node or rightmost node	
		BNode * AtCursor() const;

		void GoToBeginning();
		void GoToEnd();
		void GoToNext();
		void GoToPrev();
		void GoToRoot();

		void ClearList();
		void CopyList(const BST & b);	// To be called by copy constructor and assignment
		bool Empty() const;

		BNode * findMin() const;
		BNode * findMax() const;


};