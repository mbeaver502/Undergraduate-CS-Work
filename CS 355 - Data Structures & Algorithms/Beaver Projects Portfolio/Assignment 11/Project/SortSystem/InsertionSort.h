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

#ifndef INSERTIONSORT_H
#define INSERTIONSORT_H

#include "NSquaredSorts.h"

//-------------------------------------------------------------------
// INSERTIONSORT
//-------------------------------------------------------------------

class InsertionSort: public NSquaredSorts {

	
	private:

		int size;
		int * arr;

	protected:

		int numOuterLoops;
		int numInnerLoops;
		int numSwaps;

		void swap(int val1, int val2);
		void init(int * a, int arrSize);

	public:

		InsertionSort();
		InsertionSort(int * a, int arrSize);
		~InsertionSort();

		void sort();
		void sort(int * a, int arrSize);
		void print() const;

		int getNumLoops() const;
		int getNumOuterLoops() const;
		int getNumInnerLoops() const;

		int getNumSwaps() const;

		int * getArr() const;

};

#endif