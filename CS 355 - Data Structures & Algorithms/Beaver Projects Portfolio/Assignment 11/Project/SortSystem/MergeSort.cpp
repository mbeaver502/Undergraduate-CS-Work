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

#include <iostream>
#include "MergeSort.h"

using namespace std; 

/*
  Name: MergeSort in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Initializes merge sort's array
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
MergeSort::MergeSort() {

	MergeSort::CLASS_NAME = "MergeSort";

	arr = 0;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;
	numMerges = 0;
	numDivisions = 0;
	numRecursions = 0;

}


/*
  Name: MergeSort in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Initializes an array of size 0.
  Incoming: array to be copied, and size of that array.
  Outgoing: a is a pointer to an array
  Return: N/A
*/
MergeSort::MergeSort(int * a, int arrSize) {

	MergeSort::CLASS_NAME = "MergeSort";

	init(a, arrSize);

}


/*
  Name: ~MergeSort in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: deconstructor that deletes merge sort's array
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
MergeSort::~MergeSort() {

	delete [] arr;
	size = 0;

	numOuterLoops = 0;
	numInnerLoops = 0;
	numMerges = 0;

}


/*
  Name: init in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Initializes merge sort's array
  Incoming: an array to be copied, size of the array.
  Outgoing: a is a pointer to an array
  Return: N/A
*/
void MergeSort::init(int * a, int arrSize) {

	size = arrSize;
	arr = new int [size];

	// Copy array values into data member array
	for (int i = 0; i < arrSize; i++)
		arr[i] = a[i];

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;
	numMerges = 0;
	numDivisions = 0;
	numRecursions = 0;

}


/*
  Name: merge in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Merges two lists together
  Incoming: array to be copied, a temporary array to be merge,
			left of the list, middle of the list, right of the list
  Outgoing: a and temp are pointers to arrays
  Return: N/A
*/
void MergeSort::merge(int * a, int * temp, int left, int middle, int right) {

	int endLeft;
	int numElems;
	int tempPos;

	int M = middle;
	int L = left;
	int R = right;

	endLeft = M - 1;
	tempPos = L;
	numElems = (R - L) + 1;

	// Loop through until the end of either list is reached
	while ((L <= endLeft) && (M <= R)) {

		// Merge the Left value
		if (a[L] <= a[M]) {

			temp[tempPos] = a[L];
			tempPos++;
			L++;

			numSwaps++;
			numMerges++;

		}

		// Merge the Right value
		else if (a[L] > a[M]) {

			temp[tempPos] = a[M];
			tempPos++;
			M++;

			numSwaps++;
			numMerges++;

		}

		numOuterLoops++;

	}

	// Fill in the remaining Left values
	while (L <= endLeft) {

		temp[tempPos] = a[L];
		L++;
		tempPos++;

		numMerges++;
		numOuterLoops;

	}

	// Fill in the remaining Right values
	while (M <= R) {

		temp[tempPos] = a[M];
		M++;
		tempPos++;

		numMerges++;
		numOuterLoops++;

	}

	// Copy data from temp array to actual array
	for (int i = 0; i <= numElems; i++) {

		a[R] = temp[R];
		R--;

	}

}


/*
  Name: sortHelper in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Recursively sorts an array via merging several sub-arrays.
  Incoming: a and temp are pointers to arrays, left is the left index,
			and right is the right index
  Outgoing: a and temp are pointers to arrays
  Return: N/A
*/
void MergeSort::sortHelper(int * a, int * temp, int left, int right) {

	int middle = 0;

	// Base case: "array" of one element
	if (right > left) {

		numRecursions++;

		middle = (right + left) / 2;

		numDivisions++;

		// Recursively divide the array into left and right arrays
		sortHelper(a, temp, left, middle);
		sortHelper(a, temp, (middle + 1), right);

		// Merge and sort values
		merge(a, temp, left, (middle + 1), right);

	}
	
}


/*
  Name: sort in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: sorts Merge sort's array 
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void MergeSort::sort() {

	if (size <= 1)
		return;

	int * temp = new int [size];

	sortHelper(arr, temp, 0, (size - 1));

}


/*
  Name: sort in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Initializes merge sort's array and sorts it by the merge sort method
  Incoming: an array to be copied, and size of that array
  Outgoing: a is a pointer to an array
  Return: N/A
*/
void MergeSort::sort(int * a, int arrSize) {

	init(a, arrSize);

	sort();

}


/*
  Name: getNumLoops in MergeSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of loops executed by Merge Sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the total number of loops
*/
int MergeSort::getNumLoops() const {

	return (numInnerLoops + numOuterLoops);

}


/*
  Name: getNumInnerLoops in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of inner loops executed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of inner loops executed
*/
int MergeSort::getNumInnerLoops() const {

	return numInnerLoops;

}


/*
  Name: getNumOuterLoops in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of outer loops executed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of outer loops executed
*/
int MergeSort::getNumOuterLoops() const {

	return numOuterLoops;

}


/*
  Name: getNumSwaps in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of swaps.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of swaps
*/
int MergeSort::getNumSwaps() const {

	return numSwaps;

}



/*
  Name: getNumMerges in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of merges executed by Merge sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of merges
*/
int MergeSort::getNumMerges() const {

	return numMerges;

}


/*
  Name: getNumDivisions in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of divisions into sub-arrays.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of divisions of sub-arrays
*/
int MergeSort::getNumDivisions() const {

	return numDivisions;

}


/*
  Name: getNumRecursions in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Return the amount of recursions executed by Merge Sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of recursions
*/
int MergeSort::getNumRecursions() const {

	return numRecursions;

}


/*
  Name: print in scope of MergeSort
  Writer(s): Michael Beaver
  Purpose: Prints out the array of merge sort
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void MergeSort::print() const {

	for (int i = 0; i < size; i++)
		cout << arr[i] << "\t";

	cout << endl;

}


/*
  Name: getArr()
  Writer(s): Michael Beaver
  Purpose: Return the array of Merge Sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to a copy of the data member array
*/
int * MergeSort::getArr() const {

	int * tmp = new int [size];

	for (int i = 0; i < size; i++)
		tmp[i] = arr[i];

	return tmp;

}