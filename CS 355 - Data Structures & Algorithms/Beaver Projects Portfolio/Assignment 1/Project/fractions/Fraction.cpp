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
#include "Fraction.h"

using namespace std;



//---------------------------------------------------------------
// CONSTANTS
//---------------------------------------------------------------

const unsigned int RAND_LIMIT = 20; // Random value limit



/*
  Name: Fraction in scope of Fraction                                                          
  Purpose: Default constructor produces a 1/1 fraction
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Fraction::Fraction() {

	num = 1;
	den = 1;

} // end Fraction



/*
  Name: Fraction in scope of Fraction                                                          
  Purpose: Overloaded constructor produces a whole number fraction (e.g., 3/1)
  Incoming: numerator is the integer numerator portion of the Fraction
  Outgoing: N/A
  Return: N/A
*/
Fraction::Fraction(int numerator) {

	num = numerator;
	den = 1;

} // end Fraction



/*
  Name: Fraction in scope of Fraction                                                         
  Purpose: Overloaded constructor produces a custom fraction in the form of num/den
  Incoming: numerator is the integer numerator portion of the Fraction; denominator is the integer 
			denominator portion of the Fraction
  Outgoing: N/A
  Return: N/A
*/
Fraction::Fraction(int numerator, int denominator) {

	num = numerator;
	den = denominator;

} // end Fraction



/*
  Name: Fraction in scope of Fraction                                                        
  Purpose: Overloaded constructor produces a random Fraction
  Incoming: random is a boolean indicator of whether or not to create a random fraction (true = yes)
  Outgoing: N/A
  Return: N/A
*/
Fraction::Fraction(bool random) {

	// Generate a random Fraction if 'true' is passed in
	if (random) {

		// Random values in range [1, RAND_LIMIT] -- do NOT want 0
		num = rand() % RAND_LIMIT;
		num++; 

		den = rand() % RAND_LIMIT;
		den++; 

	}

	// If false, use the default constructor to create a 1/1 Fraction
	else
		Fraction();

} // end Fraction



/*
  Name: ~Fraction in scope of Fraction                                                        
  Purpose: Destructor that destructs instances of the Fraction class. The Fraction class's data members
		   are only two integers, so an explicitly defined destructor is not necessary.
  Incoming: N/A
  Outgoing: N/A
  Return: N/A
*/
Fraction::~Fraction() {

	// Nothing to define

} // end ~Fraction



/*
  Name: print in scope of Fraction                                                          
  Purpose: Outputs the Fraction instance to the screen
  Incoming: os is a pointer to an ostream (for cout)
  Outgoing: os is a pointer to an ostream (for cout), which will output the Fraction data to the screen
  Return: N/A (void)
*/
void Fraction::print(ostream *os) const {

	*os << this->num << "/" << this->den;

} // end print



/*
  Name: << (friend)                                                      
  Purpose: Overloaded insertion operator allows for a user-friendly screen representation
		   of a Fraction instance
  Incoming: os is an ostream that will be used to output the Fraction to the screen; source is a 
			Fraction passed by reference -- it is the Fraction to be output to the screen
  Outgoing: os is an ostream passed by reference; source is a Fraction passed by reference
  Return: Returns an ostream to be output to the screen
*/
ostream& operator <<(ostream& os, Fraction& source) {

	source.print(&os); // Obtains the user-friendly version of the Fraction instance

	return os;

} // end <<



/*
  Name: >> (friend)                                                          
  Purpose: Overloaded extraction operator allows the user to input a custom Fraction
  Incoming: is is an istream that contains the user's entered Fraction; source is the destination
			Fraction instance that will assume the custom Fraction's attributes
  Outgoing: is and source are passed by reference -- source will have the user's custom Fraction's attributes
  Return: Returns an istream that contains the user's input custom Fraction
*/
istream& operator >>(istream& is, Fraction& source) {

	string input;
	int pos;

	is >> input;           // Get the custom Fraction

	pos = input.find('/'); // Find the separator, if it exists
	

	// Try to create a custom fraction if the separator exists
	if (pos != string::npos) {

		string tmpStr;

		// No given numerator (e.g., "/3"), instantiate with 0 as the numerator (e.g., "0/3")
		if (pos == 0)
			source.num = 0;

		// Numerator present (e.g., "1/x"), instantiate with the given numerator
		else {

			// Get the entire numerator
			for (int i = 0; i < pos; i++) 
				tmpStr += input[i];

			// Convert the string numerator to an integer
			source.num = atoi(tmpStr.c_str());
		
		}

		tmpStr = "";


		// No denominator given (e.g., "4/"), instantiate with 1 for a valid fraction (e.g., "4/1")
		if ((pos + 1) == input.length())
			source.den = 1;

		// Denominator present (e.g., "1/3"), instantiate with the given denominator
		else {

			// Get the entire denominator
			for (int i = (pos + 1); i < input.length(); i++)
				tmpStr += input[i];

			// The denominator is 0, replace it with 1 for a valid fraction
			if (atoi(tmpStr.c_str()) == 0)
				source.den = 1;

			// Otherwise, convert the string denominator to an integer
			else
				source.den = atoi(tmpStr.c_str());

		}
	

	// A whole number was given, use 1 for denominator
	} else {

		source.num = atoi(input.c_str());
		source.den = 1;

	}

	return is;

} // end >>



