/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 3
	Description: This program implements and tests a modified version of the LL class.  The modified version
				 uses generic insert(int) and remove(int) methods.  The generic forms of the methods make 
				 calls to the original insertFront(int), insertBack(int), removeFront(int), and removeBack(int)
				 as necessary.  Eleven cases are used to test the modified version of the LL class.
    Due Date: September 4, 2012
*/



#include <iostream>
#include "LL.h"

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
template <class T>
Node<T>::Node() {

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
template <class T>
Node<T>::Node(T e) {

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
template <class T>
Node<T>::Node(T e, Node *n) {

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
template <class T>
Node<T>::~Node() {

	next = NULL;

}


/*
  Name: print in scope of LL                                                      
  Purpose: Prepares a user-friendly representation of the list.
  Incoming: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Outgoing: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Return: N/A
*/
template <class T>
void Node<T>::print(ostream *os) const {

	*os << this->data << " ";

}



/*
  Name: << (friend)                                             
  Purpose: Overloaded insertion operator allows the user to output the list to the screen.
  Incoming: os is an ostream passed-by-reference that will contain an output stream (e.g., cout). list is the
		    LL that is to be printed; it is passed-by-reference
  Outgoing: os and list are passed-by-reference, though os is the only one that should change
  Return: Returns a reference to an ostream that contains the user-friendly version of the list.
*/
template <class T>
ostream& operator <<(ostream& os, const Node<T>& n) {

	n.print(&os);

	return os;

}



//----------------------------------------------------------------------------------------
// LL
//----------------------------------------------------------------------------------------

/*
  Name: LL in scope of LL                                                      
  Purpose: Default constructor.  The head is the beginning of the list, and it is NULL by default.  The size
		   is the number of Nodes in the list.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
template <class T>
LL<T>::LL() {

	head = NULL;
	size = 0;

}



/*
  Name: ~LL in scope of LL                                                      
  Purpose: Destructor.  Destroys all the Node objects in the list.  It is pointless to decrement size, but it
		   does not hurt to do so anyway.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
template <class T>
LL<T>::~LL() {

	Node *temp;

	while (head != NULL) {

		temp = head;
		head = (head->next);
		delete temp;

		size--;
		
	}

}



/*
  Name: insert in scope of LL                                                      
  Purpose: Inserts a Node into the LinkedList at a point where the value e is <= a value in the list. If the
		   list is empty, it is the first Node.
  Incoming: e is the value to be inserted as a Node into the list
  Outgoing: N/A
  Return: Returns true if the Node was added successfully and false if otherwise
*/
template <class T>
bool LL<T>::insert(T e) {

	// Either empty list or value less than the first data value
	if ((this->getSize() == 0) || (head->data >= e))
		if (insertFront(e)) 
			return true;
		else 
			return false;

	// Temporary nodes used to insert data in the middle of the list
	Node<T> *current = head;
	Node<T> *nextNode = current->next;
	Node<T> *previousNode = current;

	// Insertion in middle of list (anywhere between head and the end)
	while (nextNode != NULL) {

		previousNode = current;
		current = current->next;
		nextNode = nextNode->next;

		// Finding where to insert the new Node
		if (current->data >= e) {

			Node *temp = previousNode->next;
			Node *newNode = new Node(e);

			previousNode->next = newNode;
			newNode->next = temp;

			size++;

			return true;

		}

	}

	// Data must go at the end
	if (insertBack(e)) 
		return true;

	return false;

}



/*
  Name: remove in scope of LL                                                      
  Purpose: Removes a Node from the LinkedList based on the value e.  Essentially performs a crude search to
		   find the Node with the value e and delete the Node.  If the value is not in the list, nothing is
		   removed.
  Incoming: e is the integer data value to be searched for and removed
  Outgoing: N/A
  Return: Returns true if the Node was delete successfully and false if otherwise
*/
bool LL::remove(int e) {

	// Not going to delete from an empty list
	if (this->getSize() == 0) 
		return false;

	// The value is at the head
	if (head->data == e)
		if (removeFront()) 
			return true;
		else 
			return false;

	// Temporary nodes for traversing the list
	Node *current = head;
	Node *nextNode = current->next;
	Node *previousNode = current;

	// Removal of Node in middle of list (anywhere between head and the end)
	while (nextNode != NULL) {

		previousNode = current;
		current = current->next;
		nextNode = nextNode->next;

		// Finding the Node in the middle
		if (current->data == e) {
			
			delete current;

			previousNode->next = nextNode;

			size--;

			return true;

		}

	}

	// Data may be at the end
	if (current->data == e)
		if (removeBack()) 
			return true;

	return false;

}



/*
  Name: insertFront in scope of LL                                                      
  Purpose: Inserts a Node at the beginning of the list (head).
  Incoming: e is the integer data to be stored in the Node's data field.
  Outgoing: N/A
  Return: Returns true if the Node was inserted successfully or false if unsuccessful
*/
bool LL::insertFront(int e) {

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
  Name: insertBack in scope of LL                                                      
  Purpose: Inserts a Node at the end of the list.
  Incoming: e is the integer data to be stored in the Node's data field
  Outgoing: N/A
  Return: Returns true if the Node was added successfully or false if unsuccessful
*/
bool LL::insertBack(int e) {

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
  Name: removeFront in scope of LL                                                      
  Purpose: Removes the first Node in the list (Node at head)
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the Node is removed successfully or false if unsuccessful
*/
bool LL::removeFront() {

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
  Name: removeBack in scope of LL                                                      
  Purpose: Removes the last Node in the list
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true if the Node is removed successfully or false if unsuccessful
*/
bool LL::removeBack() {

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





//----------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------


Node* LL::search(int e) const {

	// Temporary Nodes for traversing the list
	Node *current = head;
	Node *nextNode = current->next;
	Node *previousNode;

	// Find the last Node and the next-to-last Node
	while (nextNode != NULL) {
	
		previousNode = current;
		if (current->data == e) 
			return previousNode;

		current = current->next;
		nextNode = nextNode->next;

	}

}



//----------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------
//----------------------------------------------------------------------------------------------------




/*
  Name: getSize in scope of LL                                                      
  Purpose: Returns the size of the list (number of Nodes in the list)
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the size of the list
*/
int LL::getSize() const {

	return this->size;

}



/*
  Name: clearList in scope of LL                                                      
  Purpose: Removes ALL Nodes in the list using removeBack() (removeFront() could be used).
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void LL::clearList() {

	int listSize = this->getSize();

	// Remove ALL Nodes in the List
	for (int i = 0; i < listSize; i++)
		removeBack();

}



/*
  Name: print in scope of LL                                                      
  Purpose: Prepares a user-friendly representation of the list.
  Incoming: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Outgoing: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Return: N/A
*/
void LL::print(ostream *os) const {

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
		    LL that is to be printed; it is passed-by-reference
  Outgoing: os and list are passed-by-reference, though os is the only one that should change
  Return: Returns a reference to an ostream that contains the user-friendly version of the list.
*/
ostream& operator <<(ostream& os, const LL& list) {

	list.print(&os);

	return os;

}