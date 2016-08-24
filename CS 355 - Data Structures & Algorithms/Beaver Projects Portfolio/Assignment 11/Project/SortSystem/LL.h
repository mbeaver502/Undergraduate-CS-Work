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



#ifndef LL_H
#define LL_H

#include <iostream>


template <class T>
class LL;


//----------------------------------------------------------------------------------------
// NODE
//----------------------------------------------------------------------------------------

template <class T>
class Node {

	private:
	
		T data;    // Integer data value
		Node<T> *next;  // Pointer to the next Node in the list -- either an address or NULL (empty list)

		void print(std::ostream *os) const;

	public:

		// Constructors
		Node();					//    Default constructor -- 0 for data, NULL next pointer
		Node(T e);			// Overloaded constructor -- data only, NULL next pointer
		Node(T e, Node *n);	// Overloaded constructor -- data and next pointer (*n)


		// Destructor
		~Node();

		template <class T1>
		friend std::ostream& operator <<(std::ostream& os, const Node<T1>& n);	


	friend class LL<T>;


};



//----------------------------------------------------------------------------------------
// LL
//----------------------------------------------------------------------------------------

template <class T>
class LL {

	private:

		Node<T> *head;  // Beginning of the list
		int size;		// The number of Nodes in the list

		// Member methods
		bool insertFront(T e);
		bool insertBack(T e);
		
		bool removeFront();
		bool removeBack();

		void print(std::ostream *os) const;

	public:

		// Constructor
		LL();		// Default -- empty list


		// Destructor
		~LL(); 


		// Member methods
		bool insert(T e);
		bool remove(T e);

		Node<T>* search(T e) const;
		
		int getSize() const;
		void clearList();

		T * getData() const;

		// Friend functions
		template <class T1>
		friend std::ostream& operator <<(std::ostream& os, const LL<T1>& list);

};




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

	if (head == NULL)
		if (insertFront(e))
			return true;
		else
			return false;


	// Either empty list or value less than the first data value
	else if (this->getSize() == 0) {

		if (head->data >= e) {

			if (insertFront(e)) 
				return true;
			else 
				return false;

		}

	}

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
template <class T>
bool LL<T>::remove(T e) {

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

	return false;

}



/*
  Name: insertFront in scope of LL                                                      
  Purpose: Inserts a Node at the beginning of the list (head).
  Incoming: e is the integer data to be stored in the Node's data field.
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
  Incoming: e is the integer data to be stored in the Node's data field
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

template <class T>
Node<T>* LL<T>::search(T e) const {

	if (this->getSize() == 0)
		return NULL;

	// Temporary Nodes for traversing the list
	Node<T> *current = head;
	Node<T> *nextNode = current->next;
	Node<T> *previousNode;

	// Find the last Node and the next-to-last Node
	while (nextNode != NULL) {
	
		previousNode = current;
		if (current->data == e) 
			return previousNode;

		current = current->next;
		nextNode = nextNode->next;

	}

	return NULL;

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
  Name: print in scope of LL                                                      
  Purpose: Prepares a user-friendly representation of the list.
  Incoming: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Outgoing: os is a pointer to an ostream which will contain the formatted list for output to the screen
  Return: N/A
*/
template <class T>
void LL<T>::print(std::ostream *os) const {

	
	// List is empty
	if (head == NULL)
		return;
	

	// Traverse the list and output each value
	else {
		
		Node<T> *current = head;

		while (current != NULL) {

			*os << current->data << "\t";
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


/*
  Name: getData in scope of LL   
  Writer(s): Michael Beaver
  Purpose: Returns a pointer to an array of the LL's data.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns a pointer to an array of the LL's data
*/
template <class T>
T * LL<T>::getData() const {

	T * result = new T [size];

	if (size == 0) {

		result[0] = -1;
		return result;

	}

	else {
	
		for (int i = 0; i < size; i++)
			result[i] = -1;

	}

	Node<T> * current = head;
	int i = 0;

	while (current != NULL) {

		result[i] = current->data;
		current = current->next;

		i++;

	}

	return result;

}



#endif