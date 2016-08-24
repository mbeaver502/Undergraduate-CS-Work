/*
	Names: Drew Aaron, Michael Beaver, Andrew Hamilton, and Elizabeth McClellan
	CS355 Fall 2012
	Assignment: Assignment 6, Homework 4 -- Group 2
	Description: This program implements and tests an integer-based Binary Search Tree.  The BST
				 has the ability to insert values, remove values, copy and assign itself to other
				 BSTs, print itself in three ways (Pre, Post, and In order), and manipulate the
				 position of the cursor.
    Due Date: October 9, 2012
*/


#include "Bst.h"

 
//-------------------------------------------------------------------------
// BNode
//-------------------------------------------------------------------------


/*
  Name: BNode in scope of BNode     
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Default constructor.  Data is defaulted to 0, and the left and right pointers are NULL. 
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BNode::BNode() {

	data = 0;
	left = NULL;
	right = NULL;

}


/*
  Name: BNode in scope of BNode 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Overloaded constructor.  Data is set to e, and the left and right pointers are NULL. 
  Incoming: e is the integer data value to be stored in the BNode
  Outgoing: N/A
  Return: N/A
*/
BNode::BNode(int e) {

	data = e;
	left = NULL;
	right = NULL;

}


/*
  Name: BNode in scope of BNode 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Overloaded constructor.  Data is set to e, and the left and right pointers are set 
		   to l and r, respectively. 
  Incoming: e is the integer data value to be stored in the BNode, l is the pointer to the left child
			BNode, and r is the pointer to the right child Node
  Outgoing: N/A
  Return: N/A
*/
BNode::BNode(int e, BNode * l, BNode * r) {

	data = e;
	left = l;
	right = r;

}


/*
  Name: GetData in scope of BNode 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Returns the data stored in a BNode.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the integer data stored in the BNode
*/
int BNode::GetData() const {

	return data;

}



//-------------------------------------------------------------------------
// BST
//-------------------------------------------------------------------------


/*
  Name: BST in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Default constructor.  The root is set to NULL, and the cursor is set to point to the root (NULL).
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BST::BST() {

	root = NULL;
	cursor = root;

}


/*
  Name: BST in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Copy constructor.  The root and cursor are set to NULL in preparation for a call to CopyList,
		   which copies the source BST values into this BST.
  Incoming: b is the source BST to be copied
  Outgoing: b is the source BST to be copied (const modifier)
  Return: N/A
*/
BST::BST(const BST & b) {

	root = NULL;
	cursor = root;

	CopyList(b);

}


/*
  Name: ~BST in scope of BST 
  Writer(s): Michael
  Purpose: Destructor.  Clears all BNodes from the BST by calling ClearList.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
BST::~BST() {

	ClearList();

}


/*
  Name: PrintInHelp in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Recursively prints the BST's values in order, lowest to highest, with brackets around the
		   value the cursor points to. Solution inspired by Dr. David Eck.
  Incoming: os is the ostream object, n is a pointer a BNode (used to traverse the BST)
  Outgoing: os is the ostream object, n is a pointer a BNode (used to traverse the BST)
  Return: N/A (void)
*/
void BST::PrintInHelp(ostream & os, BNode * n) const  {

	// Try to print if the pointer is not NULL
	if (n != NULL) {

		// Try to recurse left
		PrintInHelp(os, n->left);

		// Print brackets if at the cursor, otherwise just print the value
		if (n == cursor) 
			os << "[" << n->GetData() << "]" << "\t";		

		else
			os << n->GetData() << "\t";

		// Try to recurse right
		PrintInHelp(os, n->right);

	}

}


/*
  Name: PrintPreHelp in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Recursively prints the BST's values in pre-order with brackets around the
		   value the cursor points to.
  Incoming: os is the ostream object, n is a pointer a BNode (used to traverse the BST)
  Outgoing: os is the ostream object, n is a pointer a BNode (used to traverse the BST)
  Return: N/A (void)
*/
void BST::PrintPreHelp(ostream & os, BNode * n) const  {

	// Try to print if the pointer is not NULL
	if (n != NULL) {

		// If at the cursor, print brackets, otherwise print the value
		if (n == cursor) 
			os << "[" << n->GetData() << "]" << "\t";		

		else
			os << n->GetData() << "\t";

		// Try to recurse left
		PrintPreHelp(os, n->left);

		// Try to recurse right
		PrintPreHelp(os, n->right);

	}

}


