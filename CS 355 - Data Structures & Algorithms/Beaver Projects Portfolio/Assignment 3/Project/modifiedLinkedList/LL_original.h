/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 3
	Description: This program implements and demonstrates the usage of a templated version of a singly
				 LinkedList class.  The program uses four generic tests on three different data types:
				 int, double, and string.
    Due Date: September 11, 2012
*/



#ifndef LL_H
#define LL_H

#include <iostream>



// Forward declaration so the Node class knows the LL class will eventually exist
template <class T>
class LL;



template <class T>
class Node {

	private:
	
		T data;			// Integer data value
		Node<T> *next;  // Pointer to the next Node in the list -- either an address or NULL (empty list)

		void print(std::ostream *os) const;

	public:

		// Constructors
		Node();					//    Default constructor -- 0 for data, NULL next pointer
		Node(T e);				// Overloaded constructor -- data only, NULL next pointer
		Node(T e, Node *n);		// Overloaded constructor -- data and next pointer (*n)


		// Destructor
		~Node();

		template <class T1>
		friend std::ostream& operator <<(std::ostream& os, const Node<T1>& n);	


	friend class LL<T>;


};



template <class T>
class LL {

	private:

		Node<T> *head;	   // Beginning of the list
		int size;		   // The number of Nodes in the list
		Node<T> *cursor;   // Cursor location in list

		// Member methods -- Recommend using the public insert(T e) and remove(T e) methods
		bool insertFront(T e);
		bool insertBack(T e);
		
		bool removeFront();
		bool removeBack();

		void print(std::ostream *os) const;

	public:

		// Constructor
		LL();					     // Default -- empty list
		LL(const  LL<T>& source);	 // Copy constructor


		// Destructor
		~LL(); 


		// Member methods
		bool insert(T e);
		bool remove(T e);

		Node<T>* search(T e) const;
		
		int getSize() const;
		void clearList();

		LL<T>& operator =(const LL<T>& source);

		T getCursorVal();
		bool placeCursor(int pos);


		// Friend functions
		template <class T1>
		friend std::ostream& operator <<(std::ostream& os, const LL<T1>& list);

};



