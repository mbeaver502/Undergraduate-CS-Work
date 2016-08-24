/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 3
	Description: This program implements and demonstrates the usage of a templated version of a singly
				 LinkedList class.  The program uses multiple test cases per member method in the 
				 LinkedList class.
    Due Date: September 11, 2012
*/



#include "LL.h"
#include <iostream>
using namespace std;


//--------------------------------------------------------------------------------------------
// FUNCTION SIGNATURES
//--------------------------------------------------------------------------------------------

void PrintMenu();
void TestMenu();
void TestExec(char testSel);

// Test Cases
void insertTest1();
void insertTest2();

void removeTest1();
void removeTest2();

void searchTest1();
void searchTest2();

void printTest1();
void printTest2();

void cursorTest1();
void cursorTest2();
void cursorTest3();
void cursorTest4();

void clearTest1();
void clearTest2();

void emptyTest1();
void emptyTest2();

void fullTest1();
void fullTest2();

void assignmentTest1();
void assignmentTest2();

void copyTest1();
void copyTest2(); 



//--------------------------------------------------------------------------------------------
// FUNCTION DEFINITIONS
//--------------------------------------------------------------------------------------------

int main(){
	LL<int> testList;
	LL<int> testAssign;
	int data;
	char choice, testSel;

	PrintMenu();
	cout << "-->";
	cin >> choice;

	while(choice != 'q' && choice != 'Q') {
		
		if (choice == '!') {

			LL<int> testCopy(testList);

			cout << "Result:" << "Print New Copy" << endl;
			testCopy.Print(); cout << endl;

			testCopy.Insert(-10000);

			cout << "Result: " << "Print Modified Copy" << endl;
			testCopy.Print(); cout << endl;

			cout << "Result: " << "Print Original Test List" << endl;
			testList.Print(); cout << endl;

		}

		else {

			switch (choice) {

				// Display commands
				case 'h':

				case 'H': 
					PrintMenu();
					break;


				// Input data
				case '+':
					cin >> data;
					testList.Insert(data);
					break;	


				// Remove data
				case '-':
					cin >> data;
					testList.Remove(data);
					break;
				

				// Display data at cursor
				case '@':
					cout <<"Result:" <<  testList.AtCursor() << endl;
					break;


				// Move cursor to beginning
				case '<':
					testList.GoToBeginning();
					break;


				// Move cursor to end
				case '>':
					testList.GoToEnd();
					break;


				// Move cursor to next Node
				case 'N':
					testList.GoToNext();
					break;


				// Move cursor to previous Node
				case 'P':
					testList.GoToPrev();
					break;


				// Clear the LinkedList
				case 'C':
					//testList.~LL();
					testList.ClearList();
					break;
					

				// Check if LinkedList is empty
				case 'E':
					if (testList.Empty())
						cout <<"Result:" <<  "List Is Empty" << endl;
					else
						cout <<"Result:" <<  "List is Not Empty" << endl;
					break;


				// Check if LinkedList is full
				case 'F':
					if (testList.Full())
						cout <<"Result:" <<  "List Is Full" << endl;
					else
						cout <<"Result:" <<  "List Is Not Full" << endl;
					break;



				// Test assignment operator
				case '#':

					//assign list
					testAssign = testList;

					testAssign.Insert(-100000);
					cout << "Modify New List" << endl;
					testAssign.Print(); cout << endl;

					cout << "Old List should not be affected" << endl;
					testList.Print(); cout << endl;

					testAssign.ClearList();
					cout << "Destroy New List" << endl;

					cout << "Old List should not be affected" << endl;
					testList.Print(); cout << endl;
					break;


				// Search for data in LinkedList
				case '?':
					cin >> data;
					if (testList.Search(data) != NULL)
						cout << "Result:" << data << "\tfound" << endl;
					else
						cout << "Result:" << data << "\tnot found" << endl;
					break;

				
				// Show Test Cases
				case 'T':
					TestMenu();
					cout << "--> ";
					cin >> testSel;
					TestExec(testSel);
					break;
			

				// Invalid user choice
				default:
					cout << "INVALID CHOICE, Choose Again" << endl;

			}

		}

		testList.Print(); 
		cout << endl;

		system("PAUSE");

		PrintMenu();
		cout << "-->";
		cin >> choice;

	}

	return 0;
}