/*
  Name: + in scope of Fraction                                                       
  Purpose: Overloaded addition operator allows the user to add two Fractions using the Cross Multiply method
  Incoming: f2 is the right-side operand
  Outgoing: f2 is passed by reference (const modifier prevents accidental editing of data members)
  Return: Returns a new Fraction that is the result of the addition operation
*/
Fraction Fraction::operator +(const Fraction& f2) const {

	Fraction result;
	int crossMultNum;

	/*
		Cross Multiplying to find the numerator of the resulting fraction
		(Numerator1 * Denominator2) + (Denominator1 * Numerator2)
	*/
	crossMultNum = this->num * f2.den;
	crossMultNum += this->den * f2.num;

	result.num = crossMultNum;
	result.den = this->den * f2.den;

	return result;

} // end +



/*
  Name: - in scope of Fraction                                                    
  Purpose: Overloaded subtraction operator allows the user to subtract two Fractions
  Incoming: f2 is the right-side operand
  Outgoing: f2 is passed by reference (const modifier prevents accidental editing of data members)
  Return: Returns a new Fraction that is the result of the subtraction operation
*/
Fraction Fraction::operator -(const Fraction& f2) const {

	Fraction result;
	int LCM;

	// Get the Lowest Common Multiple of the denominators
	LCM = getLCM(this->den, f2.den); 


	/*
		First, make the numerator of the left-side operand correct by multiplying it with the value needed
			for the fractions to have the same denominator.
		Second, make the numerator of the right-side operand correct by multiplying it with the value needed
			for the fractions to have the same denominator.
		Third, subtract the right-side numerator from the left-side numerator.
		Finally, the resulting Fraction's denominator is the LCM.
	*/
	result.num = (this->num * (LCM / this->den));
	result.num -= (f2.num * (LCM / f2.den));
	result.den = LCM;

	return result;

} // end -



/*
  Name: = in scope of Fraction                                                         
  Purpose: Overloaded assignment operator allows Fractions to be assigned the values of other Fractions
		   or the results of operations performed on Fractions.
  Incoming: source is the original Fraction whose data is being assigned to the new Fraction
  Outgoing: source is passed by reference
  Return: Returns a Fraction whose data members have the same values as source's data members
*/
Fraction& Fraction::operator =(const Fraction& source) {

	// Check for self-assignment
	if (this == &source)
		return *this;

	num = source.num;
	den = source.den;

	return *this;

} // end =



/*
  Name: * (friend)                                                      
  Purpose: Overloaded multiplication operator allows the user to multiply two Fractions
  Incoming: f1 and f2 are Fractions to be multiplied; f1 is the left-side operand, and f2 is 
			the right-side operand
  Outgoing: f1 and f2 are passed by reference (with const modifier)
  Return: Returns a Fraction that is the result of multiplying the original Fractions
*/
Fraction operator *(const Fraction& f1, const Fraction& f2) {

	Fraction result;

	// Simple multiplication
	result.num = f1.num * f2.num;
	result.den = f1.den * f2.den;

	return result;

} // end *



/*
  Name: / (friend)                                                          
  Purpose: Overloaded division operator allows the user to divide two Fractions
  Incoming: f1 and f2 are the Fractions to be divided; f1 is the left-side operand, and f2 is
			the right-side operand
  Outgoing: f1 and f2 are passed by reference (with const modifier)
  Return: Returns the Fraction that is the result of the division
*/
Fraction operator /(const Fraction& f1, const Fraction& f2) {

	Fraction result;

	if (f2.num != 0) {

		/*
			Divide by multiplying by the reciprocal.

			n     n       n     d
			--- / ---  =  --- x ---
			d     d       d     n

		*/
		result.num = f1.num * f2.den;
		result.den = f1.den * f2.num;

		// Just in case the denominator is ever 0
		if (result.den == 0)
			result.den = 1;

	} 
	
	else
		cout << "ERROR: Division by zero!" << endl;

	return result;

} // end /



/*
  Name: getGCD                                                           
  Purpose: Calculates the Greatest Common Divisor of two integers -- using the Euclidean algorithm
  Incoming: a and b are integers whose GCD is to be calculated
  Outgoing: N/A
  Return: Returns the GCD of a and b as an integer
*/
int getGCD(int a, int b) {

	int tmp;


	/*
		Loop until b == 0, at which point a common divisor should be found. 
		The GCD will be the last value of b (a = tmp) before b == 0.
	*/
	while (b != 0) {

		tmp = b;
		b = a % b;
		a = tmp;

	}

	return a;

} // end getGCD



/*
  Name: getLCM                                                          
  Purpose: Calculates the Lowest Common Multiplier of two integers
  Incoming: a and b are integers whose LCM is to be calculated
  Outgoing: N/A
  Return: Returns the LCM of a and b as an integer
*/
int getLCM(int a, int b) {


	/*
			  |a * b|   <-- Want positive values only
		LCM = --------
			  gcd(a,b)
	*/
	return (abs(a * b) / getGCD(a,b));


} // end getLCM

