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
#include "BubbleSort.h"

using namespace std;


/*
  Name: BubbleSort in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: Constructor for Bubble Sort 
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BubbleSort::BubbleSort() {

	BubbleSort::CLASS_NAME = "BubbleSort";

	arr = 0;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;

}


/*
  Name: BubbleSort in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: Initializes the array of Bubble sort's data members
  Incoming: int to be copied to arr of bubblesort, size of array
  Outgoing: N/A
  Return: N/A
*/
BubbleSort::BubbleSort(int * a, int arrSize) {

	BubbleSort::CLASS_NAME = "BubbleSort";

	init(a, arrSize);

}


/*
  Name: ~BubbleSort in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: Deconstructs the array of bubble sort
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BubbleSort::~BubbleSort() {

	delete [] arr;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;

}


/*
  Name: init in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: initializes the arr in Bubble sort
  Incoming: an array to be initialized, array size
  Outgoing: a is a pointer to the array
  Return: N/A
*/
void BubbleSort::init(int * a, int arrSize) {

	size = arrSize;
	arr = new int [size];

	// Copy array values into data member array
	for (int i = 0; i < arrSize; i++)
		arr[i] = a[i];

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;

}


/*
  Name: sort in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: sorts the array using the bubble sort method
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void BubbleSort::sort() {

	bool flag = true;
	int i = size - 1;
	int count = 0;

	// Loop while the array is unsorted
	while (flag) {

		flag = false;

		// Loop through the array elements
		for (i; i >= count; i--) {

			// Pairwise comparison, swap if necessary
			if (arr[i] < arr[i - 1]) {

				swap(i, i-1);
				numSwaps++;

				flag = true;

			}

			numInnerLoops++;

		}

		i = size - 1;
		count++;
		numOuterLoops++;

	}

}

/*
  Name: sort in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: sorts the array passed in
  Incoming: a is a pointer to the array, arrSize is the size of the array
  Outgoing: a is a pointer to the array
  Return: N/A
*/
void BubbleSort::sort(int * a, int arrSize) {

	init(a, arrSize);

	sort();

}


/*
  Name: swap in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: swaps positions of two values in the array
  Incoming: val1 and val2 are the indices of the two values
  Outgoing: N/A
  Return: N/A
*/
void BubbleSort::swap(int val1, int val2) {

	int temp;
	temp = arr[val2];
	arr[val2] = arr[val1];
	arr[val1] = temp;

}


/*
  Name: print in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: print the array of Bubble sort
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
void BubbleSort::print() const {

	for (int i = 0; i < size; i++)
		cout << arr[i] << "\t";

	cout << endl;

}



/*
  Name: getNumLoops in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: return the number of total loops executed by Bubble sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of total loops executed
*/
int BubbleSort::getNumLoops() const {

	return (numInnerLoops + numOuterLoops);

}


/*
  Name: getNumOuterLoops in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: return the number of outer loops executed by Bubble Sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of outer loops
*/
int BubbleSort::getNumOuterLoops() const {

	return numOuterLoops;

}


/*
  Name: GetNumInnerLoops in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of inner loops executed
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of inner loops executed
*/
int BubbleSort::getNumInnerLoops() const {

	return numInnerLoops;

}


/*
  Name: getNumSwaps in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: return the number of swaps executed by Bubble sort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of swaps 
*/
int BubbleSort::getNumSwaps() const {

	return numSwaps;

}


/*
  Name: getArr in scope of BubbleSort
  Writer(s): Michael Beaver
  Purpose: return array of BubbleSort
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to a copy of the sorted array
*/
int * BubbleSort::getArr() const {

	int * tmp = new int [size];

	for (int i = 0; i < size; i++)
		tmp[i] = arr[i];

	return tmp;

}