/*
	Names: Michael Beaver
	CS355 Fall 2012
	Assignment: Assignment 7, Homework 5
	Description: This program is an experiment testing basic inheritance principles.
    Due Date: October 9, 2012
*/


#include "Pair.h"


/*
  Name: Pair in scope of Pair
  Purpose: Default constructor. Uses an initialization list to set x and y to zero.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Pair::Pair(): x(0), y(0) {}


/*
  Name: Pair in scope of Pair
  Purpose: Overloaded constructor. Uses an initialization list to set x and y to user-defined values.
  Incoming: a and b are the new values for x and y, respectively.
  Outgoing: N/A
  Return: N/A
*/
Pair::Pair(int a, int b): x(a), y(b) {} 


/*
  Name: SetX in scope of Pair
  Purpose: Sets the value of the x data member.
  Incoming: a is the new integer value.
  Outgoing: N/A
  Return: N/A (void)
*/
void Pair::SetX(int a) { x = a; }


/*
  Name: SetY in scope of Pair
  Purpose: Sets the value of y.
  Incoming: b is the new integer value for y.
  Outgoing: N/A
  Return: N/A (void)
*/
void Pair::SetY(int b) { y = b; }


/*
  Name: Set in scope of Pair
  Purpose: Sets the values of x and y to new values.
  Incoming: a and b are the new values for x and y, respectively.
  Outgoing: N/A
  Return: N/A (void)
*/
void Pair::Set(int a, int b) {

	SetX(a);
	SetY(b);

}


/*
  Name: GetX in scope of Pair
  Purpose: Returns the x data member's value.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the x data member's value.
*/
int Pair::GetX() const { return x; }


/*
  Name: GetY in scope of Pair
  Purpose: Returns the y data member's value.
  Incoming: N/A
  Outgoing: N/A
  Return: Returns the y data member's value.
*/
int Pair::GetY() const { return y; }


/*
  Name: Print in scope of Pair
  Purpose: Prints the Pair object to the screen.
  Incoming: os is a reference to the ostream object (cout).
  Outgoing: os is a reference to the ostream object (cout).
  Return: N/A (void)
*/
void Pair::Print(ostream & os) const { os << x << "," << y; }
