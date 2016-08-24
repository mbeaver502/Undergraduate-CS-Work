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
#include "RadixSort.h"
#include "LL.h"

using namespace std;


/*
  Name: RadixSort in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Default constructor. Initializes object to valid state.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
RadixSort::RadixSort() {

	RadixSort::CLASS_NAME = "RadixSort";

	arr = 0;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;

}


/*
  Name: RadixSort in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Overloaded. constructor. Initializes object to valid state.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
RadixSort::RadixSort(int * a, int aSize) {

	RadixSort::CLASS_NAME = "RadixSort";

	init(a, aSize);

}


/*
  Name: ~RadixSort in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Destructor. Returns memory and resets data members.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
RadixSort::~RadixSort() {

	delete [] arr;
	size = 0;

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;

}


/*
  Name: init in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Initializes data members.
  Incoming: a is a pointer to an array and aSize is the size of the array
  Outgoing: a is a pointer to an array
  Return: N/A (void)
*/
void RadixSort::init(int * a, int aSize) {

	size = aSize;
	arr = new int [size];

	// Copy array values into data member array
	for (int i = 0; i < aSize; i++)
		arr[i] = a[i];

	numInnerLoops = 0;
	numOuterLoops = 0;
	numSwaps = 0;

}


/*
  Name: sort in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Performs radix sort on arr data member.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void RadixSort::sort() {

	// For getting digits
	int divisor = 1;
	int modulo = 10;
	int pos;

	// For keeping up with how many digits have been processed
	int numDigits = int(floor(log10(double(arr[0])))) + 1;
	int count = 0;

	// Buckets and temporary array
	int * temp = new int [size];
	LL<int> bins[10];

	// Copy original values into temporary array
	for (int i = 0; i < size; i++)
		temp[i] = arr[i];


	// Iterate once for each digit
	while (count < numDigits) {


		// Iterate once for each value in the array
		for (int i = 0; i < size; i++) {

			// Gets digit
			pos = temp[i] % modulo;
			pos /= divisor;
			
			// Sort values into bins
			bins[pos].insert(temp[i]);

			numInnerLoops++;

		}


		int * data;
		int dataSize;
		int j = 0;
		int k = 0;


		// Iterate over each bin and get its data
		for (int i = 0; i < 10; i++) {

			data = bins[i].getData();
			dataSize = bins[i].getSize();

			// Overwrite the temporary array's data with the sorted data
			while (j < dataSize) {

				temp[k] = data[j];

				j++;
				k++;

				numSwaps++;

			}

			j = 0;

			// Reset the bins for the next iteration
			bins[i].clearList();

			numInnerLoops++;

		}

		// For getting next digit
		count++;
		divisor = modulo;
		modulo *= 10;

		numOuterLoops++;

	}

	// Copy the final sorted list back to the original array
	for (int i = 0; i < size; i++)
		arr[i] = temp[i];

	delete [] temp;

	// Runtime = D * N, N = number of sorts / D
	numSwaps /= numDigits;

}


/*
  Name: sort in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Performs radix sort on arr data member.
  Incoming: a is a pointer to an array and aSize is the size of the array
  Outgoing: a is a pointer to an array
  Return: N/A (void)
*/
void RadixSort::sort(int * a, int aSize) {

	// Initialize the data members
	init(a, aSize);

	sort();

}



/*
  Name: print in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Prints the arr data member.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void RadixSort::print() const {

	for (int i = 0; i < size; i++)
		cout << arr[i] << "\t";

	cout << endl;

}



/*
  Name: getNumLoops in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Returns the total number of loops performed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the total number of loops performed
*/
int RadixSort::getNumLoops() const {

	return (numInnerLoops + numOuterLoops);

}


/*
  Name: getNumInnerLoops in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of inner loops performed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of inner loops performed
*/
int RadixSort::getNumInnerLoops() const {

	return numInnerLoops;

}


/*
  Name: getNumOuterLoops in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of outer loops performed.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of outer loops performed
*/
int RadixSort::getNumOuterLoops() const {

	return numOuterLoops;

}
		

/*
  Name: getnumSwaps in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Returns the number of sorts performed in the radix sort.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of sorts performed in the radix sort
*/
int RadixSort::getNumSwaps() const {

	return numSwaps;

}


/*
  Name: getArr in scope of RadixSort
  Writer(s): Michael Beaver
  Purpose: Returns a pointer to the array.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to the array
*/
int * RadixSort::getArr() const {

	int * tmp = new int [size];

	for (int i = 0; i < size; i++)
		tmp[i] = arr[i];

	return tmp;

}