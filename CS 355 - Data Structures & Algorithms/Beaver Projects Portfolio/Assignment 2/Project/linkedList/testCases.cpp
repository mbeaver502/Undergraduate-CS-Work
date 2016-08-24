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
#include "testCases.h"

using namespace std;



/*
  Name: testCase1                                                  
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase1(LinkedList& list) {	

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;
	
	// Insert Back
	if (list.insertBack(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase2                                                  
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase2(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;
	
	// Insert Front
	if (list.insertFront(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase3                                               
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase3(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;
	
	// Insert Back
	if (list.insertBack(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase4                                                 
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase4(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;
	
	// Insert Front
	if (list.insertFront(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase5                                                
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase5(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;
	
	// Insert Back
	if (list.insertBack(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase6                                                 
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase6(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;
	
	// Insert Front
	if (list.insertFront(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase7                                             
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase7(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;
	
	// Insert Back
	if (list.insertBack(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase8                                                
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int) and 
		   insertBack(int) methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase8(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;
	
	// Insert Front
	if (list.insertFront(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase9                                                
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), removeBack(), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase9(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;
	
	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase10                                            
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), removeBack(), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase10(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;
	
	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase11                                                
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), removeBack(), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase11(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;
	
	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase12                                            
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), removeBack(), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase12(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase13                                                
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), removeBack(), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase13(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;

	// Insert Front
	if (list.insertFront(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Front!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase14                                             
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), removeBack(), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase14(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;

	// Insert Back
	if (list.insertBack(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Back!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase15                                         
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), and removeBack() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase15(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Front
	if (list.insertFront(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Front!" << endl;

	// Insert Front
	if (list.insertFront(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Front!" << endl;

	// Insert Front
	if (list.insertFront(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Front!" << endl;

	// Insert Front
	if (list.insertFront(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Front!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	// Remove Back
	if (list.removeBack())
		cout << list << endl;
	else
		cout << "Failed to remove Back!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: testCase16                                              
  Purpose: Demonstrates the usage and functionality of the LinkedList class's insertFront(int), 
		   insertBack(int), and removeFront() methods.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void testCase16(LinkedList& list) {

	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert Back
	if (list.insertBack(13))
		cout << list << endl;
	else
		cout << "Failed to insert 13 at Back!" << endl;

	// Insert Back
	if (list.insertBack(20))
		cout << list << endl;
	else
		cout << "Failed to insert 20 at Back!" << endl;

	// Insert Back
	if (list.insertBack(18))
		cout << list << endl;
	else
		cout << "Failed to insert 18 at Back!" << endl;

	// Insert Back
	if (list.insertBack(40))
		cout << list << endl;
	else
		cout << "Failed to insert 40 at Back!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	// Remove Front
	if (list.removeFront())
		cout << list << endl;
	else
		cout << "Failed to remove Front!" << endl;

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}



/*
  Name: spreeTestCase                                             
  Purpose: Demonstrates the LinkedList class's insertFront(int), insertBack(int), removeBack(), and removeFront()
		   methods by inserting and removing relatively large sequential integer data into and from the list.
  Incoming: list is a LinkedList passed-by-reference.  It is the list that is to be manipulated and printed.
  Outgoing: list is a LinkedList passed-by-reference.  It is modified and printed.
  Return: N/A (void)
*/
void spreeTestCase(LinkedList& list) {

	int limit;  // The range of the data -- list has range [0, limit)

	cout << "Spree limit (e.g., 12): ";
	cin >> limit;


	cout << endl << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert the data at the back of the list
	cout << "Inserting at back . . . " << endl;
	cout << list << endl;
	for (int i = 0; i < limit; i++) {

		list.insertBack(i);
		cout << list << endl;

	}


	// Remove the data from the back of the list
	cout << endl << "Removing at back . . . " << endl;
	cout << list << endl;
	for (int i = 0; i < limit; i++) {

		list.removeBack();
		cout << list << endl;

	}

	cout << endl << "Final list: " << endl;
	cout << list << endl;

	cout << endl;
	system("PAUSE");
	system("CLS");


	cout << "Pre-Test list: " << endl;
	cout << list << endl << endl;


	cout << "Testing: " << endl;

	// Insert data at the front of the list
	cout << "Inserting at front . . . " << endl;
	cout << list << endl;
	for (int i = 0; i < limit; i++) {

		list.insertFront(i);
		cout << list << endl;

	}


	// Remove data from the front of the list
	cout << endl << "Removing at front . . . " << endl;
	cout << list << endl;
	for (int i = 0; i < limit; i++) {

		list.removeFront();
		cout << list << endl;

	}

	cout << endl << "Final list: " << endl;
	cout << list << endl;

}