/*
  Name: PrintMenu                                                     
  Purpose: Displays a general command menu.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void PrintMenu() {

	system("CLS");

	/*Commands borrowed from Lab*/
	cout << "  Command Line Options" << endl;
	cout << "  H   : Show Command Line Options" << endl;
	cout << "  +x  :Insert x in order" << endl;
	cout << "  -x  : Remove x" << endl << endl;

	cout << "  @   : Display the data item marked by the cursor" << endl;
    cout << "  <   : Go to the beginning of the list" << endl;
    cout << "  >   : Go to the end of the list" << endl;
    cout << "  N   : Go to the next data item" << endl;
    cout << "  P   : Go to the prior data item" << endl << endl;

    cout << "  C   : Clear the list" << endl;
    cout << "  E   : Empty list?" << endl;
    cout << "  F   : Full list?" << endl << endl;

    cout << "  !   : Test copy constructor" << endl;
    cout << "  #   : Test assignment operator, ClearList must work first before testing" << endl;
    cout << "  ?x  : Search rest of list for x, from cursor to end " << endl << endl;

	cout << "  T   : Test Cases Menu" << endl << endl;

    cout << "  Q   : Quit the test program" << endl;
    cout << endl;
	
}


/*
  Name: TestMenu                                                     
  Purpose: Displays a menu of test cases.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void TestMenu() {

	system("CLS");

	cout << "  Test Cases" << endl << endl;

	cout << "  a   : Insertion Test 1        |        m   : Clear Test 1" << endl;
	cout << "  b   : Insertion Test 2        |        n   : Clear Test 2" << endl << endl;

	cout << "  c   : Removal Test 1          |        o   : Empty Test 1" << endl;
	cout << "  d   : Removal Test 2          |        p   : Empty Test 2" << endl << endl;

	cout << "  e   : Search Test 1           |        q   : Full Test 1" << endl;
	cout << "  f   : Search Test 2           |        r   : Full Test 2" << endl << endl;

	cout << "  g   : Print Test 1            |        s   : Assignment Test 1" << endl;
	cout << "  h   : Print Test 2            |        t   : Assignment Test 2" << endl << endl;

	cout << "  i   : Cursor Test 1           |        u   : Copy Constructor Test 1" << endl;
	cout << "  j   : Cursor Test 2           |        v   : Copy Constructor Test 2" << endl;
	cout << "  k   : Cursor Test 3           |" << endl;
	cout << "  l   : Cursor Test 4           |    <other> : Return" << endl << endl;

	/*
	cout << "  m   : Clear Test 1" << endl;
	cout << "  n   : Clear Test 2" << endl << endl;

	cout << "  o   : Empty Test 1" << endl;
	cout << "  p   : Empty Test 2" << endl << endl;

	cout << "  q   : Full Test 1" << endl;
	cout << "  r   : Full Test 2" << endl << endl;

	cout << "  s   : Assignment Test 1" << endl;
	cout << "  t   : Assignment Test 2" << endl << endl;

	cout << "  u   : Copy Constructor Test 1" << endl;
	cout << "  v   : Copy Constructor Test 2" << endl << endl;
	*/
}


