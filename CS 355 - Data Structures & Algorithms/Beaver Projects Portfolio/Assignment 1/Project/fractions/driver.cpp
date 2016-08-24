/*
	Michael Beaver
	CS355 Fall 2012
	Assignment: Homework 1
	Description: This program implements and demonstrates the usage and functionality of a Fraction class. The
			Fraction class is made up of two integers (numerator and denominator). Fractions may be instantiated
			as 1/1, a whole number, a custom fraction, or as a pseudo-random fraction. Binary operations 
			(+, -, *, /) may be performed on Fractions. This program uses several cases to test the accuracy 
			of the Fraction class's implementation of these operations. The Fraction class lacks the
			ability to reduce fractions.
    Due Date: August 28, 2012
*/



#include <iostream>
#include <string>
#include <time.h>
#include "Fraction.h"

using namespace std;



//---------------------------------------------------------------
// FUNCTION SIGNATURES
//---------------------------------------------------------------

// Driver menu handlers
int menu();
void drawMenu();
void selectionHandler(int selection);

// Tests
void testAdd(Fraction& randomFrac);
void testSub(Fraction& randomFrac);
void testMul(Fraction& randomFrac);
void testDiv(Fraction& randomFrac);
void testMix(Fraction& randomFrac);
void testCustom();

// Misc.
Fraction genRandomFraction();



//---------------------------------------------------------------
// CONSTANTS
//---------------------------------------------------------------

const string MENU_STARS = "**************************************";
const string BLANK_STARS = "*                                    *";



//---------------------------------------------------------------
// FUNCTION DEFINITIONS
//---------------------------------------------------------------

int main() {

	srand(time(NULL));  // Initalize pseudo-random number generator

	int menuSelection;
	
	do {

		// I realize that the system commands CLS and PAUSE are not necessarily portable
		system("CLS");

		// Show a menu, and perform an action based on the user's choice
		menuSelection = menu();
		selectionHandler(menuSelection);

		cout << endl;
		system("PAUSE");

	} while (menuSelection != 0); // 0 = Exit program

	return 0;

} // end main



/*
  Name: menu                                                         
  Purpose: Displays a menu for the user to make a selection from
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the user's menu selection as an integer
*/
int menu() {

	int selection;

	drawMenu();

	cout << "Enter your selection: ";
	cin >> selection;

	return selection;

} // end menu



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
	cout << "*     Fractions Class Test Menu      *" << endl;
	cout << MENU_STARS << endl;
	cout << BLANK_STARS << endl;
	cout << "*   1) Case 1 -- Addition            *" << endl;
	cout << "*   2) Case 2 -- Subtraction         *" << endl;
	cout << "*   3) Case 3 -- Multiplication      *" << endl;
	cout << "*   4) Case 4 -- Division            *" << endl;
	cout << "*   5) Case 5 -- Mixed Operations    *" << endl;
	cout << "*   6) Custom Fractions              *" << endl;
	cout << BLANK_STARS << endl;
	cout << "*   0) Exit                          *" << endl;
	cout << BLANK_STARS << endl;
	cout <<  MENU_STARS << endl << endl;

	cout << MENU_STARS << endl;
	cout << "*           IMPORTANT NOTE           *" << endl;
	cout << MENU_STARS << endl;  
	cout << "* Test cases 1-5 use one pseudo-     *" << endl;
	cout << "* random fraction, two whole number  *" << endl;
	cout << "* fractions (1/1 & 7/1), and two     *" << endl;
	cout << "* normal fractions (3/4 & 11/16).    *" << endl;
	cout << MENU_STARS << endl << endl;

} // end drawMenu



/*
  Name: selectionHandler                                                       
  Purpose: Executes a test case function based on the user's menu selection
  Incoming: selection is the integer result from menu() -- the user's menu selection
  Outgoing: N/A
  Return: N/A (void)
*/
void selectionHandler(int selection) {

	// Generating the random fraction -- changes each time the user makes a menu selection
	Fraction random(true); 


	// Choose which action to execute based on the user's choice
	switch (selection) {

		case 1:
			testAdd(random);    // Addition test case
			break;

		case 2:
			testSub(random);    // Subtraction test case
			break;

		case 3:
			testMul(random);    // Multiplication test case
			break;

		case 4:
			testDiv(random);    // Division test case
			break;

		case 5:
			testMix(random);    // Mixed (complex) operations test case
			break;

		case 6:
			testCustom();       // Custom fractions test case
			break;

		default: 
			break; 

	}

} // end selectionHandler



