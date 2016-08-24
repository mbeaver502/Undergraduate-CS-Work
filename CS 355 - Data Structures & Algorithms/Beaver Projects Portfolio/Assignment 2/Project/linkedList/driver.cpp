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
#include <string>
#include "LinkedList.h"
#include "testCases.h"

using namespace std;



//----------------------------------------------------------------------------------------
// FUNCTION SIGNATURES
//----------------------------------------------------------------------------------------

// Driver menu handlers
int menu();
void drawMenu();
void selectionHandler(int selection, LinkedList& list);

// See testCases.h and testCases.cpp for test cases



//----------------------------------------------------------------------------------------
// CONSTANTS
//----------------------------------------------------------------------------------------
						
const string MENU_STARS = "*****************************************";
const string BLANK_STARS = "*                                       *";



//----------------------------------------------------------------------------------------
// FUNCTION DEFINITIONS
//----------------------------------------------------------------------------------------

int main() {

	LinkedList myList;

	int menuSelection = 0;


	// Loop until the user chooses to quit the program
	do {

		system("CLS");

		menuSelection = menu();
		selectionHandler(menuSelection, myList); // Executes test cases based on user menu selection

		myList.clearList();						 // Clear the LinkedList between test cases

		cout << endl;
		system("PAUSE");

	} while (menuSelection != 0); // 0 = Exit

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
	cout << "*      LinkedList Class Test Menu       *" << endl;
	cout << MENU_STARS << endl;
	cout << BLANK_STARS << endl;

	cout << "* (01) Test Case 01 | (02) Test Case 02 *" << endl;
	cout << "* (03) Test Case 03 | (04) Test Case 04 *" << endl;
	cout << "* (05) Test Case 05 | (06) Test Case 06 *" << endl;
	cout << "* (07) Test Case 07 | (08) Test Case 08 *" << endl;
	cout << "* (09) Test Case 09 | (10) Test Case 10 *" << endl;
	cout << "* (11) Test Case 11 | (12) Test Case 12 *" << endl;
	cout << "* (13) Test Case 13 | (14) Test Case 14 *" << endl;
	cout << "* (15) Test Case 15 | (16) Test Case 16 *" << endl;
	cout << BLANK_STARS << endl;
	cout << "*         (17) Spree Test Case          *" << endl;
	cout << BLANK_STARS << endl;
	cout << "*               (00) Exit               *" << endl;

	cout << BLANK_STARS << endl;
	cout <<  MENU_STARS << endl << endl;

}



/*
  Name: selectionHandler                                                       
  Purpose: Executes a test case function based on the user's menu selection
  Incoming: selection is the integer result from menu() -- the user's menu selection
  Outgoing: N/A
  Return: N/A (void)
*/
void selectionHandler(int selection, LinkedList& list) {

	// Choose which action to execute based on the user's choice
	switch (selection) {

		case 1: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 1              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase1(list);
			break;

		}

		case 2: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 2              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase2(list);
			break;

		}

		case 3: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 3              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase3(list);
			break;

		}

		case 4: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 4              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase4(list);	
			break;

		}

		case 5: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 5              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase5(list);	
			break;

		}

		case 6: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 6              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase6(list);	
			break;

		}

		case 7: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 7              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase7(list);	
			break;

		}

		case 8: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 8              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase8(list);	
			break;

		}

		case 9: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 9              *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase9(list);	
			break;

		}

		case 10: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 10             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase10(list);
			break;

		}

		case 11: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 11             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase11(list);	
			break;

		}

		case 12: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 12             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase12(list);	
			break;

		}

		case 13: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 13             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase13(list);	
			break;

		}

		case 14: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 14             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase14(list);	
			break;

		}

		case 15: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 15             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase15(list);
			break;

		}

		case 16: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*              Test Case 16             *" << endl;     
			cout << MENU_STARS << endl << endl;

			testCase16(list);
			break;

		}

		case 17: {

			system("CLS");
			cout << MENU_STARS << endl;
			cout << "*            Spree Test Case            *" << endl;     
			cout << MENU_STARS << endl << endl;

			spreeTestCase(list);
			break;

		}

		default: 
			break; 

	}

}