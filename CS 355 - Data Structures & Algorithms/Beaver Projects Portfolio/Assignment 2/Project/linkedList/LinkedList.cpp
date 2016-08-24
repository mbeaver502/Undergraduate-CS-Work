/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 2
	Description: This program demonstrates possible usage of a programmer-defined singly LinkedList class.  
				 The LinkedList class has the ability to insert a Node at the head, insert a Node at the end,
				 remove the beginning Node, remove the last Node, remove all Nodes, and print itself.  A 
				 series of 17 cases of varying complexity test the accuracy of the LinkedList's implementation.
				 The LinkedList is cleared before executing each user-selected test case.
    Due Date: August 30, 2012
*/



#include <iostream>
#include "LinkedList.h"

using namespace std;



//----------------------------------------------------------------------------------------
// NODE
//----------------------------------------------------------------------------------------

/*
  Name: Node in scope of Node                                                      
  Purpose: Default constructor.  Data is defaulted to 0, and the next pointer is NULL.  Essentially, the
		   constructed Node could be the end of a Linked List.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Node::Node() {

	data = 0;
	next = NULL;

}



/*
  Name: Node in scope of Node                                                      
  Purpose: Overloaded constructor.  Data is set to the user's data value, and the next pointer is NULL.  
		   Essentially, the constructed Node could be the end of a Linked List.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Node::Node(int e) {

	data = e;
	next = NULL;

}



/*
  Name: Node in scope of Node                                                      
  Purpose: Overloaded constructor.  Data is set to the user's data value, and the next pointer is set to
		   the paseed-in pointer, which should point to the next Node in a Linked List.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Node::Node(int e, Node *n) {

	data = e;
	next = n;

}



/*
  Name: ~Node in scope of Node                                                      
  Purpose: Destructor.  When the Node object is destroyed, the pointer to the next object will be set to NULL.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Node::~Node() {

	next = NULL;

}



//----------------------------------------------------------------------------------------
// LINKEDLIST
//----------------------------------------------------------------------------------------

/*
  Name: LinkedList in scope of LinkedList                                                      
  Purpose: Default constructor.  The head is the beginning of the list, and it is NULL by default.  The size
		   is the number of Nodes in the list.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
LinkedList::LinkedList() {

	head = NULL;
	size = 0;

}



/*
  Name: ~LinkedList in scope of LinkedList                                                      
  Purpose: Destructor.  Destroys all the Node objects in the list.  It is pointless to decrement size, but it
		   does not hurt to do so anyway.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
LinkedList::~LinkedList() {

	Node *temp;

	while (head != NULL) {

		temp = head;
		head = (head->next);
		delete temp;

		size--;
		
	}

}



/*
  Name: insertFront in scope of LinkedList                                                      
  Purpose: Inserts a Node at the beginning of the list (head).
  Incoming: e is the integer data to be stored in the Node's data field.
  Outgoing: N/A
  Return: Returns true if the Node was inserted successfully or false if unsuccessful
*/
bool LinkedList::insertFront(int e) {

	/*
		Virtually equivalent:
		newNode->data = e;
		newNode->next = head;
	*/
	Node *newNode = new Node(e, head);

	// Assign head to the new beginning Node
	head = newNode;


	// If head is not NULL, then the Node was inserted (hence, size is incremented)
	if (head != NULL) {

		size++;

		return true;

	}

	// If for some reason the Node was not inserted, 
	// it would need to be found and removed to prevent memory leaks.
	return false;

}



