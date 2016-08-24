/*
	Names: Drew Aaron, Michael Beaver, and Andrew Hamilton
	CS355 Fall 2012
	Assignment: Assignment 13, Group 4
	Description: This program allows a user to create Graph objects, which are composed of Node and Edge
			objects.  The user may manipulate the Graph in various ways, such as adding Nodes.  This program
			features file I/O that allows the user to load data from a formatted file and save a formatted file.
    Due Date: December 4, 2012
*/


#include <vector>
#include "minBinaryHeap.h"

/*
  Name: minBinaryHeap in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Default constructor. Creates a minBinaryHeap with a valid state.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
minBinaryHeap::minBinaryHeap() {

	numItems = 0;
	heap.resize(1);

	heap[0] = 0;

}


/*
  Name: minBinaryHeap in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Overloaded constructor. Creates a minBinaryHeap with an assigned size.
  Incoming: size is the specified size of the heap.
  Outgoing: N/A
  Return: N/A
*/
minBinaryHeap::minBinaryHeap(int size) {

	numItems = 0;
	heap.resize(size);

	// Initialize to NULL -- could be done with heap.resize() method
	for (int i = 0; i < size; i++) 
		heap[i] = 0;

}


/*
  Name: minBinaryHeap in scope of minBinaryHeap
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Copy constructor. Creates a minBinaryHeap with a valid state from another minBinaryHeap.
  Incoming: h is the address of the specified minBinaryHeap to copy from.
  Outgoing: N/A
  Return: N/A
*/
minBinaryHeap::minBinaryHeap(const minBinaryHeap & h) {

	numItems = h.getNumItems();

	// Copy heap values
	heap.clear();
	heap.resize(h.getHeap().size());
	for (unsigned int i = 0; i < h.getHeap().size(); i++)
		heap[i] = h.getHeap()[i];

}


/*
  Name: ~minBinaryHeap in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Detructor. Returns memory and cleans up any data members.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
minBinaryHeap::~minBinaryHeap() {
	
	// Set back to Null
	// vector.clear() calls object destructors, which we do NOT want to do
	for (int i = 0; i < numItems; i++)
		heap[i] = 0;

	numItems = 0;

}


/*
  Name: Overloaded assignment operator in scope of Node
  Writer(s): Drew Aaron and Michael Beaver
  Purpose: Allows the user to set a valid minBinaryHeap object and all of its properties 
		equal to another minBinaryHeap object and all of its properties.
  Incoming: h is the source minBinaryHeap
  Outgoing: N/A
  Return: Returns this minBinaryHeap with its new properties.
*/
minBinaryHeap & minBinaryHeap::operator =(const minBinaryHeap & h) {

	// Self-assignment
	if(this == &h)
		return *this;
	
	numItems = h.getNumItems();

	// Copy heap
	heap.clear();
	heap.resize(h.getHeap().size());
	for (unsigned int i = 0; i < h.getHeap().size(); i++)
		heap[i] = h.getHeap()[i];

	return *this;

}


/*
  Name: getLeftChild in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the left child of a specified heap index.
  Incoming: index is the specified index of the slot of the heap in which to find its left child
  Outgoing: N/A
  Return: Returns the left child of a specified heap index.
*/
int minBinaryHeap::getLeftChild(int index) const {

	// Formula for getting the left child of a root
	int temp = (2 * index + 1);

	return temp;

}


/*
  Name: getRightChild in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the right child of a specified heap index.
  Incoming: index is the specified index of the slot of the heap in which to find its right child
  Outgoing: N/A
  Return: Returns the right child of a specified heap index.
*/
int minBinaryHeap::getRightChild(int index) const {

	// Formula for getting the right child of a root
	int temp = (2 * index + 2);

	return temp;

}


/*
  Name: getLeftChild in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the parent of a specified heap index.
  Incoming: index is the specified index of the slot of the heap in which to find its parent
  Outgoing: N/A
  Return: Returns the parent of a specified heap index.
*/
int minBinaryHeap::getParent(int index) const {

	// Special case: Root's parent is itself
	if (index == 0)
		return 0;

	// Formula for getting the parent of a child
	int temp = (index - 1);
	temp /= 2;

	return temp;

}


