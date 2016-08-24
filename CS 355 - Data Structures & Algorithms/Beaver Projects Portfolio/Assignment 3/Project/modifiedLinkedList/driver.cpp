/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 3
	Description: This program implements and demonstrates the usage of a templated version of a singly
				 LinkedList class.  The program uses four generic tests on three different data types:
				 int, double, and string.
    Due Date: September 11, 2012
*/



#include <iostream>
#include <string>
#include "LL.h"

using namespace std;



//----------------------------------------------------------------------------------------
// FUNCTION SIGNATURES
//----------------------------------------------------------------------------------------

// Driver menu handlers
int menu();
void drawMenu();
void selectionHandler(int selection, LL<int>& intList, LL<double>& doubleList, LL<string>& stringList);

// Test cases
void intCase1(LL<int>& list);
void intCase2(LL<int>& list);
void intCase3(LL<int>& list);
void intCase4(LL<int>& list);

void doubleCase1(LL<double>& list);
void doubleCase2(LL<double>& list);
void doubleCase3(LL<double>& list);
void doubleCase4(LL<double>& list);

void stringCase1(LL<string>& list);
void stringCase2(LL<string>& list);
void stringCase3(LL<string>& list);
void stringCase4(LL<string>& list);



//----------------------------------------------------------------------------------------
// CONSTANTS
//----------------------------------------------------------------------------------------

const string MENU_STARS = "*********************************************";
const string BLANK_STARS = "*                                           *";



//----------------------------------------------------------------------------------------
// FUNCTION DEFINITIONS
//----------------------------------------------------------------------------------------

int main() {

	LL<int> intList;
	LL<double> doubleList;
	LL<string> stringList;

	int menuSelection = 0;

	// Loop until the user chooses to quit the program
	do {

		system("CLS");

		menuSelection = menu();
		selectionHandler(menuSelection, intList, doubleList, stringList);  // Executes test cases

		// Clear the LLs between test cases
		intList.clearList();  
		doubleList.clearList();
		stringList.clearList();

		cout << endl;
		system("PAUSE");

	} while (menuSelection != 0);  // 0 = Exit


	return 0;

}



/*
  Name: menu                                                         
  Purpose: Displays a menu for the user to make a selection from.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the user's menu selection as an integer
*/
int menu() {

	int selection;

	// Displays the menu on screen
	drawMenu();

	cout << "Enter your selection: ";
	cin >> selection;

	return selection;

}



/*
  Name: drawMenu                                                        
  Purpose: Draws the actual menu on the screen. It is accompanied with a notice that this program uses
		   random data as well as hardcoded data.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void drawMenu() {

	// See CONSTANTS for MENU_STARS and BLANK_STARS
	cout << MENU_STARS << endl;
	cout << "*        Linked List Class Test Menu        *" << endl;
	cout << MENU_STARS << endl;
	cout << BLANK_STARS << endl;

	cout << "* (01) int Test 01    | (02) int Test 02    *" << endl;
	cout << "* (03) int Test 03    | (04) int Test 04    *" << endl;
	cout << BLANK_STARS << endl;
	cout << "* (05) double Test 01 | (06) double Test 02 *" << endl;
	cout << "* (07) double Test 03 | (08) double Test 04 *" << endl;
	cout << BLANK_STARS << endl;
	cout << "* (09) string Test 01 | (10) string Test 02 *" << endl;
	cout << "* (11) string Test 03 | (12) string Test 04 *" << endl;
	cout << BLANK_STARS << endl;
	cout << "*                 (00) Exit                 *" << endl;

	cout << BLANK_STARS << endl;
	cout <<  MENU_STARS << endl << endl;

}



/*
  Name: selectionHandler                                                       
  Purpose: Executes a test case function based on the user's menu selection
  Incoming: selection is the integer result from menu() -- the user's menu selection; intList, doubleList,
			and stringList are all template LinkedLists
  Outgoing: intList, doubleList, and stringList are the template LinkedLists that will be tested
  Return: N/A (void)
*/
void selectionHandler(int selection, LL<int>& intList, LL<double>& doubleList, LL<string>& stringList) {

	// Choose which action to execute based on the user's choice
	switch (selection) {

		case 1: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*                 int Test 1                *" << endl;     
			cout << MENU_STARS << endl << endl;

			intCase1(intList);
			break;

		}

		case 2: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*                 int Test 2                *" << endl;     
			cout << MENU_STARS << endl << endl;

			intCase2(intList);
			break;

		}

		case 3: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*                 int Test 3                *" << endl;     
			cout << MENU_STARS << endl << endl;

			intCase3(intList);
			break;

		}

		case 4: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*                 int Test 4                *" << endl;     
			cout << MENU_STARS << endl << endl;

			intCase4(intList);	
			break;

		}

		case 5: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               double Test 1               *" << endl;     
			cout << MENU_STARS << endl << endl;

			doubleCase1(doubleList);	
			break;

		}

		case 6: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               double Test 2               *" << endl;     
			cout << MENU_STARS << endl << endl;

			doubleCase2(doubleList);	
			break;

		}

		case 7: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               double Test 3               *" << endl;     
			cout << MENU_STARS << endl << endl;

			doubleCase3(doubleList);	
			break;

		}

		case 8: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               double Test 4               *" << endl;     
			cout << MENU_STARS << endl << endl;

			doubleCase4(doubleList);	
			break;

		}

		case 9: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               string Test 1               *" << endl;     
			cout << MENU_STARS << endl << endl;

			stringCase1(stringList);	
			break;

		}

		case 10: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               string Test 2               *" << endl;     
			cout << MENU_STARS << endl << endl;

			stringCase2(stringList);
			break;

		}

		case 11: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               string Test 3               *" << endl;     
			cout << MENU_STARS << endl << endl;

			stringCase3(stringList);	
			break;

		}

		case 12: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*               string Test 4               *" << endl;     
			cout << MENU_STARS << endl << endl;

			stringCase4(stringList);	
			break;

		}

		default: 
			break; 

	}

}