/*
  Name: PrintPostHelp in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Recursively prints the BST's values in post-order with brackets around the
		   value the cursor points to.
  Incoming: os is the ostream object, n is a pointer a BNode (used to traverse the BST)
  Outgoing: os is the ostream object, n is a pointer a BNode (used to traverse the BST)
  Return: N/A (void)
*/
void BST::PrintPostHelp(ostream & os, BNode * n) const  {

	// Try to print if the pointer is not NULL
	if (n != NULL) {

		// Try to recurse left
		PrintPostHelp(os, n->left);

		// Try to recurse right
		PrintPostHelp(os, n->right);

		// If at the cursor, print with brackets, otherwise print the value
		if (n == cursor) 
			os << "[" << n->GetData() << "]" << "\t";		

		else
			os << n->GetData() << "\t";

	}

}


/*
  Name: GoToNextHelp in scope of BST 
  Writer(s): Michael
  Purpose: Recursively moves the cursor pointer down the list to the next BNode
  Incoming: e is the value at the cursor's current location, n is a pointer to a BNode, and flag signals
			when the appropriate next value is found
  Outgoing: n is a pointer to a BNode, and flag signals
			when the appropriate next value is found
  Return: N/A (void)
*/
void BST::GoToNextHelp(int e, BNode * n, bool & flag) {
	
	// Recurse through the tree until the proper value is reached or the end of the tree
	if ((n != NULL) && (flag == false)) {

		// Branch left
		GoToNextHelp(e, n->left, flag);

		// Value found, set the cursor
		if ((n->GetData() > e) && (flag == false)) {

			cursor = n;
			flag = true;
			return;

		}

		// Branch right
		GoToNextHelp(e, n->right, flag);

	}

	// The next Node was not found through recursion, so it must be the root
	if (flag == false)
		cursor = root;

}


/*
  Name: GoToPrevHelp in scope of BST 
  Writer(s): Michael
  Purpose: Recursively moves the cursor pointer up the list to the previous BNode
  Incoming: e is the value at the cursor's current location, n is a pointer to a BNode, and flag signals
			when the appropriate next value is found
  Outgoing: n is a pointer to a BNode, and flag signals
			when the appropriate next value is found
  Return: N/A (void)
*/
void BST::GoToPrevHelp(int e, BNode * n, bool & flag) {
	
	// Recurse through the tree until the proper value is reached or the end of the tree
	if ((n != NULL) && (flag == false)) {

		// Branch right
		GoToPrevHelp(e, n->right, flag);

		// Value found, set the cursor
		if ((n->GetData() < e) && (flag == false)) {

			cursor = n;
			flag = true;
			return;

		}

		// Branch left
		GoToPrevHelp(e, n->left, flag);

	}

	// The previous Node was not found through recursion, so it must be the root
	if (flag == false)
		cursor = root;

}


/*
  Name: ClearHelp in scope of BST 
  Writer(s): Michael
  Purpose: Recursively deletes BNodes in the BST
  Incoming: n is a pointer to a BNode
  Outgoing: n is a pointer to a BNode
  Return: N/A (void)
*/
void BST::ClearHelp(BNode * n) {

	// Try to delete if the pointer is not NULL
	if (n != NULL) {

		// Try to recurse left
		ClearHelp(n->left);

		// Try to recurse right
		ClearHelp(n->right);

		// Delete the BNode
		delete n;

	}

}


/*
  Name: CopyHelp in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Recursively copies BNode values from one BST to this BST
  Incoming: src is a pointer to a BNode in the source BST, ths is a reference pointer to a BNode in this
		    BST, and srcCursor is a pointer to the cursor in the source BST
  Outgoing: src is a pointer to a BNode in the source BST, ths is a reference pointer to a BNode in this
		    BST, and srcCursor is a pointer to the cursor in the source BST
  Return: N/A (void)
*/
void BST::CopyHelp(BNode * src, BNode *& ths, BNode * srcCursor) {

	// Copy if the source Node is not NULL -> Pre-Order copy
	if (src != NULL) {
	
		// Create a new Node and insert it into the BST
		BNode * temp = new BNode(src->data);
		ths = temp;

		// We're at the source BST's cursor, so set this BST's cursor
		if (src == srcCursor)
			cursor = ths;

		// Recurse in a Pre-Order fashion
		CopyHelp(src->left, ths->left, srcCursor);
		CopyHelp(src->right, ths->right, srcCursor);

	}

}