//----------------------------------------------------------------------------------------
// NODE METHODS
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
void Node<T>::print(std::ostream *os) const {

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
std::ostream& operator <<(std::ostream& os, const Node<T>& n) {

	n.print(&os);

	return os;

}




//----------------------------------------------------------------------------------------
// LL METHODS
//----------------------------------------------------------------------------------------



/*
  Name: LL in scope of LL                                                      
  Purpose: Default constructor.  The head is the beginning of the list, and it is NULL by default.  The size
		   is the number of Nodes in the list.  The cursor is set to the first position [0].
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
template <class T>
LL<T>::LL() {

	head = NULL;
	size = 0;
	
	placeCursor(0);

}



/*
  Name: LL in scope of LL                                                      
  Purpose: Copy constructor.  Data members from source are copied into the calling object.  The calling object's
		   size is updated, and its cursor takes the value of the source's cursor value.
  Incoming: source is the LinkedList that is being instantiated from
  Outgoing: source is the original LinkedList, and it is not modified
  Return: N/A
*/
template <class T>
LL<T>::LL(const LL<T>& source) {

	int sourceSize = source.getSize();

	head = NULL;
	size = 0;
	cursor = head;

	// Temporary Nodes used to traverse the source LinkedList
	Node<T> *current = source.head;
	Node<T> *nextNode = current->next;
	Node<T> *srcCursor = source.cursor;

	// Copy all the data from the source LinkedList
	for (int i = 0; i < sourceSize; i++) {

		insert(current->data);
		
		if (current == srcCursor)
			placeCursor(i);

		current = current->next;

		// Loop until the end of the list -- could be eliminated because of the for loop's definition
		if (nextNode != NULL)
			nextNode = nextNode->next;
		else
			break;

	}

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

	Node<T> *temp;

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

	/*
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

			Node<T> *temp = previousNode->next;
			Node<T> *newNode = new Node<T>(e);

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
	*/

	int count = 0;
	Node<T> *current = this->head;
	Node<T> *previous = current;

	// Traverse the list
	while (count < this->size) {

		// Need to insert a new Node to preserve the sorting lowest-to-highest value
		if ((current->data) >= e)
			break;

		// Need to keep traversing the list to find the proper insertion point
		else {

			previous = current;
			current = current->next;

		}

		count++;

	}

	Node<T> *newNode = new Node<T>(e); // Allocating a new Node to insert

	// Insert the new Node at the start of the list
	if (count == 0) {

		newNode->next = previous;
		this->head = newNode;

		this->size++;

		return true;

	}

	// Insert the Node at the end of the list
	else if (count == size) {

		previous->next = newNode;
		newNode->next = NULL;

		this->size++;

		return true;

	}

	// Insert the Node in the middle of the list
	else {

		newNode->next = previous->next;
		previous->next = newNode;

		this->size++;

		return true;

	}

	return false;

}



/*
  Name: remove in scope of LL                                                      
  Purpose: Removes a Node from the LinkedList based on the value e.  Essentially performs a crude search to
		   find the Node with the value e and delete the Node.  If the value is not in the list, nothing is
		   removed.
  Incoming: e is the data value to be searched for and removed
  Outgoing: N/A
  Return: Returns true if the Node was delete successfully and false if otherwise
*/
template <class T>
bool LL<T>::remove(T e) {

	// Not going to delete from an empty list
	if (this->getSize() == 0) 
		return false;

	/*
	// The value is at the head
	if (head->data == e)
		if (removeFront()) 
			return true;
		else 
			return false;

	// Temporary nodes for traversing the list
	Node<T> *current = head;
	Node<T> *nextNode = current->next;
	Node<T> *previousNode = current;

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
	*/

	int count = 0;
	Node<T> *current = this->head;
	Node<T> *previous = current;

	// Traverse the list
	while (count < this->size) {

		// Need remove the Node
		if ((current->data) == e)
			break;

		// Need to keep traversing the list to find the proper insertion point
		else {

			previous = current;
			current = current->next;

		}

		count++;

	}

	// Only one Node in the list
	if (this->head->next == NULL) {

		delete this->head;
		this->head = NULL;

		this->size--;

		return true;

	}

	// Remove Node at the start of the list
	else if (count == 0) {

		this->head = current->next;
		delete current;
		
		this->size--;

		return true;

	}

	// Remove Node from end of the list
	else if (count == ((this->size)-1)) {

		delete current;
		previous->next = NULL;

		this->size--;

		return true;

	}

	// Remove Node from middle of the list
	else {

		previous->next = current->next;
		delete current;

		this->size--;

		return true;

	}
	
	return false;

}



/*
  Name: insertFront in scope of LL                                                      
  Purpose: Inserts a Node at the beginning of the list (head).
  Incoming: e is the data to be stored in the Node's data field.
  Outgoing: N/A
  Return: Returns true if the Node was inserted successfully or false if unsuccessful
*/
template <class T>
bool LL<T>::insertFront(T e) {

	/*
		Virtually equivalent:
		newNode->data = e;
		newNode->next = head;
	*/
	Node<T> *newNode = new Node<T>(e, head);

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
  Incoming: e is the data to be stored in the Node's data field
  Outgoing: N/A
  Return: Returns true if the Node was added successfully or false if unsuccessful
*/
template <class T>
bool LL<T>::insertBack(T e) {

	// If the list is empty, then use the insertFront function
	if (head == NULL) {

		if (insertFront(e))
			return true;

		else
			return false;

	}


	// Using two temporary nodes to find the end of the list
	Node<T> *current = head;
	Node<T> *nextNode = current->next;

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
	Node<T> *newNode = new Node<T>(e);
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
template <class T>
bool LL<T>::removeFront() {

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
		Node<T> *current = head;
		Node<T> *temp = current->next;

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
template <class T>
bool LL<T>::removeBack() {

	if (this->getSize() == 0)
		return false;

	// If there is only one Node, remove the one Node
	if (head->next == NULL) {

		delete head;
		head = NULL; // Empty list
		size--;

		return true;

	}


	// Temporary Nodes for traversing the list
	Node<T> *current = head;
	Node<T> *nextNode = current->next;
	Node<T> *previousNode;

	// Find the last Node and the next-to-last Node
	while (nextNode != NULL) {

		previousNode = current;
		current = current->next;
		nextNode = nextNode->next;

	}

	// Delete the last Node
	delete current;
	previousNode->next = NULL; // The new last Node's next pointer is made NULL
	size--;

	return true;

}



/*
  Name: search in scope of LL                                                      
  Purpose: Searches for a data value e in the LinkedList.
  Incoming: e is the value to be searched for
  Outgoing: N/A
  Return: Returns the a pointer to the Node if found; otherwise, returns NULL
*/
template <class T>
Node<T>* LL<T>::search(T e) const {

	int listSize = this->getSize();


	// Nothing to search for, so return NULL
	if (listSize == 0)
		return NULL;


	// Temporary Node for traversing the list
	Node<T> *current = head;

	// Find the value in the list
	for (int i = 0; i < listSize; i++) {

		// Return pointer if found
		if (current->data == e) 
			return current;

		current = current->next;

	}

	return NULL;

}



/*
  Name: getSize in scope of LL                                                      
  Purpose: Returns the size of the list (number of Nodes in the list)
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the size of the list
*/
template <class T>
int LL<T>::getSize() const {

	return this->size;

}



/*
  Name: clearList in scope of LL                                                      
  Purpose: Removes ALL Nodes in the list using removeBack() (removeFront() could be used).
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
template <class T>
void LL<T>::clearList() {

	int listSize = this->getSize();

	// Remove ALL Nodes in the List
	for (int i = 0; i < listSize; i++)
		removeBack();

}



/*
  Name: = in scope of LL                                                      
  Purpose: Overloaded assignment operator allows for one LinkedList to assume the data members of another
		   LinkedList at any point other than instantiation (see: copy constructor).
  Incoming: source is the right-side LinkedList whose data members will be copied
  Outgoing: source is the right-side LinkedList that will not be modified
  Return: Returns a reference to the modified LinkedList object
*/
template <class T>
LL<T>& LL<T>::operator =(const LL<T>& source) {

	// Check for self-assignment
	if (this == &source)
		return *this;

	// Preparing the list to accept new data members
	this->clearList();

	// Temporary Nodes used to traverse the source list
	Node<T> *current = source.head;
	Node<T> *nextNode = current->next;
	Node<T> *srcCursor = source.cursor;

	int sourceSize = source.getSize();

	// Traverse the list and copy over the source data members
	for (int i = 0; i < sourceSize; i++) {

		this->insert(current->data);

		if (current == srcCursor)
			this->placeCursor(i);

		current = current->next;

		// Break out if the last Node is NULL -- could be eliminated due to the use of a for loop
		if (nextNode != NULL)
			nextNode = nextNode->next;
		else
			break;

	}
	
	return *this;

}



/*
  Name: getCursorVal in scope of LL                                                      
  Purpose: Returns the data value stored in the Node that the cursor points at.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the data value stored in the Node that the cursor points at, or returns NULL if there is 
		  no Nodes (size == 0)
*/
template <class T>
T LL<T>::getCursorVal() {

	// There is a list, so return the cursor Node's data
	if (this->getSize() > 0)
		return cursor->data;

	// No Nodes
	return NULL;

}



/*
  Name: placeCursor in scope of LL                                                      
  Purpose: Moves the cursor on the LinkedList to a certain point, following array notation conventions.  For
		   example, in a LinkedList of size 5, the cursor may have range [0 .. 4].
  Incoming: pos is the new position of the cursor 
  Outgoing: N/A
  Return: Returns true if the cursor was moved successfully, or false if otherwise
*/
template <class T>
bool LL<T>::placeCursor(int pos) {

	int listSize = this->getSize();

	// Will return false if the list is empty or if the position is out of range
	if (pos >= listSize)
		return false;

	// Temporary Node for traversing the LinkedList
	Node<T> *current = this->head;

	// Traverse the list until pos is reached
	for (int i = 0; i < pos; i++)
		current = current->next;

	// Reassign the cursor to its new position
	this->cursor = current;

	return true;

}



/*
  Name: print in scope of LL                                                      
  Purpose: Prepares a user-friendly representation of the list.
  Incoming: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Outgoing: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Return: N/A
*/
template <class T>
void LL<T>::print(std::ostream *os) const {

	// List is empty
	if (this->head == NULL)
		*os << "(empty)";


	// Traverse the list and output each value
	else {
		
		Node<T> *current = this->head;

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
template <class T>	
std::ostream& operator <<(std::ostream& os, const LL<T>& list) {

	list.print(&os);

	return os;

}



#endif