/*
  Name: testAdd                                                        
  Purpose: Demonstrates the usage and functionality of the Fraction class's overloaded + operator
  Incoming: randomFrac is a random Fraction passed by reference
  Outgoing: randomFrac is a random Fraction passed by reference
  Return: N/A (void)
*/
void testAdd(Fraction& randomFrac) {

	system("CLS");

	// Header
	cout << MENU_STARS << endl;
	cout << "*         Addition Test Case         *" << endl;
	cout << MENU_STARS << endl << endl;

	Fraction frac1 = randomFrac; // Random fraction
	Fraction frac2;              // 1/1
	Fraction frac3(7);           // 7/1
	Fraction frac4(3,4);         // 3/4
	Fraction frac5(11,16);       // 11/16

	cout << "Random Fraction: " << frac1 << endl << endl;


	// 15 cases rather than 25 because addition is commutative -- redundant to have more cases
	cout << "01) " << frac1 << " + " << frac1 << " = " << (frac1 + frac1) << endl;
	cout << "02) " << frac1 << " + " << frac2 << " = " << (frac1 + frac2) << endl;
	cout << "03) " << frac1 << " + " << frac3 << " = " << (frac1 + frac3) << endl;
	cout << "04) " << frac1 << " + " << frac4 << " = " << (frac1 + frac4) << endl;
	cout << "05) " << frac1 << " + " << frac5 << " = " << (frac1 + frac5) << endl << endl;

	cout << "06) " << frac2 << " + " << frac2 << " = " << (frac2 + frac2) << endl;
	cout << "07) " << frac2 << " + " << frac3 << " = " << (frac2 + frac3) << endl;
	cout << "08) " << frac2 << " + " << frac4 << " = " << (frac2 + frac4) << endl;
	cout << "09) " << frac2 << " + " << frac5 << " = " << (frac2 + frac5) << endl << endl;

	cout << "10) " << frac3 << " + " << frac3 << " = " << (frac3 + frac3) << endl;
	cout << "11) " << frac3 << " + " << frac4 << " = " << (frac3 + frac4) << endl;
	cout << "12) " << frac3 << " + " << frac5 << " = " << (frac3 + frac5) << endl << endl;

	cout << "13) " << frac4 << " + " << frac4 << " = " << (frac4 + frac4) << endl;
	cout << "14) " << frac4 << " + " << frac5 << " = " << (frac4 + frac5) << endl << endl;

	cout << "15) " << frac5 << " + " << frac5 << " = " << (frac5 + frac5) << endl << endl;
	
} // end testAdd