/*
  Name: = in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Overloaded assignment operator uses CopyList to copy a source BST into this BST.
  Incoming: b is the source BST
  Outgoing: b is the source BST
  Return: Returns a reference to this BST
*/
BST & BST::operator =(const BST & b) {

	// Check for self-assignment
	if (this == &b)
		return *this;

	// Clear this list and copy the source list's elements
	ClearList();
	CopyList(b);

	return *this;

}


/*
  Name: Insert in scope of BST 
  Writer(s): Michael
  Purpose: Iteratively inserts BNodes into the BST
  Incoming: e is the integer value to insert
  Outgoing: N/A
  Return: Returns true if the value was inserted successfully and false if otherwise
*/
bool BST::Insert(int e) {

	// No root, insert at root
	if (Empty()) {

		root = new BNode(e);
		cursor = root;

		return true;

	}

	BNode * current = root;

	// Traverse the tree
	while (current != NULL) {

		// Data already in tree
		if (current->data == e)
			return false;

		// Branch left
		else if (e < current->data) {

			// There is no left child Node, so create it
			if (current->left == NULL) {

				BNode * newNode = new BNode(e);
				current->left = newNode;
				cursor = newNode;

				return true;

			}

			// There is a left child Node, so keep traversing
			else
				current = current->left;

		}

		// Branch right
		else {

			// There is no right child Node, so create it
			if (current->right == NULL) {

				BNode * newNode = new BNode(e);
				current->right = newNode;
				cursor = newNode;

				return true;

			}

			// There is a right child Node, so keep traversing
			else
				current = current->right;

		}

	}

	return false;

}


/*
  Name: Remove in scope of BST 
  Writer(s): Drew
  Purpose: Removes a node from the tree
  Incoming: e is data in a node to remove
  Outgoing: N/A
  Return: true if found and removed, false otherwise
*/
bool BST::Remove(int e) {

	cursor = root;
	BNode* n = Find(e); //if e is found in the tree, sets n and cursor to the location

	if (n == NULL) { //if not found, returns false

		cursor = root;
		return false;

	}

	BNode* parent = findParent(n->data);

	if (n == root) { //if the root is what we are deleting

		if (n->right == NULL) { //if right is null, check if left is null

			if (n->left == NULL) { // if left is also null, make everything null and delte root node

				cursor = NULL;
				root = NULL;
				delete n;

				return true;
			}

			else if (n->left != NULL) { //if right is null but left isn't null, one left of root is new root
				
				root = root->left;
				cursor = root;
				delete n;

				return true;
			}

		}

		else if (n->right != NULL) { //if the roots right side isn't null, make the next largest number root

			n = n->right; //next largest number in a tree is one to the right, then all the way left

			if (n->left == NULL) { //if the node to the right of root has a null left
				
				root->data = n->data; //put the data in that node into root, and move the pointers around it, then delete it
				root->right = n->right;
				delete n;

				return true;
			}

			else if(n ->left != NULL) { //if the node to the right of root doesn't have a null left

				while(n ->left != NULL) // move left as far as possible
					n = n->left;

				BNode* p = findParent(n->data); 
				p->left = n->right;//now put this node's data into root, and reroute pointers around it
				root->data = n->data;
				delete n;

				return true;

			}

		}

	} //end of if item to delete is the root

	if (n->right == NULL) { //if leaf node or node with no right

		cursor = findParent(n->data); 

		if ( (cursor->left != NULL)  &&  (cursor->left->data == e) ) //find which side of the parent node the current node is on
			cursor->left = n->left;							//and reroute that side around n

		else if ( (cursor->right != NULL)  &&  (cursor->right->data == e) )
			cursor->right = n->left;

		cursor = parent;
		delete n;

		return true;

	}

	if (n->right != NULL) { //if the node is a stem without a null right

		n = n->right;

		if (n->left == NULL) { 

			cursor->data = n->data; 
			cursor->right = n->right;
			cursor = parent;
			delete n;

			return true;
		}

		else if (n->left != NULL) { //if the node to the right of original node doesn't have a null left

			while(n->left != NULL) // move left as far as possible
				n = n->left;

			BNode* p = findParent(n->data); //now put this node's data into original node, and reroute pointers around it
			
			p->left = n->right;
			cursor->data = n->data;
			cursor = parent;
			delete n;

			return true;

		}

	}

	return false; 

}


