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
#include <stack>
#include "QuickSort.h"

using namespace std;



/*
  Name: QuickSort in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Initializes quick sort's data members
  Incoming: a is a pointer to an array, arrSize is the size of the array
  Outgoing: a is a pointer to an array
  Return: N/A
*/
void QuickSort::init(int * a, int arrSize) {

	size = arrSize;
	arr = new int [size];

	// Copy array values into data member array
	for (int i = 0; i < arrSize; i++)
		arr[i] = a[i];

	numInnerLoops = 0;
	numOuterLoops = 0;
	numRecursions = 0;
	numDivisions = 0;
	numSwaps = 0;

}


/*
  Name: QuickSort in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Constructs the class and initializes data members
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
QuickSort::QuickSort() {

	QuickSort::CLASS_NAME = "QuickSort";

	arr = 0;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numRecursions = 0;
	numDivisions = 0;
	numSwaps = 0;

}


/*
  Name: QuickSort in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Overloaded constructor initializes data members
  Incoming: a is a pointer to an array, arrSize is the size of the array
  Outgoing: a is a pointer to an array
  Return: N/A
*/
QuickSort::QuickSort(int * a, int arrSize) {

	QuickSort::CLASS_NAME = "QuickSort";

	init(a, arrSize);

}


/*
  Name: ~QuickSort in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Destructor returns memory to the system and reinitializes data members
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
QuickSort::~QuickSort(){

	delete [] arr;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numRecursions = 0;
	numSwaps = 0;

}


/*
  Name: sortHelper in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Non-recursive stack-based implementation of QuickSort; adapted from Varun Jain. 
		   Recursive implementations experienced stack overflows with large data sets.
  Incoming: a is a pointer to an array, left is the left index, right is the right index
  Outgoing: a is a pointer to an array
  Return: N/A
*/
void QuickSort::sortHelper(int * a, int left, int right) {

	stack<int> Stack;
	
	int pivot;
	
	// Prefix "i" == indexOf
	int iPivot = 0;
	int iLeft = iPivot + 1;
	int iRight = size - 1;
	int iLeftSubSet;
	int iRightSubSet;

	// Push onto stack: index of pivot and index of the right
	Stack.push(iPivot);
	Stack.push(iRight);

	// Loop while there is data on the stack
	while (Stack.size() > 0) {

		// Get the index of the Right Sub-Array off the stack
		iRightSubSet = Stack.top();
		Stack.pop();

		// Get the index of the Left Sub-Array off the stack
		iLeftSubSet = Stack.top();
		Stack.pop();

		iLeft = iLeftSubSet + 1;
		iPivot = iLeftSubSet;
		iRight = iRightSubSet;

		numDivisions++;

		pivot = a[iPivot];
	
		if (iLeft > iRight)
			continue;

		// Loop through the Sub-Array
		while (iLeft < iRight) {

			// Move the left location to the right
			while ((iLeft <= iRight) && (a[iLeft] <= pivot)) 
				iLeft++;

			// Move the right location to the left
			while ((iLeft <= iRight) && (a[iRight] >= pivot)) 
				iRight--;

			// Swap values
			if (iRight >= iLeft)
				swap(a, iLeft, iRight);

			numInnerLoops++;

		}

		// If the right index is to the right of the pivot . . .
		if (iPivot <= iRight) {

			// Swap if the pivot value is greater than the right value
			if (a[iPivot] >= a[iRight])
				swap(a, iPivot, iRight);

		}

		// Pushing indices of smaller Sub-Arrays onto stack
		if (iLeftSubSet < iRight) {

			Stack.push(iLeftSubSet);
			Stack.push(iRight - 1);

		}

		// Pushing indices of smaller Sub-Arrays onto stack
		if (iRightSubSet > iRight) {

			Stack.push(iRight + 1);
			Stack.push(iRightSubSet);

		}

		numOuterLoops++;

	}

}


/*
  Name: sort in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Sorts the data member array.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void QuickSort::sort() {

	if (size <= 1)
		return;

	sortHelper(arr, 0, size - 1);

}


/*
  Name: sort in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Sorts an array of a certain size.
  Incoming: a is a pointer to an array, arrSize is the size of the array
  Outgoing: a is a pointer to an array
  Return: N/A
*/
void QuickSort::sort(int * a, int arrSize) {

	init(a, arrSize);
	
	sort();

}


/*
  Name: getNumLoops in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns the total number of loops executed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the total number of loops executed
*/
int QuickSort::getNumLoops() const {

	return (numInnerLoops + numOuterLoops);

}


/*
  Name: getNumInnerLoops in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of inner loops.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of inner loops
*/
int QuickSort::getNumInnerLoops() const {

	return numInnerLoops;

}


/*
  Name: getNumOuterLoops in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of outer loops.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of outer loops
*/
int QuickSort::getNumOuterLoops() const {

	return numOuterLoops;

}


/*
  Name: getNumRecursions in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of recursions executed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of recursions
*/
int QuickSort::getNumRecursions() const {

	return numRecursions;

}


/*
  Name: getNumDivisions in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of divisions of the array.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of divisions of the array
*/
int QuickSort::getNumDivisions() const {

	return numDivisions;

}


/*
  Name: getNumSwaps in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of swaps executed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of swaps
*/
int QuickSort::getNumSwaps() const {

	return numSwaps;

}


/*
  Name: swap in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Swaps two values at given indices.
  Incoming: a is a pointer to an array, val1 and val2 are the indices
  Outgoing: a is a pointer to an array
  Return: N/A
*/
void QuickSort::swap(int * a, int val1, int val2) {

	int temp;
	temp = a[val2];
	a[val2] = a[val1];
	a[val1] = temp;
	numSwaps++;

}


/*
  Name: print in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Prints the data member array.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void QuickSort::print() const {

	for (int i = 0; i < size; i++)
		cout << arr[i] << "\t";

	cout << endl;

}


/*
  Name: getArr in scope of QuickSort
  Writer(s): Michael Beaver
  Purpose: Returns a pointer to a copy of the data member array.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to a copy of the data member array
*/
int * QuickSort::getArr() const {

	int * tmp = new int [size];

	for (int i = 0; i < size; i++)
		tmp[i] = arr[i];

	return tmp;

}