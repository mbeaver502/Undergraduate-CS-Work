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

#ifndef RADIXSORT_H
#define RADIXSORT_H

#include "LinearSorts.h"


//-------------------------------------------------------------------
// RADIXSORT
//-------------------------------------------------------------------

class RadixSort: public LinearSorts {

	private:

		int * arr;
		int size;

		int numOuterLoops;
		int numInnerLoops;
		int numSwaps;

	protected:

		void init(int * a, int aSize);

	public:

		RadixSort();
		RadixSort(int * a, int aSize);
		~RadixSort();

		void sort();
		void sort(int * a, int aSize);

		void print() const;

		int getNumLoops() const;
		int getNumInnerLoops() const;
		int getNumOuterLoops() const;
		int getNumSwaps() const;

		int * getArr() const;

};


#endif