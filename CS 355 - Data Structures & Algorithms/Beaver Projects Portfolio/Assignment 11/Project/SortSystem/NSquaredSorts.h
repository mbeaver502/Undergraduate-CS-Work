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

#ifndef NSQUAREDSORTS_H
#define NSQUAREDSORTS_H

#include "AbstractSort.h"

//-------------------------------------------------------------------
// NSQUAREDSORTS
//-------------------------------------------------------------------

class NSquaredSorts: public AbstractSort {

	public:

		virtual int getNumLoops() const = 0;
		virtual int getNumOuterLoops() const {return 0;}
		virtual int getNumInnerLoops() const {return 0;}
		virtual int getNumSwaps() const {return 0;}

};


#endif