/*
  Name: testSub                                                         
  Purpose: Demonstrates the usage and functionality of the Fraction class's overloaded - operator
  Incoming: randomFrac is a random Fraction passed by reference
  Outgoing: randomFrac is a random Fraction passed by reference
  Return: N/A (void)
*/
void testSub(Fraction& randomFrac) {

	system("CLS");

	// Header
	cout << MENU_STARS << endl;
	cout << "*        Subtraction Test Case       *" << endl;
	cout << MENU_STARS << endl << endl;

	Fraction frac1 = randomFrac; // Random fraction
	Fraction frac2;              // 1/1
	Fraction frac3(7);           // 7/1
	Fraction frac4(3,4);         // 3/4
	Fraction frac5(11,16);       // 11/16

	cout << "Random Fraction: " << frac1 << endl << endl;


	// 25 cases because subtraction is not commutative like addition
	cout << "01) " << frac1 << " - " << frac1 << " = " << (frac1 - frac1) << endl;
	cout << "02) " << frac1 << " - " << frac2 << " = " << (frac1 - frac2) << endl;
	cout << "03) " << frac1 << " - " << frac3 << " = " << (frac1 - frac3) << endl;
	cout << "04) " << frac1 << " - " << frac4 << " = " << (frac1 - frac4) << endl;
	cout << "05) " << frac1 << " - " << frac5 << " = " << (frac1 - frac5) << endl << endl;

	cout << "06) " << frac2 << " - " << frac1 << " = " << (frac2 - frac1) << endl;
	cout << "07) " << frac2 << " - " << frac2 << " = " << (frac2 - frac2) << endl;
	cout << "08) " << frac2 << " - " << frac3 << " = " << (frac2 - frac3) << endl;
	cout << "09) " << frac2 << " - " << frac4 << " = " << (frac2 - frac4) << endl;
	cout << "10) " << frac2 << " - " << frac5 << " = " << (frac2 - frac5) << endl << endl;

	cout << "11) " << frac3 << " - " << frac1 << " = " << (frac3 - frac1) << endl;
	cout << "12) " << frac3 << " - " << frac2 << " = " << (frac3 - frac2) << endl;
	cout << "13) " << frac3 << " - " << frac3 << " = " << (frac3 - frac3) << endl;
	cout << "14) " << frac3 << " - " << frac4 << " = " << (frac3 - frac4) << endl;
	cout << "15) " << frac3 << " - " << frac5 << " = " << (frac3 - frac5) << endl << endl;

	cout << "16) " << frac4 << " - " << frac1 << " = " << (frac4 - frac1) << endl;
	cout << "17) " << frac4 << " - " << frac2 << " = " << (frac4 - frac2) << endl;
	cout << "18) " << frac4 << " - " << frac3 << " = " << (frac4 - frac3) << endl;
	cout << "19) " << frac4 << " - " << frac4 << " = " << (frac4 - frac4) << endl;
	cout << "20) " << frac4 << " - " << frac5 << " = " << (frac4 - frac5) << endl << endl;

	cout << "21) " << frac5 << " - " << frac1 << " = " << (frac5 - frac1) << endl;
	cout << "22) " << frac5 << " - " << frac2 << " = " << (frac5 - frac2) << endl;
	cout << "23) " << frac5 << " - " << frac3 << " = " << (frac5 - frac3) << endl;
	cout << "24) " << frac5 << " - " << frac4 << " = " << (frac5 - frac4) << endl;
	cout << "25) " << frac5 << " - " << frac5 << " = " << (frac5 - frac5) << endl << endl;

} // end testSub



/*
  Name: testMul                                                          
  Purpose: Demonstrates the usage and functionality of the Fraction class's overloaded * operator
  Incoming: randomFrac is a random Fraction passed by reference
  Outgoing: randomFrac is a random Fraction passed by reference
  Return: N/A (void)
*/
void testMul(Fraction& randomFrac) {

	system("CLS");

	// Header
	cout << MENU_STARS << endl;
	cout << "*      Multiplication Test Case      *" << endl;
	cout << MENU_STARS << endl << endl;

	Fraction frac1 = randomFrac; // Random fraction
	Fraction frac2;              // 1/1
	Fraction frac3(7);           // 7/1
	Fraction frac4(3,4);         // 3/4
	Fraction frac5(11,16);       // 11/16

	cout << "Random Fraction: " << frac1 << endl << endl;


	// 15 cases rather than 25 because multiplications is commutative -- redundant to have more cases
	cout << "01) " << frac1 << " * " << frac1 << " = " << (frac1 * frac1) << endl;
	cout << "02) " << frac1 << " * " << frac2 << " = " << (frac1 * frac2) << endl;
	cout << "03) " << frac1 << " * " << frac3 << " = " << (frac1 * frac3) << endl;
	cout << "04) " << frac1 << " * " << frac4 << " = " << (frac1 * frac4) << endl;
	cout << "05) " << frac1 << " * " << frac5 << " = " << (frac1 * frac5) << endl << endl;

	cout << "06) " << frac2 << " * " << frac2 << " = " << (frac2 * frac2) << endl;
	cout << "07) " << frac2 << " * " << frac3 << " = " << (frac2 * frac3) << endl;
	cout << "08) " << frac2 << " * " << frac4 << " = " << (frac2 * frac4) << endl;
	cout << "09) " << frac2 << " * " << frac5 << " = " << (frac2 * frac5) << endl << endl;

	cout << "10) " << frac3 << " * " << frac3 << " = " << (frac3 * frac3) << endl;
	cout << "11) " << frac3 << " * " << frac4 << " = " << (frac3 * frac4) << endl;
	cout << "12) " << frac3 << " * " << frac5 << " = " << (frac3 * frac5) << endl << endl;

	cout << "13) " << frac4 << " * " << frac4 << " = " << (frac4 * frac4) << endl;
	cout << "14) " << frac4 << " * " << frac5 << " = " << (frac4 * frac5) << endl << endl;

	cout << "15) " << frac5 << " * " << frac5 << " = " << (frac5 * frac5) << endl << endl;

} // end testMul



