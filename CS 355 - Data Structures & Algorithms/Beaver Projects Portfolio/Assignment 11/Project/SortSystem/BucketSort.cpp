/*
	Names: Jeff Allen and Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 11, Group 3
	Description: TO BE DEFINED
    Due Date: November 20, 2012
*/

#include <iostream>
//#include "SortingSystem.h"
#include "BucketSort.h"
#include "LL.h"

using namespace std;


/*
  Name: BucketSort in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Default constructor. Initializes object to valid state.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BucketSort::BucketSort() {

	arr = 0;
	size = 0;

	binA = 0;
	range = 0;

	numLoops = 0;

}


/*
  Name: BucketSort in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Overloaded constructor. Initializes object to valid state.
  Incoming: a is a pointer to an array, aSize is the size of the array, and aRange is the range of values
  Outgoing: a is a pointer to an array
  Return: N/A
*/
BucketSort::BucketSort(int * a, int aSize, int aRange) {

	init(a, aSize, aRange);

}


/*
  Name: ~BucketSort in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Destructor. Returns memory and resets data members.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BucketSort::~BucketSort() {

	delete [] arr;
	size = 0;

	delete [] binA;
	range = 0;

	numLoops = 0;

}


/*
  Name: init in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Initializes data members to valid state.
  Incoming: a is a pointer to an array, aSize is the size of the array, and aRange is the range of the data
  Outgoing: a is a pointer to an array
  Return: N/A (void)
*/
void BucketSort::init(int * a, int aSize, int aRange) {

	size = aSize;
	arr = new int [size];

	// Copy array values into data member array
	for (int i = 0; i < aSize; i++)
		arr[i] = a[i];

	range = aRange + 1;
	binA = new LL<int> [range];

	numLoops = 0;

}


/*
  Name: sort in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Performs a bucket sort on the arr data member.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BucketSort::sort() {

	int pos;

	for (int i = 0; i < size; i++) {

		pos = arr[i];
		binA[pos].insert(arr[i]);

		numLoops++;

	}

	int j = 0;
	int k = 0;
	int * temp;

	for (int i = 0; i < range; i++) {

		temp = binA[i].getData();
		k = binA[i].getSize();

		for (int h = 0; h < k; h++)
			arr[j] = temp[h];

		j++;

	}

}



/*
  Name: sort in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Performs a bucket sort on the data
  Incoming: a is a pointer to an array, aSize is the size of the array, and aRange is the range of the data
  Outgoing: a is a pointer to an array
  Return: N/A (void)
*/
void BucketSort::sort(int * a, int aSize, int aRange) {

	// Initialize data members
	init(a, aSize, aRange);

	sort();

}




/*
  Name: print in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Prints the sorted array.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BucketSort::print() const {

	for (int i = 0; i < size; i++) 
		cout << arr[i] << "\t";

	cout << endl;

}



/*
  Name: getArr in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Returns a pointer to the data array.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
int * BucketSort::getArr() const {

	int * tmp = new int [size];

	for (int i = 0; i < size; i++)
		tmp[i] = arr[i];

	return tmp;

}


/*
  Name: reset in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Resets the buckets.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BucketSort::reset() {

	delete [] binA;

	binA = new LL<int> [range];

}

/*
  Name: getNumLoops in scope of BucketSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of loops performed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of loops performed while sorting
*/
int BucketSort::getNumLoops() const {

	return numLoops;

}