/*
  Name: insertBack in scope of LinkedList                                                      
  Purpose: Inserts a Node at the end of the list.
  Incoming: e is the integer data to be stored in the Node's data field
  Outgoing: N/A
  Return: Returns true if the Node was added successfully or false if unsuccessful
*/
bool LinkedList::insertBack(int e) {

	// If the list is empty, then use the insertFront function
	if (head == NULL) {

		if (insertFront(e))
			return true;

		else
			return false;

	}


	// Using two temporary nodes to find the end of the list
	Node *current = head;
	Node *nextNode = current->next;

	// Loop until the end of the list is found
	while (nextNode != NULL) {

		current = current->next;
		nextNode = nextNode->next;

	}


	/*
		Virtually equivalent:
		newNode->data = e;
		newNode->next = NULL;
	*/
	Node *newNode = new Node(e);
	current->next = newNode;
	size++;

	return true;

}



/*
  Name: removeFront in scope of LinkedList                                                      
  Purpose: Removes the first Node in the list (Node at head)
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the Node is removed successfully or false if unsuccessful
*/
bool LinkedList::removeFront() {

	// If the list is empty, then there is nothing to remove
	if (head == NULL)
		return false;

	// If there is only one Node, remove the one Node
	else if (head->next == NULL) {

		delete head;
		head = NULL; // Empty list
		size--;

	}


	// Otherwise, remove the first Node at head
	else {
	
		// Using temporary Nodes so the entire list is not lost, which would result in a memory leak
		Node *current = head;
		Node *temp = current->next;

		delete current;
		size--;

		// Re-assign head to point to the new beginning of the list
		head = temp;

	}

	return true;

}



/*
  Name: removeBack in scope of LinkedList                                                      
  Purpose: Removes the last Node in the list
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the Node is removed successfully or false if unsuccessful
*/
bool LinkedList::removeBack() {

	// If there is only one Node, remove the one Node
	if (head->next == NULL) {

		delete head;
		head = NULL; // Empty list
		size--;

		return true;

	}


	// Temporary Nodes for traversing the list
	Node *current = head;
	Node *nextNode = current->next;
	Node *previousNode;

	// Find the last Node and the next-to-last Node
	while (nextNode != NULL) {

		previousNode = current;
		current = current->next;
		nextNode = nextNode->next;

	}

	// Delete the last Node
	delete current;
	size--;
	previousNode->next = NULL; // The new last Node's next pointer is made NULL


	// About to traverse the list again
	current = head;
	nextNode = current->next;

	// Finding the end of the list
	while (nextNode != NULL) {

		current = current->next;
		nextNode = nextNode->next;

	}

	// If the Node after the last Node is NULL, then the original Node was successfully removed
	if (nextNode == NULL)
		return true;
	
	return false;

}



/*
  Name: getSize in scope of LinkedList                                                      
  Purpose: Returns the size of the list (number of Nodes in the list)
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the size of the list
*/
int LinkedList::getSize() const {

	return this->size;

}



/*
  Name: clearList in scope of LinkedList                                                      
  Purpose: Removes ALL Nodes in the list using removeBack() (removeFront() could be used).
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void LinkedList::clearList() {

	int listSize = this->getSize();

	// Remove ALL Nodes in the List
	for (int i = 0; i < listSize; i++)
		removeBack();

}



/*
  Name: print in scope of LinkedList                                                      
  Purpose: Prepares a user-friendly representation of the list.
  Incoming: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Outgoing: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Return: N/A
*/
void LinkedList::print(ostream *os) const {

	// List is empty
	if (head == NULL)
		*os << "(empty)";


	// Traverse the list and output each value
	else {
		
		Node *current = head;

		while (current != NULL) {

			*os << current->data << " ";
			current = current->next;

		}

	}

}



/*
  Name: << (friend)                                             
  Purpose: Overloaded insertion operator allows the user to output the list to the screen.
  Incoming: os is an ostream passed-by-reference that will contain an output stream (e.g., cout). list is the
		    LinkedList that is to be printed; it is passed-by-reference
  Outgoing: os and list are passed-by-reference, though os is the only one that should change
  Return: Returns a reference to an ostream that contains the user-friendly version of the list.
*/
ostream& operator <<(ostream& os, const LinkedList& list) {

	list.print(&os);

	return os;

}