/*
  Name: testDiv                                                         
  Purpose: Demonstrates the usage and functionality of the Fraction class's overloaded / operator
  Incoming: randomFrac is a random Fraction passed by reference
  Outgoing: randomFrac is a random Fraction passed by reference
  Return: N/A (void)
*/
void testDiv(Fraction& randomFrac) {

	system("CLS");

	cout << MENU_STARS << endl;
	cout << "*         Division Test Case         *" << endl;
	cout << MENU_STARS << endl << endl;

	Fraction frac1 = randomFrac; // Random fraction
	Fraction frac2;              // 1/1
	Fraction frac3(7);           // 7/1
	Fraction frac4(3,4);         // 3/4
	Fraction frac5(11,16);       // 11/16

	cout << "Random Fraction: " << frac1 << endl << endl;

	// 25 Cases because division is not commutative like multiplication
	cout << "01) " << frac1 << " / " << frac1 << " = " << (frac1 / frac1) << endl;
	cout << "02) " << frac1 << " / " << frac2 << " = " << (frac1 / frac2) << endl;
	cout << "03) " << frac1 << " / " << frac3 << " = " << (frac1 / frac3) << endl;
	cout << "04) " << frac1 << " / " << frac4 << " = " << (frac1 / frac4) << endl;
	cout << "05) " << frac1 << " / " << frac5 << " = " << (frac1 / frac5) << endl << endl;

	cout << "06) " << frac2 << " / " << frac1 << " = " << (frac2 / frac1) << endl;
	cout << "07) " << frac2 << " / " << frac2 << " = " << (frac2 / frac2) << endl;
	cout << "08) " << frac2 << " / " << frac3 << " = " << (frac2 / frac3) << endl;
	cout << "09) " << frac2 << " / " << frac4 << " = " << (frac2 / frac4) << endl;
	cout << "10) " << frac2 << " / " << frac5 << " = " << (frac2 / frac5) << endl << endl;

	cout << "11) " << frac3 << " / " << frac1 << " = " << (frac3 / frac1) << endl;
	cout << "12) " << frac3 << " / " << frac2 << " = " << (frac3 / frac2) << endl;
	cout << "13) " << frac3 << " / " << frac3 << " = " << (frac3 / frac3) << endl;
	cout << "14) " << frac3 << " / " << frac4 << " = " << (frac3 / frac4) << endl;
	cout << "15) " << frac3 << " / " << frac5 << " = " << (frac3 / frac5) << endl << endl;

	cout << "16) " << frac4 << " / " << frac1 << " = " << (frac4 / frac1) << endl;
	cout << "17) " << frac4 << " / " << frac2 << " = " << (frac4 / frac2) << endl;
	cout << "18) " << frac4 << " / " << frac3 << " = " << (frac4 / frac3) << endl;
	cout << "19) " << frac4 << " / " << frac4 << " = " << (frac4 / frac4) << endl;
	cout << "20) " << frac4 << " / " << frac5 << " = " << (frac4 / frac5) << endl << endl;

	cout << "21) " << frac5 << " / " << frac1 << " = " << (frac5 / frac1) << endl;
	cout << "22) " << frac5 << " / " << frac2 << " = " << (frac5 / frac2) << endl;
	cout << "23) " << frac5 << " / " << frac3 << " = " << (frac5 / frac3) << endl;
	cout << "24) " << frac5 << " / " << frac4 << " = " << (frac5 / frac4) << endl;
	cout << "25) " << frac5 << " / " << frac5 << " = " << (frac5 / frac5) << endl << endl;

} // end testDiv