/*
  Name: PrintPre in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Prints the BST in pre-order
  Incoming: os is the ostream object
  Outgoing: os is the ostream object
  Return: N/A (void)
*/
void BST::PrintPre(ostream & os) const {

	// Check if tree is empty
	if(Empty()) {

		os << "Tree is empty!";
		return;

	}

	// Only root Node in tree
	if((root ->left == NULL) && (root ->right == NULL)) {

		os << "[" << root ->data << "]";
		return;

	}

	// Print the entire tree
	PrintPreHelp(os, root);

}


/*
  Name: PrintPost in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Prints the BST in post-order
  Incoming: os is the ostream object
  Outgoing: os is the ostream object
  Return: N/A (void)
*/
void BST::PrintPost(ostream & os) const {
		
	// Check if tree is empty
	if(Empty()) {

		os << "Tree is empty!";
		return;

	}

	// Only root Node in tree
	if((root ->left == NULL) && (root ->right == NULL)) {

		os << "[" << root ->data << "]";
		return;

	}

	// Print entire tree
	PrintPostHelp(os, root);

}


/*
  Name: PrintIn in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Prints the BST in order, lowest to highest
  Incoming: os is the ostream object
  Outgoing: os is the ostream object
  Return: N/A (void)
*/
void BST::PrintIn(ostream & os) const {

	// Check if tree is empty
	if (Empty()) {

		os << "Tree is empty!";
		return;

	}

	// Only root Node in tree
	if ((root->left == NULL) && (root->right == NULL)) {

		os << "[" << root ->data << "]";
		return;

	}

	// Print entire tree
	PrintInHelp(os, root);

}


/*
  Name: Find in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Searches for a value e, starting from the cursor
  Incoming: e is the value to find
  Outgoing: N/A
  Return: Returns the address to BNode containing the value e, returns NULL if not found
*/
BNode * BST::Find(int e) {

	// Empty tree, then there are no Nodes, so return NULL
	if (Empty())
		return NULL;

	// Pointer n to traverse list
	BNode * n = cursor;

	while (n != NULL) {

		// Value found, set the cursor to point to it, and return the address
		if (n->data == e) {

			cursor = n;

			return n;

		}

		// Branch left
		else if (n->data > e)
			n = n->left;

		// Branch right
		else if (n->data < e)
			n = n->right;

	}

	// Value not found, set the cursor to the last value in the tree
	GoToEnd();

	return NULL;

}


/*
  Name: AtCursor in scope of BST 
  Writer(s): Drew and Andrew
  Purpose: Returns the address of the BNode the cursor is pointing at
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the address of the BNode the cursor is pointing at
*/
BNode * BST::AtCursor() const {

	return cursor;

}


