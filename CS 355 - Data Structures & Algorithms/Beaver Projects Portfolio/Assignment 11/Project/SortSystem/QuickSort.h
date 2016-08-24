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

#ifndef QUICKSORT_H
#define QUICKSORT_H

#include "NLogNSorts.h"

//-------------------------------------------------------------------
// QUICKSORT -- STACK-BASED
//-------------------------------------------------------------------

class QuickSort: public NLogNSorts {

private:

	int size;
	int * arr;

protected:

	int numInnerLoops;
	int numOuterLoops;
	int numRecursions;
	int numDivisions;
	int numSwaps;

	void sortHelper(int * a, int left, int right);
	void init(int * a, int arrSize);
	void swap(int * a,int val1, int val2);

public:

	QuickSort();
	QuickSort(int * a, int arrSize);
	~QuickSort();

	void sort();
	void sort(int * a, int arrSize);

	int getNumLoops() const;
	int getNumInnerLoops() const;
	int getNumOuterLoops() const;
	int getNumRecursions() const;
	int getNumDivisions() const;
	int getNumSwaps() const;

	void print() const;

	int * getArr() const;

};


#endif