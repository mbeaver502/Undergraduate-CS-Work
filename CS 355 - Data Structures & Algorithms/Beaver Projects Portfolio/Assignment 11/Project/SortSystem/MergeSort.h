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


#ifndef MERGESORT_H
#define MERGESORT_H

#include "NLogNSorts.h"

//-------------------------------------------------------------------
// MERGESORT
//-------------------------------------------------------------------

class MergeSort: public NLogNSorts {

	private:

		int size;
		int * arr;

	protected:

		int numInnerLoops;
		int numOuterLoops;
		int numDivisions;
		int numSwaps;
		int numMerges;
		int numRecursions;

		void merge(int * a, int * temp, int left, int middle, int right);
		void init(int * a, int arrSize);
		void sortHelper(int * a, int * temp, int left, int right);

	public:

		MergeSort();
		MergeSort(int * a, int arrSize);
		~MergeSort();

		void sort();
		void sort(int * a, int arrSize);

		int getNumLoops() const;
		int getNumOuterLoops() const;
		int getNumInnerLoops() const;
		int getNumSwaps() const;
		int getNumMerges() const;
		int getNumDivisions() const;
		int getNumRecursions() const;

		void print() const;
		int * getArr() const;

};



#endif