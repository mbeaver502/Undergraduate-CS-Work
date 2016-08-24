/*
	Names: Jeffrey Allen and Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 11, Group 3
	Description: This program implements a system design for comparing different sorting algorithms.  The class
				hierarchy allows for "plug-n-play" of sorting algorithm class objects.  The driver program allows
				the user to input data either manually or via file.  The data is sorted and iteration information
				is collected.  The user then has the option to view the iteration information for each algorithm.
    Due Date: November 20, 2012
*/

#ifndef ABSTRACTSORT_H
#define ABSTRACTSORT_H

#include <string>

//-------------------------------------------------------------------
// ABSTRACTSORT
//-------------------------------------------------------------------

class AbstractSort {

	public:

		std::string CLASS_NAME;

		virtual void sort() = 0;
		virtual void sort(int * a, int arrSize) {}
		virtual void swap(int val1, int val2) {}
		virtual int * merge(int * arr1, int * arr2) {return 0;}
		virtual void print() const {}

		virtual int * getArr() const {return 0;}

		virtual int getNumLoops() const {return 0;}
		virtual int getNumInnerLoops() const {return 0;}
		virtual int getNumOuterLoops() const {return 0;}
		virtual int getNumSwaps() const {return 0;}
		virtual int getNumMerges() const {return 0;}
		virtual int getNumRecursions() const {return 0;}
		virtual int getNumDivisions() const {return 0;}

		std::string getSortType() const {return CLASS_NAME;}

};


#endif