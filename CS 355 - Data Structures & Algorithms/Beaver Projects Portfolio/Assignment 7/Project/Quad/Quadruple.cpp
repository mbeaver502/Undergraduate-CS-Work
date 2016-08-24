/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#include "Quadruple.h"


/*
  Name: Quadruple in scope of Quadruple
  Purpose: Default constructor. Uses an initialization list to set q to zero.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Quadruple::Quadruple(): q(0) {}


/*
  Name: Quadruple in scope of Quadruple
  Purpose: Overloaded constructor. Uses an initialization list to set data member values.
  Incoming: a, b, c, and d are the integer values that the data members will take on.
  Outgoing: N/A
  Return: N/A
*/
Quadruple::Quadruple(int a, int b, int c, int d): Triple(a, b, c), q(d) {}


/*
  Name: SetQ in scope of Quadruple
  Purpose: Sets the q value.
  Incoming: d is the new integer value for q.
  Outgoing: N/A
  Return: N/A (void)
*/
void Quadruple::SetQ(int d) { q = d; }


/*
  Name: Set in scope of Quadruple
  Purpose: Sets the data members' values to new values.
  Incoming: a, b, c, and d are the new integer values for x, y, z, and q, respectively.
  Outgoing: N/A
  Return: N/A (void)
*/
void Quadruple::Set(int a, int b, int c, int d) {

	x = a;
	y = b;
	z = c;
	q = d;

}


/*
  Name: GetQ in scope of Quadruple
  Purpose: Returns the q data member's value.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the q data member's value.
*/
int Quadruple::GetQ() const { return q; }


/*
  Name: Print in scope of Quadruple
  Purpose: Prints the Quadruple object to the screen.
  Incoming: os is a reference to the ostream object (cout).
  Outgoing: os is a reference to the ostream object (cout).
  Return: N/A (void)
*/
void Quadruple::Print(ostream & os) const {

	Triple::Print(os);
	os << "," << q;

}