/*
  Name: testMix                                                          
  Purpose: Performs mixed operations of varying complexity on Fractions to demonstrate how the binary
		   operators perform together
  Incoming: randomFrac is a random Fraction passed by reference
  Outgoing: randomFrac is a random Fraction passed by reference
  Return: 
*/
void testMix(Fraction& randomFrac) {

	system("CLS");

	// Header
	cout << MENU_STARS << endl;
	cout << "*     Mixed Operations Test Case     *" << endl;
	cout << MENU_STARS << endl << endl;

	Fraction frac1 = randomFrac; // Random fraction
	Fraction frac2;              // 1/1
	Fraction frac3(7);           // 7/1
	Fraction frac4(3,4);         // 3/4
	Fraction frac5(11,16);       // 11/16
	Fraction tmp;                // 1/1, temporary

	cout << "Random Fraction: " << frac1 << endl << endl;
	

	// Mixed operations on fractions of varying complexity
	// (b + c) - d
	tmp = (frac2 + frac3) - (frac4);
	cout << "01) " 
		 << "(" << frac2 << " + " << frac3 << ")" << " - "
		 << "(" << frac4 << ")" << " = " 
		 << tmp << endl << endl;
	
	// (d * e) / b
	tmp = (frac4 * frac5) / (frac2);
	cout << "02) " 
		 << "(" << frac4 << " * " << frac5 << ")" << " / " 
		 << "(" << frac2 << ")" <<  " = " 
		 << tmp << endl << endl;

	// [(a + b) / (c * d)] + (b - e)
	tmp = ((frac1 + frac2) / (frac3 * frac4)) + (frac2 - frac5);
	cout << "03) " 
		 << "[(" << frac1 << " + " << frac2 << ")" << " / "
		 << "(" << frac3 << " * " << frac4 << ")]" << " + " 
		 << "(" << frac2 << " - " << frac5 << ")" << " = "
		 << tmp << endl << endl;

	// (c + b) - (d / e)
	tmp = (frac3 + frac2) - (frac4 / frac5);
	cout << "04) "
		 << "(" << frac3 << " + " << frac2 << ")" << " - "
		 << "(" << frac4 << " / " << frac5 << ")" << " = "
		 << tmp << endl << endl;

	// (a + e) * (b - d) / c
	tmp = (frac1 + frac5) * (frac2 - frac4) / (frac3);
	cout << "05) "
		 << "(" << frac1 << " + " << frac5 << ")" << " * "
		 << "(" << frac2 << " - " << frac4 << ")" << " / "
		 << "(" << frac3 << ")" << " = "
		 << tmp << endl << endl;

} // end testMix


/*
  Name: testCustom                                                         
  Purpose: Allows the user to enter Fractions which are then evaluated with the Fraction class's 
		   overloaded binary operators in a for-loop
  Incoming: N/A
  Outgoing: N/A
  Return: N/A (void)
*/
void testCustom() {

	system("CLS");

	// Header
	cout << MENU_STARS << endl;
	cout << "*     Custom Fractions Test Case     *" << endl;
	cout << MENU_STARS << endl << endl;


	int count; // Number of fractions to enter

	cout << "Number of fractions: ";
	cin >> count;
	cout << endl;

	// Creating a new dynamic array to hold the Fractions
	Fraction *fracArr = new Fraction [count];
	Fraction tmpFrac;

	// Input the Fraction and add it to the array
	for (int i = 0; i < count; i++) {

		cout << "Fraction " << i+1 << " (#/#): ";
		cin >> tmpFrac;
		fracArr[i] = tmpFrac;

	}

	cout << endl;
	

	/*
		Perform the binary operations on the custom Fractions
		Note that not all combinations of Fractions and operations are evaluated and represented on screen
		j is used to retrieve the next Fraction after i
	*/
	int j = 1;
	for (int i = 0; i < count; i++) {
		
		// Addition
		cout << fracArr[i] << " + " << fracArr[j % count] << " = " 
			 << (fracArr[i] + fracArr[j % count]) << endl;

		// Subtraction
		cout << fracArr[i] << " - " << fracArr[j % count] << " = " 
			 << (fracArr[i] - fracArr[j % count]) << endl;

		// Subtraction (reversed)
		cout << fracArr[j % count] << " - " << fracArr[i] << " = " 
			 << (fracArr[j % count] - fracArr[i]) << endl;

		// Multiplication
		cout << fracArr[i] << " * " << fracArr[j % count] << " = " 
			 << (fracArr[i] * fracArr[j % count]) << endl;

		// Division
		cout << fracArr[i] << " / " << fracArr[j % count] << " = " 
			 << (fracArr[i] / fracArr[j % count]) << endl;

		// Division (reversed)
		cout << fracArr[j % count] << " / " << fracArr[i] << " = " 
			 << (fracArr[j % count] / fracArr[i]) << endl << endl;

		j++;

	}
	
	// Free the memory from the array
	delete [] fracArr;

} // end testCustom