/*
  Name: intCase1                                                 
  Purpose: This test case tests the insertion and search methods of an int LinkedList.
  Incoming: intList is the templated int LinkedList to be edited and printed
  Outgoing: intList is the templated int LinkedList to be edited and printed
  Return: N/A (void)
*/
void intCase1(LL<int>& intList) {	

	// Testing the insertion method
	cout << "Inserting 05 . . . ";
	if (intList.insert(5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 10 . . . ";
	if (intList.insert(10))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 17 . . . ";
	if (intList.insert(17))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 20 . . . ";
	if (intList.insert(20))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 03 . . . ";
	if (intList.insert(3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 15 . . . ";
	if (intList.insert(15))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing the search method
	cout << endl;
	cout << "Searching for 05 . . . " << intList.search(5) << endl;
	cout << "Searching for 10 . . . " << intList.search(10) << endl;
	cout << "Searching for 17 . . . " << intList.search(17) << endl;
	cout << "Searching for 20 . . . " << intList.search(20) << endl;
	cout << "Searching for 03 . . . " << intList.search(3) << endl;
	cout << "Searching for 15 . . . " << intList.search(15) << endl;


	cout << endl << "Final list: " << endl;
	cout << intList << endl;
	
}



/*
  Name: intCase2                                                 
  Purpose: This test case tests the insertion, search, and removal methods of the LinkedList class. Also,
		   this test case tests the assignment operator.
  Incoming: intList is the templated int LinkedList to be edited and printed
  Outgoing: intList is the templated int LinkedList to be edited and printed
  Return: N/A (void)
*/
void intCase2(LL<int>& intList) {	

	LL<int> intList2;

	cout << " intList: " << intList << endl;
	cout << "intList2: " << intList2 << endl << endl;


	// Testing insertion method
	cout << "Inserting 05 into intList . . . ";
	if (intList.insert(5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 03 into intList . . . ";
	if (intList.insert(3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 17 into intList . . . ";
	if (intList.insert(17))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 15 into intList . . . ";
	if (intList.insert(15))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing assignment operator
	intList2 = intList;

	cout << endl << " intList: " << intList << endl;
	cout << "intList2: " << intList2 << endl << endl;


	// Demonstrating that two unique lists have been formed
	cout << "Searching for 15 in  intList . . . " << intList.search(15) << endl;
	cout << "Searching for 15 in intList2 . . . " << intList2.search(15) << endl;
	cout << "Searching for 03 in  intList . . . " << intList.search(3) << endl;
	cout << "Searching for 03 in intList2 . . . " << intList2.search(3) << endl << endl;

	cout << "Removing 03 from  intList . . . ";
	if (intList.remove(3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Removing 15 from intList2 . . . ";
	if (intList2.remove(15))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << endl << " intList: " << intList << endl;
	cout << "intList2: " << intList2 << endl;
	
}



/*
  Name: intCase3                                                 
  Purpose: This test case tests the insertion, search, and removal methods of the LinkedList class. The test
		   case also tests the copy constructor.
  Incoming: intList is the templated int LinkedList to be edited and printed
  Outgoing: intList is the templated int LinkedList to be edited and printed
  Return: N/A (void)
*/
void intCase3(LL<int>& intList) {	

	cout << "intList: " << intList << endl << endl;

	// Testing insertion method
	cout << "Inserting 05 into intList . . . ";
	if (intList.insert(5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 03 into intList . . . ";
	if (intList.insert(3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 17 into intList . . . ";
	if (intList.insert(17))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 15 into intList . . . ";
	if (intList.insert(15))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing copy constructor
	LL<int> intList2 = intList;

	cout << endl << " intList: " << intList << endl;
	cout << "intList2: " << intList2 << endl << endl;

	// Demonstrating that there are two unique LinkedLists
	cout << "Searching for 15 in  intList . . . " << intList.search(15) << endl;
	cout << "Searching for 15 in intList2 . . . " << intList2.search(15) << endl;
	cout << "Searching for 03 in  intList . . . " << intList.search(3) << endl;
	cout << "Searching for 03 in intList2 . . . " << intList2.search(3) << endl << endl;

	cout << "Removing 03 from  intList . . . ";
	if (intList.remove(3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Removing 15 from intList2 . . . ";
	if (intList2.remove(15))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << endl << " intList: " << intList << endl;
	cout << "intList2: " << intList2 << endl;
	
}



/*
  Name: intCase4                                                
  Purpose: This test cases tests the insertion method and cursor methods of the LinkedList class.
  Incoming: intList is the templated int LinkedList to be edited and printed
  Outgoing: intList is the templated int LinkedList to be edited and printed
  Return: N/A (void)
*/
void intCase4(LL<int>& intList) {	

	cout << "intList: " << intList << endl << endl;

	// Testing insertion method
	cout << "Inserting 05 into intList . . . ";
	if (intList.insert(5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 03 into intList . . . ";
	if (intList.insert(3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 17 into intList . . . ";
	if (intList.insert(17))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 15 into intList . . . ";
	if (intList.insert(15))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	cout << endl << "intList: " << intList << endl << endl;


	// Testing cursor methods
	cout << "Moving cursor to 1 . . . ";
	if (intList.placeCursor(1)) {

		cout << "SUCCESS!" << endl;
		cout << "intList[1]: " << intList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 3 . . . ";
	if (intList.placeCursor(3)) {

		cout << "SUCCESS!" << endl;
		cout << "intList[3]: " << intList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 0 . . . ";
	if (intList.placeCursor(0)) {

		cout << "SUCCESS!" << endl;
		cout << "intList[0]: " << intList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 2 . . . ";
	if (intList.placeCursor(2)) {

		cout << "SUCCESS!" << endl;
		cout << "intList[2]: " << intList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 5 . . . ";
	if (intList.placeCursor(5)) {

		cout << "SUCCESS!" << endl;
		cout << "intList[5]: " << intList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	
}



/*
  Name: doubleCase1                                                 
  Purpose: This test case tests the insertion and search methods of a double LinkedList.
  Incoming: doubleList is the templated double LinkedList to be edited and printed
  Outgoing: doubleList is the templated double LinkedList to be edited and printed
  Return: N/A (void)
*/
void doubleCase1(LL<double>& doubleList) {	

	// Testing insertion method
	cout << "Inserting   3.5 . . . ";
	if (doubleList.insert(3.5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting   1.0 . . . ";
	if (doubleList.insert(1.0))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting   7.3 . . . ";
	if (doubleList.insert(7.3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting   5.2 . . . ";
	if (doubleList.insert(5.2))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 10.25 . . . ";
	if (doubleList.insert(10.25))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 0.125 . . . ";
	if (doubleList.insert(0.125))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing search method
	cout << endl;
	cout << "Searching for   3.5 . . . " << doubleList.search(3.5) << endl;
	cout << "Searching for   1.0 . . . " << doubleList.search(1.0) << endl;
	cout << "Searching for   7.3 . . . " << doubleList.search(7.3) << endl;
	cout << "Searching for   5.2 . . . " << doubleList.search(5.2) << endl;
	cout << "Searching for 10.25 . . . " << doubleList.search(10.25) << endl;
	cout << "Searching for 0.125 . . . " << doubleList.search(0.125) << endl;


	cout << endl << "Final list: " << endl;
	cout << doubleList << endl;
	
}



/*
  Name: doubleCase2                                                 
  Purpose: This test case tests the insertion, search, and removal methods of the LinkedList class. Also,
		   this test case tests the assignment operator.
  Incoming: doubleList is the templated double LinkedList to be edited and printed
  Outgoing: doubleList is the templated double LinkedList to be edited and printed
  Return: N/A (void)
*/
void doubleCase2(LL<double>& doubleList) {	

	LL<double> doubleList2;

	cout << " doubleList: " << doubleList << endl;
	cout << "doubleList2: " << doubleList2 << endl << endl;


	// Testing insertion method
	cout << "Inserting 3.5 into doubleList . . . ";
	if (doubleList.insert(3.5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 1.3 into doubleList . . . ";
	if (doubleList.insert(1.3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 7.2 into doubleList . . . ";
	if (doubleList.insert(7.2))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 5.4 into doubleList . . . ";
	if (doubleList.insert(5.4))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing assignment operator
	doubleList2 = doubleList;

	cout << endl << " doubleList: " << doubleList << endl;
	cout << "doubleList2: " << doubleList2 << endl << endl;


	// Demonstrating that there are two unique LinkedLists
	cout << "Searching for 5.4 in  doubleList . . . " << doubleList.search(5.4) << endl;
	cout << "Searching for 5.4 in doubleList2 . . . " << doubleList2.search(5.4) << endl;
	cout << "Searching for 1.3 in  doubleList . . . " << doubleList.search(1.3) << endl;
	cout << "Searching for 1.3 in doubleList2 . . . " << doubleList2.search(1.3) << endl << endl;

	cout << "Removing 5.4 from  doubleList . . . ";
	if (doubleList.remove(5.4))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Removing 1.3 from doubleList2 . . . ";
	if (doubleList2.remove(1.3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << endl << " doubleList: " << doubleList << endl;
	cout << "doubleList2: " << doubleList2 << endl;
	
}



/*
  Name: doubleCase3                                                 
  Purpose: This test case tests the insertion, search, and removal methods of the LinkedList class. The test
		   case also tests the copy constructor.
  Incoming: doubleList is the templated double LinkedList to be edited and printed
  Outgoing: doubleList is the templated double LinkedList to be edited and printed
  Return: N/A (void)
*/
void doubleCase3(LL<double>& doubleList) {	

	cout << "doubleList: " << doubleList << endl << endl;


	// Testing insertion method
	cout << "Inserting 1.2 into doubleList . . . ";
	if (doubleList.insert(1.2))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 3.5 into doubleList . . . ";
	if (doubleList.insert(3.5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 1.1 into doubleList . . . ";
	if (doubleList.insert(1.1))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 2.3 into doubleList . . . ";
	if (doubleList.insert(2.3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing copy constructor
	LL<double> doubleList2 = doubleList;

	cout << endl << " doubleList: " << doubleList << endl;
	cout << "doubleList2: " << doubleList2 << endl << endl;


	// Demonstrating that there are two unique LinkedLists
	cout << "Searching for 2.3 in  doubleList . . . " << doubleList.search(2.3) << endl;
	cout << "Searching for 2.3 in doubleList2 . . . " << doubleList2.search(2.3) << endl;
	cout << "Searching for 1.1 in  doubleList . . . " << doubleList.search(1.1) << endl;
	cout << "Searching for 1.1 in doubleList2 . . . " << doubleList2.search(1.1) << endl << endl;

	cout << "Removing 2.3 from  doubleList . . . ";
	if (doubleList.remove(2.3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Removing 1.1 from doubleList2 . . . ";
	if (doubleList2.remove(1.1))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << endl << " doubleList: " << doubleList << endl;
	cout << "doubleList2: " << doubleList2 << endl;
	
}



/*
  Name: doubleCase4                                                
  Purpose: This test cases tests the insertion method and cursor methods of the LinkedList class.
  Incoming: doubleList is the templated double LinkedList to be edited and printed
  Outgoing: doubleList is the templated double LinkedList to be edited and printed
  Return: N/A (void)
*/
void doubleCase4(LL<double>& doubleList) {	

	cout << "doubleList: " << doubleList << endl << endl;

	// Testing insertion method
	cout << "Inserting 0.5 into doubleList . . . ";
	if (doubleList.insert(0.5))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 0.3 into doubleList . . . ";
	if (doubleList.insert(0.3))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 3.4 into doubleList . . . ";
	if (doubleList.insert(3.4))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting 2.1 into doubleList . . . ";
	if (doubleList.insert(2.1))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	cout << endl << "doubleList: " << doubleList << endl << endl;


	// Testing cursor methods
	cout << "Moving cursor to 1 . . . ";
	if (doubleList.placeCursor(1)) {

		cout << "SUCCESS!" << endl;
		cout << "doubleList[1]: " << doubleList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 3 . . . ";
	if (doubleList.placeCursor(3)) {

		cout << "SUCCESS!" << endl;
		cout << "doubleList[3]: " << doubleList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 0 . . . ";
	if (doubleList.placeCursor(0)) {

		cout << "SUCCESS!" << endl;
		cout << "doubleList[0]: " << doubleList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 2 . . . ";
	if (doubleList.placeCursor(2)) {

		cout << "SUCCESS!" << endl;
		cout << "doubleList[2]: " << doubleList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 5 . . . ";
	if (doubleList.placeCursor(5)) {

		cout << "SUCCESS!" << endl;
		cout << "doubleList[5]: " << doubleList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	
}



/*
  Name: stringCase1                                                 
  Purpose: This test case tests the insertion and search methods of a string LinkedList.
  Incoming: stringList is the templated string LinkedList to be edited and printed
  Outgoing: stringList is the templated string LinkedList to be edited and printed
  Return: N/A (void)
*/
void stringCase1(LL<string>& stringList) {	

	// Testing insertion method
	cout << "Inserting   a . . . ";
	if (stringList.insert("a"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting   B . . . ";
	if (stringList.insert("B"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting  cD . . . ";
	if (stringList.insert("cD"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting  7a . . . ";
	if (stringList.insert("7a"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting abc . . . ";
	if (stringList.insert("abc"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting ABC . . . ";
	if (stringList.insert("ABC"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing search method
	cout << endl;
	cout << "Searching for   a . . . " << stringList.search("a") << endl;
	cout << "Searching for   B . . . " << stringList.search("B") << endl;
	cout << "Searching for  cD . . . " << stringList.search("cD") << endl;
	cout << "Searching for  7a . . . " << stringList.search("7a") << endl;
	cout << "Searching for abc . . . " << stringList.search("abc") << endl;
	cout << "Searching for ABC . . . " << stringList.search("ABC") << endl;


	cout << endl << "Final list: " << endl;
	cout << stringList << endl;
	
}



/*
  Name: stringCase2                                                 
  Purpose: This test case tests the insertion, search, and removal methods of the LinkedList class. Also,
		   this test case tests the assignment operator.
  Incoming: stringList is the templated string LinkedList to be edited and printed
  Outgoing: stringList is the templated string LinkedList to be edited and printed
  Return: N/A (void)
*/
void stringCase2(LL<string>& stringList) {	

	LL<string> stringList2;

	cout << " stringList: " << stringList << endl;
	cout << "stringList2: " << stringList2 << endl << endl;


	// Testing insertion method
	cout << "Inserting  a into stringList . . . ";
	if (stringList.insert("a"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting  B into stringList . . . ";
	if (stringList.insert("B"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting  f into stringList . . . ";
	if (stringList.insert("f"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting CD into stringList . . . ";
	if (stringList.insert("CD"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing assignment operator
	stringList2 = stringList;

	cout << endl << " stringList: " << stringList << endl;
	cout << "stringList2: " << stringList2 << endl << endl;


	// Demonstrating that there are two unique LinkedLists
	cout << "Searching for  a in  stringList . . . " << stringList.search("a") << endl;
	cout << "Searching for  a in stringList2 . . . " << stringList2.search("a") << endl;
	cout << "Searching for CD in  stringList . . . " << stringList.search("CD") << endl;
	cout << "Searching for CD in stringList2 . . . " << stringList2.search("CD") << endl << endl;

	cout << "Removing  a from  stringList . . . ";
	if (stringList.remove("a"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Removing CD from stringList2 . . . ";
	if (stringList2.remove("CD"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << endl << " stringList: " << stringList << endl;
	cout << "stringList2: " << stringList2 << endl;
	
}



/*
  Name: stringCase3                                                 
  Purpose: This test case tests the insertion, search, and removal methods of the LinkedList class. The test
		   case also tests the copy constructor.
  Incoming: stringList is the templated string LinkedList to be edited and printed
  Outgoing: stringList is the templated string LinkedList to be edited and printed
  Return: N/A (void)
*/
void stringCase3(LL<string>& stringList) {	

	cout << "stringList: " << stringList << endl << endl;


	// Testing insertion method
	cout << "Inserting  a into stringList . . . ";
	if (stringList.insert("a"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting  B into stringList . . . ";
	if (stringList.insert("B"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting CD into stringList . . . ";
	if (stringList.insert("CD"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting  f into stringList . . . ";
	if (stringList.insert("f"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	// Testing copy constructor
	LL<string> stringList2 = stringList;

	cout << endl << " stringList: " << stringList << endl;
	cout << "stringList2: " << stringList2 << endl << endl;


	// Demonstrating that there are two unique LinkedLists
	cout << "Searching for  B in  stringList . . . " << stringList.search("B") << endl;
	cout << "Searching for  B in stringList2 . . . " << stringList2.search("B") << endl;
	cout << "Searching for CD in  stringList . . . " << stringList.search("CD") << endl;
	cout << "Searching for CD in stringList2 . . . " << stringList2.search("CD") << endl << endl;

	cout << "Removing  B from  stringList . . . ";
	if (stringList.remove("B"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Removing CD from stringList2 . . . ";
	if (stringList2.remove("CD"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << endl << " stringList: " << stringList << endl;
	cout << "stringList2: " << stringList2 << endl;
	
}



/*
  Name: stringCase4                                                
  Purpose: This test cases tests the insertion method and cursor methods of the LinkedList class.
  Incoming: stringList is the templated string LinkedList to be edited and printed
  Outgoing: stringList is the templated string LinkedList to be edited and printed
  Return: N/A (void)
*/
void stringCase4(LL<string>& stringList) {	

	cout << "stringList: " << stringList << endl << endl;


	// Testing insertion method
	cout << "Inserting a into stringList . . . ";
	if (stringList.insert("a"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting c into stringList . . . ";
	if (stringList.insert("c"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting B into stringList . . . ";
	if (stringList.insert("B"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;

	cout << "Inserting D into stringList . . . ";
	if (stringList.insert("D"))
		cout << "SUCCESS!" << endl;
	else
		cout << "FAILURE!" << endl;


	cout << endl << "stringList: " << stringList << endl << endl;


	// Testing cursor methods
	cout << "Moving cursor to 1 . . . ";
	if (stringList.placeCursor(1)) {

		cout << "SUCCESS!" << endl;
		cout << "stringList[1]: " << stringList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 3 . . . ";
	if (stringList.placeCursor(3)) {

		cout << "SUCCESS!" << endl;
		cout << "stringList[3]: " << stringList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 0 . . . ";
	if (stringList.placeCursor(0)) {

		cout << "SUCCESS!" << endl;
		cout << "stringList[0]: " << stringList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 2 . . . ";
	if (stringList.placeCursor(2)) {

		cout << "SUCCESS!" << endl;
		cout << "stringList[2]: " << stringList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	

	cout << "Moving cursor to 5 . . . ";
	if (stringList.placeCursor(5)) {

		cout << "SUCCESS!" << endl;
		cout << "stringList[5]: " << stringList.getCursorVal() << endl << endl;

	}
	else
		cout << "FAILURE!" << endl;
	
}