/*
  Name: GoToBeginning in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Moves the cursor to the beginning (minimum value) of the BST
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BST::GoToBeginning() {

	// Return if there is no tree
	if (Empty()) {

		cursor = NULL;
		return;

	}

	// Otherwise, find the minimum value (the beginning)
	cursor = findMin();

}


/*
  Name: GoToEnd in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Moves the cursor to the end (maximum value) of the BST
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BST::GoToEnd() {

	// Return if tree is empty
	if (Empty()) {

		cursor = NULL;
		return;

	}

	// Otherwise, find the maximum value (the end)
	cursor = findMax();

}


/*
  Name: GoToNext in scope of BST 
  Writer(s): Michael
  Purpose: Moves the cursor to the next value in the BST.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BST::GoToNext() {

	// No tree, so return
	if (Empty())
		return;

	// Only root, so return
	else if ((root->left == NULL) && (root->right == NULL)) 
		return;

	// If at last value in tree, just go to the beginning
	else if (cursor == findMax())
		GoToBeginning();

	// Let's look for the next Node and move the cursor to it
	else {

		bool flag = false;
		int cursorData = AtCursor()->GetData();
		BNode * n = root;

		// Just search the left sub-tree
		if (cursorData < root->GetData())
			n = n->left;

		// Just search the right sub-tree
		if (cursorData > root->GetData())
			n = n->right;

		// Recursively move the cursor
		GoToNextHelp(cursorData, n, flag);

	}

}


/*
  Name: GoToPrev in scope of BST 
  Writer(s): Michael
  Purpose: Moves the cursor to the previous BNode in the BST
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BST::GoToPrev() {

	// No tree, so return
	if (Empty())
		return;

	// Only root, so return
	else if ((root->left == NULL) && (root->right == NULL)) 
		return;

	// If at first value in tree, just go to the end
	else if (cursor == findMin())
		GoToEnd();

	// Let's look for the previous Node and move the cursor to it
	else {

		bool flag = false;
		int cursorData = AtCursor()->GetData();
		BNode * n = root;

		// Just search the left sub-tree
		if (cursorData < root->GetData())
			n = n->left;

		// Just search the right sub-tree
		if (cursorData > root->GetData())
			n = n->right;

		// Recursively move the cursor
		GoToPrevHelp(cursorData, n, flag);

	}

}


/*
  Name: GoToRoot in scope of BST 
  Writer(s): Michael
  Purpose: Moves the cursor to root of the BST.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BST::GoToRoot() {

	cursor = root;

}


/*
  Name: ClearList in scope of BST 
  Writer(s): Michael
  Purpose: Deletes each BNode in the BST and resets the BST's basic data members.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void BST::ClearList() {

	// Do not delete if empty tree
	if (Empty())
		return;

	// Recursively delete each Node
	ClearHelp(root);

	root = NULL;
	cursor = root;

}


/*
  Name: CopyList in scope of BST 
  Writer(s): Michael and Andrew
  Purpose: Copies the data from a source BST into this BST
  Incoming: b is the source BST
  Outgoing: b is the source BST
  Return: N/A (void)
*/
void BST::CopyList(const BST & b) {

	// Need the address of the cursor in the source BST
	BNode * srcCursor = b.AtCursor();

	// Recursively copy the BST
	CopyHelp(b.root, root, srcCursor);

}


/*
  Name: Empty in scope of BST 
  Writer(s): Drew, Michael, Andrew, and Elizabeth
  Purpose: Indicates if the BST is empty or not.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns true is there is no root, otherwise returns false
*/
bool BST::Empty() const {

	// No root means empty list
	if (root == NULL)
		return true;
	
	return false;

}


/*
  Name: findMin in scope of BST 
  Writer(s): Michael
  Purpose: Finds the minimum value in the BST.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the address of the minimum value in the BST
*/
BNode * BST::findMin() const  {

	BNode * n = root;

	// Start at the root and go all the way to the left
	while (n->left != NULL)
		n = n->left;

	return n;

}


/*
  Name: findMax in scope of BST 
  Writer(s): Michael
  Purpose: Finds the maximum value in the BST.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the address of the maximum value in the BST
*/
BNode * BST::findMax() const  {

	BNode * n = root;

	// Start at the root and go all the way to the right
	while (n->right != NULL)
		n = n->right;

	return n;

}


/*
  Name: findParent in scope of BST
  Writer(s): Drew
  Purpose: Finds parent node of entered data.
  Incoming: e is the integer whose parent is to be found
  Outgoing: N/A
  Return: Returns the address of the parent BNode
*/
BNode * BST::findParent(int e) const {

	BNode * current = root;   //current is what we traverse the tree with
	BNode * parent = current; //parent follows behind the current pointer

	while (current != NULL) {

		parent = current;              //every iteration will catch the parent node up with current

		if (e < current ->data)        //if the entered data is less than the current node's data
			current = current->left;   // we move the current to the left

		else if (e > current->data)    // if more
			current = current->right;  //we move right
		
		if (e == current->data)        //if it was on the right spot, or moved to the right spot, return the spot it was just at.
			return parent;             //exits the loop when data is found

	}

	return NULL;                       //if loop failed, return null

}