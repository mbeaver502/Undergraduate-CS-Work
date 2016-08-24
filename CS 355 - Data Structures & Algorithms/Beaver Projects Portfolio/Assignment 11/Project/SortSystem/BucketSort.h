/*
	Names: Jeff Allen and Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 11, Group 3
	Description: TO BE DEFINED
    Due Date: November 20, 2012
*/

#ifndef BUCKETSORT_H
#define BUCKETSORT_H

#include "MiscSorts.h"
#include "LL.h"

//-------------------------------------------------------------------
// BUCKETSORT -- NEEDS TO BE CLEANED UP
//-------------------------------------------------------------------

class BucketSort: public MiscSorts {

	private:
		
		int * arr;
		int size;

		LL<int> * binA;
		int range;

		int numLoops;

	protected:
	
		void init(int * a, int aSize, int aRange);

	public:

		BucketSort();
		BucketSort(int * a, int aSize, int aRange);
		~BucketSort();

		void sort();
		void sort(int * a, int aSize, int aRange);

		void print() const;
		int * getArr() const;

		void reset();

		int getNumLoops() const;

};

#endif