/*
  Name: getMin in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the minimum value in the heap.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the minimum value in the heap.
*/
Node * minBinaryHeap::getMin() const {

	// Return the top root, if the heap is not empty
	if (!isEmpty())
		return heap[0];

	// Return NULL by default
	return 0;

}


/*
  Name: isEmpty in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Checks to see if the heap is empty.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the heap is empty, false if it is not.
*/
bool minBinaryHeap::isEmpty() const {

	// No items in the heap
	if (numItems == 0)
		return true;

	return false;

}


/*
  Name: insert in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Inserts a new Node into the heap, keeping binary heap order.
  Incoming: iNode is the node to insert into the heap.
  Outgoing: heap is the name of the heap to resize and change from inserting the node.
  Return: N/A
*/
void minBinaryHeap::insert(Node * iNode) {

	// If the heap is empty, then the Node is the top root
	if (isEmpty()) {

		heap.resize(1);
		heap[0] = iNode;
		numItems = 1;

		return;

	}

	// Grow the heap if the last element is not NULL
	if (heap[heap.size() - 1] != 0)
		heap.resize(heap.size() + 1, 0);

	// Index of last spot
	int i = heap.size() - 1;
	numItems = i + 1;
	
	// Put the value in the new slot
	heap[i] = iNode;	

	int j;
	Node * temp;	
					

	// Percolate Up
	// need to move up the tree until iNode > parent or iNode == root
	while ((iNode->getTotalWeight() < heap[getParent(i)]->getTotalWeight()) && (i != 0)) {		
		
		// swap iNode with parent until iNode < parent
		j = getParent(i);	

		temp = heap[i];	
		heap[i] = heap[j];
		heap[j] = temp;

		i = getParent(i);

	}


}


/*
  Name: print in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Prints the entire heap in order.
  Incoming: N/A
  Outgoing: Prints to screen every Node and its weight.
  Return: N/A
*/
void minBinaryHeap::print() const {

	// Name: TotalWeight
	for (int i = 0; i <= numItems; i++)
		cout << heap[i]->getName() << ": " << heap[i]->getTotalWeight() << endl;

}


/*
  Name: popMin in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Removes the first, or minimum, item from the heap, and then rearranges 
		it back into a proper binary heap. 
  Incoming: N/A
  Outgoing: index is the starting index to percolate down from
  Return: Returns a pointer to the node that was popped off the heap.
*/
Node * minBinaryHeap::popMin() {

	// If the heap is empty, there is no min
	if (isEmpty())
		return 0;

	// Min is the first Node in the heap
	Node * min = heap[0];
	int index = 0;
	
	// Move the last Node in the heap to the top root and reduce the number of items
	heap[0] = heap[numItems - 1];
	heap[numItems - 1] = 0;

	numItems--;
	heap.resize(numItems);
	
	// Percolate Down as necessary
	if (numItems > 0)
		percolateDown(index);

	return min;

}


/*
  Name: percolateDown in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Moves the index value down into its correct spot in order to have a proper binary heap
  Incoming: index is the slot of the node in the heap.
  Outgoing: index is the slot of the node in the heap.
  Return: N/A
*/
void minBinaryHeap::percolateDown(int index) {

	int leftChild, rightChild, min;
	Node * temp;

	// Get the left and right children of the heap index
	leftChild = getLeftChild(index);
	rightChild = getRightChild(index);

	// Stopping condition
	if (rightChild >= numItems) {

		if (leftChild >= numItems)
			return;
		else
			min = leftChild;

	}
	
	// Find min
	else {

		if (heap[leftChild] <= heap[rightChild])
			min = leftChild;
		else
			min = rightChild;

	}

	// Swap and percolate down to keep heap order
	if (heap[index]->getTotalWeight() > heap[min]->getTotalWeight()) {

		temp = heap[index];
		heap[index] = heap[min];
		heap[min] = temp;

		percolateDown(min);

	}

}


/*
  Name: getHeap in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the binary heap.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the binary heap as a vector.
*/
const vector<Node *> & minBinaryHeap::getHeap() const {

	return heap;

}
		

/*
  Name: getNumitems in scope of minBinaryHeap
  Writer(s): Drew Aaron, Michael Beaver, and Andrew Hamilton
  Purpose: Returns the number of items in the heap.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the number of items in the heap.
*/
int minBinaryHeap::getNumItems() const {

	return numItems;

}