/*
  Name: TestExec                                                     
  Purpose: Executes test cases based on user input.
  Incoming: testSel is the user input.
  Outgoing: N/A
  Return: N/A (void)
*/
void TestExec(char testSel) {

	system("CLS");

	switch (testSel) {

		// Insertion Test 1
		case 'a':
			insertTest1();
			break;


		// Insertion Test 2
		case 'b':
			insertTest2();
			break;


		// Removal Test 1
		case 'c':
			removeTest1();
			break;


		// Removal Test 2
		case 'd':
			removeTest2();
			break;


		// Search Test 1
		case 'e':
			searchTest1();
			break;


		// Search Test 2
		case 'f':
			searchTest2();
			break;


		// Print Test 1
		case 'g':
			printTest1();
			break;


		// Print Test 2
		case 'h':
			printTest2();
			break;


		// Cursor Test 1
		case 'i':
			cursorTest1();
			break;


		// Cursor Test 2
		case 'j':
			cursorTest2();
			break;


		// Cursor Test 3
		case 'k':
			cursorTest3();
			break;


		// Cursor Test 4
		case 'l':
			cursorTest4();
			break;


		// Clear Test 1
		case 'm':
			clearTest1();
			break;


		// Clear Test 2
		case 'n':
			clearTest2();
			break;


		// Empty Test 1
		case 'o':
			emptyTest1();
			break;


		// Empty Test 2
		case 'p':
			emptyTest2();
			break;


		// Full Test 1
		case 'q':
			fullTest1();
			break;


		// Full Test 2
		case 'r':
			fullTest2();
			break;


		// Assignment Test 1
		case 's':
			assignmentTest1();
			break;


		// Assignment Test 2
		case 't':
			assignmentTest2();
			break;


		// Copy Constructor Test 1
		case 'u':
			copyTest1();
			break;


		// Copy Constructor Test 2
		case 'v':
			copyTest2();
			break;


		default:
			break;

	}

}


