/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#include "Triple.h"


/*
  Name: Triple in scope of Triple
  Purpose: Default constructor. Uses an initialization list to set z to zero.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Triple::Triple(): z(0) {}


/*
  Name: Triple in scope of Triple
  Purpose: Overloaded constructor. Uses an initialization list to set data members.
  Incoming: a, b, and c are integer values to be set as data member values.
  Outgoing: N/A
  Return: N/A
*/
Triple::Triple(int a, int b, int c): Pair(a,b), z(c) {} 


/*
  Name: SetZ in scope of Triple
  Purpose: Sets the z value.
  Incoming: c is the integer value that z will be changed to.
  Outgoing: N/A
  Return: N/A (void)
*/
void Triple::SetZ(int c) { z = c; }


/*
  Name: Set in scope of Triple
  Purpose: Sets all data member values.
  Incoming: a, b, and, c whose values are the new values for x, y, and z, respectively.
  Outgoing: N/A
  Return: N/A (void)
*/
void Triple::Set(int a, int b, int c) {

	x = a;
	y = b;
	z = c;

}


/*
  Name: GetZ in scope of Triple
  Purpose: Returns the value stored in z.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the value stored in z.
*/
int Triple::GetZ() const { return z; }


/*
  Name: Print in scope of Triple
  Purpose: Prints the Triple object's data members.
  Incoming: os is a reference to the ostream object (cout).
  Outgoing: os is a reference to the ostream object (cout).
  Return: N/A (void)
*/
void Triple::Print(ostream & os) const {

	Pair::Print(os);
	os <<"," << z;

}