/*
  Name: insertTest1                                                     
  Purpose: Demonstrates the use of the LinkedList's Insert() method.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void insertTest1() {

	LL<int> testList;

	cout << "Before test: " << testList << endl << endl;

	cout << "Inserting 10 . . . ";
	if (testList.Insert(10)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 17 . . . ";
	if (testList.Insert(17)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 5 . . . ";
	if (testList.Insert(5)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 3 . . . ";
	if (testList.Insert(3)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 20 . . . ";
	if (testList.Insert(20)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 21 . . . ";
	if (testList.Insert(21)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 22 . . . ";
	if (testList.Insert(22)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 7 . . . ";
	if (testList.Insert(7)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << endl << "After Test: " << testList << endl << endl;

}


/*
  Name: insertTest2                                                    
  Purpose: Demonstrates the use of the LinkedList's Insert() method.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void insertTest2() {

	LL<int> testList;

	cout << "Before test: " << testList << endl << endl;

	cout << "Inserting 1000 . . . ";
	if (testList.Insert(1000)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting -20 . . . ";
	if (testList.Insert(-20)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 50 . . . ";
	if (testList.Insert(50)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 1 . . . ";
	if (testList.Insert(1)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 0 . . . ";
	if (testList.Insert(0)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting -300 . . . ";
	if (testList.Insert(-300)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 50 . . . ";
	if (testList.Insert(50)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << "Inserting 999 . . . ";
	if (testList.Insert(999)) {

		cout << "SUCCESS!" << endl;
		cout << testList << endl << endl;

	}
	else
		cout << "FAILURE!" << endl << endl;

	cout << endl << "After Test: " << testList << endl << endl;

}


/*
  Name: removeTest1                                                
  Purpose: Demonstrates the use of the LinkedList's Remove(), GoToBeginning, and Insert() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void removeTest1() {

	LL<int> testList;

	cout << "Before test: " << testList << endl << endl;

	cout << "Removing from empty list: " << endl;
	if (testList.Remove(1)) 
		cout << "New list: " << testList << endl;
	else
		cout << "No change: " << testList << endl;

	for (int i = 0; i < 8; i++) {

		if (testList.Insert(i))
			cout << "Inserted " << i << " successfully!" << endl;
		else
			cout << "Failed to insert " << i << endl;

	}

	cout << "List before: " << testList << endl << endl;

	testList.GoToBeginning();

	if (testList.Remove(5))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(0))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(7))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(3))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(10))
		cout << testList << endl;

	cout << "After test: " << testList << endl << endl;

}


/*
  Name: removeTest2                                               
  Purpose: Demonstrates the use of the LinkedList's Remove(), GoToBeginning, and Insert() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void removeTest2() {

	LL<int> testList;

	cout << "Before test: " << testList << endl << endl;

	cout << "Removing from empty list: " << endl;
	if (testList.Remove(1)) 
		cout << "New list: " << testList << endl;
	else
		cout << "No change: " << testList << endl;

	if (testList.Insert(5))
		cout << "Inserted 5 successfully!" << endl;

	if (testList.Insert(4))
		cout << "Inserted 4 successfully!" << endl;

	if (testList.Insert(3))
		cout << "Inserted 3 successfully!" << endl;

	if (testList.Insert(2))
		cout << "Inserted 2 successfully!" << endl;

	if (testList.Insert(-10))
		cout << "Inserted -10 successfully!" << endl;

	testList.GoToBeginning();

	cout << "List before: " << testList << endl << endl;

	if (testList.Remove(4))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(2))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(-10))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(3))
		cout << testList << endl;

	testList.GoToBeginning();

	if (testList.Remove(10))
		cout << testList << endl;

	cout << "After test: " << testList << endl << endl;

}


/*
  Name: searchTest1                                                
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToBeginning(), and Search() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void searchTest1() {

	LL<int> testList;

	cout << "Before test: " << testList << endl;

	if (testList.Insert(10))
		cout << testList << endl;

	if (testList.Insert(12))
		cout << testList << endl;

	if (testList.Insert(14))
		cout << testList << endl;

	if (testList.Insert(8))
		cout << testList << endl;

	if (testList.Insert(6))
		cout << testList << endl;

	testList.GoToBeginning();

	cout << "Before Search: " << testList << endl;

	cout << "Search(12): " << testList.Search(12) << endl;
	cout << "Search(10): " << testList.Search(10) << endl;
	cout << "Search(14): " << testList.Search(14) << endl;
	testList.GoToBeginning();
	cout << "Search(06): " << testList.Search(06) << endl;
	cout << "Search(08): " << testList.Search(8) << endl;

	cout << "After Test: " << testList << endl << endl;

}


/*
  Name: searchTest2                                               
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToBeginning(), and Search() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void searchTest2() {

	LL<int> testList;

	cout << "Before test: " << testList << endl;

	cout << "Searching empty list: " << testList.Search(1) << endl;

	if (testList.Insert(10))
		cout << testList << endl;

	if (testList.Insert(12))
		cout << testList << endl;

	if (testList.Insert(14))
		cout << testList << endl;

	if (testList.Insert(8))
		cout << testList << endl;

	if (testList.Insert(6))
		cout << testList << endl;

	testList.GoToBeginning();

	cout << "Before Search: " << testList << endl;

	cout << "Search(06): " << testList.Search(6) << endl;
	cout << "Search(08): " << testList.Search(8) << endl;
	cout << "Search(10): " << testList.Search(10) << endl;
	cout << "Search(12): " << testList.Search(12) << endl;
	cout << "Search(14): " << testList.Search(14) << endl;
	testList.GoToBeginning();
	cout << "Search(16): " << testList.Search(16) << endl;

	cout << "After Test: " << testList << endl << endl;

}


/*
  Name: printTest1                                                
  Purpose: Demonstrates the use of the LinkedList's Print() and Insert() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void printTest1() {

	LL<int> testList;

	cout << "Before Test: ";
	testList.Print();
	cout << endl;

	for (int i = 0; i < 8; i++) {

		if (testList.Insert(i))
			cout << "Inserted " << i << " successfully!" << endl;
		
	}

	cout << "Printing with Print() method: " << endl;
	testList.Print();
	cout << endl;

}


/*
  Name: printTest2                                                
  Purpose: Demonstrates the use of the LinkedList's Insert() method and overloaded insertion operator.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void printTest2() {

	LL<int> testList;

	cout << "Before Test: " << testList << endl;

	for (int i = 10; i < 16; i++) {

		if (testList.Insert(i))
			cout << "Inserted " << i << " successfully!" << endl;
		
	}

	cout << "Printing with insertion operator: " << testList << endl;

}


/*
  Name: cursorTest1                                                
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToNext(), and GoToPrev() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void cursorTest1() {

	LL<int> testList;

	cout << "Before Test: " << testList << endl;

	if (testList.Insert(10))
		cout << testList << endl;

	if (testList.Insert(5))
		cout << testList << endl;

	if (testList.Insert(12))
		cout << testList << endl;

	if (testList.Insert(3))
		cout << testList << endl;

	testList.GoToNext();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToNext();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

}


/*
  Name: cursorTest2                                               
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToBeginning(), GoToEnd(), 
		   and GoToPrev() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void cursorTest2() {

	LL<int> testList;

	cout << "Before Test: " << testList << endl;

	if (testList.Insert(10))
		cout << testList << endl;

	if (testList.Insert(5))
		cout << testList << endl;

	if (testList.Insert(12))
		cout << testList << endl;

	if (testList.Insert(3))
		cout << testList << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToBeginning();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToEnd();
	cout << "Cursor value: " << testList.AtCursor() << endl;

}


/*
  Name: cursorTest3                                               
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToEnd(), and GoToPrev() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void cursorTest3() {

	LL<int> testList;

	cout << "Before Test: " << testList << endl;

	if (testList.Insert(10))
		cout << testList << endl;

	if (testList.Insert(5))
		cout << testList << endl;

	if (testList.Insert(12))
		cout << testList << endl;

	if (testList.Insert(3))
		cout << testList << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToPrev();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToEnd();
	cout << "Cursor value: " << testList.AtCursor() << endl;

}


/*
  Name: cursorTest4                                              
  Purpose: Demonstrates the use of the LinkedList's Insert() and GoToNext() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void cursorTest4() {

	LL<int> testList;

	cout << "Before Test: " << testList << endl;

	if (testList.Insert(10))
		cout << testList << endl;

	if (testList.Insert(5))
		cout << testList << endl;

	if (testList.Insert(12))
		cout << testList << endl;

	if (testList.Insert(3))
		cout << testList << endl;

	testList.GoToNext();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToNext();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToNext();
	cout << "Cursor value: " << testList.AtCursor() << endl;

	testList.GoToNext();
	cout << "Cursor value: " << testList.AtCursor() << endl;

}


/*
  Name: clearTest1                                               
  Purpose: Demonstrates the use of the LinkedList's Insert() and ClearList() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void clearTest1() {

	LL<int> testList;

	if (testList.Insert(5))
		cout << testList << endl;

	if (testList.Insert(7))
		cout << testList << endl;

	if (testList.Insert(1))
		cout << testList << endl;

	if (testList.Insert(3))
		cout << testList << endl;


	cout << endl << "Before clearing: " << testList << endl;

	testList.ClearList();
	cout << "After clearing : " << testList << endl << endl;

}


/*
  Name: clearTest2                                              
  Purpose: Demonstrates the use of the LinkedList's Insert() and ClearList() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void clearTest2() {

	LL<int> testList;

	if (testList.Insert(-100))
		cout << testList << endl;

	if (testList.Insert(200))
		cout << testList << endl;

	if (testList.Insert(-500))
		cout << testList << endl;

	cout << endl << "Before clearing: " << testList << endl;

	testList.ClearList();
	cout << "After clearing : " << testList << endl << endl;

	if (testList.Insert(1000))
		cout << testList << endl;

	if (testList.Insert(-250))
		cout << testList << endl;

	if (testList.Insert(0))
		cout << testList << endl;

	cout << endl << "Before clearing: " << testList << endl;

	testList.ClearList();
	cout << "After clearing : " << testList << endl << endl;

}


/*
  Name: emptyTest1                                               
  Purpose: Demonstrates the use of the LinkedList's Insert() and Empty() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void emptyTest1() {

	LL<int> testList;

	cout << "  Before test: " << testList << endl;
	cout << "List is empty: " << testList.Empty() << endl;

	testList.Insert(10);
	testList.Insert(20);
	testList.Insert(30);

	cout << " After insert: " << testList << endl;
	cout << "List is empty: " << testList.Empty() << endl;

}


/*
  Name: emptyTest2                                               
  Purpose: Demonstrates the use of the LinkedList's Insert(), ClearList(), and Empty() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void emptyTest2() {

	LL<int> testList;

	cout << "  Before test: " << testList << endl;
	cout << "List is empty: " << testList.Empty() << endl;

	testList.Insert(10);
	testList.Insert(20);
	testList.Insert(30);

	cout << " After insert: " << testList << endl;
	cout << "List is empty: " << testList.Empty() << endl;

	testList.ClearList();
	cout << " List cleared: " << testList << endl;
	cout << "List is empty: " << testList.Empty() << endl;

}


/*
  Name: fullTest1                                               
  Purpose: Demonstrates the use of the LinkedList's Insert() and Full() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void fullTest1() {

	LL<int> testList;

	cout << " Before test: " << testList << endl;
	cout << "List is full: " << testList.Full() << endl;

	testList.Insert(10);
	testList.Insert(20);
	testList.Insert(30);

	cout << "After insert: " << testList << endl;
	cout << "List is full: " << testList.Full() << endl;

}


/*
  Name: fullTest2                                              
  Purpose: Demonstrates the use of the LinkedList's Insert() and Full() methods.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void fullTest2() {

	LL<int> testList;

	cout << " Before test: " << testList << endl;
	cout << "List is full: " << testList.Full() << endl;

	for (int i = 0; i < 10000; i++)
		testList.Insert(i);

	cout << "After insert: " << testList << endl;
	cout << "List is full: " << testList.Full() << endl;

}


/*
  Name: assignmentTest1                                             
  Purpose: Demonstrates the use of the LinkedList's Insert() method and overloaded assignment operator.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void assignmentTest1() {

	LL<int> testList;
	LL<int> assignedList;

	testList.Insert(10);
	testList.Insert(20);

	cout << "Original List: " << testList << endl;

	assignedList = testList;

	cout << "Assigned List: " << assignedList << endl << endl;

	assignedList.Insert(15);

	cout << "Original List: " << testList << endl;
	cout << "Assigned List: " << assignedList << endl;

}


/*
  Name: assignmentTest2                                            
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToBeginning(), and Remove() methods
		   and overloaded assignment operator.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void assignmentTest2() {

	LL<int> testList;
	LL<int> assignedList;

	testList.Insert(10);
	testList.Insert(20);

	cout << "Original List: " << testList << endl;

	assignedList = testList;

	cout << "Assigned List: " << assignedList << endl << endl;

	testList.GoToBeginning();
	testList.Remove(10);
	assignedList.Insert(5);

	cout << "Original List: " << testList << endl;
	cout << "Assigned List: " << assignedList << endl;

}


/*
  Name: copyTest1                                            
  Purpose: Demonstrates the use of the LinkedList's Insert() method and copy constructor.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void copyTest1() {

	LL<int> testList;

	testList.Insert(10);
	testList.Insert(20);

	cout << "Original List: " << testList << endl;

	LL<int> copyList = testList;

	cout << "  Copied List: " << copyList << endl << endl;

	copyList.Insert(15);

	cout << "Original List: " << testList << endl;
	cout << "  Copied List: " << copyList << endl;

}


/*
  Name: copyTest2                                            
  Purpose: Demonstrates the use of the LinkedList's Insert(), GoToBeginning(), and Remove() methods
		   and copy constructor.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void copyTest2() {

	LL<int> testList;

	testList.Insert(10);
	testList.Insert(20);

	cout << "Original List: " << testList << endl;

	LL<int> copyList = testList;

	cout << "  Copied List: " << copyList << endl << endl;

	testList.GoToBeginning();
	testList.Remove(10);
	copyList.Insert(5);

	cout << "Original List: " << testList << endl;
	cout << "  Copied List: " << copyList << endl;


}
